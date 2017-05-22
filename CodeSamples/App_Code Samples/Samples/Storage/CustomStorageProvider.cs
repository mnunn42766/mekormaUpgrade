using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.IO;

/// <summary>
/// This sample class shows how to override method for creating AbstractFile instance.
/// </summary>
public class CustomStorageProvider : StorageProvider
{
    /// <summary>
    /// Creates instance of FileProvider object.
    /// </summary>
    protected override AbstractFile CreateFileProviderObject()
    {
        return new CustomFile();
    }
}
