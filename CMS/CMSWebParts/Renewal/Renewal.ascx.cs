using System;

using CMS.Helpers;
using CMS.Localization;
using CMS.PortalControls;
using CMS.SiteProvider;
using CMS.WebAnalytics;

public partial class CMSWebParts_Renewal_Renewal : CMSAbstractWebPart
{

    protected override void OnLoad(EventArgs e)
    {

        base.OnLoad(e);
    }


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();        
    }


    /// <summary>
    /// Reloads data for partial caching.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
      
        }
        else
        {
      
        }
    }


}