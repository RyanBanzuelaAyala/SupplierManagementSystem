using Core.Common.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Folder
{
    public abstract class ProcessFolder : IProcessFolder
    {
        protected string srcF;
        protected string tmpF;
        protected string bckF;        
        protected string errF;

        OperationResult op = new OperationResult();

        /// <summary>
        /// Get all Files in the Folder and return it as List
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileInfo[] GetAllFile(string path, string filetype)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            DirectoryInfo d = new DirectoryInfo(path);

            FileInfo[] files = d.GetFiles("*." + filetype);

            return files;
        }
        
        /// <summary>
        /// Move the File to Source Folder
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual OperationResult moveToSrcFolder(FileInfo file)
        {
            if(File.Exists(Path.Combine(srcF, file.Name)))
            {
                File.Delete(file.FullName);

                op.Success = false;

                op.AddMessage("File must be existing");

                return op;
            }

            try
            {
                File.Move(file.FullName, Path.Combine(srcF, file.Name));

                op.Success = true;
            }
            catch(Exception e)
            {
                op.Success = false;

                op.AddMessage(e.Message);
            }

            return op;
        }
        
        /// <summary>
        /// Move the File to Temporary Folder
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual OperationResult moveToTmpFolder(FileInfo file)
        {
            if(File.Exists(Path.Combine(tmpF, file.Name)))
            {
                File.Delete(file.FullName);

                op.Success = false;

                op.AddMessage("File must be existing");

                return op;
            }

            try
            {
                File.Move(file.FullName, Path.Combine(tmpF, file.Name));

                op.Success = true;                
            }            
            catch (Exception e)
            {
                op.Success = false;

                op.AddMessage(e.Message);
            }

            return op;
        }

        /// <summary>
        /// Move the File to Backup Folder
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual OperationResult moveToBckFolder(FileInfo file)
        {
            if(File.Exists(Path.Combine(bckF, file.Name)))
            {
                File.Delete(file.FullName);

                op.Success = false;

                op.AddMessage("File must be existing");
                
                return op;
            }

            try
            {
                CreateFolder(bckF);

                File.Move(file.FullName, Path.Combine(bckF, file.Name));

                op.Success = true;
            }
            catch(Exception e)
            {
                op.Success = false;

                op.AddMessage(e.Message);
            }

            return op;            
        }

        /// <summary>
        /// Move the File to Error Folder
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual OperationResult moveToErrFolder(FileInfo file)
        {
            if(File.Exists(Path.Combine(errF, file.Name)))
            {
                File.Delete(file.FullName);

                op.Success = false;

                op.AddMessage("File must be existing");

                return op;
            }

            try
            {
                File.Move(file.FullName, Path.Combine(errF, file.Name));

                op.Success = true;
            }
            catch(Exception e)
            {
                op.Success = false;

                op.AddMessage(e.Message);
            }

            return op;
        }
        
        /// <summary>
        /// Create the Path Folder
        /// </summary>
        /// <param name="path"></param>
        public void CreateFolder(string path)
        {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        
    }
}
