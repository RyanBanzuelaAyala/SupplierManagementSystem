using eApp.Web.Client.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eApp.Web.Client.Resources.LibraryClass
{
    public static class sysLog
    {
        public static void Log(string msg, string action, string user)
        {
            dnbmssqlEntities db = new dnbmssqlEntities();

            var newObj = new syssuplog
            {
                userid = user,
                sysaction = msg,
                sysdate = DateTime.Now.ToString(),
                status = action
            };

            db.syssuplogs.Add(newObj);

            db.SaveChanges();
        }
    }
}