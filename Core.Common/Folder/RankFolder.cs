using Core.Common.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Folder
{
    public class RankFolder : ProcessFolder
    {
        OperationResult op = new OperationResult();

        /// <summary>
        /// Create a file
        /// </summary>
        /// <param name="path"></param>
        public OperationResult CreateFile(string path)
        {
            if(File.Exists(path))
            {
                op.Success = false;
                op.AddMessage("File already exist");

                return op;
            }
            else
            {
                File.Create(path).Close();
                op.Success = true;

                return op;
            }
        }
        
        /// <summary>
        /// Get all files specifically by Type
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public FileInfo[] GetFileToFolder(string path, string fileType)
        {
            DirectoryInfo d = new DirectoryInfo(path);

            FileInfo[] files = d.GetFiles("*." + fileType);

            return files;
        }
                
    }
}
