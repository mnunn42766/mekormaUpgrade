using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.IO;
using CMS.Helpers;
using CMS.Base;

/// <summary>
/// Sample of override of methods from CMS.Storage.File class.
/// </summary>
public class CustomFile : CMS.FileSystemStorage.File
{
    #region "Variables"

    private static string mCustomStorageRootUrl = null;

    #endregion


    #region "Properties"

    /// <summary>
    /// Returns custom storage root URL from web.config file.
    /// </summary>
    private static string CustomStorageRootUrl
    {
        get
        {
            if (mCustomStorageRootUrl == null)
            {
                mCustomStorageRootUrl = ValidationHelper.GetString(SettingsHelper.AppSettings["CMSCustomStorageRootUrl"], string.Empty);
            }

            return mCustomStorageRootUrl;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Returns URL to file. If can be accessed directly then direct URL is generated else URL with GetFile page is generated.
    /// </summary>
    /// <param name="path">Virtual path starting with ~ or absolute path.</param>
    /// <param name="siteName">Site name.</param>
    public override string GetFileUrl(string path, string siteName)
    {
        AbstractStorageProvider provider = AbstractStorageProvider.GetStorageProvider(path);
        
        string originalPath = path;

        if (path.StartsWith("~"))
        {
            path = path.Substring(1);
        }

        if (!string.IsNullOrEmpty(provider.CustomRootUrl))
        {
            return provider.CustomRootUrl + path;
        }
        else if (!string.IsNullOrEmpty(CustomStorageRootUrl))
        {
            return CustomStorageRootUrl + path;
        }

        return base.GetFileUrl(originalPath, siteName);
    }

    #endregion
}
