using Core.Common.Extension;
using Core.Common.Folder;
using Core.Common.Log;
using eApp.Win.RankPro.ADO;
using eDNB.POBL.Email;
using eDNB.POBL.FileProcess;
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
    public partial class iProcess : Form
    {       

        #region Delegates Declaration

        public delegate void SystemLog(object sender, LOGArgs e);
        public event SystemLog SysLog;

        void _sysLog(string msg, Color color, StatusType typ)
        {
            SysLog(this, new LOGArgs(msg, color, typ));
        }

        RankFolder rFolder = new RankFolder();

        RANKProcess rProcess = new RANKProcess();
        
        dnbmssqlEntities db = new dnbmssqlEntities();

        public iProcess()
        {
            InitializeComponent();
        }


        #endregion
        
        #region Process all Files

        private void iProcess_Load(object sender, EventArgs e)
        {
            Bg_DoWork();

            _checkErrorFile();

            _checkErrorFilePDF();

            _sysLog("Success..", Color.Green, StatusType.Sys);

            //Email.SendEmail("DNB : " + POCount.ToString() + " PO has successfully processed.", "DNB : PO Report-");

            this.Close();
        }

        private void _checkErrorFile()
        {
            var checkError = rProcess.GetAllFile(rProcess.errFolder, "txt");

            if(!checkError.Count().Equals(0))
            {
                // Loop on Error Folder if it has PDF
                foreach(var item in checkError)
                {
                    // Move PDF -> _public folder                
                    rProcess.moveToSrcFolder(item);
                }
            }
        }

        private void _checkErrorFilePDF()
        {
            var checkError = rProcess.GetAllFile(rProcess.errFolder, "pdf");

            if(!checkError.Count().Equals(0))
            {
                // Loop on Error Folder if it has PDF
                foreach(var item in checkError)
                {
                    // Move PDF -> _public folder                
                    rProcess.moveToTmpFolder(item);
                    
                }
            }
        }
        
        public void _convertTxtToPDF(string item)
        {
            var tempFile = Path.Combine(rProcess.tmpFolder, item);

            new eDNB.POBL.Utilities.PDF().ConvertToPDF(tempFile + ".txt", tempFile + ".pdf");

            File.Delete(tempFile + ".txt");
        }
        
        #endregion

        #region Upload PDF -> Server

        private void Bg_DoWork()
        {
            // Get all PDF files in Public Folder            
            var files = rFolder.GetFileToFolder(rProcess.genFolder, "txt");

            // Check if there is PO
            if(!files.Count().Equals(0))
            {
                // Get 10 PDF - Move to Temp Folder and Process it
                foreach(var chunk in files.Split(10))
                {
                    // loop one by one and process each PDF
                    foreach(var item in chunk)
                    {
                        var crFile = Path.Combine(rProcess.tmpFolder, item.Name);

                        // catch an error if file already in Temp Folder
                        try
                        {
                            // move PDF to Temp Folder
                            rProcess.moveToTmpFolder(item);

                            // Convert Text file to PDF and delete Text File                        
                            _convertTxtToPDF(Path.GetFileNameWithoutExtension(item.Name));

                        }
                        catch
                        {
                            // update richbox of current exception status
                            updateLog(item, "Error", Color.Red);
                                
                            // move PDF to Error Folder
                            rProcess.moveToErrFolder(item);
                        }
                    }

                    // after moving the PDF to temp folder it is time to process it using Parallel Threading.                    
                    ProcessParallelThread();
                }
            }
        }

        void ProcessParallelThread()
        {
            var Files = rProcess.GetFileToFolder(rProcess.tmpFolder, "pdf");

            foreach(var xFile in Files)
            {
                try
                {
                    
                    var ftp = new ftp(@"ftp://188.121.43.20/services.danubeco.com/", "ranking");

                    // Upload PDF using FTP to Server Folder -> supplierx         
                    var result = ftp.Upload(Path.Combine("ranking", xFile.Name), Path.Combine(new RANKProcess().tmpFolder, xFile.Name));
                    
                    if(result.Success)
                    {
                        var reqIDD = Path.GetFileNameWithoutExtension(xFile.Name);

                        var newObj = db.reqlists.FirstOrDefault(s => s.reqid.Equals(reqIDD));

                        if(newObj.sts.Equals("processing"))
                        {
                            newObj.sts = "processed";

                            newObj.lnk = xFile.Name;

                            db.SaveChanges();
                        }
                        
                        rProcess.moveToBckFolder(xFile);

                        updateLog(xFile, "Success", Color.Green);
                    }
                    else
                    {
                        rProcess.moveToErrFolder(xFile);

                        updateLog(xFile, "ErrorFTP", Color.Red);
                    }

                }
                catch(Exception ex)
                {
                    rProcess.moveToErrFolder(xFile);

                    updateLog(xFile, "ErrorFile : " + ex.Message, Color.Red);
                }
            }
            
        }

        #endregion

        #region Func Methods
        
        private void updateLog(FileInfo localfile, string type, Color clr)
        {

            _sysLog("[" + Path.GetFileNameWithoutExtension(localfile.Name) + "] : " + type, clr, StatusType.Err);

        }

        #endregion
    }
}


public partial class requestResponse
{
    public string reqid { get; set; }
    public string sts { get; set; }
    public string lnk { get; set; }
    public string lnka { get; set; }
}