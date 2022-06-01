using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models.Master;

namespace UI.Controllers.Master
{
    public class CountryController : Controller
    {
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetNationality()
        {
            List<CountryModel> model = new List<CountryModel>();
            model.Add(new CountryModel { Id = 0, Nationality = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Nationality, Value = c.Nationality });
            try
            {
                CountryRepository dal = new CountryRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Nationality, Value = c.Nationality });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}