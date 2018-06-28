using Core.Common.Log;
using eApp.Win.Mobile.ADO;
using eApp.Win.Mobile.Lib;
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

namespace eApp.Win.Mobile
{
    public partial class MainForm : Form
    {

        #region Response Log Declaration

        #region FORM SETTINGS

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        private void sysLOG(object sender, LOGArgs e)
        {
            RichTxt(rtBox, e.msgg, e.clrr);
        }

        void RichTxt(RichTextBox rch, string text, Color color)
        {
            RTBox.append(rch, text, color);
        }

        #endregion

        #region Main Process
        
        int OrigTime = 10;
        
        private void MainForm_Load_1(object sender, EventArgs e)
        {
            RichTxt(rtBox, "Activation Started..", Color.Green);
        }

        private void tmx_Tick_1(object sender, EventArgs e)
        {
            // Minus the Original Time
            OrigTime--;

            // Show Original Time to Label
            label3.Text = OrigTime / 60 + ":" + ((OrigTime % 60) >= 10 ? (OrigTime % 60).ToString() : "0" + OrigTime % 60);

            //this.Text = "DNB : RANKING ( " + label8.Text + " )";

            // Check Label if equal to 0
            if (label3.Text == "0:00")
            {
                // Disabled Timer
                tmx.Enabled = false;

                // Set Original Time to Default
                OrigTime = 10;

                processValidation();

                // Enable timer     
                tmx.Enabled = true;

            }

        }


        private void processValidation()
        {
            dnbmssqlEntities db = new dnbmssqlEntities();

            var smslist = db.suppliermobileactivtions.Distinct().ToList();

            foreach (var item in smslist.Where(s => s.status.Equals("pending")))
            {
                try
                {
                    var sms = new xSms().GetCode(6);

                    item.codesms = sms;                    
                    item.datetimesent = string.Format("{0:dd-MM-yyyy hh:mm:ss}", DateTime.Now);
                    
                    var msg = "Requested an activation code " + sms + " for mobile registration.";
                    
                    var result = new SMS().SendSMS(msg, item.mobile);

                    if(result.Success)
                    {
                        RichTxt(rtBox, item.mobile + " : " + sms + " : success.", Color.Green);
                        item.status = "request";
                    }
                    else
                    {
                        RichTxt(rtBox, item.mobile + " : " + sms + " : error csms.", Color.Red);
                        item.status = "error";
                    }                   

                }
                catch
                {
                    RichTxt(rtBox, item.mobile + " : error.", Color.Red);
                }
                
            }

            db.SaveChanges();
        }

        #endregion
 }
}
