using Core.Common.Log;
using eApp.Win.PO.ADO;
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

namespace eApp.Win.PO.App
{
    public partial class Expired : Form
    {
        #region FORM SETTINGS

        BackgroundWorker bg = new BackgroundWorker();

        #region Delegates Declaration

        public delegate void SystemLog(object sender, LOGArgs e);
        public event SystemLog SysLog;

        void _sysLog(string msg, Color color, StatusType typ)
        {
            SysLog(this, new LOGArgs(msg, color, typ));
        }

        #endregion
                
        public Expired()
        {
            InitializeComponent();
        }

        #endregion

        private void Expired_Load(object sender, EventArgs e)
        {
            if(lblMdd.Text.Equals("auto"))
            {
                label1.Text = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
            }
            
            Bg_DoWork();

            _sysLog("Archived Success..", Color.Green, StatusType.Sys);

            //Email.SendEmail("Successfully archived all expired P.O(s) dated : " + label1.Text, "DNB : Archive Report-");

            this.Close();

        }

        #region Archive Process
        
        private void Bg_DoWork()
        {
            dnbmssqlEntities db = new dnbmssqlEntities();
                        
            var result = Convert.ToBoolean(db.sppoarchive(label1.Text));

            if(!result)
            {
                _sysLog("No archive..", Color.Green, StatusType.Sys);
            }
            else
            {
                deleteOldPDF(label1.Text);
            }

        }

        private void deleteOldPDF(string date)
        {
            dnbmssqlEntities db = new dnbmssqlEntities();

            var expdate = Convert.ToDateTime(date).AddDays(-7);

            var yeExp = expdate.ToString("yyyyMMdd");

            var result = db.ar_po.Where(s => s.expiration.Equals(date)).ToList<ar_po>();

            var xftp = new ftp(@"ftp://188.121.43.20/services.danubeco.com/supplier/", "");
            
            foreach (var item in result)
            {
                try
                {
                    xftp.delete(Path.GetFileName(item.link));

                    _sysLog("FTP File " + item + " : deleted..", Color.Green, StatusType.Sys);
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
