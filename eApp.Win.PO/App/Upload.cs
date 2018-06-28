using Core.Common.Extension;
using Core.Common.Log;
using eApp.Win.PO.ADO;
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

namespace eApp.Win.PO.App
{
    public partial class Upload : Form
    {
        #region Declaration

        POProcess FolderPO = new POProcess();

        BackgroundWorker bg = new BackgroundWorker();
        
        #region Delegates Declaration

        public delegate void SystemLog(object sender, LOGArgs e);
        public event SystemLog SysLog;

        void _sysLog(string msg, Color color, StatusType typ)
        {
            SysLog(this, new LOGArgs(msg, color, typ));
        }

        #endregion
        
        public Upload()
        {
            InitializeComponent();
        }

        #endregion

        #region File Process
        
        private void Upload_Load(object sender, EventArgs e)
        {
            Bg_DoWork();

            Bg_checkErrorFile();

            //Email.SendEmail("DNB : " + POCount.ToString() + " PO has successfully processed.", "DNB : PO Report-");

            this.Close();
        }

        private void Bg_checkErrorFile()
        {
            var checkError = FolderPO.GetAllFile(FolderPO.errFolder, "txt");

            if(!checkError.Count().Equals(0))
            {
                // Loop on Error Folder if it has PDF
                foreach(var item in checkError)
                {
                    // Move PDF -> _public folder                
                    FolderPO.moveToSrcFolder(item);
                }
            }

            var checkErrorPDF = FolderPO.GetAllFile(FolderPO.errFolder, "pdf");

            if(!checkErrorPDF.Count().Equals(0))
            {
                // Loop on Error Folder if it has PDF
                foreach(var item in checkErrorPDF)
                {
                    // Move PDF -> _public folder                
                    FolderPO.moveToTmpFolder(item);
                }
            }
        }

        private void Bg_convertTxtToPDF(string item)
        {
            var tempFile = Path.Combine(FolderPO.tmpFolder, item);

            new eDNB.POBL.Utilities.PDF().ConvertToPDFPO(tempFile + ".txt", tempFile + ".pdf", "Danube.jpg");

            File.Delete(tempFile + ".txt");
        }

        private void Bg_DoWork()
        {
            // Get all PDF files in Public Folder
            var files = FolderPO.GetAllFile(FolderPO.srcFolder, "txt");

            // Check if there is PO
            if(files.Count().Equals(0))
                return;

            // Get 10 PDF - Move to Temp Folder and Process it
            foreach(var chunk in files.Split(10))
            {
                // loop one by one and process each PDF
                foreach(var item in chunk)
                {
                    // catch an error if file already in Temp Folder
                    try
                    {
                        // move text file to Temp Folder
                        FolderPO.moveToTmpFolder(item);

                        // after moving convert Text to PDF
                        Bg_convertTxtToPDF(Path.GetFileNameWithoutExtension(item.Name));
                    }
                    catch
                    {
                        // update richbox of current exception status
                        updateLog(item, "Error", Color.Red);
                        // move PDF to Error Folder
                        FolderPO.moveToErrFolder(item);
                    }
                }
                
                // after moving the PDF to temp folder it is time to process it using Parallel Threading.
                ProcessParallelThread();
                                
            }
        }

        public void ProcessParallelThread()
        {
            // Get all PDF -> _mdr
            var Files = FolderPO.GetAllFile(FolderPO.tmpFolder, "pdf");

            dnbmssqlEntities db = new dnbmssqlEntities();

            foreach (var xFile in Files)
            {
                try
                {

                    #region Convert FileInfo to PDF Object
                    
                    // Split PDF Filename into Object
                    var pdf = SplitPdf(xFile);

                    #endregion

                    #region FTP Processes
                    
                    // Set ftp values                                                               
                    var ftp = new ftp(@"ftp://188.121.43.20/services.danubeco.com/", "supplier");

                    // Upload PDF using FTP to Server Folder -> supplierx         
                    var result = ftp.Upload(Path.Combine("supplier", xFile.Name), Path.Combine(FolderPO.tmpFolder, xFile.Name));

                    if (result.Success)
                    {

                        #region Set PO Object

                        // ----------------------------------------------- PO ---------------------------------------------------------

                        var newPO = new po
                        {
                            sid = pdf.sid,
                            pono = pdf.pono,
                            region = pdf.region,
                            location = pdf.location,
                            division = pdf.division,
                            link = xFile.Name,
                            filestatus = "Available",
                            released = DateTime.Now.ToString("yyyy-MM-dd"),
                            expiration = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"),
                            isexpired = "no"
                        };

                        #endregion

                        #region Store PO Object in Database
                        
                        var isExist = db.poes.FirstOrDefault(p => p.pono.Equals(newPO.pono));

                        if (isExist != null)
                        {
                            isExist.filestatus = "Updated";
                            isExist.link = newPO.link;
                        }
                        else
                        {
                            db.poes.Add(newPO);
                        }

                        #endregion

                        #region Set SMS

                        // ----------------------------------------------- SMS ---------------------------------------------------------

                        var isExists = db.sms.FirstOrDefault(x => x.sid.Equals(newPO.sid) && x.region.Equals(newPO.region));

                        if (isExists == null)
                        {
                            var user = db.supplierregions.FirstOrDefault(s => s.sid.Equals(newPO.sid) && s.region.Equals(newPO.region));

                            if (user != null)
                            {
                                if (user.sms.Equals("y"))
                                {
                                    db.sms.Add(new sm
                                    {
                                        sid = newPO.sid,
                                        region = newPO.region,
                                        icurrent = user.mobile,
                                        timesent = DateTime.Now.ToString(),
                                        status = "waiting"
                                    });
                                }
                            }
                        }

                        #endregion

                        #region Store & Backup File + Log

                        // save changes to Database
                        db.SaveChanges();

                        // move files to backup folder
                        FolderPO.moveToBckFolder(xFile);

                        // write log ni rtbox
                        updateLog(xFile, "Success", Color.Green);

                        #endregion

                    }
                    else
                    {

                        #region Backup File + Log
                        
                        // move to error folder
                        FolderPO.moveToErrFolder(xFile);

                        updateLog(xFile, "ErrorFTP", Color.Red);

                        #endregion

                    }

                    #endregion
                    
                }
                catch (Exception ex)
                {
                    // move to error folder
                    FolderPO.moveToErrFolder(xFile);

                    updateLog(xFile, "ErrorFile" + ex.Message, Color.Red);
                }
            }
            
        }
        
        #endregion

        #region Func Methods

        private void updateLog(FileInfo localfile, string type, Color clr)
        {

            _sysLog("[" + Path.GetFileNameWithoutExtension(localfile.Name) + "] : " + type, clr, StatusType.Err);

        }

        public PDF SplitPdf(FileInfo file)
        {
            string[] item = file.Name.Split('_');

            PDF pdf = new PDF();

            pdf.filename = file.Name;
            pdf.sid = item[0].ToString();
            pdf.pono = item[1].ToString();
            pdf.region = item[2].ToString();

            if(item.Length == 4) // has location
            {
                pdf.location = Path.GetFileNameWithoutExtension(item[3].ToString());
                pdf.division = "";
            }
            else if(item.Length == 5) // has location and division
            {
                pdf.location = item[3].ToString();
                pdf.division = Path.GetFileNameWithoutExtension(item[4].ToString());
            }

            return pdf;
        }

        #endregion
    }
}


public partial class PDF
{
    public string sid { get; set; }

    public string pono { get; set; }

    public string region { get; set; }

    public string location { get; set; }

    public string division { get; set; }

    public string filename { get; set; }
}