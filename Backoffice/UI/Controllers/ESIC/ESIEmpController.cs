using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using UI.Models.Master;

namespace UI.Controllers.ESIC
{
    [Authorize]
    public class ESIEmpController : Controller
    {
        // GET: ESIEmp
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                ESIEmpRepository dal = new ESIEmpRepository();
                List<ESIEmpModel> model = new List<ESIEmpModel>();
                model = dal.GetAll().Where(x=>x.Status == 0).ToList();
                int count = model.Count;
                model = model.OrderByDescending(x => x.Name).ToList();
                List<ESIEmpModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(ESIEmpModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                ESIEmpRepository dal = new ESIEmpRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CreateEsiEmp(ESIEmpModel objemp)
        {
            try
            {
                ESIEmpRepository dal = new ESIEmpRepository();
                dal.Insert(objemp);
                return Json(new { Result = "OK", Record = objemp });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Update(ESIEmpModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                ESIEmpRepository dal = new ESIEmpRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult CreateESIEmp()
        {

            ViewBag.Gender = new SelectList(new[] {
                new SelectListItem { Text = "Male", Value = "Male" },
                new SelectListItem { Text = "Female", Value = "Female" },
                new SelectListItem { Text = "Transgender", Value = "Transgender" },
            }, "Value", "Text");

            ViewBag.Relation = new SelectList(new[] {
                new SelectListItem { Text = "Father", Value = "Father" },
                new SelectListItem { Text = "Husband", Value = "Husband" },
            }, "Value", "Text");
            return View();
        }

        [HttpGet]
        public JsonResult GetESIEmp()      //For Updating changes   
        {
            try
            {
                ESIEmpRepository dal = new ESIEmpRepository();
                var data = dal.GetAll().Where(x=>x.Status==0).ToList();
                string message = data.Count().ToString();
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BulkESIEmpUpload()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadBulkESIEmp()
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
                        var _comPath = Server.MapPath(Url.Content("~/Upload/Temp/")) + fileName;
                        var path = _comPath;
                        hpf.SaveAs(path);

                        MailMessage msg = new MailMessage();
                        msg.To.Add(new MailAddress("Savvybackoffice@gmail.com"));
                        msg.To.Add(new MailAddress("pp@savvycorporate.in"));
                        msg.To.Add(new MailAddress("shweta.savvy@gmail.com"));
                        msg.Subject = "New ESI Employee Registration-" + fileName;
                        msg.Bcc.Add("abawankar@gmail.com");
                        msg.Body = "<p>Hi</p> <p>Please find attached new ESI Employee Registration file</p>" +
                            "<p> Regards /Portal Admin</p>";
                        msg.Attachments.Add(new Attachment(path));
                        msg.IsBodyHtml = true;
                        Common.EmailSetting.SendEmail(msg);
                    }
                }
            }
            return Json(Convert.ToString(messages), JsonRequestBehavior.AllowGet);
        }
    }
}