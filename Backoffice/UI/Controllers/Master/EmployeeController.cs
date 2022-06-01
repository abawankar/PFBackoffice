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
using UI.Models.Master;

namespace UI.Controllers.Master
{
    [HandleError]
    [Authorize]
    public class EmployeeController : Controller
    {
        static List<EmployeeModel> draftEmployee = new List<EmployeeModel>();
        static List<EmployeeModel> draftPMRPY = new List<EmployeeModel>();
        static List<EmployeeModel> ActiveMemberList = new List<EmployeeModel>();
        static List<EmployeeModel> memberList = new List<EmployeeModel>();

        // GET: Employee R007
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R007"))
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
                ViewBag.comp = new SelectList(dal.GetAll() , "Id", "Name");
            }

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "Name", Value = "2" },
                new SelectListItem { Text = "EmpCode", Value = "3" },
                new SelectListItem { Text = "UAN", Value = "1" },

            }, "Value", "Text");

            ViewBag.Status = new SelectList(new[] {
                new SelectListItem { Text = "Active", Value = "1" },
                new SelectListItem { Text = "Exited", Value = "2" },
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult List(int status =0,int compid = 0, int field = 0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();
                if (compid != 0)
                {
                    model = dal.GetByCompId(compid).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    switch (field)
                    {
                        case 1:
                            model = model.Where(x => x.UAN == search.Trim()).ToList();
                            break;
                        case 2:
                            model = model.Where(x => (x.Name + "").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                        case 3:
                            model = model.Where(x => x.EmpCode == search.Trim()).ToList();
                            break;
                    }
                }
                switch (status)
                {
                    case 1:
                        model = model.Where(x => x.Status == 1).ToList();
                        break;
                    case 2:
                        model = model.Where(x => x.Status == 2).ToList();
                        break;
                }

                int count = model.Count;
                memberList = model.ToList();
                model = model.OrderBy(x => x.Name).ToList();
                List<EmployeeModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R007
        [HttpPost]
        public JsonResult GetEmpByComp(int compid = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R007"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            }
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = dal.GetByCompId(compid).ToList();//dal.GetAll().ToList();
                int count = model.Count;
                List<EmployeeModel> Model = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(EmployeeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                EmployeeRepository dal = new EmployeeRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //ROO8
        [HttpPost]
        public JsonResult CreateEmp(EmployeeModel model, int compid)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R008"))
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

                EmployeeRepository dal = new EmployeeRepository();
                if(dal.GetByCompAndCode(compid,model.EmpCode)!= null)
                {
                    return Json(new { Result = "ERROR", Message = "Employee Code/name already exist! Please recheck or use new code!" });
                }

                dal.Insert(model, compid);
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R009
        [HttpPost]
        public JsonResult Update(EmployeeModel model)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R009"))
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
                EmployeeRepository dal = new EmployeeRepository();
                dal.Edit(model);
                EmployeeModel model1 = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model1 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(EmployeeModel model)
        {
            if (User.IsInRole("Admin"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            }
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                var data = dal.Delete(model.Id);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R010
        public ActionResult ExportMember()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R010"))
            {
            }
            else {
                return View("Security");
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Branch", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("UAN", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
            dt.Columns.Add(new DataColumn("ESIIP", typeof(string)));
            dt.Columns.Add(new DataColumn("Gender", typeof(string)));
            dt.Columns.Add(new DataColumn("DOB", typeof(string)));
            dt.Columns.Add(new DataColumn("DOJ", typeof(string)));
            dt.Columns.Add(new DataColumn("F/HName", typeof(string)));
            dt.Columns.Add(new DataColumn("F/H", typeof(string)));
            dt.Columns.Add(new DataColumn("MaritalStaus", typeof(string)));
            dt.Columns.Add(new DataColumn("Mobile", typeof(string)));
            dt.Columns.Add(new DataColumn("EmailId", typeof(string)));
            dt.Columns.Add(new DataColumn("CellingEPF", typeof(string)));
            dt.Columns.Add(new DataColumn("CellingEPS", typeof(string)));
            dt.Columns.Add(new DataColumn("Nationality", typeof(string)));
            dt.Columns.Add(new DataColumn("VPF", typeof(string)));
            dt.Columns.Add(new DataColumn("PFExempted", typeof(string)));
            dt.Columns.Add(new DataColumn("ESIxempted", typeof(string)));
            dt.Columns.Add(new DataColumn("Aadhaar", typeof(string)));
            dt.Columns.Add(new DataColumn("PAN", typeof(string)));
            dt.Columns.Add(new DataColumn("BankAcc", typeof(string)));
            dt.Columns.Add(new DataColumn("DOE", typeof(string)));

            GridView gv = new GridView();
            EmployeeRepository dal = new EmployeeRepository();
            foreach (var item in memberList.OrderBy(x=>x.Name))
            {
                DataRow dr = dt.NewRow();
                dr["Branch"] = item.Branch;
                dr["EmpCode"] = item.EmpCode;
                dr["Name"] = item.Name;
                dr["UAN"] = "'" + item.UAN;
                dr["MemberId"] = item.MemberId;
                dr["ESIIP"] = item.ESIIP;
                dr["Gender"] = item.Gender;
                dr["DOB"] = item.DOB != null ? Convert.ToDateTime(item.DOB).ToString("dd/MM/yyyy") : "";
                dr["DOJ"] = item.DOJ != null ? Convert.ToDateTime(item.DOJ).ToString("dd/MM/yyyy") : "";
                dr["DOE"] = item.DOE != null ? Convert.ToDateTime(item.DOE).ToString("dd/MM/yyyy") : "";
                dr["F/HName"] = item.ForHName;
                dr["F/H"] = item.ForHFlag;
                dr["MaritalStaus"] = item.MaritalStatus;
                dr["Mobile"] = item.Mobile;
                dr["EmailId"] = item.EmailId;
                dr["CellingEPF"] = item.CellingEPF;
                dr["CellingEPS"] = item.CellingEPS;
                dr["Nationality"] = item.Nationality;
                dr["VPF"] = item.VPF;
                dr["PFExempted"] = item.PFExempted;
                dr["ESIxempted"] = item.ESIExempted;
                dr["Aadhaar"] = item.Aadhaar;
                dr["PAN"] = item.PAN;
                dr["BankAcc"] = item.BankAcc;
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "MemberList.xls";
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

        // R011, R012 
        #region ---- KYC ----

        //R011
        public JsonResult GetKYC(int empid)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R011"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            }
            try
            {
                EmployeeKYCRepository dal = new EmployeeKYCRepository();
                List<EmployeeKYCModel> model = new List<EmployeeKYCModel>();
                model = dal.GetAll(empid).ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R012
        [HttpPost]
        public JsonResult AddKYC(EmployeeKYCModel model, int empid)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R012"))
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

                EmployeeKYCRepository dal = new EmployeeKYCRepository();
                dal.Insert(model, empid);
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //R012
        [HttpPost]
        public JsonResult UpdateKYC(EmployeeKYCModel model)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R012"))
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

                EmployeeKYCRepository dal = new EmployeeKYCRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        #endregion

        //R013
        #region ---- Bulk Upload Employee ----

        public ActionResult BulkUpload()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R013"))
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
            return View();
        }

        [HttpPost]
        public JsonResult BulkList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();
                model = draftEmployee.ToList();
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
            if (draftEmployee.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                IList<IEmployee> list = new List<IEmployee>();

                foreach (var obj in draftEmployee)
                {
                    IEmployee bl = new Employee();
                    bl.UAN = obj.UAN;
                    bl.ESIIP = obj.ESIIP;
                    bl.MemberId = obj.MemberId;
                    bl.EmpCode = obj.EmpCode;
                    bl.Name = obj.Name;
                    bl.Gender = obj.Gender;
                    bl.MaritalStatus = obj.MaritalStatus;
                    bl.Mobile = obj.Mobile;
                    bl.EmailId = obj.EmailId;
                    bl.Nationality = obj.Nationality;
                    bl.ForHName = obj.ForHName;
                    bl.ForHFlag = obj.ForHFlag;
                    bl.DOB = obj.DOB;
                    bl.DOJ = obj.DOJ;
                    bl.CellingEPF = obj.CellingEPF;
                    bl.CellingEPS = obj.CellingEPS;
                    bl.Company = new Company { Id = obj.CompId };
                    bl.Status = 1;
                    bl.VPF = obj.VPF;
                    bl.PFExempted = obj.PFExempted;
                    bl.ESIExempted = obj.ESIExempted;
                    bl.IPStatus = obj.ESIExempted == "N" ? 1 : 0;
                    bl.Branch = obj.Branch;
                    if(!string.IsNullOrEmpty(obj.Aadharno))
                    {
                        IEmployeeKYC kyc = new EmployeeKYC();
                        kyc.DoxType = "A";
                        kyc.DocumentNumber = obj.Aadharno;
                        kyc.NameonDox = obj.Name;
                        bl.KYC.Add(kyc);
                    }
                    list.Add(bl);
                }
                EmployeeDAL dal = new EmployeeDAL();
                dal.InsertBulk(list);
                draftEmployee.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(int compid)
        {
            draftEmployee.Clear();
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
                                draftEmployee.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        EmployeeModel bl = new EmployeeModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.CompId = compid;
                                        bl.Branch = x[0].Trim();
                                        bl.EmpCode = x[1].Trim();
                                        bl.Name = x[2].Trim();
                                        bl.UAN = x[3].Trim();
                                        bl.MemberId = x[4].Trim();
                                        bl.ESIIP = x[5].Trim();
                                        bl.Gender = x[6].Trim();
                                        if(!string.IsNullOrEmpty(x[7].Trim()))
                                            bl.DOB = Convert.ToDateTime(Convert.ToDateTime(x[7].Trim()).ToString("yyyy-MM-dd"));
                                        if (!string.IsNullOrEmpty(x[8].Trim()))
                                            bl.DOJ = Convert.ToDateTime(Convert.ToDateTime(x[8].Trim()).ToString("yyyy-MM-dd"));
                                        bl.ForHName = x[9].Trim();
                                        bl.ForHFlag = x[10].Trim();
                                        bl.MaritalStatus = x[11].Trim();
                                        bl.Mobile = x[12].Trim();
                                        bl.EmailId = x[13].Trim();
                                        bl.CellingEPF = x[14].Trim();
                                        bl.CellingEPS = x[15].Trim();
                                        bl.Nationality = x[16].Trim()==""?"INDIAN": x[16].Trim().ToUpper();
                                        bl.VPF = x[17].Trim() ==""?0:Convert.ToDouble(x[17].Trim());
                                        bl.PFExempted = x[18].Trim();
                                        bl.ESIExempted = x[19].Trim();
                                        bl.Aadharno = x[20].Trim();
                                        draftEmployee.Add(bl);
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

        [HttpPost]
        public JsonResult GetKYCdoxtype()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("A", "Aadhar Card");
            values.Add("P", "Pan Card");
            values.Add("B", "Bank A/c");
            values.Add("T", "Passport");
            values.Add("E", "Election Card");
            values.Add("R", "Ration Card");
            values.Add("D", "Driving License");
            values.Add("N", "National Population Register");
            values.Add("H", "AADHAAR Enrolment Number");
            try
            {
                var list = values.Select(c => new { DisplayText = c.Value, Value = c.Key });
                return Json(new { Result = "OK", Options = list});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetNationality()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("A", "Aadhar Card");
            values.Add("P", "Pan Card");
            values.Add("B", "Bank A/c");
            values.Add("T", "Passport");
            values.Add("E", "Election Card");
            values.Add("R", "Ration Card");
            values.Add("D", "Driving License");
            values.Add("N", "National Population Register");
            values.Add("H", "AADHAAR Enrolment Number");
            try
            {
                var list = values.Select(c => new { DisplayText = c.Value, Value = c.Key });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //R014
        #region ---- Active Member -----

        public ActionResult ActiveMember()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R014"))
            {
            }
            else {
                return View("Security");
            }

            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll(), "Id", "Name");

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "EmpCode", Value = "3" },
                new SelectListItem { Text = "Name", Value = "2" },
                new SelectListItem { Text = "UAN", Value = "1" },
            }, "Value", "Text");

            ViewBag.Status = new SelectList(new[] {
                new SelectListItem { Text = "Active", Value = "1" },
                new SelectListItem { Text = "Exited", Value = "2" },
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult ActiveList(int status =0,int compid=0,int field=0,string search=null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();
                if(compid != 0)
                {
                    model = dal.GetByCompId(compid).Where(x=>x.PFExempted=="N").ToList();
                    //model = dal.GetByCompId(compid).Where(x=>x.PFExempted == "N").ToList();
                }
                if (status!=0)
                {
                    model = model.Where(x => x.Status == status).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    switch (field)
                    {
                        case 1:
                            model = model.Where(x => x.UAN == search.Trim()).ToList();
                            break;
                        case 2:
                            model = model.Where(x => (x.Name + "").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                        case 3:
                            model = model.Where(x => x.EmpCode == search.Trim()).ToList();
                            break;
                    }
                }
                int count = model.Count;
                ActiveMemberList = model.ToList();
                List<EmployeeModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult ExportActiveMember()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SrNo", typeof(int)));
            dt.Columns.Add(new DataColumn("UAN", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Gender", typeof(string)));
            dt.Columns.Add(new DataColumn("DOB", typeof(string)));
            dt.Columns.Add(new DataColumn("DOJ", typeof(string)));
            dt.Columns.Add(new DataColumn("F/HName", typeof(string)));
            dt.Columns.Add(new DataColumn("F/H", typeof(string)));
            dt.Columns.Add(new DataColumn("MaritalStaus", typeof(string)));
            dt.Columns.Add(new DataColumn("Mobile", typeof(string)));
            dt.Columns.Add(new DataColumn("EmailId", typeof(string)));
            dt.Columns.Add(new DataColumn("Aadhaar", typeof(string)));
            dt.Columns.Add(new DataColumn("PAN", typeof(string)));
            dt.Columns.Add(new DataColumn("BankAcc", typeof(string)));

            GridView gv = new GridView();
            EmployeeRepository dal = new EmployeeRepository();
            int sr = 1;
            foreach (var item in ActiveMemberList)
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = sr;
                dr["UAN"] = item.UAN;
                dr["MemberId"] = item.MemberId;
                dr["EmpCode"] = item.EmpCode;
                dr["Name"] = item.Name;
                dr["Gender"] = item.Gender;
                dr["DOB"] = item.DOB != null?Convert.ToDateTime(item.DOB).ToString("dd/MM/yyyy"):"";
                dr["DOJ"] = item.DOJ != null ? Convert.ToDateTime(item.DOJ).ToString("dd/MM/yyyy") : "";
                dr["F/HName"] = item.ForHName;
                dr["F/H"] = item.ForHFlag;
                dr["MaritalStaus"] = item.MaritalStatus;
                dr["Mobile"] = item.Mobile;
                dr["EmailId"] =item.EmailId;
                dr["Aadhaar"] = item.Aadhaar;
                dr["PAN"] = item.PAN;
                dr["BankAcc"] = item.BankAcc;
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "ActiveMember.xls";
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

        #endregion

        public ActionResult BulkDelete()
        {
            if (User.IsInRole("Admin"))
            {
                CompanyRepository dal = new CompanyRepository();
                ViewBag.comp = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");
                return View();
            }
            else {
                return View("Security");
            }
        }
        
        public ActionResult ProcessDelete(int id)
        {
            if (User.IsInRole("Admin"))
            {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Branch", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("UAN", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
            dt.Columns.Add(new DataColumn("ESIIP", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Gender", typeof(string)));
            dt.Columns.Add(new DataColumn("DOB", typeof(string)));
            dt.Columns.Add(new DataColumn("DOJ", typeof(string)));
            dt.Columns.Add(new DataColumn("DOE", typeof(string)));
            dt.Columns.Add(new DataColumn("F/HName", typeof(string)));
            dt.Columns.Add(new DataColumn("F/H", typeof(string)));
            dt.Columns.Add(new DataColumn("MaritalStaus", typeof(string)));
            dt.Columns.Add(new DataColumn("Mobile", typeof(string)));
            dt.Columns.Add(new DataColumn("EmailId", typeof(string)));
            dt.Columns.Add(new DataColumn("CellingEPF", typeof(string)));
            dt.Columns.Add(new DataColumn("CellingEPS", typeof(string)));
            dt.Columns.Add(new DataColumn("Nationality", typeof(string)));
            dt.Columns.Add(new DataColumn("VPF", typeof(string)));
            dt.Columns.Add(new DataColumn("PFExempted", typeof(string)));
            dt.Columns.Add(new DataColumn("ESIxempted", typeof(string)));

            GridView gv = new GridView();
            EmployeeRepository dal = new EmployeeRepository();
            foreach (var item in dal.GetByCompId(id).OrderBy(x=>x.Name))
            {
                DataRow dr = dt.NewRow();
                dr["Branch"] = item.Branch;
                dr["EmpCode"] = item.EmpCode;
                dr["Name"] = item.Name;
                dr["UAN"] = "'" + item.UAN;
                dr["MemberId"] = item.MemberId;
                dr["ESIIP"] = item.ESIIP;
                dr["Gender"] = item.Gender;
                dr["DOB"] = item.DOB != null ? Convert.ToDateTime(item.DOB).ToString("dd/MM/yyyy") : "";
                dr["DOJ"] = item.DOJ != null ? Convert.ToDateTime(item.DOJ).ToString("dd/MM/yyyy") : "";
                dr["F/HName"] = item.ForHName;
                dr["F/H"] = item.ForHFlag;
                dr["MaritalStaus"] = item.MaritalStatus;
                dr["Mobile"] = item.Mobile;
                dr["EmailId"] = item.EmailId;
                dr["CellingEPF"] = item.CellingEPF;
                dr["CellingEPS"] = item.CellingEPS;
                dr["Nationality"] = item.Nationality;
                dr["VPF"] = item.VPF;
                dr["PFExempted"] = item.PFExempted;
                dr["ESIxempted"] = item.ESIExempted;
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "MemberList.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            var result = EmployeeDAL.DeleteBulkEmployee(id);
            TempData["Message"] = "Record successfully deleted and Backup file created";
            return RedirectToAction("BulkDelete");
            }
            else {
                return View("Security");
            }
        }

        //R015
        

        #region ----- Bulk Update Employee ----

        public ActionResult BulkUpdateEmp()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R013"))
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
            return View();
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFileForUpdate(int compid)
        {
            draftEmployee.Clear();
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
                                draftEmployee.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        EmployeeModel bl = new EmployeeModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.CompId = compid;
                                        bl.Branch = x[0].Trim();
                                        bl.EmpCode = x[1].Trim();
                                        bl.Name = x[2].Trim();
                                        bl.UAN = x[3].Trim();
                                        bl.MemberId = x[4].Trim();
                                        bl.ESIIP = x[5].Trim();
                                        draftEmployee.Add(bl);
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
        public JsonResult BulkUpdateList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();
                model = draftEmployee.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        public JsonResult UpdateBulk()
        {
            string message = "";
            if (draftEmployee.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                EmployeeDAL dal = new EmployeeDAL();
                foreach (var obj in draftEmployee)
                {
                    IEmployee bl = dal.GetByCompAndCode(obj.CompId, obj.EmpCode);
                    bl.UAN = obj.UAN;
                    bl.ESIIP = obj.ESIIP;
                    dal.InsertOrUpdate(bl);
                   
                }
                draftEmployee.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }


        #endregion

        public ActionResult GenrateTxtFile(string list)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            string[] emp = list.Split(',');
            List<EmployeeModel> model = new List<EmployeeModel>();

            for (int i = 1; i < emp.Length; i++)
            {
                int id = Convert.ToInt32(emp[i]);
                var data = ActiveMemberList.Where(x => x.Id == id).SingleOrDefault();
                tw.WriteLine(data.UAN + "#~##~#" +
                             Convert.ToDateTime(data.DOE).ToString("dd/MM/yyyy") + "#~#" + "5");
            }

            tw.Flush();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = ms;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "plain/txt", "BulkExit.txt");
        }

    }
}