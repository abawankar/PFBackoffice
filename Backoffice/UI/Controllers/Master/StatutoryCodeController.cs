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
    public class StatutoryCodeController : Controller
    {
        // GET: StatutoryCode
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                StatutoryCodeRepository dal = new StatutoryCodeRepository();
                List<StatutoryCodeModel> model = new List<StatutoryCodeModel>();
                if (string.IsNullOrEmpty(name))
                {
                    model = dal.GetAll().ToList();
                }
                else
                {
                    model = dal.GetAll().ToList();
                }
                int count = model.Count;
                List<StatutoryCodeModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(StatutoryCodeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                StatutoryCodeRepository dal = new StatutoryCodeRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(StatutoryCodeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                StatutoryCodeRepository dal = new StatutoryCodeRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetStatutoryCodeOptions()
        {
            List<StatutoryCodeModel> model = new List<StatutoryCodeModel>();
            model.Add(new StatutoryCodeModel { Id = 0, Code = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Code, Value = c.Id });
            try
            {
                StatutoryCodeRepository dal = new StatutoryCodeRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Code, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}