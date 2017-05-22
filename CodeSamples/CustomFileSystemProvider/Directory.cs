using System;
using System.Security.AccessControl;

using CMS.IO;

namespace CMS.CustomFileSystemProvider
{
    /// <summary>
    /// Sample of Directory class of CMS.IO provider.
    /// </summary>
    public class Directory : CMS.IO.AbstractDirectory
    {
        #region "Public override methods"

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">Path to test.</param>  
        public override bool Exists(string path)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates all directories and subdirectories as specified by path.
        /// </summary>
        /// <param name="path">Path to create.</param> 
        public override CMS.IO.DirectoryInfo CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">Path to retrieve files from.</param> 
        public override string[] GetFiles(string path)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">Path to retrieve files from.</param> 
        /// <param name="searchPattern">Search pattern.</param>
        public override string[] GetFiles(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets the names of subdirectories in the specified directory.
        /// </summary>
        /// <param name="path">Path to retrieve directories from.</param> 
        public override string[] GetDirectories(string path)
        {
            return GetDirectories(path, "*");
        }


        /// <summary>
        /// Gets the names of subdirectories in the specified directory.
        /// </summary>
        /// <param name="path">Path to retrieve directories from.</param> 
        /// <param name="searchPattern">Pattern to be searched.</param> 
        public override string[] GetDirectories(string path, string searchPattern)
        {
            return GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }


        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the specified search
        /// pattern in the current directory, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">Path to retrieve directories from.</param>
        /// <param name="searchPattern">Pattern to be searched.</param>
        /// <param name="searchOption">Specifies whether to search the current directory, or the current directory and all subdirectories.</param>
        public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets the current working directory of the application.
        /// </summary>  
        public override string GetCurrentDirectory()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes an empty directory and, if indicated, any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <param name="recursive">If delete if sub directories exists.</param>
        public override void Delete(string path, bool recursive)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Moves directory.
        /// </summary>
        /// <param name="sourceDirName">Source directory name.</param>
        /// <param name="destDirName">Destination directory name.</param>
        public override void Move(string sourceDirName, string destDirName)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes an empty directory.
        /// </summary>
        /// <param name="path">Path to directory</param>        
        public override void Delete(string path)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets a FileSecurity object that encapsulates the access control list (ACL) entries for a specified file.
        /// </summary>
        /// <param name="path">Path to directory.</param>        
        public override DirectorySecurity GetAccessControl(string path)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Prepares files for import. Converts them to media library.
        /// </summary>
        /// <param name="path">Path.</param>
        public override void PrepareFilesForImport(string path)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes all files in the directory structure. It works also in a shared hosting environment.
        /// </summary>
        /// <param name="path">Full path of the directory to delete</param>
        public override void DeleteDirectoryStructure(string path)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
