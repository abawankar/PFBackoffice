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
    public class CompanyController : Controller
    {
        static List<CompanyModel> draftCompany = new List<CompanyModel>();

        //R001
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R001"))
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
            else {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(int compid = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                CompanyRepository dal = new CompanyRepository();
                List<CompanyModel> model = new List<CompanyModel>();

                if(User.IsInRole("User"))
                {
                    model = dal.GetAll(User.Identity.Name).ToList();
                }
                else
                {
                    model = dal.GetAll().ToList();
                }
                
                if(compid != 0){
                    model = model.Where(x => x.Id == compid).ToList();
                }
                int count = model.Count;
                List<CompanyModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R002
        [HttpPost]
        public JsonResult Create(CompanyModel model)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R002"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do create new company!" });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                CompanyRepository dal = new CompanyRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R003
        [HttpPost]
        public JsonResult Update(CompanyModel model)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R003"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to update company info!" });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                CompanyRepository dal = new CompanyRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetCompanyOptions()
        {
            List<CompanyModel> model = new List<CompanyModel>();
            model.Add(new CompanyModel { Id = 0, Code = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Code, Value = c.Id });
            try
            {
                CompanyRepository dal = new CompanyRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Code, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetCompanyName()
        {
            List<CompanyModel> model = new List<CompanyModel>();
            model.Add(new CompanyModel { Id = 0, Code = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                CompanyRepository dal = new CompanyRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Name, Value = c.Name });
                return Json(new { Result = "OK", Options = list.OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #region ---- Bulk Upload Company ----

        //R004
        public ActionResult BulkUpload()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R004"))
            { return View(); }
            else {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult BulkList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                CompanyRepository dal = new CompanyRepository();
                List<CompanyModel> model = new List<CompanyModel>();
                model = draftCompany.ToList();
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
            if (draftCompany.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                IList<ICompany> list = new List<ICompany>();

                foreach (var obj in draftCompany)
                {
                    ICompany bl = new Company();
                    bl.Code = obj.Code;
                    bl.Name = obj.Name;
                    bl.PAN = obj.PAN;
                    bl.CIN = obj.CIN;
                    bl.EstablishmentCode = obj.EstablishmentCode;
                    bl.ESIRegistrationNumber = obj.ESIRegistrationNumber;
                    bl.StatutoryCode = new StatutoryCode { Id = obj.StatutoryCodeId };
                    list.Add(bl);
                }
                CompanyDAL dal = new CompanyDAL();
                dal.InsertBulk(list);
                draftCompany.Clear();
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
                                draftCompany.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        CompanyModel bl = new CompanyModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.Code = x[0].Trim();
                                        bl.Name = x[1].Trim();
                                        bl.PAN = x[2].Trim();
                                        bl.CIN = x[3].Trim();
                                        bl.EstablishmentCode = x[4].Trim();
                                        bl.ESIRegistrationNumber = x[5].Trim();
                                        bl.StatutoryCodeId = Convert.ToInt32(x[6].Trim());
                                        draftCompany.Add(bl);
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

        #region ------Digital Signatory -----

        [HttpPost]
        public JsonResult SignatoryList(int compid)
        {
            try
            {
                DigitalSignatureRepository dal = new DigitalSignatureRepository();
                List<DigitalSignatureModel> model = new List<DigitalSignatureModel>();
                model = dal.GetAll(compid).ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R005
        [HttpPost]
        public JsonResult CreateSign(DigitalSignatureModel model, int compid)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R005"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not authorize to do this action!" });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                DigitalSignatureRepository dal = new DigitalSignatureRepository();
                dal.Insert(model,compid);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R005
        [HttpPost]
        public JsonResult UpdateSign(DigitalSignatureModel model)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R005"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not authorize to do this action!" });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                DigitalSignatureRepository dal = new DigitalSignatureRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R006
        public ActionResult DigitalSignReport()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R006"))
            { return View(); }
            else {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult SignReportList()
        {
            try
            {
                CompanyRepository dal = new CompanyRepository();
                List<DigitalSignatureModel> model = new List<DigitalSignatureModel>();
                var data = dal.GetAll().Where(x => x.DigitalSign != null).ToList();
                foreach (var item in data)
                {
                    
                    foreach (var sign in item.DigitalSign)
                    {
                        DigitalSignatureModel bl = new DigitalSignatureModel();
                        bl.CompName = item.Name;
                        bl.Name = sign.Name;
                        bl.Validity = sign.Validity;
                        bl.Days = Math.Round((sign.Validity - DateTime.Now.AddDays(-1)).TotalDays);
                        model.Add(bl);
                    }
                }
                int count = model.Count;
                model = model.OrderBy(x => x.Validity).ToList();
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        public ActionResult DashboardView()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult DashboardList()
        {
            try
            {
                CompanyRepository dal = new CompanyRepository();
                List<DigitalSignatureModel> model = new List<DigitalSignatureModel>();
                var data = dal.GetAll().Where(x => x.DigitalSign != null).ToList();
                foreach (var item in data)
                {
                    
                    foreach (var sign in item.DigitalSign)
                    {
                        DigitalSignatureModel bl = new DigitalSignatureModel();
                        bl.CompName = item.Name;
                        bl.Name = sign.Name;
                        bl.Validity = sign.Validity;
                        bl.Days = Math.Round((sign.Validity - DateTime.Now.AddDays(-1)).TotalDays);
                        model.Add(bl);
                    }
                }
                model = model.Where(x => x.Days <= 30).ToList();

                int count = model.Count;
                model = model.OrderBy(x => x.Validity).ToList();
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        #endregion
    }
}