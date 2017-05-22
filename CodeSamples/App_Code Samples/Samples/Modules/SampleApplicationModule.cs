using System;

using CMS.Base;

/// <summary>
/// Sample handler module
/// </summary>
[SampleApplicationModuleLoader]
public partial class CMSModuleLoader
{
    /// <summary>
    /// Module registration
    /// </summary>
    private class SampleApplicationModuleLoader : CMSLoaderAttribute
    {
        /// <summary>
        /// PreInitializes the module
        /// </summary>
        public override void PreInit()
        {
            // Uncomment following line to initialize the connection string programmatically
            // SettingsHelper.ConnectionStrings.SetConnectionString("CMSConnectionString", "<enter connection string>");
        }
    }
}