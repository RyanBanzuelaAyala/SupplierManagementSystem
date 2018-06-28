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
    public class PAYProcess : RankFolder
    {
        OperationResult op = new OperationResult();

        public string srvFolderReq = @"C:\wamp\www\_paymentreq";

        public string srvFolderPro = @"C:\wamp\www\_paymentpro";

        public string srvFolderProTemp = @"C:\wamp\www\_paymentpro\_mdr";

        public string srvFolderProErr = @"C:\wamp\www\_paymentpro\_xrm";

        public string srvPaymentBackup = @"C:\Payment";

        public PAYProcess()
        {
            srcF = srvFolderPro;
            tmpF = srvFolderProTemp;            
            errF = srvFolderProErr;

            bckF = srvPaymentBackup;
        }

        
    }
}
