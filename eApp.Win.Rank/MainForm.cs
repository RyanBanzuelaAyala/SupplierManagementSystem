using Core.Common.Log;
using Core.Common.Result;
using eApp.Win.Rank.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eApp.Win.Rank
{
    public partial class MainForm : Form
    {
        #region Declaration

        RankLib rnk = new RankLib();

        BackgroundWorker bg = new BackgroundWorker();
        
        #region FORM SETTING

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
        
        #region Response Log Declaration

        private void sysLOG(object sender, LOGArgs e)
        {
            RichTxt(rtBox, e.msgg, e.clrr);
        }

        void RichTxt(RichTextBox rch, string text, Color color)
        {
            RTBox.append(rch, text, color);
        }

        #endregion

        #endregion

        #region Ranking Process

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RichTxt(rtBox, "Started..", Color.Green);
        }

        int OrigTime = 10;

        private void tme_Tick(object sender, EventArgs e)
        {
            // Minus the Original Time
            OrigTime--;

            // Show Original Time to Label
            label8.Text = OrigTime / 60 + ":" + ((OrigTime % 60) >= 10 ? (OrigTime % 60).ToString() : "0" + OrigTime % 60);

            this.Text = "DNB : RANKING ( " + label8.Text + " )";

            // Check Label if equal to 0
            if (label8.Text == "0:00")
            {
                // Disabled Timer
                tme.Enabled = false;

                // Set Original Time to Default
                OrigTime = 10;

                // Set Operation Result for Request
                var result = new OperationResult();

                // Get all available Brand Request
                result = rnk._getReqBList("process");

                // Check if there are requests
                if (result.Success)
                { RichTxt(rtBox, result.MessageList[0].ToString(), Color.DarkBlue); }

                // Get all available SKU Request
                result = rnk._getReqSList("process");

                // Check if there are requests
                if (result.Success)
                { RichTxt(rtBox, result.MessageList[0].ToString(), Color.Violet); }

                // Get all available Department Request
                result = rnk._getReqDList("process");

                // Check if there are requests
                if (result.Success)
                { RichTxt(rtBox, result.MessageList[0].ToString(), Color.Orange); }

                // Enable timer     
                tme.Enabled = true;

            }
        }
        
        #endregion

    }
}
