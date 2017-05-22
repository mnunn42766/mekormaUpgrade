using CMS;
using CMS.AzureStorage;
using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.WebFarmSync;

using Microsoft.WindowsAzure.ServiceRuntime;

[assembly: AssemblyDiscoverable]
[assembly: RegisterModule(typeof(AzureInit))]

/// <summary>
/// Intitializes Azure web role when its running using full IIS.
/// </summary>
public class AzureInit : Module
{
    #region "Constructor"

    /// <summary>
    /// Default constructor.
    /// </summary>
    public AzureInit()
        : base("AzureInit")
    {
    }

    #endregion


    #region "Events"

    /// <summary>
    /// OnPreInit event handler.
    /// </summary>
    protected override void OnPreInit()
    {
        ApplicationStartInit();
        base.OnPreInit();
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Code executed on application start.
    /// </summary>
    private void ApplicationStartInit()
    {
        // Provide the configuration to the engine
        CMSAppSettings.GetApplicationSettings += GetApplicationSettings;
        CMSConnectionStrings.GetConnectionString += GetApplicationSettings;

        AzureHelper.CurrentInstanceID = RoleEnvironment.CurrentRoleInstance.Id;
        AzureHelper.OnRestartRequired += (sender, args) => RoleEnvironment.RequestRecycle();

        // Get path for Temp
        LocalResource temp = RoleEnvironment.GetLocalResource("AzureTemp");
        PathHelper.TempPath = temp.RootPath;

        // Get path for Cache
        LocalResource cache = RoleEnvironment.GetLocalResource("AzureCache");
        PathHelper.CachePath = cache.RootPath;

        // Get internal instance endpoints
        foreach (var instance in RoleEnvironment.Roles["CMSApp"].Instances)
        {
            // Current instance ID
            if (instance.Id == RoleEnvironment.CurrentRoleInstance.Id)
            {
                // Set current internal endpoint
                RoleInstanceEndpoint endpoint = instance.InstanceEndpoints["InternalHttpIn"];
                AzureHelper.CurrentInternalEndpoint = "http://" + endpoint.IPEndpoint;
            }
        }

        // Set Azure deployment
        AzureHelper.DeploymentID = RoleEnvironment.DeploymentId;

        // Set number of instances
        AzureHelper.NumberOfInstances = RoleEnvironment.Roles["CMSApp"].Instances.Count;

        // Setup Web farm server name (required for both install and application)
        SystemContext.ServerName = ValidationHelper.GetCodeName(AzureHelper.CurrentInstanceID + "_" + AzureHelper.DeploymentID); 
    }


    /// <summary>
    /// Reads settings from service configuration file. 
    /// </summary>
    /// <param name="key">Setting key.</param>    
    private string GetApplicationSettings(string key)
    {
        try
        {
            return RoleEnvironment.GetConfigurationSettingValue(key);
        }
        catch
        {
            // Setting key was not found
            return null;
        }
    }

    #endregion
}
