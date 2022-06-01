using DAL.Master;
using DAL.Transaction;
using Domain.Implementation;
using Domain.Implementation.Transaction;
using Domain.Interface;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models.Master;
using UI.Models.Transaction;

namespace UI.Controllers.Transaction
{
    
    [HandleError]
    [Authorize]
    public class MonthlyReturnDraftController : Controller
    {
        enum empPF {Branch,EmpCode,EmpName,UAN,GrossWages,Wages,NCPDays,ESIWages,DOL };

        static List<MonthlyReturnDraftModel> drafContr = new List<MonthlyReturnDraftModel>();
        // GET: MonthlyReturnDraft
        //R015
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R015"))
            {
            }
            else {
                return View("Security");
            }

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

            ViewBag.ECRType = new SelectList(new[] {
                new SelectListItem { Text = "ECR", Value = "E" },
                new SelectListItem { Text = "Arrear", Value = "A" },
                new SelectListItem { Text = "Suppliment", Value = "S" },
            }, "Value", "Text");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(1, Convert.ToInt32(ConfigurationManager.AppSettings["pfyears"]))
                .Select(i => new SelectListItem {Value= now.AddMonths(-i).ToString("MM/yyyy"),Text= now.AddMonths(-i).ToString("MM/yyyy")});
            ViewBag.Months = new SelectList(data, "Value", "Text");
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(int compid, string ECRType, string months)
        {
            drafContr.Clear();
            var dirPath = Server.MapPath(Url.Content("~/Upload/Temp/"));
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dirPath);
            foreach (FileInfo fi in directory.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }
            CompanyRepository dal = new CompanyRepository();
            CompanyModel comp = dal.GetById(compid);

