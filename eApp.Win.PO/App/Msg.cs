using Core.Common.Log;
using eApp.Win.PO.ADO;
using eDNB.POBL.Email;
using eDNB.POBL.SMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Core.Common.Log.LOGArgs;

namespace eApp.Win.PO.App
{
    public partial class Msg : Form
    {
        #region Declaration

        BackgroundWorker bg = new BackgroundWorker();

        #region Delegates Declaration

        public delegate void SystemLog(object sender, LOGArgs e);
        public event SystemLog SysLog;

        void _sysLog(string msg, Color color, StatusType typ)
        {
            SysLog(this, new LOGArgs(msg, color, typ));
        }

        #endregion
        
        public Msg()
        {
            InitializeComponent();
        }

        #endregion

        #region SMS Processing

        List<sm> smsList = new List<sm>();

        private void Msg_Load(object sender, EventArgs e)
        {
            
            Bg_DoWork();

            _sysLog("Sent..", Color.Green, StatusType.Sys);

            StringBuilder ssSMP = new StringBuilder();

            foreach (var item in smsList)
            {
                ssSMP.AppendLine("<br /> " + item.sid + " - " + item.region + " - " + item.icurrent);
            }

            Email.SendEmail("<h3><b> Supplier Management Portal : SMS Report</b></h3><hr /><h4> Supplier List </h4>" + ssSMP, "DANUBE SMP : SMS Report-");

            this.Close();
        }

        private void Bg_DoWork()
        {
            dnbmssqlEntities db = new dnbmssqlEntities();

            var smslist = db.sms.Distinct().ToList<sm>();

            foreach(var item in smslist)
            {
                var supp = item.sid + "-" + item.region;

                try
                {
                    var msg = supp + " : you have approved P.O. dated : " + DateTime.Now.ToString("yyyy-MM-dd") + " Visit http://services.danubeco.com for more info. Thank you!";

                    var result = new SMS().SendSMS(msg, item.icurrent);

                    _sysLog(supp + result.MessageList[0].ToString(), Color.Violet, StatusType.Sys);

                }
                catch
                {
                    _sysLog(supp + " error in sending SMS", Color.Red, StatusType.Err);
                }
                
                item.status = "Sent";

                smsList.Add(item);
            }

            db.SaveChanges();

            var smsArch = Convert.ToBoolean(db.spsmsarchive());

            if(smsArch)
            {
                _sysLog("SMS archived..", Color.Green, StatusType.Sys);
            }
            else
            {
                _sysLog("SMS error..", Color.Red, StatusType.Sys);
            }
        }

        #endregion

    }
}
