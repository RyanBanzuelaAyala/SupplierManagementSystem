using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace eApp.Web.Client.Resources.LibraryClass
{
    public class xCSVPerformance
    {
        public xPerformance getCSVInfo(string empid, string searcby, string filePath)
        {
            var userInfo = new xPerformance();

            string[] csvHandler;

            using (StreamReader CsvReader = new StreamReader(filePath))
            {
                string inputLine = "";

                while ((inputLine = CsvReader.ReadLine()) != null)
                {
                    char[] delimiterChars = { ',' };

                    csvHandler = inputLine.Split(delimiterChars);

                    var sindex = Convert.ToInt32(searcby);

                    var isSuccess = false;

                    if (sindex == 0)
                    {
                        if (csvHandler[0].Equals(empid))
                            isSuccess = true;
                    }
                    else
                    {
                        var newName = csvHandler[1].ToString().Replace("\"", "");

                        if (newName.Contains(empid))
                            isSuccess = true;
                    }

                    if (isSuccess)
                    {
                        userInfo.company = csvHandler[0].ToString();
                            userInfo.store = csvHandler[0].ToString();
                            userInfo.region = csvHandler[0].ToString();
                            userInfo.vendor = csvHandler[0].ToString();
                            userInfo.vendorname = csvHandler[0].ToString();
                            userInfo.pono = csvHandler[0].ToString();
                            userInfo.poentrydate = csvHandler[0].ToString();
                            userInfo.totalnoofpos = csvHandler[0].ToString();
                            userInfo.totalnoofposdelivered = csvHandler[0].ToString();
                            userInfo.totalposcost = csvHandler[0].ToString();
                            userInfo.totalpostdeliveredcost = csvHandler[0].ToString();
                            //userInfo.totalpostdeliveredretail = csvHandler[0].ToString();
                            //userInfo.totalnoofontimepos = csvHandler[0].ToString();
                            //userInfo.totalnooffullydelivered = csvHandler[0].ToString();
                            userInfo.totalnoofexpiredpo = csvHandler[0].ToString();
                            userInfo.totalnooflatedelivered = csvHandler[0].ToString();
                            userInfo.totallatedays = csvHandler[0].ToString();
                            //userInfo.totalnoofearlydeliered = csvHandler[0].ToString();
                            userInfo.totalearlyday = csvHandler[0].ToString();
                    }
                }

                CsvReader.Close();  
            }

            return userInfo;
        }
        
        private string getCurrentRegion(string reg, string filePath2)
        {
            string[] csvlines = File.ReadAllLines(filePath2);

            try
            {
                var query = from csvline in csvlines
                            let data = csvline.Split(',')
                            where data[2].Contains(reg)
                            select new
                            {
                                company = data[1].ToString()
                            };

                return query.FirstOrDefault().company;
            }
            catch
            {
                return "203";
            }
                
        }

        #region 3rd Layer

        // --------------- 3rd Layer  ----------------------- //

        public List<xPerformance> getSupplierDataByBranchStore(string sid, string region, string store, string filePath, string regpath)
        {
            string[] csvlines = File.ReadAllLines(filePath);

            var reg = getCurrentRegion(region, regpath);

            var query = (from csvline in csvlines
                         let data = csvline.Split(',')
                         where data[0].Equals("2") && data[3].Equals(sid) && data[2].Equals(reg) && data[1].Equals(store)
                         select new xPerformance()
                         {
                             company = data[0].ToString(),
                             store = data[1].ToString(),
                             region = data[2].ToString(),
                             vendor = data[3].ToString(),
                             vendorname = data[4].ToString(),
                             pono = data[5].ToString(),
                             poentrydate = data[6].ToString(),
                             totalnoofpos = data[7].ToString(),
                             totalnoofposdelivered = data[8].ToString(),
                             totalposcost = data[9].ToString(),
                             totalpostdeliveredcost = data[10].ToString(),
                             totalpostdeliveredretail = data[11].ToString(),
                             totalnoofexpiredpo = data[14].ToString(),
                             totalnooflatedelivered = data[15].ToString(),
                             totallatedays = data[16].ToString(),
                             totalearlyday = data[18].ToString(),

                         }).ToList();


            return query;

        }


        // --------------- 3rd Layer Date Range ----------------------- //

        public List<xPerformance> getSupplierDataByBranchStoreRangeDate(List<xPerformance> iList, DateTime startDate, DateTime endDate)
        {
            var iListReg = new List<xPerformance>();

            if (!iList.Count().Equals(0))
            {
                foreach (var itemX in iList)
                {
                    var dateToCheck = Convert.ToDateTime(itemX.poentrydate);

                    if (startDate <= dateToCheck && dateToCheck <= endDate)
                    {
                        iListReg.Add(itemX);
                    }
                }
            }

            return iListReg;
        }


        #endregion

        #region 2nd Layer

        // --------------- 2nd Layer ----------------------- //

        public List<xPerformance> getSupplierDataByBranch(string sid, string region, string filePath, string regpath)
        {
            string[] csvlines = File.ReadAllLines(filePath);

            var reg = getCurrentRegion(region, regpath);

            var query = (from csvline in csvlines
                         let data = csvline.Split(',')
                         where data[0].Equals("2") && data[3].Equals(sid) && data[2].Equals(reg)
                         select new xPerformance()
                         {
                             company = data[0].ToString(),
                             store = data[1].ToString(),
                             region = data[2].ToString(),
                             vendor = data[3].ToString(),
                             vendorname = data[4].ToString(),
                             pono = data[5].ToString(),
                             poentrydate = data[6].ToString(),
                             totalnoofpos = data[7].ToString(),
                             totalnoofposdelivered = data[8].ToString(),
                             totalposcost = data[9].ToString(),
                             totalpostdeliveredcost = data[10].ToString(),
                             totalpostdeliveredretail = data[11].ToString(),
                             totalnoofexpiredpo = data[14].ToString(),
                             totalnooflatedelivered = data[15].ToString(),
                             totallatedays = data[16].ToString(),
                             totalearlyday = data[18].ToString(),

                         }).ToList();


            return query;

        }

        public List<xPerformance> generateSummaryBranch(List<xPerformance> iList)
        {
            var iListReg = new List<xPerformance>();

            if (!iList.Count().Equals(0))
            {
                foreach (var itemX in iList)
                {
                    NullFiller.FillNullFields<xPerformance>(itemX);

                    if (iListReg.Any(d => d.store.Equals(itemX.store)))
                    {
                        iListReg.Where(d => d.store.Equals(itemX.store)).ToList().ForEach(c => {
                            c.poentrydate = itemX.poentrydate;
                            c.store = itemX.store;
                            c.totalnoofpos = (Convert.ToInt16(c.totalnoofpos) + Convert.ToInt16(itemX.totalnoofpos)).ToString();
                            c.totalnoofposdelivered = (Convert.ToInt16(c.totalnoofposdelivered) + Convert.ToInt16(itemX.totalnoofposdelivered)).ToString();
                            c.totalposcost = (Convert.ToDecimal(c.totalposcost) + Convert.ToDecimal(itemX.totalposcost)).ToString();
                            c.totalpostdeliveredcost = (Convert.ToDecimal(c.totalpostdeliveredcost) + Convert.ToDecimal(itemX.totalpostdeliveredcost)).ToString();
                            c.totalpostdeliveredretail = (Convert.ToDecimal(c.totalpostdeliveredretail) + Convert.ToDecimal(itemX.totalpostdeliveredretail)).ToString();
                            c.totalnoofexpiredpo = (Convert.ToInt16(c.totalnoofexpiredpo) + Convert.ToInt16(itemX.totalnoofexpiredpo)).ToString();
                            c.totalnooflatedelivered = (Convert.ToInt16(c.totalnooflatedelivered) + Convert.ToInt16(itemX.totalnooflatedelivered)).ToString();
                            c.totallatedays = (Convert.ToInt16(c.totallatedays) + Convert.ToInt16(itemX.totallatedays)).ToString();
                            c.totalearlyday = (Convert.ToInt16(c.totalearlyday) + Convert.ToInt16(itemX.totalearlyday)).ToString();
                        });
                    }
                    else
                    {
                        iListReg.Add(itemX);
                    }

                }
            }

            return iListReg;
        }

        // --------------- 2nd Layer Date Range ----------------------- //

        public List<xPerformance> generateSummaryBranchRangeDate(List<xPerformance> iList, DateTime startDate, DateTime endDate)
        {
            var iListReg = new List<xPerformance>();

            if (!iList.Count().Equals(0))
            {
                foreach (var itemX in iList)
                {
                    var dateToCheck = Convert.ToDateTime(itemX.poentrydate);

                    if (startDate <= dateToCheck && dateToCheck <= endDate)
                    {
                        iListReg.Add(itemX);
                    }
                }
            }

            return iListReg;
        }
        
        #endregion

        #region 1st Layer

        // --------------- 1st Layer ----------------------- //

        public List<xPerformance> getSupplierData(string sid, string region, string filePath, string regpath)
        {
            string[] csvlines = File.ReadAllLines(filePath);

            var reg = getCurrentRegion(region, regpath);

            var query = (from csvline in csvlines
                         let data = csvline.Split(',')
                         where data[0].Equals("2") && data[3].Equals(sid) && data[2].Equals(reg)
                         select new xPerformance()
                         {
                             company = data[0].ToString(),
                             store = data[1].ToString(),
                             region = data[2].ToString(),
                             vendor = data[3].ToString(),
                             vendorname = data[4].ToString(),
                             pono = data[5].ToString(),
                             poentrydate = data[6].ToString(),
                             totalnoofpos = data[7].ToString(),
                             totalnoofposdelivered = data[8].ToString(),
                             totalposcost = data[9].ToString(),
                             totalpostdeliveredcost = data[10].ToString(),
                             totalpostdeliveredretail = data[11].ToString(),
                             totalnoofexpiredpo = data[14].ToString(),
                             totalnooflatedelivered = data[15].ToString(),
                             totallatedays = data[16].ToString(),
                             totalearlyday = data[18].ToString(),

                         }).ToList();


            return query;

        }

        public List<xPerformance> generateSummary(List<xPerformance> iList)
        {
            var iListReg = new List<xPerformance>();

            if (!iList.Count().Equals(0))
            {
                foreach (var itemX in iList)
                {
                    NullFiller.FillNullFields<xPerformance>(itemX);

                    if (!iListReg.Count().Equals(0))
                    {
                        iListReg.ToList().ForEach(c => {
                            c.poentrydate = itemX.poentrydate;
                            c.region = itemX.region;
                            c.totalnoofpos = (Convert.ToInt16(c.totalnoofpos) + Convert.ToInt16(itemX.totalnoofpos)).ToString();
                            c.totalnoofposdelivered = (Convert.ToInt16(c.totalnoofposdelivered) + Convert.ToInt16(itemX.totalnoofposdelivered)).ToString();
                            c.totalposcost = (Convert.ToDecimal(c.totalposcost) + Convert.ToDecimal(itemX.totalposcost)).ToString();
                            c.totalpostdeliveredcost = (Convert.ToDecimal(c.totalpostdeliveredcost) + Convert.ToDecimal(itemX.totalpostdeliveredcost)).ToString();
                            c.totalpostdeliveredretail = (Convert.ToDecimal(c.totalpostdeliveredretail) + Convert.ToDecimal(itemX.totalpostdeliveredretail)).ToString();
                            c.totalnoofexpiredpo = (Convert.ToInt16(c.totalnoofexpiredpo) + Convert.ToInt16(itemX.totalnoofexpiredpo)).ToString();
                            c.totalnooflatedelivered = (Convert.ToInt16(c.totalnooflatedelivered) + Convert.ToInt16(itemX.totalnooflatedelivered)).ToString();
                            c.totallatedays = (Convert.ToInt16(c.totallatedays) + Convert.ToInt16(itemX.totallatedays)).ToString();
                            c.totalearlyday = (Convert.ToInt16(c.totalearlyday) + Convert.ToInt16(itemX.totalearlyday)).ToString();
                        });
                    }
                    else
                    {
                        iListReg.Add(itemX);
                    }

                }
            }

            return iListReg;
        }

        // --------------- 1st Layer Date Range ----------------------- //

        public List<xPerformance> generateSummaryRangeDate(List<xPerformance> iList, DateTime startDate, DateTime endDate)
        {
            var iListReg = new List<xPerformance>();

            if (!iList.Count().Equals(0))
            {
                foreach (var itemX in iList)
                {
                    var dateToCheck = Convert.ToDateTime(itemX.poentrydate);

                    if (startDate <= dateToCheck && dateToCheck <= endDate)
                    {
                        iListReg.Add(itemX);
                    }
                }
            }

            return iListReg;
        }
        
        #endregion

    }
}

