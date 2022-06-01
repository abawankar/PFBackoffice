using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models.Master;

namespace UI.Controllers.Master
{
    [HandleError]
    [Authorize]
    public class AppsUserController : Controller
    {
        // View Employee
        public ActionResult Index()
        {
            ViewBag.status = new SelectList(new[] {
                new SelectListItem{Value="1", Text="Active"},
                new SelectListItem{Value="2", Text="InActive"},
                }, "Value", "Text");

            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R004"))
            { return View(); }
            else {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult List(string name = null, int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                AppsUserRepository dal = new AppsUserRepository();
                List<AppsUserModel> model = new List<AppsUserModel>();

                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().Where(x => (x.Name + " " + x.Emailid + " " + x.PAN + " " + x.MobileNo).ToLower().Contains(name.ToLower())).ToList();
                }
                if (status != 0)
                {
                    model = model.Where(x => x.Status == status).ToList();
                }
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<AppsUserModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(AppsUserModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R004"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    AppsUserRepository dal = new AppsUserRepository();
                    dal.Insert(model);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(AppsUserModel model)
        {
            try
            {
                if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R004"))
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                    }
                    AppsUserRepository dal = new AppsUserRepository();
                    dal.Edit(model);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetEmployeeOptions()
        {
            try
            {
                AppsUserRepository dal = new AppsUserRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GetEmployee()
        {
            try
            {
                AppsUserRepository dal = new AppsUserRepository();
                var list = from m in dal.GetAll().OrderBy(x => x.Name)
                           select new { Id = m.Id, Name = m.Name };
                return Json(list, "employee", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult AddEmpRights(string id, string list)
        {
            AppsUserRepository.AddEmpRights(Convert.ToInt32(id), list);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetEmpRights(int id = 0)
        {
            AppsUserRepository dal = new AppsUserRepository();
            try
            {
                var data = dal.GetById(id).EmpRight.ToList();

                return Json(new { Result = "OK", Records = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ListOfEmpRights(int id = 0,string menu=null, int jtStartIndex = 0, int jtPageSize = 0)
        {
            AppsUserRepository dal = new AppsUserRepository();
            UserRightsRepository cdal = new UserRightsRepository();
            try
            {
                var empRights = dal.GetById(id).EmpRight;
                var rightId = from c in empRights
                              select c.Id;

                var list = cdal.GetAll().Where(c => !rightId.Contains(c.Id)).ToList();
                if(!string.IsNullOrEmpty(menu))
                {
                    list = list.Where(x => x.MnuName == menu).ToList();
                }
                int count = list.Count;
                var Model1 = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetEmpRightsOptions(int id = 0)
        {
            AppsUserRepository dal = new AppsUserRepository();
            UserRightsRepository cdal = new UserRightsRepository();
            try
            {
                if (id == 0)
                {
                    var curr = cdal.GetAll()
                               .Select(c => new { DisplayText = c.Description, Value = c.Id });
                    return Json(new { Result = "OK", Options = curr });
                }
                else
                {
                    var curr = dal.GetById(id).EmpRight
                               .Select(c => new { DisplayText = c.Description, Value = c.Id });
                    return Json(new { Result = "OK", Options = curr });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //View Employee Rights Page, Only for Admin
        public ActionResult UserRightsList(int id)
        {
            if (User.IsInRole("Admin"))
            {
                AppsUserRepository dal = new AppsUserRepository();
                ViewBag.Employee = new SelectList(dal.GetAll().Where(x => x.Id == id && x.Role !="Admin" ), "Id", "Name");

                ViewBag.menu = new SelectList(new[] {
                new SelectListItem{Value="MASTER", Text="MASTER"},
                new SelectListItem{Value="REPORT", Text="REPORT"},
                new SelectListItem{Value="PF TRANSACTION", Text="PF TRANSACTION"},
                new SelectListItem{Value="ESI TRANSACTION", Text="ESI TRANSACTION"},
                new SelectListItem{Value="CMC TRANSACTION", Text="CMC TRANSACTION"},
                }, "Value", "Text");

                return PartialView();
            }
            else
            {
                return View("Security");
            }
        }

        public JsonResult DeleteRights(UserRightsModel model, int empId)
        {
            try
            {

                if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("R009"))
                {
                    AppsUserRepository.DeleteRights(empId, model.Id);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        public ActionResult UserCompanyList(int id)
        {

            if (User.IsInRole("Admin"))
            {
                AppsUserRepository dal = new AppsUserRepository();
                ViewBag.Employee = new SelectList(dal.GetAll().Where(x => x.Id == id && x.Role != "Admin"), "Id", "Name");
                return PartialView();
            }
            else
            {
                return View("Security");
            }
        }

        [HttpPost]
        public JsonResult ListOfCompany(int id = 0, string menu = null, int jtStartIndex = 0, int jtPageSize = 0)
        {
            AppsUserRepository dal = new AppsUserRepository();
            CompanyRepository cdal = new CompanyRepository();

            try
            {
                //var empRights = dal.GetById(id).AssignCompany;
                var compId = from c in dal.GetAll().SelectMany(x=>x.AssignCompany)
                              select c.Id;

                var list = cdal.GetAll().Where(c => !compId.Contains(c.Id)).ToList();
                int count = list.Count;
                var Model1 = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult AddCompany(string id, string list)
        {
            AppsUserRepository.AddAssignComp(Convert.ToInt32(id), list);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetCompany(int id = 0)
        {
            AppsUserRepository dal = new AppsUserRepository();
            try
            {
                var data = dal.GetById(id).AssignCompany.ToList();

                return Json(new { Result = "OK", Records = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult DeleteComp(CompanyModel model, int empId)
        {
            try
            {

                if (User.IsInRole("Admin"))
                {
                    AppsUserRepository.DeleteCompany(empId, model.Id);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
    }
}