            EmployeeRepository empdal = new EmployeeRepository();

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
                                drafContr.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {

                                        MonthlyReturnDraftModel bl = new MonthlyReturnDraftModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.Branch = x[(int)empPF.Branch].Trim();
                                        bl.EmpCode = x[(int)empPF.EmpCode].Trim();
                                        bl.UAN = x[(int)empPF.UAN].Trim()=="0"?"": x[(int)empPF.UAN].Trim();
                                        bl.GrossWages = Convert.ToDouble(x[(int)empPF.GrossWages].Trim());
                                        bl.EPFWages = Convert.ToDouble(x[(int)empPF.Wages].Trim());
                                        bl.NCPDays = x[(int)empPF.NCPDays].Trim()!=""?Convert.ToInt32(x[(int)empPF.NCPDays].Trim()):0;
                                        bl.EDLIWages = bl.EPFWages > comp.StatutoryCode.PFLimit ? comp.StatutoryCode.PFLimit : bl.EPFWages;
                                        bl.EPSWages = Convert.ToDouble(x[(int)empPF.Wages].Trim());
                                        bl.ESIWages = string.IsNullOrEmpty(x[(int)empPF.ESIWages].Trim())?0:Convert.ToDouble(x[(int)empPF.ESIWages].Trim());
                                        EmployeeModel emp = empdal.GetByCompAndCode(comp.Id,bl.EmpCode);
                                        bl.IsPF = emp.PFExempted;
                                        if(emp.CellingEPF == "Y")
                                        {
                                            if (bl.EPFWages > comp.StatutoryCode.PFLimit)
                                                bl.EPFWages = comp.StatutoryCode.PFLimit;
                                        }
                                        if(emp.CellingEPS == "Y")
                                        {
                                            if (bl.EPFWages > comp.StatutoryCode.EPSLimit)
                                                bl.EPSWages = comp.StatutoryCode.EPSLimit;
                                        }
                                        if(emp.Nationality == "International")
                                        {
                                            bl.EPSWages = bl.EPFWages;
                                            bl.EDLIWages = bl.EPFWages;
                                        }
                                        if (emp.CellingEPS == "Z")
                                        {
                                            bl.EPSWages = 0;
                                        }
                                        
                                        bl.EECont = Math.Round((bl.EPFWages * comp.StatutoryCode.PFPercent) / 100);
                                        bl.EPSCont = Math.Round((bl.EPSWages * comp.StatutoryCode.Act10) / 100);
                                        bl.ERCont = bl.EECont - bl.EPSCont;
                                        bl.Name = emp.Name;
                                        if(!string.IsNullOrEmpty(x[(int)empPF.DOL].Trim()))
                                        {
                                            bl.DOL = Convert.ToDateTime(Convert.ToDateTime(x[(int)empPF.DOL].Trim()).ToString("yyyy-MM-dd"));
                                        }
                                        bl.ContType = ECRType;
                                        bl.MonthYear = months;
                                        bl.CompId = compid;

                                        if(emp.VPF !=0)
                                        {
                                            bl.EECont = bl.EECont + emp.VPF;
                                        }

                                        if (emp.PFExempted == "Y")
                                        {
                                            bl.EDLIWages = 0;
                                            bl.EPFWages = Convert.ToDouble(x[(int)empPF.Wages].Trim());
                                            bl.EPSWages = 0;
                                            bl.EECont = 0;
                                            bl.EPSCont = 0;
                                            bl.ERCont = 0;
                                        }

                                        drafContr.Add(bl);
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
            return Json(Convert.ToString(error == "" ? "No Error Found in Record" : "Error Found in Record" + error), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BulkList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<MonthlyReturnDraftModel> model = new List<MonthlyReturnDraftModel>();
                model = drafContr.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult FooterData()
        {
            try
            {
                List<FooterModel> model = new List<FooterModel>();
                var data = drafContr.ToList();
                FooterModel bl = new FooterModel();
                bl.ExemptedEmp = data.Where(x => x.IsPF =="Y").Count();
                bl.EPFEmp = data.Count() - bl.ExemptedEmp;
                bl.EPSEmp = bl.EPFEmp - data.Where(x => x.EPSWages == 0).Count()+bl.ExemptedEmp;
                bl.ExemptedGrossWages = data.Where(x => x.IsPF == "Y").Sum(x => x.GrossWages);
                bl.ExemptedWages = data.Where(x => x.IsPF == "Y").Sum(x => x.EPFWages);
                bl.EPFGrossWages = data.Sum(x => x.GrossWages)-bl.ExemptedGrossWages;
                bl.EPFWages = data.Sum(x => x.EPFWages)-bl.ExemptedWages;
                bl.EPSWages = data.Sum(x => x.EPSWages);
                bl.EDLIWages = data.Sum(x => x.EDLIWages);
                bl.EECont = data.Sum(x => x.EECont);
                bl.EPSCont = data.Sum(x => x.EPSCont);
                bl.ERCont = data.Sum(x => x.ERCont);
                bl.NCPDays = data.Sum(x => x.NCPDays);
                bl.ESIWages = data.Sum(x => x.ESIWages);
                model.Add(bl);
                return Json(new { Result = "OK", Records = model});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult LastMonthReturn(int compid, string month, string ecr)
        {
            string message = "";
            var now = DateTimeOffset.Now;
            string monthyear = now.AddMonths(-2).ToString("MM/yyyy");
            MonthlyReturnMasterRepository dal = new MonthlyReturnMasterRepository();
            var data = dal.GetById(monthyear, compid, ecr);
            int sr = 1;
            if (data != null)
            {
                foreach (var item in data.MonthlyReturn)
                {
                    MonthlyReturnDraftModel bl = new MonthlyReturnDraftModel();
                    EmployeeRepository empdal = new EmployeeRepository();
                    bl.Id = sr;
                    sr++;
                    bl.UAN = item.UAN;
                    bl.GrossWages = item.GrossWages;
                    bl.EPFWages = item.EPFWages;
                    bl.NCPDays = item.NCPDays;
                    bl.EDLIWages = bl.EPFWages > data.Company.StatutoryCode.PFLimit ? data.Company.StatutoryCode.PFLimit : bl.EPFWages;
                    bl.EPSWages = item.EPFWages;
                    bl.ESIWages = item.ESIWages;
                    EmployeeModel emp = empdal.GetByCompAndCode(data.Company.Id, bl.EmpCode);
                    if (emp.CellingEPF == "Y")
                    {
                        if (bl.EPFWages > data.Company.StatutoryCode.PFLimit)
                            bl.EPFWages = data.Company.StatutoryCode.PFLimit;
                    }
                    if (emp.CellingEPS == "Y")
                    {
                        if (bl.EPFWages > data.Company.StatutoryCode.EPSLimit)
                            bl.EPSWages = data.Company.StatutoryCode.EPSLimit;
                    }
                    if (emp.CellingEPS == "Z")
                    {
                        bl.EPSWages = 0;
                    }
                    bl.EECont = Math.Round((bl.EPFWages * data.Company.StatutoryCode.PFPercent) / 100);
                    bl.EPSCont = Math.Round((bl.EPSWages * data.Company.StatutoryCode.Act10) / 100);
                    bl.ERCont = bl.EECont - bl.EPSCont;
                    bl.Name = emp.Name;
                    bl.ContType = ecr;
                    bl.MonthYear = month;
                    bl.CompId = compid;
                    drafContr.Add(bl);
                }
                message = "Success: Return taken from last month successfully!";
            }
            else
            {
                message = "Error: No Return Found!";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDraft(MonthlyReturnDraftModel model)
        {
            try
            {
                var data = drafContr.Where(x => x.EmpCode == model.EmpCode).SingleOrDefault();
                data.GrossWages = model.GrossWages;
                data.EPFWages = model.EPFWages;
                data.EPSWages = model.EPSWages;
                data.EDLIWages = model.EDLIWages;
                data.EECont = model.EECont;
                data.EPSCont = model.EPSCont;
                data.ERCont = model.ERCont;
                data.NCPDays = model.NCPDays;
                data.ESIWages = model.ESIWages;
                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult SaveDraft()
        {
            string message = "";
            IList<IMonthlyReturnDraft> list = new List<IMonthlyReturnDraft>();
            EmployeeDAL empdal = new EmployeeDAL();
            foreach (var obj in drafContr)
            {
                IMonthlyReturnDraft bl = new MonthlyReturnDraft();
                bl.Branch = obj.Branch;
                bl.ContType = obj.ContType;
                bl.MonthYear = obj.MonthYear;
                bl.Name = obj.Name;
                bl.EmpCode = obj.EmpCode;
                bl.UAN = obj.UAN;
                bl.GrossWages = obj.GrossWages;
                bl.EPFWages = obj.EPFWages;
                bl.EPSWages = obj.EPSWages;
                bl.EDLIWages = obj.EDLIWages;
                bl.EECont = obj.EECont;
                bl.EPSCont = obj.EPSCont;
                bl.ERCont = obj.ERCont;
                bl.NCPDays = obj.NCPDays;
                bl.ESIWages = obj.ESIWages;
                bl.Company = new Company { Id = obj.CompId };
                bl.IsPF = obj.IsPF;
                list.Add(bl);

                if(obj.DOL != null){
                    IEmployee emp = empdal.GetByCompAndCode(obj.CompId, obj.EmpCode);
                    emp.DOE = obj.DOL;
                    emp.Status = 2;
                    empdal.InsertOrUpdate(emp);
                }
            }
            MonthlyReturnDraftDAL dal = new MonthlyReturnDraftDAL();
            dal.InsertBulk(list);
            drafContr.Clear();
            message = "Draft Record Saved Successfully";

            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}