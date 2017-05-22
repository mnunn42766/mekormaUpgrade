using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Microsoft.WindowsAzure.ServiceRuntime;

/// <summary>
/// Azure web role.
/// </summary>
public class WebRole : RoleEntryPoint
{
    #region "Override methods"

    /// <summary>
    /// On start event.
    /// </summary>    
    public override bool OnStart()
    {
        RoleEnvironment.Changing += RoleEnvironmentChanging;
        
        return base.OnStart();
    }

    #endregion


    #region "Event handlers"

    /// <summary>
    /// Role environment changing event. Fired before change is applied.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Arguments.</param>
    private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
    {
        // If a configuration setting is changing
        if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
        {
            // Set e.Cancel to true to restart this role instance
            e.Cancel = true;
        }
    }


    /// <summary>
    /// OnStop() event handling.
    /// </summary>
    public override void OnStop()
    {
        try
        {
            Trace.TraceInformation("[CMSApp.WebRole.OnStop]: OnStop called");
            var performanceCounter = new PerformanceCounter("ASP.NET", "Requests Current", "");

            while (true)
            {
                var requestsCount = performanceCounter.NextValue();
                Trace.TraceInformation("[CMSApp.WebRole.OnStop]: Pending requests count: " + requestsCount);
                if (requestsCount <= 0)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
        }
        catch (Exception e)
        {
            Trace.TraceError(e.Message);
        }
    }

    #endregion
}