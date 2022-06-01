using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers.Admin
{
    //[HandleError]
    [Authorize]
    public class DBController : Controller
    {
        // GET: DB
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateDataBase()
        {
            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            CreateDB.CreateDatabase(strCon);
            ViewData["message"] = "Database Created Successfully";
            return View();
        }
    }
}