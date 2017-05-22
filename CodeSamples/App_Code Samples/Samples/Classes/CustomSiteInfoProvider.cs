using System;

using CMS;
using CMS.EventLog;
using CMS.SiteProvider;

// Custom provider registration. Uncomment the following line to enable the custom provider.
//[assembly: RegisterCustomProvider(typeof(CustomSiteInfoProvider))]

/// <summary>
/// Sample custom user info provider, does log an event upon the user update.
/// </summary>
public class CustomSiteInfoProvider : SiteInfoProvider
{
    /// <summary>
    /// Sets the specified site data.
    /// </summary>
    /// <param name="siteInfoObj">New site info data</param>
    protected override void SetSiteInfoInternal(SiteInfo siteInfoObj)
    {
        base.SetSiteInfoInternal(siteInfoObj);

        // Log the event that the site was updated
        EventLogProvider.LogEvent(EventType.INFORMATION, "MyCustomSiteInfoProvider", "SetSiteInfo", "The site was updated", null);
    }
}