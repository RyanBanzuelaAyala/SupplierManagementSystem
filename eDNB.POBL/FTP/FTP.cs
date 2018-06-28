
using Core.Common.Authorization;
using Core.Common.Result;
using System.IO;
using System.Net;

namespace eDNB.POBL.FTP
{
    public class ftp 
    {        
        private string host = null;
        private string user = Auth._ftpUser;
        private string pass = Auth._ftpPw;
        private string dir = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private int bufferSize = 2048;

        /// <summary>
        /// ftp Constructor
        /// </summary>
        /// <param name="hostIP"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="directory"></param>
        public ftp(string hostIP, string directory)
        {
            host = hostIP; dir = directory;
        }
               
        /// <summary>
        /// Upload PDF files in the server 
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="localFile"></param>
        /// <returns></returns>
        public OperationResult Upload(string remoteFile, string localFile)
        {
            var op = new OperationResult();

            if(string.IsNullOrWhiteSpace(localFile))
            {
                op.Success = false;
                op.AddMessage("remoteFile must not be empty");

                return op;
            }                            

            if(string.IsNullOrWhiteSpace(remoteFile))
            {
                op.Success = false;
                op.AddMessage("localFile must not be empty");

                return op;
            }
                                             
            //if(!DirectoryExists(dir)) CreateDirectory(dir);

            try
            {

                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Path.Combine(host, remoteFile));
                        
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;            
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Proxy = null;

                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                //ftpRequest.EnableSsl = false;

                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
            
                FileStream localFileStream = new FileStream(localFile, FileMode.Open);

                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                            
                while(bytesSent != 0)
                {
                    ftpStream.Write(byteBuffer, 0, bytesSent);
                    bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                }

                op.Success = true;
                op.AddMessage("Successfully Uploaded.");

                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;

            }
            catch(WebException ex)
            {
                op.Success = false;
                op.AddMessage(ex.Message);
            }

            return op;           
        }

        /// <summary>
        /// Check if directory folder is existing in the server
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private bool DirectoryExists(string directory)
        {            
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Path.Combine(host, directory));
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Proxy = null;
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                            
                using(ftpRequest.GetResponse())
                {
                    return true;
                }
            }
            catch(WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Create directory folder in the server
        /// </summary>
        /// <param name="newDirectory"></param>
        /// <returns></returns>
        private bool CreateDirectory(string newDirectory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(Path.Combine(host, newDirectory));
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Proxy = null;
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();

                ftpRequest = null;

                return true;
            }
            catch(WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Download file from server
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="localFile"></param>
        public OperationResult download(string remoteFile, string localFile)
        {
            var op = new OperationResult();

            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Path.Combine(host, remoteFile));
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.EnableSsl = false;

                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                try
                {
                    while(bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch(WebException ex)
                {
                    op.Success = false;
                    op.AddMessage(ex.Message);
                }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch(WebException ex)
            {
                op.Success = false;
                op.AddMessage(ex.Message);

            }
            
            return op;
        }

        /// <summary>
        /// Delete PDF file in Server
        /// </summary>
        /// <param name="delete"></param>
        /// <returns></returns>
        public bool delete(string filepath)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + filepath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Proxy = null;
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Commnication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Response Cleanup */
                ftpResponse.Close();
                ftpRequest = null;

                return true;                
            }
            catch(WebException)
            {
                return false;
            }
        }
    }
}
