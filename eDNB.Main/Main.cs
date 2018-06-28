using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eDNB.Main
{
    public partial class Main : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Terminate("eDNB.Server");
            //Terminate("eDNB.Ranking");
            //Terminate("eDNB.RankPro");
        }

        Process p;
        String proc = "";
        string procalias = "";
        
        private const int SW_HIDE = 0;
        private const int SW_SHOWNORMAL = 1;
        private int hWnd;

        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);
        
        #region Function

        private string RunApp()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.FileName = proc;

            p = new Process();
            p.StartInfo = psi;
            p.Start();

            Thread.Sleep(3000);

            Process[] processRunning = Process.GetProcesses();
            foreach(Process pr in processRunning)
            {
                if(pr.ProcessName.Equals(procalias))
                {
                    hWnd = pr.MainWindowHandle.ToInt32();

                    ShowWindow(hWnd, SW_HIDE);
                }
            }

            return hWnd.ToString();
        }
        
        private void HideApp()
        {
            Process[] processRunning = Process.GetProcesses();

            foreach(Process pr in processRunning)
            {
                ShowWindow(hWnd, SW_HIDE);
            }
        }

        private void Terminate(string name)
        {
            Process[] processRunning = Process.GetProcessesByName(name);

            foreach(var item in processRunning)
            {
                item.Kill();
                item.Close();
                item.Dispose();
            }
           
        }

        private string IsProcessOpen(string name)
        {
            foreach(Process clsProcess in Process.GetProcesses())
            {
                if(clsProcess.ProcessName.Equals(name))
                {
                    return "OK";
                }
            }

            return "NO";
        }

        private void IsHang(string name)
        {
            var restart = false;

            foreach(Process clsProcess in Process.GetProcesses())
            {
                if(clsProcess.ProcessName.Contains(name))
                {
                    if (!clsProcess.Responding)
                    {
                        restart = true;
                        clsProcess.Kill();
                        break;
                    }
                }
            }

            if(restart)
            {
                StartCloseApp(true, "eDNB.Server", @"C:\wamp\www\App\idnb\po\eDNB.Server.exe", btnPStart, "Stop", btnPOpen, true, lblPO, label10, Color.Green, progressBar1, true);
            }
        }

        private void isThere(string appName, Label lbNotify, ProgressBar pb, Button btnMM, Button btnNN, Label lbAppWN)
        {
            var isRunning = IsProcessOpen(appName);

            if(isRunning.Equals("NO"))
            {
                lbNotify.BackColor = Color.Red;

                lbNotify.Text = "NO";

                pb.Visible = false;

                btnMM.Text = "Start";

                btnNN.Text = "Show";

                btnNN.Enabled = false;

                lbAppWN.Text = "";
            }
            else
            {
                lbNotify.Text = "OK";

                lbNotify.BackColor = Color.Green;

                pb.Visible = true;

                btnNN.Enabled = true;

                btnMM.Text = "Stop";
                
            }
        }

        private void StartCloseApp(bool isRun, string alias, string appPath, Button btnApp, string btnAppText, Button btnSH, bool isEnable, Label lbtype, Label lbnotify, Color color, ProgressBar pb, bool isBool)
        {
            Terminate(alias);

            proc = appPath;

            procalias = alias;
            
            btnApp.Text = btnAppText;

            btnSH.Enabled = isEnable;

            if(isRun)
            {
                lbtype.Text = RunApp();
            }

            Thread.Sleep(3000);

            lbnotify.Text = IsProcessOpen(alias);
            
            lbnotify.BackColor = color;

            pb.Visible = isBool;
            
        }
               

        private void HideShowApp(int isHide, Button btn, string bText, string lblWN)
        {
            btn.Text = bText;

            Process[] processRunning = Process.GetProcesses();

            foreach(Process pr in processRunning)
            {
                ShowWindow(Convert.ToInt32(lblWN), isHide);
            }
        }
        
        #endregion

        #region Checker Timer

        private void timer1_Tick(object sender, EventArgs e)
        {
            //isThere("eDNB.Server", label10, progressBar1, btnPStart, btnPOpen, lblPO);

            ////IsHang("eDNB.ServerB");

            //isThere("eDNB.Ranking", lRR, progressBar2, pRR, pRRS, lRRH);

            ////IsHang("eDNB.RankingB");

            //isThere("eDNB.RankPro", lPR, progressBar3, pRP, pRPS, lPR);

            ////IsHang("eDNB.RankProB");
        }

        #endregion
        
        private void button6_Click(object sender, EventArgs e)
        {
            if (btnPStart.Text.Equals("Start"))
            {         

                StartCloseApp(true, "eDNB.Server", @"C:\wamp\www\App\idnb\po\eDNB.Server.exe", btnPStart, "Stop", btnPOpen, true, lblPO, label10, Color.Green, progressBar1, true);
            }
            else
            {
                StartCloseApp(false, "eDNB.Server", "", btnPStart, "Start", btnPOpen, false, lblPO, label10, Color.Red, progressBar1, false);
            }            
        }       
        
        private void button10_Click(object sender, EventArgs e)
        {            
            if(btnPOpen.Text.Equals("Show"))
            {                
                HideShowApp(1, btnPOpen, "Hide", lblPO.Text);
            }
            else
            {
                HideShowApp(0, btnPOpen, "Show", lblPO.Text);
            }            
        }

        private void pRR_Click(object sender, EventArgs e)
        {
            if(pRR.Text.Equals("Start"))
            {

                StartCloseApp(true, "eDNB.Ranking", @"C:\wamp\www\App\idnb\rankreq\eDNB.Ranking.exe", pRR, "Stop", pRRS, true, lRRH, lRR, Color.Green, progressBar2, true);
            }
            else
            {
                StartCloseApp(false, "eDNB.Ranking", "", pRR, "Start", pRRS, false, lRRH, lRR, Color.Red, progressBar2, false);
            }
        }

        private void pRRS_Click(object sender, EventArgs e)
        {
            if(pRRS.Text.Equals("Show"))
            {
                HideShowApp(1, pRRS, "Hide", lRRH.Text);
            }
            else
            {
                HideShowApp(0, pRRS, "Show", lRRH.Text);
            }
        }

        private void pRP_Click(object sender, EventArgs e)
        {
            if(pRP.Text.Equals("Start"))
            {

                StartCloseApp(true, "eDNB.RankPro", @"C:\wamp\www\App\idnb\rankpro\eDNB.RankPro.exe", pRP, "Stop", pRPS, true, lPRH, lPR, Color.Green, progressBar3, true);
            }
            else
            {
                StartCloseApp(false, "eDNB.RankPro", "", pRP, "Start", pRPS, false, lPRH, lPR, Color.Red, progressBar3, false);
            }
        }

        private void pRPS_Click(object sender, EventArgs e)
        {
            if(pRPS.Text.Equals("Show"))
            {
                HideShowApp(1, pRPS, "Hide", lPRH.Text);
            }
            else
            {
                HideShowApp(0, pRPS, "Show", lPRH.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            if(btnCStart.Text.Equals("Start"))
            {

                StartCloseApp(true, "eDNB.Contacts", @"C:\wamp\www\App\idnb\contacts\eDNB.Contacts.exe", btnCStart, "Stop", btnCOpen, true, lblCO, lblCCO, Color.Green, progressBar6, true);
            }
            else
            {
                StartCloseApp(false, "eDNB.Contacts", "", btnCStart, "Start", btnCOpen, false, lblCO, lblCCO, Color.Red, progressBar6, false);
            }
        }

        private void btnCOpen_Click(object sender, EventArgs e)
        {
            if(btnCOpen.Text.Equals("Show"))
            {
                HideShowApp(1, btnCOpen, "Hide", lblCO.Text);
            }
            else
            {
                HideShowApp(0, btnCOpen, "Show", lblCO.Text);
            }
        }
    }
}
