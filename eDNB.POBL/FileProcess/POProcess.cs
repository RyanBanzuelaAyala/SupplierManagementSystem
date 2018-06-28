using Core.Common.Folder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDNB.POBL.FileProcess
{
    public class POProcess : ProcessFolder
    {
        public string srcFolder = @"C:\wamp\www\App\dnb.app\po";

        public string tmpFolder = @"C:\wamp\www\App\dnb.app\po\_mdr";

        public string errFolder = @"C:\wamp\www\App\dnb.app\po\_xrm";

        public string tmpXFolder = @"C:\wamp\www\App\dnb.app\po\_tmp";

        public string bckFolder = Path.Combine(@"D:\Appbk\dnb.backup\po", DateTime.Now.ToString("yyyyMMdd"));
        
        public POProcess()
        {
            srcF = srcFolder;
            tmpF = tmpFolder;
            errF = errFolder;
            bckF = bckFolder;            
        }
    }
}
