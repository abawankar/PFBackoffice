using DAL.Master;
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
    public class ESIEmployeeController : Controller
    {
        static List<EmployeeModel> memberList = new List<EmployeeModel>();
        // GET: ESIEmployee
        public ActionResult Index()
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

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "IP", Value = "1" },
                new SelectListItem { Text = "Name", Value = "2" },
                new SelectListItem { Text = "EmpCode", Value = "3" },
            }, "Value", "Text");


            return View();
        }

        [HttpPost]
        public JsonResult List(int compid = 0, int field = 0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();
                if (compid != 0)
                {
                    model = dal.GetESIByCompId(compid).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    switch (field)
                    {
                        case 1:
                            model = model.Where(x => x.ESIIP == search.Trim()).ToList();
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

        [HttpPost]
        public JsonResult Update(EmployeeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                EmployeeRepository dal = new EmployeeRepository();
                dal.EditESI(model);
                EmployeeModel model1 = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model1 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        public ActionResult ExportMember()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("IP", typeof(string)));
            dt.Columns.Add(new DataColumn("DOB", typeof(string)));
            dt.Columns.Add(new DataColumn("DOJ", typeof(string)));

            GridView gv = new GridView();
            foreach (var item in memberList.OrderBy(x => x.Name))
            {
                DataRow dr = dt.NewRow();
                dr["EmpCode"] = item.EmpCode;
                dr["Name"] = item.Name;
                dr["IP"] = "'" + item.ESIIP;
                dr["DOB"] = item.DOB != null ? Convert.ToDateTime(item.DOB).ToString("dd/MM/yyyy") : "";
                dr["DOJ"] = item.DOJ != null ? Convert.ToDateTime(item.DOJ).ToString("dd/MM/yyyy") : "";
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "ESIMemberList.xls";
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
    }
}