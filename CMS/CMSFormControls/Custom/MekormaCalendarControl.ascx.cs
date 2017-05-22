using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Base;
//using CMS.ExtendedControls;
//using CMS.FormControls;
using CMS.Globalization;
using CMS.Helpers;
using CMS.MacroEngine;

public partial class CMSFormControls_Custom_MekormaCalendarControl /*: *//**FormEngineUserControl*/
{


    /// <summary>
    /// Gets or sets form control value.
    /// </summary>
    public override object Value
    {
        get
        {
            return mk_datepicker.Text;
        }
        set
        {
            mk_datepicker.Text = (string)value;
        }
    }

   

    public bool DisableWeekend
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisableWeekend"), false);
        }
        set
        {
            SetValue("DisableWeekend", value);
        }
    }

    private object GetValue(string v)
    {
        throw new NotImplementedException();
    }

    public bool SelectOnlyMonthAndDay
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyMonthAndDay"), false);
        }
        set
        {
            SetValue("SelectOnlyMonthAndDay", value);
        }
    }

    public bool ShowOnlyFutureDates
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowOnlyFutureDates"), false);
        }
        set
        {
            SetValue("ShowOnlyFutureDates", value);
        }
    }

    private void SetValue(string v, bool value)
    {
        throw new NotImplementedException();
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (DisableWeekend)
        {
            noWeekendHdn.Value = "true";
        }

        if (SelectOnlyMonthAndDay)
        {
            monthAndDayHdn.Value = "true";
        }

        if (ShowOnlyFutureDates)
        {
            onlyFutureDatesHdn.Value = "true";
        }

    }
    /// <summary>
    /// Returns true if user control is valid.
    /// </summary>
    //public override bool IsValid()
    //{
       
    //}
}