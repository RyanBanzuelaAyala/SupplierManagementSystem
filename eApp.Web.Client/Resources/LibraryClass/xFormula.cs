using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eApp.Web.Client.Resources.LibraryClass
{
    public class xFormula
    {
        #region Hidden Variables        
        private string region { get; set; }
        private string vendor { get; set; }
        private string vendorname { get; set; }
        private string totalpocost { get; set; }
        private string deliveredpocost { get; set; }
        private int c3 { get; set; }
        private decimal d3 { get; set; }
        private decimal e3 { get; set; }
        private decimal f3 { get; set; }
        private decimal g3 { get; set; }
        private decimal h3 { get; set; }
        private decimal i3 { get; set; }
        private decimal j3 { get; set; }
        private decimal k3 { get; set; }
        private decimal l3 { get; set; }

        #endregion

        public xFormula(xPerformance p, string reg)
        {
            region = reg;
            vendor = p.vendor;
            vendorname = p.vendorname;
            totalpocost = p.totalposcost;
            deliveredpocost = p.totalpostdeliveredcost;
            c3 = Convert.ToInt16(p.totalnoofpos);
            d3 = Convert.ToDecimal(p.totalposcost);             
            e3= Convert.ToDecimal(p.totalpostdeliveredcost);             
            h3 = Convert.ToDecimal(p.totalnoofexpiredpo);             
            i3= Convert.ToDecimal(p.totalnooflatedelivered); 
            j3 = Convert.ToDecimal(p.totallatedays);            
            l3 = Convert.ToDecimal(p.totalearlyday);
            //k3 = Convert.ToInt16(p.totalnoofearlydeliered);             
            //f3 = Convert.ToInt16(p.totalnoofontimepos); 
            //g3 = Convert.ToInt16(p.totalnooffullydelivered); 
        }

        public SupplierPerformance performace()
        {
            return new SupplierPerformance
            {
                region = region,
                vendor= vendor,
                vendorname = vendorname,
                totalpocost = totalpocost,
                deliveredpocost = deliveredpocost,
                PercentPODelivered = PercentPODelivered(),
                PercentValuePODelivered  = PercentValuePODelivered(),               
                PercentPODeliveredLate  = PercentPODeliveredLate(),                
                OnAverageEaPoLate = OnAverageEaPoLate(),
                POExpiredNotDelivered  = POExpiredNotDelivered()
                //PercentPODeliveredinFull  = PercentPODeliveredinFull(),
                //PercentPODeliveredonTime  = PercentPODeliveredonTime(),
                //PercentPODeliveredEarly  = PercentPODeliveredEarly(),
            };
        }

        #region Computation
        
        private string PercentPODelivered()
        {
            try
            {
                decimal ho = (c3 - h3);

                var percent =  ho / c3;

                var final = percent * 100;

                return Math.Round(final, 2).ToString() + " %";
            }
            catch
            {
                return "0%";
            }
                
        }

        private string PercentValuePODelivered()
        {
            try
            {
                decimal ho = (e3 / d3);
                
                var final = ho * 100;

                return Math.Round(final, 2).ToString() + " %";
                
            }
            catch
            {
                return "0%";
            }
            
        }
        
        private string PercentPODeliveredLate()
        {
            try
            {
                decimal ho = (i3 / c3);
                
                var final = ho * 100;

                return Math.Round(final, 2).ToString() + " %";
                
            }
            catch
            {
                return "0%";
            }

        }
        
        private string OnAverageEaPoLate()
        {
            try
            {
                decimal ho = (j3 / i3);

                var final = ho * 100;

                return Math.Round(final, 2).ToString();

            }
            catch
            {
                return "0";
            }

        }

        private string POExpiredNotDelivered()
        {
            try
            {
                decimal ho = (h3 / c3);

                var final = ho * 100;

                return Math.Round(final, 2).ToString() + " %";

            }
            catch
            {
                return "0%";
            }

        }


        private string PercentPODeliveredinFull()
        {
            try
            {
                return ((g3 / c3) * 100) + "%";
            }
            catch
            {
                return "0%";
            }
            
        }

        private string PercentPODeliveredonTime()
        {
            try
            {
                return ((f3 / c3) * 100) + "%";
            }
            catch
            {
                return "0%";
            }
            
        }

        private string PercentPODeliveredEarly()
        {
            try
            {
                return ((k3 / c3) * 100) + "%";
            }
            catch
            {
                return "0%";
            }
            
        }

        #endregion
    }
}

public partial class SupplierPerformance
{
    public string region { get; set; }
    public string vendor { get; set; }
    public string vendorname { get; set; }
    public string totalpocost { get; set; }
    public string deliveredpocost { get; set; }
    public string PercentPODelivered { get; set; }
    public string PercentValuePODelivered { get; set; }
    //public string PercentPODeliveredinFull { get; set; }
    //public string PercentPODeliveredonTime { get; set; }
    public string PercentPODeliveredLate { get; set; }
    //public string PercentPODeliveredEarly { get; set; }
    public string OnAverageEaPoLate { get; set; }
    public string POExpiredNotDelivered { get; set; }
   
}