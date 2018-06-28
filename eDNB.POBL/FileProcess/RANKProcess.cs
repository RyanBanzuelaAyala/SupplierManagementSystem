using Core.Common.Folder;
using Core.Common.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDNB.POBL.FileProcess
{
    public class RANKProcess : RankFolder
    {
        OperationResult op = new OperationResult();

        public string srvFolder = @"C:\wamp\www\App\dnb.app\rank";

        public string genFolder = @"C:\wamp\www\App\dnb.bck\rank";
        public string tmpFolder = @"C:\wamp\www\App\dnb.bck\rank\_mdr";
        public string tmpXFolder = @"C:\wamp\www\App\dnb.bck\rank\_tmp";
        public string errFolder = @"C:\wamp\www\App\dnb.bck\rank\_xrm";

        public string bckFolder = Path.Combine(@"D:\Appbk\dnb.backup\rank", DateTime.Now.ToString("yyyyMMdd"));
              
        public RANKProcess()
        {
            srcF = genFolder;
            tmpF = tmpFolder;
            bckF = bckFolder;
            errF = errFolder;
        }
                
        #region Method for Checking Existence

        /// <summary>
        /// Remove Request CSV file if existing
        /// </summary>
        /// <param name="_pth"></param>
        public OperationResult RemoveIfRequestIsExist(string[] _pth)
        {
            try
            {
                foreach(var item in _pth)
                    if(File.Exists(item))
                        File.Delete(item);

                op.Success = true;
                return op;

            }
            catch (Exception)
            {
                op.Success = false;
                op.AddMessage("Error removing a file");

                return op;
            }        
            
        }

        /// <summary>
        /// Generate new CSV file if not existing
        /// </summary>
        /// <param name="_pth"></param>
        public OperationResult GenerateIfNotExist(string[] _pth)
        {          
            try
            {
                foreach(var item in _pth)
                    if(!File.Exists(item))
                        File.Create(item).Close();

                op.Success = true;
                return op;

            }
            catch(Exception)
            {
                op.Success = false;
                op.AddMessage("Error generating a file");

                return op;
            }
        }

        #endregion

        
        
    }
}


