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
    public class UserRightsController : Controller
    {

        // GET: EmpRights
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return View();
            else
                return View("Security");
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                UserRightsRepository dal = new UserRightsRepository();
                List<UserRightsModel> model = new List<UserRightsModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().Where(x => (x.Operation + " " + x.Code).ToLower().Contains(name.ToLower())).ToList();
                }
                int count = model.Count;
                model = model.OrderByDescending(x => x.Code).ToList();
                List<UserRightsModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(UserRightsModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                UserRightsRepository dal = new UserRightsRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(UserRightsModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                UserRightsRepository dal = new UserRightsRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
    }
}