using Core.Common.Folder;
using Core.Common.Log;
using eDNB.POBL.FileProcess;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eApp.Win.PO
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            RichTxt(rtBox, "PO Started..", Color.Green);
        }

        private void tmx_Tick(object sender, EventArgs e)
        {
            // Check if there PDF file in source folder
            POProcess FolderPO = new POProcess();
            
            var files = FolderPO.GetAllFile(FolderPO.srcFolder, "txt");

            // Check if it has
            if(!files.Length.Equals(0))
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
                App.Upload frm = new App.Upload();

                // Inform System Log -> Uploader
                RichTxt(rtBox, "Processing..", Color.Green);

                // System log 
                frm.SysLog += new App.Upload.SystemLog(sysLOG);

                if(frm.ShowDialog(this) == DialogResult.Cancel)
                {
                    // if done uploading files enable timer
                    tmx.Enabled = true;

                    RichTxt(rtBox, "Finished..", Color.Green);

                    FolderPO.CreateFolder(FolderPO.bckFolder);

                    rtBox.SaveFile(Path.Combine(FolderPO.bckFolder, DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("hmmss") + DateTime.Now.ToString("tt") + ".log"), RichTextBoxStreamType.PlainText);
                }

            }
        }

        #endregion

        #region SMS and Expired Notification 

        private void tme_Tick(object sender, EventArgs e)
        {
            #region SMS 

            if(DateTime.Now.ToString("h:mm tt") == "8:00 AM" 
                || DateTime.Now.ToString("h:mm tt") == "10:00 AM"
                || DateTime.Now.ToString("h:mm tt") == "12:00 PM"
                || DateTime.Now.ToString("h:mm tt") == "2:00 PM"
                || DateTime.Now.ToString("h:mm tt") == "4:00 PM"
                || DateTime.Now.ToString("h:mm tt") == "6:00 PM")
            {

                RichTxt(rtBox, "Waiting..", Color.Violet);

                tmx.Enabled = false;

                tme.Enabled = false;
                
                if(_ifsixty())
                {
                    App.Msg frm = new App.Msg();

                    frm.SysLog += new App.Msg.SystemLog(sysLOG);

                    if(frm.ShowDialog(this) == DialogResult.Cancel)
                    {
                        tme.Enabled = true;

                        tmx.Enabled = true;

                        RichTxt(rtBox, "SMS Sent..", Color.BlueViolet);
                    }
                }
            }

            #endregion

            #region Expired

            if(DateTime.Now.ToString("h:mm tt") == "3:00 AM")
            {

                RichTxt(rtBox, "Waiting..", Color.Violet);

                tme.Enabled = false;

                tmx.Enabled = false;
                
                if(_ifsixty())
                {
                    App.Expired frm = new App.Expired();

                    frm.SysLog += new App.Expired.SystemLog(sysLOG);

                    frm.lblMdd.Text = "auto";

                    if(frm.ShowDialog(this) == DialogResult.Cancel)
                    {
                        tme.Enabled = true;

                        tmx.Enabled = true;

                        RichTxt(rtBox, "Expired..", Color.BlueViolet);
                    }
                }
            }
            
            #endregion

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

        #region Search RTBOX
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.rtBox.SelectionBackColor = Color.Empty;

            int len = this.rtBox.TextLength;
            int index = 0;
            int lastIndex = this.rtBox.Text.LastIndexOf(this.textBox1.Text);

            while (index < lastIndex)
            {
                this.rtBox.Find(this.textBox1.Text, index, len, RichTextBoxFinds.None);
                this.rtBox.SelectionBackColor = Color.Yellow;
                index = this.rtBox.Text.IndexOf(this.textBox1.Text, index) + 1;
            }
        }


        #endregion
    }
}
