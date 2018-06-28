using Core.Common.Folder;
using Core.Common.Log;
using eDNB.POBL.FileProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eApp.Win.RankPro
{
    public partial class MainForm : Form
    {
        #region Response Log Declaration
        
        #region Form Settings

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
                
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RichTxt(rtBox, "Started..", Color.Green);
        }

        private void tmx_Tick(object sender, EventArgs e)
        {
            // Send Text Files to Server
            var FilesX = new RankFolder().GetFileToFolder(new RANKProcess().genFolder, "txt");

            if(!FilesX.Count().Equals(0))
            {
                // Check if there is Internet Connection
                if(new Ping().Send("www.danubeco.com").Status != IPStatus.Success)
                {
                    RichTxt(rtBox, "unavailable..", Color.Red);

                    return;
                }

                // Disable timer so that it won't process any pdf
                tmx.Enabled = false;

                // Show uploader Form
                App.iProcess frm = new App.iProcess();

                // Inform System Log -> Uploader
                RichTxt(rtBox, "Uploading..", Color.Green);

                // System log 
                frm.SysLog += new App.iProcess.SystemLog(sysLOG);

                if(frm.ShowDialog(this) == DialogResult.Cancel)
                {
                    // if done uploading files enable timer
                    tmx.Enabled = true;

                    RANKProcess rankP = new RANKProcess();

                    RichTxt(rtBox, "Finished..", Color.Green);

                    rankP.CreateFolder(rankP.bckFolder);

                    rtBox.SaveFile(Path.Combine(rankP.bckFolder, DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("hmmss") + DateTime.Now.ToString("tt") + ".txt"), RichTextBoxStreamType.PlainText);
                }

            }
        }

        private void tme_Tick(object sender, EventArgs e)
        {

            if(DateTime.Now.ToString("h:mm tt") == "3:00 AM")
            {
                RichTxt(rtBox, "Waiting..", Color.Violet);

                tme.Enabled = false;

                tmx.Enabled = false;

                if (_ifsixty())
                {
                    App.iExpired frm = new App.iExpired();

                    frm.SysLog += new App.iExpired.SystemLog(sysLOG);

                    if(frm.ShowDialog(this) == DialogResult.Cancel)
                    {
                        tme.Enabled = true;

                        tmx.Enabled = true;

                        RichTxt(rtBox, "Expired..", Color.BlueViolet);
                    }
                }
            }
        }

        bool _ifsixty()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            while(sw.Elapsed.TotalMinutes < 1)
            { }

            sw.Stop();

            return true;

        }

        #endregion
    }
}
