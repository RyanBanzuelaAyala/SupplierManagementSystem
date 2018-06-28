using Core.Common.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDNB.POBL.FileProcess
{
    public class SYNCProcess : RankFolder
    {
        public string srvFolder = @"C:\wamp\www\App\dnb.app\sup";

        public string srcFolder = @"C:\wamp\www\App\dnb.app\sup\DNB_Vendor_List.txt";

        public string bckFolder = @"C:\wamp\www\App\dnb.bck\sup";
    }
}
