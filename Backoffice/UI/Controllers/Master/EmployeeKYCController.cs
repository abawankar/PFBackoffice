using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models.Master;

namespace UI.Controllers.Master
{
    [HandleError]
    [Authorize]
    public class EmployeeKYCController : Controller
    {
        static List<EmployeeKYCModel> draftKYC = new List<EmployeeKYCModel>();
        // GET: EmployeeKYC
        public ActionResult BulkUpload()
        {
            if (User.IsInRole("User"))
            {
                var comp = AppsUserDAL.GetByAppLogin(User.Identity.Name).AssignCompany.ToList();
                ViewBag.comp = new SelectList(comp, "Id", "Name");
            }
            else
            {
                CompanyRepository dal = new CompanyRepository();
                ViewBag.comp = new SelectList(dal.GetAll(), "Id", "Name");
            }
            return View();
        }

        [HttpPost]
        public JsonResult BulkList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeKYCRepository dal = new EmployeeKYCRepository();
                List<EmployeeKYCModel> model = new List<EmployeeKYCModel>();
                model = draftKYC.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult SaveBulk()
        {
            EmployeeDAL dal = new EmployeeDAL();
            string message = "";
            if (draftKYC.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                foreach (var item in draftKYC.GroupBy(x=>x.EmpCode))
                {
                    int compid = 0;
                    foreach (var comp in item)
                    {
                        compid = comp.Compid;
                    }
                    IEmployee emp = dal.GetByCompAndCode(compid,item.Key);
                    foreach (var obj in item)
                    {
                        IEmployeeKYC bl = new EmployeeKYC();
                        bl.DoxType = obj.DoxType.ToUpper();
                        bl.DocumentNumber = obj.DocumentNumber.ToUpper();
                        bl.NameonDox = obj.NameonDox;
                        bl.Other = obj.Other;
                        bl.IssueDate = obj.IssueDate;
                        bl.Exipiry = obj.Exipiry;
                        bl.Place = obj.Place;
                        emp.KYC.Add(bl);
                    }
                    dal.InsertOrUpdate(emp);
                }
                draftKYC.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(int compid)
        {
            var dirPath = Server.MapPath(Url.Content("~/Upload/Temp/"));
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dirPath);
            foreach (FileInfo fi in directory.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }
            string error = string.Empty;
            string _imgname = string.Empty;
            string imagepath = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];

                    if (hpf.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(hpf.FileName);
                        var _ext = Path.GetExtension(hpf.FileName);

                        if (_ext.ToLower() == ".csv")
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Upload/Temp/")) + hpf.FileName;
                            var path = _comPath;
                            hpf.SaveAs(path);

                            try
                            {
                                draftKYC.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {

                                        EmployeeKYCModel bl = new EmployeeKYCModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.EmpCode = x[0].Trim();
                                        bl.DoxType = x[1].Trim();
                                        bl.DocumentNumber = x[2].Trim();
                                        bl.NameonDox = x[3].Trim();
                                        bl.Other = x[4].Trim();
                                        bl.Compid = compid;
                                        if (bl.DoxType == "T")
                                        {
                                            if (x[5].Trim() != "")
                                                bl.IssueDate = Convert.ToDateTime(x[5].Trim());
                                            if (x[6].Trim() != "")
                                                bl.Exipiry = Convert.ToDateTime(x[6].Trim());
                                            bl.Place = x[7].Trim();
                                        }
                                        draftKYC.Add(bl);
                                    }
                                    catch (Exception ex) { error = error + "," + (sr - 1).ToString() + ex.Message; }
                                }

                            }
                            catch (Exception ex) { return Json(Convert.ToString(ex.Message), JsonRequestBehavior.AllowGet); }
                        }
                        else
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Uploads/TempCV/")) + hpf.FileName;
                            var path = _comPath;
                            hpf.SaveAs(path);
                        }
                    }
                }
            }
            return Json(Convert.ToString("Error Found in Record " + error), JsonRequestBehavior.AllowGet);
        }

    }
}