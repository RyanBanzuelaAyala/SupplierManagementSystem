using eApp.Web.Client.ADO;
using eApp.Web.Client.Models;
using eApp.Web.Client.Resources.LibraryClass;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Client.Controllers.Supplier
{
    [Authorize(Roles = "Supplier")]
    public class RankRouteController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();
              
        #region VIEW

        public ActionResult RequestList()
        {
            var currentUser = User.Identity.GetUserName();

            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var getrk = db.reqlists.Where(s => s.sid.Equals(currentUser));

            ViewBag.RKA = getrk.Where(s => s.sts.Equals("processed")).Count();
            ViewBag.RKD = getrk.Where(s => s.sts.Equals("downloaded")).Count();

            ViewBag.RKR = db.ar_reqlist.Count(s => s.sid.Equals(currentUser));

            var getrv = db.rtvs.Where(s => s.sid.Equals(id)
                                                && s.region.Equals(reg));

            ViewBag.RVA = getrv.Where(s => s.filestatus.Equals("Available")).Count();
            ViewBag.RVD = getrv.Where(s => s.filestatus.Equals("Downloaded")).Count();

            return View();

        }
        
        #endregion

        #region POST

        [HttpPost]
        public bool SubmitRankDL(string reqid)
        {
            var currentUser = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return false;
            }

            var objPO = db.reqlists.Where(e => e.sid.Equals(currentUser) && e.reqid.Contains(reqid)).FirstOrDefault();

            objPO.sts = "downloaded";

            db.SaveChanges();

            //sysLog("Download Ranking Request : " + reqid, "Ranking");

            return true;
        }

        #endregion

        #region Brand

        #region VIEW
        public ActionResult Brand()
        {
            return View();
        }

        #endregion

        #region POST
        
        public JsonResult searchCSV(string brandcode)
        {
            var currentUser = User.Identity.Name.Substring(0, 6);

            var xList = new List<BrandModel>();

            string[] csvHandler;

            using(StreamReader CsvReader = new StreamReader(Server.MapPath("~/CSV/brd2.csv")))
            {
                string inputLine = "";

                xList.Add(new BrandModel() { BrandCode = "0", BrandName = "ALL BRAND ITEM" });

                while((inputLine = CsvReader.ReadLine()) != null)
                {
                    char[] delimiterChars = { ',' };

                    csvHandler = inputLine.Split(delimiterChars);

                    if(csvHandler[0] == currentUser)
                    {
                        var xVal = new BrandModel() { BrandCode = csvHandler[1].ToString(), BrandName = csvHandler[2].ToString() };

                        xList.Add(xVal);
                    }

                }

                CsvReader.Close();
            }

            return Json(xList.OrderBy(s => s.BrandName), JsonRequestBehavior.AllowGet);

        }

        public string searchCSVB(string brandcode)
        {
            string csvHandlerLine = "";

            string[] csvHandler;

            using(StreamReader CsvReader = new StreamReader(Server.MapPath("~/CSV/brd.csv")))
            {
                string inputLine = "";

                while((inputLine = CsvReader.ReadLine()) != null)
                {
                    csvHandlerLine = inputLine.Trim().Replace(",", "").Replace(" ", "");

                    char[] delimiterChars = { '\"' };

                    csvHandler = csvHandlerLine.Split(delimiterChars);

                    if(csvHandler.Length == 3)
                    {
                        if(csvHandler[0].Equals(brandcode))
                        {
                            return csvHandler[0].ToString() + "/" + csvHandler[1].ToString();
                        }
                    }
                }

                CsvReader.Close();
            }

            return "null";

        }

        [HttpPost]
        public bool SubmitBrand(IEnumerable<string> arr)
        {
            var currentUser = User.Identity.Name;
            var combinedBrand = "BR" + DateTime.Now.ToString("yyyyMMdd") + currentUser;

            var ffrom = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var tto = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");

            foreach (var item in arr)
            {
                var reqid = combinedBrand + item;

                var isExist = db.reqbrands.Any(e => e.reqid.Equals(reqid));

                if (!isExist)
                {
                    var requestExists = db.reqbrands.Any(e => e.brandcode.Equals(item) && e.reqid.Equals(reqid));

                    if (!requestExists)
                    {

                        var newObj = new reqbrand()
                        {
                            sid = currentUser,
                            reqid = reqid,
                            ffrom = ffrom,
                            ddays = 30,
                            tto = tto,
                            brandcode = item,
                            daterequested = DateTime.Now.ToString("yyyy-MM-dd"),
                            status = "request",
                            exptn = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")
                        };
                        
                        db.reqbrands.Add(newObj);

                        db.SaveChanges();

                        addRequestToList(newObj.sid, newObj.reqid, "brand");

                        //sysLog("Request Brand : " + newObj.reqid, "Ranking");

                        
                    }
                }
            }
                        
            return true;
        }

        #endregion

        #endregion

        #region SKU

        #region VIEW

        public ActionResult SKU()
        {
            return View();
        }

        #endregion

        #region POST
        
        [HttpPost]
        public bool SubmitSKU(IEnumerable<string> arr)
        {
            var currentUser = User.Identity.Name;
            var newStr = "";

            foreach (var item in arr)
            {
                newStr += (newStr == "") ? item : "," + item;
            }

            var ffrom = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var tto = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            var newId = new NewId()._GenerateId("reqsku");

            var newObj = new reqsku()
            {
                sid = currentUser,
                reqid = "SK" + DateTime.Now.ToString("yyyyMMdd") + currentUser + newId,
                ffrom = ffrom,
                ddays = 30,
                tto = tto,
                sku1 = newStr,
                sku2 = "0",
                sku3 = "0",
                sku4 = "0",
                sku5 = "0",
                daterequested = DateTime.Now.ToString("yyyy-MM-dd"),
                status = "request",
                exptn = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")
            };
            
            db.reqskus.Add(newObj);

            db.SaveChanges();

            addRequestToList(newObj.sid, newObj.reqid, "sku");

            //sysLog("Request SKU : " + newObj.reqid, "Ranking");

            return true;

        }

        #endregion

        #endregion

        #region Department

        #region VIEW

        public ActionResult Department()
        {
            return View();
        }

        #endregion

        #region POST

        public JsonResult searchDept(string dept, string sdept, string iclass, string siclass)
        {
            var xList = new List<DeptModel>();

            var opObj = new RequesVal() { dept = dept, sdept = sdept, iclass = iclass, siclass = siclass };

            if (string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(sdept) && string.IsNullOrEmpty(iclass) && string.IsNullOrEmpty(siclass))
            {
                xList = Srch(1, 1, opObj);
            }
            else if (!string.IsNullOrEmpty(dept) && string.IsNullOrEmpty(sdept) && string.IsNullOrEmpty(iclass) && string.IsNullOrEmpty(siclass))
            {
                xList = Srch(3, 2, opObj);
            }
            else if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(sdept) && string.IsNullOrEmpty(iclass) && string.IsNullOrEmpty(siclass))
            {
                xList = Srch(2, 3, opObj);
            }
            else if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(sdept) && !string.IsNullOrEmpty(iclass) && string.IsNullOrEmpty(siclass))
            {
                xList = Srch(3, 4, opObj);
            }

            return Json(xList, JsonRequestBehavior.AllowGet);

        }
        private List<DeptModel> Srch(int codee, int chkr, RequesVal objj)
        {
            var currentUser = User.Identity.Name.Substring(0, 6);

            var xList = new List<DeptModel>();

            var cbt = 0;

            string csvHandlerLine = "";

            string[] csvHandler;

            var obj = new DeptModel() { };

            using (StreamReader CsvReader = new StreamReader(Server.MapPath("~/CSV/Dept_List.csv")))
            {
                string inputLine = "";

                while ((inputLine = CsvReader.ReadLine()) != null)
                {
                    cbt++;

                    if (cbt != 1)
                    {
                        csvHandlerLine = inputLine.Trim().Replace(" ", "");

                        char[] delimiterChars = { '\"', ',' };

                        csvHandler = csvHandlerLine.Split(delimiterChars);

                        if (chkr.Equals(1)) // Department
                        {
                            if (csvHandler[0].Equals(currentUser))
                            {
                                obj = new DeptModel() { Code = csvHandler[codee].ToString(), Name = csvHandler[2].ToString() };

                                if (!xList.Any(s => s.Code.Equals(obj.Code)))
                                {
                                    xList.Add(obj);
                                }
                            }
                        }
                        else if (chkr.Equals(2)) // Sub-Department
                        {
                            if (csvHandler[0].Equals(currentUser) && csvHandler[1].Equals(objj.dept) && !csvHandler[3].Equals(0))
                            {
                                obj = new DeptModel() { Code = csvHandler[3].ToString(), Name = csvHandler[4].ToString() };

                                if (!xList.Any(s => s.Code.Equals(obj.Code)))
                                {
                                    xList.Add(obj);
                                }
                            }
                        }
                        else if (chkr.Equals(3)) // Class
                        {
                            if (csvHandler[0].Equals(currentUser) && csvHandler[1].Equals(objj.dept) && csvHandler[3].Equals(objj.sdept) && !csvHandler[5].Equals(0))
                            {
                                obj = new DeptModel() { Code = csvHandler[5].ToString(), Name = csvHandler[6].ToString() };

                                if (!xList.Any(s => s.Code.Equals(obj.Code)))
                                {
                                    xList.Add(obj);
                                }
                            }
                        }
                        else if (chkr.Equals(4)) // Sub-Class
                        {
                            if (csvHandler[0].Equals(currentUser) && csvHandler[1].Equals(objj.dept) && csvHandler[3].Equals(objj.sdept) && csvHandler[5].Equals(objj.iclass) && !csvHandler[7].Equals(0))
                            {
                                obj = new DeptModel() { Code = csvHandler[7].ToString(), Name = csvHandler[8].ToString() };

                                if (!xList.Any(s => s.Code.Equals(obj.Code)))
                                {
                                    xList.Add(obj);
                                }
                            }
                        }

                    }

                }

                CsvReader.Close();
            }

            return xList;
        }
        public ActionResult viewDeptSupp()
        {
            var currentUser = User.Identity.Name.Substring(0, 6);

            var xList = new List<DeptModelQQ>();

            string csvHandlerLine = "";

            string[] csvHandler;

            var obj = new DeptModelQQ() { };

            using (StreamReader CsvReader = new StreamReader(Server.MapPath("~/CSV/Dept_List.csv")))
            {
                string inputLine = "";

                while ((inputLine = CsvReader.ReadLine()) != null)
                {
                    csvHandlerLine = inputLine.Trim().Replace(" ", "");

                    char[] delimiterChars = { '\"', ',' };

                    csvHandler = csvHandlerLine.Split(delimiterChars);

                    if (csvHandler[0].Contains(currentUser))
                    {
                        obj = new DeptModelQQ()
                        {
                            dept = csvHandler[1].ToString(),
                            deptname = csvHandler[2].ToString(),
                            sdept = csvHandler[3].ToString(),
                            sdeptname = csvHandler[4].ToString(),
                            iclass = csvHandler[5].ToString(),
                            iclassname = csvHandler[6].ToString(),
                            sclass = csvHandler[7].ToString(),
                            sclassname = csvHandler[8].ToString()
                        };

                        if (!xList.Any(s => s.dept.Equals(obj.dept) && s.sdept.Equals(obj.sdept) && s.iclass.Equals(obj.iclass) && s.sclass.Equals(obj.sclass)))
                        {
                            xList.Add(obj);
                        }
                    }
                }

                CsvReader.Close();
            }


            return Json(xList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool SubmitDept(RequesDEtModel model)
        {
            #region convert to Int
            //var nDept = (!model.dept.All(char.IsDigit)) ? 0 : Convert.ToInt32(model.dept);
            //var nDeptSub = (!model.sdept.All(char.IsDigit)) ? 0 : Convert.ToInt32(model.sdept);
            //var nClass = (!model.iclass.All(char.IsDigit)) ? 0 : Convert.ToInt32(model.iclass);
            //var nSubClass = (!model.siclass.All(char.IsDigit)) ? 0 : Convert.ToInt32(model.siclass);
            #endregion

            var currentUser = User.Identity.Name;
            var newID = "DP" + DateTime.Now.ToString("yyyyMMdd") + currentUser + model.dept + model.sdept + model.iclass + model.siclass;
            
            var ffrom = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var tto = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");

            var requestExists = db.reqdepts.Any(e => e.reqid.Equals(newID));

            if (!requestExists)
            {
                var newObj = new reqdept()
                {
                    sid = currentUser,
                    reqid = newID,
                    ffrom = ffrom,
                    ddays = 30,
                    tto = tto,
                    deptcode = Convert.ToInt32(model.dept),
                    deptsub = Convert.ToInt32(model.sdept),
                    dclass = Convert.ToInt32(model.iclass),
                    classsub = Convert.ToInt32(model.siclass),
                    daterequested = DateTime.Now.ToString("yyyy-MM-dd"),
                    status = "dept",
                    exptn = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")

                };
                
                db.reqdepts.Add(newObj);

                db.SaveChanges();

                addRequestToList(newObj.sid, newObj.reqid, "dept");

                //sysLog("Request SKU : " + newObj.reqid, "Ranking");

                return true;
            }

            return false;
        }

        #endregion

        #endregion

        private void addRequestToList(string isid, string ireqid, string rtype)
        {
            var ReqObject = new reqlist()
            {
                sid = isid,
                reqid = ireqid,
                dreq = DateTime.Now.ToString("yyyy-MM-dd"),
                rtype = rtype,
                exprtn = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"),
                sts = "process",
                lnk = "",
                lnka = "",
                exprd = "no"
            };

            db.reqlists.Add(ReqObject);

            db.SaveChanges();
        }

        #region JSON
        
        [HttpGet]
        public JsonResult GetAllRanking(string status)
        {
            dnbmssqlEntities db = new dnbmssqlEntities();

            var currentUser = User.Identity.Name;            

            if (status == "Requested")
            {
                var QQList = db.reqlists.Where(s => s.sid.Equals(currentUser))
                                   .Where(s => s.sts.Equals("process") || s.sts.Equals("processing") || s.sts.Equals("processed"))
                                   .OrderByDescending(s => s.dreq);

                return Json(QQList, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Downloaded")
            {
                var QQList = db.reqlists.Where(s => s.sid.Equals(currentUser) && s.sts.Equals("downloaded")).OrderByDescending(s => s.dreq);

                return Json(QQList, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Expired")
            {
                var QQList = db.ar_reqlist.Where(s => s.sid.Equals(currentUser)).OrderByDescending(s => s.dreq);

                return Json(QQList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
    }
}


public partial class BrandModel
{
    public string BrandCode { get; set; }
    public string BrandName { get; set; }
}
public partial class DeptModel
{
    public string Code { get; set; }
    public string Name { get; set; }

}

public partial class DeptModelQQ
{
    public string dept { get; set; }
    public string deptname { get; set; }
    public string sdept { get; set; }
    public string sdeptname { get; set; }
    public string iclass { get; set; }
    public string iclassname { get; set; }
    public string sclass { get; set; }
    public string sclassname { get; set; }


}

public partial class RequestModel
{
    public string sid { get; set; }
    public string reqid { get; set; }
    public string dreq { get; set; }
    public string rtype { get; set; }
    public string exprtn { get; set; }
    public string sts { get; set; }
    public string link { get; set; }
    public string linka { get; set; }
    public string exprd { get; set; }
}
public partial class RequesBRtModel
{
    public string ffrom { get; set; }
    public string tto { get; set; }
    public string brandcode { get; set; }

}
public partial class RequesSKtModel
{
    public string sku1 { get; set; }
    public string ffrom { get; set; }
    public string tto { get; set; }

}
public partial class RequesDEtModel
{
    public string dept { get; set; }
    public string sdept { get; set; }
    public string iclass { get; set; }
    public string siclass { get; set; }
    public string ffrom { get; set; }
    public string tto { get; set; }

}
public partial class RequesVal
{
    public string dept { get; set; }
    public string sdept { get; set; }
    public string iclass { get; set; }
    public string siclass { get; set; }
}

