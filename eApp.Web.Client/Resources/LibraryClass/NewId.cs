using eApp.Web.Client.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eApp.Web.Client.Resources.LibraryClass
{
    public class NewId
    {
        public string _GenerateId(string table)
        {
            try
            {
                using (var ctx = new dnbmssqlEntities())
                {
                    var result = ctx.Database.SqlQuery<int>("select Id from " + table + " order by Id desc").FirstOrDefault();

                    return Convert.ToString(result + 1);
                }
            }
            catch
            {
                return "1";
            }
        }

    }
}