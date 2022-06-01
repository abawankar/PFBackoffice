using DAL.Master;
using DAL.Transaction;
using Domain.Implementation;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Models.CMC;
using UI.Models.Master;

namespace UI.Controllers.CMC
{
    public class CMCTrnMasterController : Controller
    {
        static List<CMCTransactionModel> contlist = new List<CMCTransactionModel>();

        //CT001
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("CT001"))
            {
            }
            else {
                return View("Security");
            }
            return View();
        }

        //CT002
        public ActionResult PrepareMonthlyData()
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("CT002"))
            {
            }
            else {
                return View("Security");
            }

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(1, 6)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.Months = new SelectList(data, "Value", "Text");

            return View();
        }

        [HttpPost]
        public JsonResult List(int id = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<CMCTrnMasterModel> model = new List<CMCTrnMasterModel>();
                CMCTrnMasterRepository dal = new CMCTrnMasterRepository();
                model = dal.GetAll().ToList();
                int count = model.Count;
                List<CMCTrnMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //CT003
        public JsonResult FullDetails(int id = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("CT003"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            }

            List<CMCTransactionModel> model = new List<CMCTransactionModel>();
            CMCTrnMasterRepository dal = new CMCTrnMasterRepository();
            model = dal.GetFullDetails(id).ToList();
            int count = model.Count;
            List<CMCTransactionModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
            return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile(string months)
        {
            contlist.Clear();
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
                                contlist.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        CMCTransactionModel bl = new CMCTransactionModel();
                                        bl.Id = sr;
                                        sr++;
                                        var member = CMCMemberDAL.GetMember(x[0].Trim());
                                        bl.MemberId = member.Id;
                                        bl.Name = member.Name;
                                        bl.Code = member.Code;
                                        bl.Account = member.BankAc;
                                        bl.Honorarium = Convert.ToDouble(x[1].Trim());
                                        bl.Contingency = Convert.ToDouble(x[2].Trim());
                                        bl.TAOPE = Convert.ToDouble(x[3].Trim());
                                        bl.Review = Convert.ToDouble(x[4].Trim());
                                        bl.Meeting = Convert.ToDouble(x[5].Trim());
                                        bl.Claim = Convert.ToDouble(x[6].Trim());
                                        bl.Remarks = x[7].Trim();
                                        bl.Month = months;
                                        contlist.Add(bl);
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
        public JsonResult DraftContList(string name = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<CMCTransactionModel> model = new List<CMCTransactionModel>();
                model = contlist.ToList();
                int count = model.Count;
                List<CMCTransactionModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CMCFooterData()
        {
            try
            {
                List<CMCFooterModel> model = new List<CMCFooterModel>();
                var data = contlist.ToList();
                CMCFooterModel bl = new CMCFooterModel();
                bl.Month = data.Take(1).Select(x => x.Month).SingleOrDefault();
                bl.TotalMember = data.Count();
                bl.NoBank = data.Where(x=>x.Account==null).Count();
                bl.Honorarium = data.Sum(x => x.Honorarium);
                bl.Contingency = data.Sum(x => x.Contingency);
                bl.TAOPE = data.Sum(x => x.TAOPE);
                bl.TotalPayable = bl.Honorarium + bl.Contingency + bl.TAOPE;
                bl.Review = data.Sum(x => x.Review);
                bl.Meeting = data.Sum(x => x.Meeting);
                bl.Claim = data.Sum(x => x.Claim);
                bl.NetPayable = bl.TotalPayable + bl.Review + bl.Meeting + bl.Claim;
                model.Add(bl);
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //CT004
        public JsonResult Process()
        {
            string message = "";
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("CT004"))
            { }
            else {
                return Json(new { Result = "Error", Message = "You are not Authorize to do this action!" });
            }

            if(contlist.Where(x=>x.Account == null).Count()>0)
            {
                message = "Please update bank account of some member first!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }

            
            IList<ICMCTransaction> list = new List<ICMCTransaction>();
            string month = "";
            foreach (var obj in contlist)
            {
                ICMCTransaction bl = new CMCTransaction();
                bl.Honorarium = obj.Honorarium;
                bl.Contingency = obj.Contingency;
                bl.TAOPE = obj.TAOPE;
                bl.Review = obj.Review;
                bl.Meeting = obj.Meeting;
                bl.Claim = obj.Claim;
                bl.Remarks = obj.Remarks;
                bl.Member = new CMCMember { Id = obj.MemberId };
                month = obj.Month;
                list.Add(bl);
            }

            CMCTrnMasterRepository chk = new CMCTrnMasterRepository();
            var result = chk.GetAll().Where(x => x.MonthYear== month);
            if (result.Count() != 0)
            {
                message = "Contributon Already process for this month!";
                contlist.Clear();
                return Json(message, JsonRequestBehavior.AllowGet);
            }


            CMCTrnMasterDAL process = new CMCTrnMasterDAL();
            ICMCTrnMaster trn = new CMCTrnMaster();
            trn.MonthYear = month;
            trn.Trn = list;
            process.InsertOrUpdate(trn);
            contlist.Clear();
            message = "Saved Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //CT005
        public ActionResult DownloadHonorarium(int id = 0,string month=null)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("CT005"))
            {
            }
            else {
                return View("Security");
            }

            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            CMCTrnMasterRepository dal = new CMCTrnMasterRepository();
            var data = dal.GetFullDetails(id);
            string cmcbank = ConfigurationManager.AppSettings["cmcbank"];
            string trn = ConfigurationManager.AppSettings["trn"];
            
            foreach (var item in data.OrderBy(x=>x.Name))
            {
                tw.WriteLine(cmcbank + "|" +
                                 item.Member.IFSC + "|" +
                                 item.Member.BankAc + "|" +
                                 item.Member.Name + "|" +
                                 item.Honorarium + "|" +
                                 trn + "|10|" +
                                 item.Remarks);
            }
            tw.Flush();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = ms;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "plain/txt", "Savi-Honorarium-" + month + ".txt");
        }

        //CT006
        public ActionResult DownloadExpences(int id = 0, string month = null)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("CT006"))
            {
            }
            else {
                return View("Security");
            }
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            CMCTrnMasterRepository dal = new CMCTrnMasterRepository();
            var data = dal.GetFullDetails(id);
            string cmcbank = ConfigurationManager.AppSettings["cmcbank"];
            string trn = ConfigurationManager.AppSettings["trn"];

            foreach (var item in data.OrderBy(x => x.Name))
            {
                tw.WriteLine(cmcbank + "|" +
                                 item.Member.IFSC + "|" +
                                 item.Member.BankAc + "|" +
                                 item.Member.Name + "|" +
                                 item.OtherExpence + "|" +
                                 trn + "|10|" +
                                 item.Remarks);
            }
            tw.Flush();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = ms;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "plain/txt", "Savi-OtherExpence-" + month + ".txt");
        }

        //CT007
        public ActionResult ExportTrn(int id, string month=null)
        {
            if (User.IsInRole("Admin") || UserRightsRepository.RightList(User.Identity.Name).Contains("CT007"))
            {
            }
            else {
                return View("Security");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Month", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Honorarium", typeof(string)));
            dt.Columns.Add(new DataColumn("Contingency", typeof(string)));
            dt.Columns.Add(new DataColumn("TA&OPE", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPayable", typeof(string)));
            dt.Columns.Add(new DataColumn("CMCReview", typeof(string)));
            dt.Columns.Add(new DataColumn("CMCMotherMeeting", typeof(string)));
            dt.Columns.Add(new DataColumn("FIVPClaim", typeof(string)));
            dt.Columns.Add(new DataColumn("NetPayable", typeof(string)));
            dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
            dt.Columns.Add(new DataColumn("AccountNo", typeof(string)));

            GridView gv = new GridView();
            CMCTrnMasterRepository dal = new CMCTrnMasterRepository();
            var data = dal.GetFullDetails(id);
            int i = 1;
            foreach (var item in data.OrderBy(x => x.Code))
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = i;
                dr["Code"] = item.Code;
                dr["Month"] = month;
                dr["Name"] = item.Name;
                dr["Honorarium"] = item.Honorarium;
                dr["Contingency"] = item.Contingency;
                dr["TA&OPE"] = item.TAOPE;
                dr["TotalPayable"] = item.TotalPayable;
                dr["CMCReview"] = item.Review;
                dr["CMCMotherMeeting"] = item.Meeting;
                dr["FIVPClaim"] = item.Claim;
                dr["NetPayable"] = item.NetPayable;
                dr["Remarks"] = item.Remarks;
                dr["AccountNo"] = item.Account;
                dt.Rows.Add(dr);
                i++;
            }

            DataRow dr1 = dt.NewRow();
            dr1["Honorarium"] = data.Sum(x=>x.Honorarium);
            dr1["Contingency"] = data.Sum(x => x.Contingency);
            dr1["TA&OPE"] = data.Sum(x => x.TAOPE);
            dr1["CMCReview"] = data.Sum(x => x.Review);
            dr1["CMCMotherMeeting"] = data.Sum(x => x.Meeting);
            dr1["FIVPClaim"] = data.Sum(x => x.Claim);
            dr1["NetPayable"] = data.Sum(x => x.NetPayable);
            dt.Rows.Add(dr1);

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "CACProcessData-"+month +".xls";
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