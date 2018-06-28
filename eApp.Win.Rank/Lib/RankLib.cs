using Core.Common.Result;
using eApp.Win.Rank.ADO;
using eDNB.POBL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eApp.Win.Rank.Lib
{
    public class RankLib
    {
        
        List<ViewRankingBrandRequest> BrandRequests = new List<ViewRankingBrandRequest>();
        List<ViewRankingSKURequest> SkuRequests = new List<ViewRankingSKURequest>();
        List<ViewRankingDeptRequest> DeptRequests = new List<ViewRankingDeptRequest>();

        public OperationResult _getReqBList(string rStatus)
        {
            OperationResult op = new OperationResult();

            try
            {
                var db = new dnbmssqlEntities();

                var reqQQ = db.reqlists.Where(s => s.sts.Equals(rStatus) && s.rtype.Equals("brand"));

                var reqCnt = reqQQ.Count();

                if(reqCnt.Equals(0))
                {
                    op.Success = false;
                    return op;
                }

                foreach(var item in reqQQ)
                {
                    var objreq = db.ViewRankingBrandRequests.FirstOrDefault(s => s.reqid.Equals(item.reqid));

                    new CSV().CreateCSV(new string[]
                    {
                        "002", objreq.sid.Substring(6, 3), "BRD", objreq.sid.Substring(0, 6), objreq.ffrom.Replace("-", ""),
                        objreq.tto.Replace("-", ""), objreq.reqid, item.reqid.Substring(19, Convert.ToInt32(objreq.reqid.Length - 19)), objreq.dreq.Replace("-", ""), DateTime.Now.ToString("HH:mm:ss").Replace(":", ""), objreq.brandcode, "0", "0", "0", "0", "0"
                        
                    });

                    item.sts = "processing";
                }

                db.SaveChanges();
                db.Dispose();

                op.Success = true;
                op.AddMessage(reqCnt + " brand has been created");

                return op;

            }
            catch(Exception ex)
            {
                op.Success = false;
                op.AddMessage(ex.Message.ToString());

                return op;
            }
        }

        public OperationResult _getReqSList(string rStatus)
        {
            OperationResult op = new OperationResult();

            try
            {
                var db = new dnbmssqlEntities();

                var reqQQ = db.reqlists.Where(s => s.sts.Equals(rStatus) && s.rtype.Equals("sku"));

                var reqCnt = reqQQ.Count();

                if(reqCnt.Equals(0))
                {
                    op.Success = false;
                    return op;
                }

                foreach(var item in reqQQ)
                {
                    var objreq = db.ViewRankingSKURequests.FirstOrDefault(s => s.reqid.Equals(item.reqid));

                    new CSV().CreateCSV(new string[]
                    {
                        "002", objreq.sid.Substring(6, 3), "SKU", objreq.sid.Substring(0, 6), objreq.ffrom.Replace("-", ""),
                        objreq.tto.Replace("-", ""), objreq.reqid, item.reqid.Substring(19, Convert.ToInt32(objreq.reqid.Length - 19)), objreq.dreq.Replace("-", ""), DateTime.Now.ToString("HH:mm:ss").Replace(":", ""), "0", "0", "0", "0", "0", objreq.sku1
                    });

                    item.sts = "processing";
                }

                db.SaveChanges();
                db.Dispose();

                op.Success = true;
                op.AddMessage(reqCnt + " sku has been created");

                return op;

            }
            catch(Exception ex)
            {
                op.Success = false;
                op.AddMessage(ex.Message.ToString());

                return op;
            }

        }

        public OperationResult _getReqDList(string rStatus)
        {
            OperationResult op = new OperationResult();

            try
            {
                var db = new dnbmssqlEntities();

                var reqQQ = db.reqlists.Where(s => s.sts.Equals(rStatus) && s.rtype.Equals("dept"));

                var reqCnt = reqQQ.Count();

                if(reqCnt.Equals(0))
                {
                    op.Success = false;
                    return op;
                }

                foreach(var item in reqQQ)
                {
                    var objreq = db.ViewRankingDeptRequests.FirstOrDefault(s => s.reqid.Equals(item.reqid));

                    new CSV().CreateCSV(new string[]
                    {
                        "002", objreq.sid.Substring(6, 3), "DPT", objreq.sid.Substring(0, 6), objreq.ffrom.Replace("-", ""),
                        objreq.tto.Replace("-", ""), objreq.reqid, objreq.reqid.Substring(19, Convert.ToInt32(objreq.reqid.Length - 19)), objreq.dreq.Replace("-", ""), DateTime.Now.ToString("HH:mm:ss").Replace(":", ""), "0", objreq.deptcode.ToString(), objreq.deptsub.ToString(), objreq.dclass.ToString(), objreq.classsub.ToString(), "0"
                    });

                    item.sts = "processing";
                }

                db.SaveChanges();
                db.Dispose();

                op.Success = true;
                op.AddMessage(reqCnt + " dept has been created");

                return op;

            }
            catch(Exception ex)
            {
                op.Success = false;
                op.AddMessage(ex.Message.ToString());

                return op;
            }
        }

        public int _getAllBrand()
        {
            var db = new dnbmssqlEntities();

            var brandList = db.ViewRankingBrandRequests.ToList();

            return brandList.Count();
        }

        public int _getAllSku()
        {
            var db = new dnbmssqlEntities();

            var SkuRequests = db.ViewRankingSKURequests.ToList();

            return SkuRequests.Count();
        }

        public int _getAllDept()
        {
            var db = new dnbmssqlEntities();

            var DeptRequests = db.ViewRankingDeptRequests.ToList();

            return DeptRequests.Count();
        }

    }
}
