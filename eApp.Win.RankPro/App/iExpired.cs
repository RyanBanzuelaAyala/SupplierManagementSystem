using Core.Common.Log;
using eApp.Win.RankPro.ADO;
using eDNB.POBL.Email;
using eDNB.POBL.FTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Core.Common.Log.LOGArgs;

namespace eApp.Win.RankPro.App
{
    public partial class iExpired : Form
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

        #endregion

        public iExpired()
        {
            InitializeComponent();
        }

        private void iExpired_Load(object sender, EventArgs e)
        {
            Bg_DoWork();

            //Email.SendEmail("Successfully archived all expired P.O(s) dated : " + label1.Text, "DNB : Archive Report-");

            this.Close();
        }

        #region Archive Process

        dnbmssqlEntities db = new dnbmssqlEntities();

        private void Bg_DoWork()
        {
            var ardate = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");

            try
            {
                db.spbrandreqarchive(ardate);
                deleteOldPDF(ardate, "brand");

                db.spdeptreqarchive(ardate);
                deleteOldPDF(ardate, "dept");

                db.spskureqarchive(ardate);
                deleteOldPDF(ardate, "sku");
                
            }
            catch(Exception)
            {
                _sysLog("No Archived..", Color.Orange, StatusType.Sys);
            }

        }

        private void deleteOldPDF(string date, string typ)
        {
            var expdate = Convert.ToDateTime(date).AddDays(-7);
            
            var result = db.ar_reqlist.Where(s => s.exprtn.Equals(date)).ToList<ar_reqlist>();

            var xftp = new ftp(@"ftp://188.121.43.20/services.danubeco.com/ranking/", "");
            
            foreach (var item in result)
            {
                try
                {
                    if (item.rtype.Equals(typ))
                    {
                        xftp.delete(Path.GetFileName(item.lnk));

                        _sysLog("FTP File " + item + " : deleted..", Color.Green, StatusType.Sys);
                    }
                        
                }
                catch (Exception)
                {
                    _sysLog("FTP File " + item + " : error..", Color.Red, StatusType.Err);
                }
            }

        }
        

        #endregion

    }
}
