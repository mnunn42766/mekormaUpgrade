using System;

using CMS.IO;

namespace CMS.CustomFileSystemProvider
{
    /// <summary>
    /// Sample of DirectoryInfo class object of CMS.IO provider.
    /// </summary>
    class DirectoryInfo : CMS.IO.DirectoryInfo
    {

        #region "Constructors"

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Path to directory</param>
        public DirectoryInfo(string path)
        {
        }

        #endregion


        #region "Public properties"

        /// <summary>
        /// Full name of directory (whole path).
        /// </summary>
        public override string FullName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Last write time to directory.
        /// </summary>
        public override DateTime LastWriteTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Name of directory (without path).
        /// </summary>
        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Creation time.
        /// </summary>
        public override DateTime CreationTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Whether directory exists.
        /// </summary>
        public override bool Exists
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Parent directory.
        /// </summary>
        public override CMS.IO.DirectoryInfo Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Creates subdirectory.
        /// </summary>
        /// <param name="subdir">Subdirectory to create.</param>
        protected override CMS.IO.DirectoryInfo CreateSubdirectoryInternal(string subdir)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes directory
        /// </summary>
        protected override void DeleteInternal()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns an array of directories in the current DirectoryInfo matching the given search criteria and using a value
        /// to determine whether to search subdirectories.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">Specifies whether to search the current directory, or the current directory and all subdirectories.</param>
        protected override IO.DirectoryInfo[] GetDirectoriesInternal(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }
        

        /// <summary>
        /// Returns files of the current directory.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">Whether return files from top directory or also from any subdirectories.</param>
        protected override CMS.IO.FileInfo[] GetFilesInternal(string searchPattern, CMS.IO.SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