public partial class xPerformance
{
    public string company { get; set; }
    public string store { get; set; }
    public string region { get; set; }
    public string vendor { get; set; }
    public string vendorname { get; set; }
    public string pono { get; set; }
    public string poentrydate { get; set; }
    public string totalnoofpos { get; set; }
    public string totalnoofposdelivered { get; set; }
    public string totalposcost { get; set; }
    public string totalpostdeliveredcost { get; set; }
    public string totalpostdeliveredretail { get; set; }
    //public string totalnoofontimepos { get; set; }
    //public string totalnooffullydelivered { get; set; }
    public string totalnoofexpiredpo { get; set; }
    public string totalnooflatedelivered { get; set; }
    public string totallatedays { get; set; }
    //public string totalnoofearlydeliered { get; set; }
    public string totalearlyday { get; set; }
}

public partial class zPerformance
{    
    public string region { get; set; }
    public string vendor { get; set; }
    public string vendorname { get; set; }
    public string totalpocost { get; set; }    
    public string deliveredpocost { get; set; }
    public string perofpodelivered { get; set; }
    public string pervalueofpodelivered { get; set; }
    public string perpodeliveredinfull { get; set; }
    public string perpodeliveredontime { get; set; }
    public string perpodeliveredlate { get; set; }
    public string perpodeliveredearly { get; set; }
    public string avgeapowaslatebydays { get; set; }
    public string lossoppurtunitytosale { get; set; }

}