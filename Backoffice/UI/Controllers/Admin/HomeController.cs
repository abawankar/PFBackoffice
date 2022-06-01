using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UI.Common;
using UI.Controllers.Admin;
using UI.Models;
using UI.Models.Master;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About";

            return View();
        }
        public ActionResult Management()
        {
            ViewBag.Message = "Management";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            //MailMessage email = new MailMessage(new MailAddress(ConfigurationManager.AppSettings["mail"], "(do not reply)"),
            //   new MailAddress(model.Email));
            //email.Subject = model.Subject;
            //email.Body = model.Message;
            //email.IsBodyHtml = true;
            //EmailSetting.SendEmail(email);

            TempData["message"] = "Thank you for contact. We will revert you very soon!";
            return View();
        }

        public ActionResult IpCard()
        {
            Session["CAPTCHA"] = GetRandomText();
            return View();
        }

        [HttpPost]
        public JsonResult List(string captch=null,string empcode = null,string dob=null,string mobile=null)
        {
            string clientCaptcha = captch;
            string serverCaptcha = Session["CAPTCHA"].ToString();

            if (!clientCaptcha.Equals(serverCaptcha))
            {
                return Json(new { Result = "Error", Message = "Sorry,Captch not match" });
            }

            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();
                if(!string.IsNullOrEmpty(empcode))
                {
                    model = dal.GetByESICode(empcode).ToList();
                    if(!string.IsNullOrEmpty(mobile))
                    {
                        model = model.Where(x => x.Mobile == mobile).ToList();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dob))
                        {
                            model = model.Where(x => Convert.ToDateTime(x.DOB) == Convert.ToDateTime(dob)).ToList();
                        }
                    }
                }

                int count = model.Count;
                if(count ==0 )
                {
                    if(!string.IsNullOrEmpty(empcode))
                        return Json(new { Result = "Error", Message = "Sorry, no record match, Please contact HR" });
                }
                
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        private string GetRandomText()
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz";
            Random r = new Random();
            for (int j = 0; j <= 5; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
        }

        public FileResult GetCaptchaImage()
        {
            string text = Session["CAPTCHA"].ToString();

            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            Font font = new Font("Arial", 15);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width + 40, (int)textSize.Height + 20);
            drawing = Graphics.FromImage(img);

            Color backColor = Color.SeaShell;
            Color textColor = Color.Red;
            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 20, 10);

            drawing.Save();

            font.Dispose();
            textBrush.Dispose();
            drawing.Dispose();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            img.Dispose();

            return File(ms.ToArray(), "image/png");
        }

    }
}