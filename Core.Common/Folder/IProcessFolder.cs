using Core.Common.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Folder
{
    public interface IProcessFolder
    {
        FileInfo[] GetAllFile(string path, string filetype);

        OperationResult moveToTmpFolder(FileInfo file);

        OperationResult moveToBckFolder(FileInfo file);

        OperationResult moveToErrFolder(FileInfo file);

        void CreateFolder(string path);
    }
}
