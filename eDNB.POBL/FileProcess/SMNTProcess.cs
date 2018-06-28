using Core.Common.Folder;
using Core.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDNB.POBL.FileProcess
{
    public class SMNTProcess : RankFolder
    {
        OperationResult op = new OperationResult();

        public string smntFolder = @"C:\Statement";
        
        public SMNTProcess()
        {
            srcF = smntFolder;        
        }
    }
}
