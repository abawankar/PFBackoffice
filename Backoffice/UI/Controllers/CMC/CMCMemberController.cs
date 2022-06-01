using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Models.CMC;
using UI.Models.Master;

namespace UI.Controllers.CMC
{
    public class CMCMemberController : Controller
    {
        static List<CMCMemberModel> draftMember = new List<CMCMemberModel>();
        static List<CMCMemberModel> draftMissing = new List<CMCMemberModel>();
        
        //C001
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("C001"))
            {
            }
            else {
                return View("Security");
            }

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "Code", Value = "1" },
                new SelectListItem { Text = "Name", Value = "2" },
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult List(int field = 0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                CMCMemberRepository dal = new CMCMemberRepository();
                List<CMCMemberModel> model = new List<CMCMemberModel>();
                model = dal.GetAll().ToList();
                if (!string.IsNullOrEmpty(search))
                {
                    switch (field)
                    {
                        case 1:
                            model = model.Where(x => x.Code == search.Trim()).ToList();
                            break;
                        case 2:
                            model = model.Where(x => (x.Name + "").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                    }
                }
                int count = model.Count;
                model = model.OrderBy(x => x.Code).ToList();
                List<CMCMemberModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //C002
        [HttpPost]
        public JsonResult Create(CMCMemberModel model)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("C002"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                CMCMemberRepository dal = new CMCMemberRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //C003
        [HttpPost]
        public JsonResult Update(CMCMemberModel model)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("C003"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                CMCMemberRepository dal = new CMCMemberRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //C004
        #region ---- Bulk Upload ----

        //C004
        public ActionResult BulkUpload()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("C004"))
            {
            }
            else {
                return View("Security");
            }
            return View();
        }

        [HttpPost]
        public JsonResult BulkList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<CMCMemberModel> model = new List<CMCMemberModel>();
                model = draftMember.ToList();
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
            string message = "";
            if (draftMember.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                IList<ICMCMember> list = new List<ICMCMember>();

                foreach (var obj in draftMember)
                {
                    ICMCMember bl = new CMCMember();
                    bl.Code = obj.Code;
                    bl.Name = obj.Name;
                    bl.Gender = obj.Gender;
                    bl.FatherName = obj.FatherName;
                    bl.DutyStation = obj.DutyStation;
                    bl.District = obj.District;
                    bl.SubRegion = obj.SubRegion;
                    bl.DOB = obj.DOB;
                    bl.DOC = obj.DOC;
                    bl.BankAc = obj.BankAc;
                    bl.IFSC = obj.IFSC;
                    bl.BankName = obj.BankName;
                    bl.PAN = obj.PAN;
                    bl.Aadhaar = obj.Aadhaar;
                    bl.Mobile = obj.Mobile;
                    list.Add(bl);
                }
                CMCMemberDAL dal = new CMCMemberDAL();
                dal.InsertBulk(list);
                draftMember.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
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
                                draftMember.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {

                                        CMCMemberModel bl = new CMCMemberModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.Code = x[0].Trim();
                                        bl.Name = x[1].Trim();
                                        bl.Gender = x[2].Trim();
                                        bl.FatherName = x[3].Trim();
                                        bl.DutyStation = x[4].Trim();
                                        bl.District = x[5].Trim();
                                        bl.SubRegion = x[6].Trim();
                                        if (!string.IsNullOrEmpty(x[7].Trim()))
                                            bl.DOB = Convert.ToDateTime(x[7].Trim());
                                        if (!string.IsNullOrEmpty(x[8].Trim()))
                                            bl.DOC = Convert.ToDateTime(x[8].Trim());
                                        bl.BankAc = x[9].Trim();
                                        bl.IFSC = x[10].Trim();
                                        bl.BankName = x[11].Trim();
                                        bl.PAN = x[12].Trim();
                                        bl.Aadhaar = x[13].Trim();
                                        bl.Mobile = x[14].Trim();
                                        draftMember.Add(bl);
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
        #endregion

        //C005
        #region -- Upload KYC Document ---

        public ActionResult UploadKYCIndex()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("C005"))
            {
            }
            else {
                return View("Security");
            }
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadKYC()
        {
            string messages = "";
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
                        if (_ext.ToLower() == ".pdf")
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Upload/CMCKYC/")) + fileName;
                            var path = _comPath;
                            hpf.SaveAs(path);
                            messages = messages == "" ? hpf.FileName : messages + "#~#" + hpf.FileName;
                        }
                    }
                }
            }
            return Json(Convert.ToString(messages), JsonRequestBehavior.AllowGet);
        }
        #endregion

        //C006
        #region --- Update Missing Member Details ----

        public ActionResult MissingDataIndex()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("C006"))
            {
            }
            else {
                return View("Security");
            }
            return View();
        }

        public ActionResult GenrateFile()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("BankAct", typeof(string)));
            dt.Columns.Add(new DataColumn("IFSC", typeof(string)));
            dt.Columns.Add(new DataColumn("Mobile", typeof(string)));
            dt.Columns.Add(new DataColumn("PAN", typeof(string)));
            dt.Columns.Add(new DataColumn("Aadhaar", typeof(string)));

            GridView gv = new GridView();
            CMCMemberRepository dal = new CMCMemberRepository();
            var data = dal.GetAll().Where(x=>
                (string.IsNullOrEmpty(x.BankAc)) ||
                (string.IsNullOrEmpty(x.Mobile)) ||
                (string.IsNullOrEmpty(x.PAN)) ||
                (string.IsNullOrEmpty(x.Aadhaar))).ToList();
            foreach (var item in data.OrderBy(x => x.Name))
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = item.Id;
                dr["Code"] = item.Code;
                dr["Name"] = item.Name;
                dr["BankAct"] = string.IsNullOrEmpty(item.BankAc) ? "*" : "";
                dr["IFSC"] = string.IsNullOrEmpty(item.IFSC) ? "*" : "";
                dr["Mobile"] = string.IsNullOrEmpty(item.Mobile) ? "*" : "";
                dr["PAN"] = string.IsNullOrEmpty(item.PAN) ? "*" : "";
                dr["Aadhaar"] = string.IsNullOrEmpty(item.Aadhaar) ? "*" : "";
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "MissingDetails.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadMissingDetails()
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
                                draftMember.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Replace('*',' ').Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {

                                        CMCMemberModel bl = new CMCMemberModel();
                                        bl.Id = Convert.ToInt32(x[0].Trim());
                                        bl.Code = x[1].Trim();
                                        bl.Name = x[2].Trim();
                                        bl.BankAc = x[3].Trim();
                                        bl.IFSC = x[4].Trim();
                                        bl.Mobile = x[5].Trim();
                                        bl.PAN = x[6].Trim();
                                        bl.Aadhaar = x[7].Trim();
                                        draftMissing.Add(bl);
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

        [HttpPost]
        public JsonResult BulkMissing(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<CMCMemberModel> model = new List<CMCMemberModel>();
                model = draftMissing.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult SaveMissing()
        {
            string message = "";
            if (draftMissing.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                CMCMemberDAL dal = new CMCMemberDAL();
                foreach (var obj in draftMissing)
                {
                    ICMCMember bl = dal.GetById(obj.Id);
                    if (!string.IsNullOrEmpty(obj.BankAc))
                    {
                        bl.BankAc = obj.BankAc;
                    }
                    if (!string.IsNullOrEmpty(obj.IFSC))
                    {
                        bl.IFSC = obj.IFSC;
                    }
                    if (!string.IsNullOrEmpty(obj.Mobile))
                    {
                        bl.Mobile = obj.Mobile;
                    }
                    if (!string.IsNullOrEmpty(obj.PAN))
                    {
                        bl.PAN = obj.PAN;
                    }
                    if (!string.IsNullOrEmpty(obj.Aadhaar))
                    {
                        bl.Aadhaar = obj.Aadhaar;
                    }
                    dal.InsertOrUpdate(bl);

                }
                draftMissing.Clear();
                message = "Record Updated Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }


        #endregion


    }
}