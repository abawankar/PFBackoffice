using DAL.Master;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UI.Models.Master;

namespace UI.Controllers
{
    [HandleError]
    [Authorize]
    public class ExportController : Controller
    {
        static List<EmployeeKYCModel> kyclist = new List<EmployeeKYCModel>();
        static List<EmployeeModel> emplist = new List<EmployeeModel>();
        // GET: Import
        public ActionResult ExportKYC()
        {
            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll(), "Id", "Name");
            return View();
        }

        public ActionResult ProcessKYC(string list)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            foreach (var item in kyclist.Where(x=>list.Contains(x.Id.ToString())).OrderBy(x => x.UAN))
            {
                tw.WriteLine(item.UAN + "#~#" +
                             item.DoxType + "#~#" +
                             item.DocumentNumber + "#~#" +
                             item.NameonDox + "#~#" +
                             item.Other);
            }
            tw.Flush();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = ms;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "plain/txt", "kyc.txt");
        }

        [HttpPost]
        public JsonResult KycList(int compid=0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeDAL dal = new EmployeeDAL();
                var data = from m in dal.GetByCompId(compid)
                            from k in m.KYC.OrderByDescending(x=>x.Id)
                            select new EmployeeKYCModel
                            {
                                Id = k.Id,
                                UAN = m.UAN,
                                DoxType = k.DoxType,
                                DocumentNumber = k.DocumentNumber,
                                NameonDox = k.NameonDox,
                                Other = k.Other
                            };
                kyclist = data.ToList();                    
                int count = data.Count();
                List<EmployeeKYCModel> Model1 = data.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult EmpList(int compid = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                var data = dal.GetByCompId(compid).ToList().OrderByDescending(x=>x.Id);
                emplist = data.ToList();
                int count = data.Count();
                List<EmployeeModel> Model1 = data.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        public ActionResult ExportEmployee()
        {
            CompanyRepository dal = new CompanyRepository();
            ViewBag.comp = new SelectList(dal.GetAll(), "Id", "Name");
            return View();
        }

        public ActionResult ProcessEmp(string list)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            string[] emp = list.Split(',');
            List<EmployeeModel> model = new List<EmployeeModel>();
            EmployeeRepository dal = new EmployeeRepository();
            for (int i = 1; i < emp.Length; i++)
            {
                int id = Convert.ToInt32(emp[i]);
                EmployeeModel bl = dal.GetById(id);
                model.Add(bl);
            }
            foreach (var item in model)
            {
                tw.WriteLine(item.UAN + "#~##~#" +
                             item.Name + "#~#" +
                             Convert.ToDateTime(item.DOB).ToString("dd/MM/yyyy") + "#~#" +
                             Convert.ToDateTime(item.DOJ).ToString("dd/MM/yyyy") + "#~#" +
                             item.Gender + "#~#" +
                             item.ForHName + "#~#" +
                             item.ForHFlag + "#~#" +
                             item.Mobile + "#~#" +
                             item.EmailId + "#~#" +
                             item.Nationality + "#~##~##~#" +
                             item.MaritalStatus + "#~#N#~##~##~##~##~#N#~##~##~##~#" +
                             "#~##~##~##~##~#" + 
                             item.Aadhaar + "#~#" +
                             item.Name);
            }
            tw.Flush();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = ms;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "plain/txt", "Employee.txt");
        }
    }
}