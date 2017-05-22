﻿using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Ecommerce;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.DataEngine.Query;

using TreeNode = CMS.DocumentEngine.TreeNode;


public partial class CMSModules_Ecommerce_Controls_UI_ProductFilter : CMSAbstractDataFilterControl
{
    #region "Variables"

    private TreeNode mParentNode;
    private CMSUserControl filteredControl;
    private string mFilterMode;
    private bool allowGlobalProducts;

    #endregion


    #region "Properties"

    /// <summary>
    /// Current filter mode.
    /// </summary>
    public override string FilterMode
    {
        get
        {
            if (mFilterMode == null)
            {
                mFilterMode = ValidationHelper.GetString(filteredControl.GetValue("FilterMode"), "").ToLowerCSafe();
            }
            return mFilterMode;
        }
        set
        {
            mFilterMode = value;
        }
    }


    /// <summary>
    /// Where condition.
    /// </summary>
    public override string WhereCondition
    {
        get
        {
            base.WhereCondition = GenerateWhereCondition().ToString(true);
            return base.WhereCondition;
        }
        set
        {
            base.WhereCondition = value;
        }
    }


    private TreeNode ParentNode
    {
        get
        {
            var uiContext = UIContextHelper.GetUIContext(this);
            return mParentNode ?? (mParentNode = uiContext.EditedObject as TreeNode);
        }
    }


    /// <summary>
    /// Gets or sets a value indicating whether the advanced filter is displayed or not. 
    /// </summary>
    private bool IsAdvancedMode
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["IsAdvancedMode"], false);
        }
        set
        {
            ViewState["IsAdvancedMode"] = value;
        }
    }


  #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        filteredControl = FilteredControl as CMSUserControl;

        // Hide filter button, this filter has its own
        UniGrid grid = filteredControl as UniGrid;
        if (grid != null)
        {
            grid.HideFilterButton = true;
        }

        allowGlobalProducts = ECommerceSettings.AllowGlobalProducts(SiteContext.CurrentSiteName);

        // Display Global and site option if global products are allowed
        siteElem.ShowSiteAndGlobal = allowGlobalProducts;

        // Initialize controls
        if (!URLHelper.IsPostback())
        {
            FillThreeStateDDL(ddlNeedsShipping);
            FillThreeStateDDL(ddlAllowForSale);
            FillDocumentTypesDDL();
            ResetFilter();
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // General UI
        lnkShowAdvancedFilter.Text = GetString("general.displayadvancedfilter");
        lnkShowSimpleFilter.Text = GetString("general.displaysimplefilter");

        btnFilter.Text = GetString("general.search");
        btnFilter.Click += btnFilter_Click;

        btnReset.Text = GetString("general.reset");
        btnReset.Click += btnReset_Click;

        // Setup filter mode
        SetFieldsVisibility();

        // When global SKUs can be included in listing
        if (allowGlobalProducts)
        {
            // Display global departments, manufacturers, suppliers and global statuses too
            departmentElem.DisplayGlobalItems = true;
            publicStatusElem.DisplayGlobalItems = true;
            internalStatusElem.DisplayGlobalItems = true;
            supplierElem.DisplayGlobalItems = true;
            manufacturerElem.DisplayGlobalItems = true;
        }

        plcSite.Visible = allowGlobalProducts;
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Creates where condition according to values selected in filter.
    /// </summary>
    private WhereCondition GenerateWhereCondition()
    {
        var where = new WhereCondition();

        string productNameColumnName = (ParentNode != null) ? "DocumentName" : "SKUName";

        // Append name/number condition
        var nameOrNumber = txtNameOrNumber.Text.Trim().Truncate(txtNameOrNumber.MaxLength);
        if (!string.IsNullOrEmpty(nameOrNumber))
        {
            where.Where(k => k.Where(w => w.WhereContains(productNameColumnName, nameOrNumber)
                                           .Or()
                                           .WhereContains("SKUNumber", nameOrNumber)));
        }

        // Append site condition
        if (allowGlobalProducts && (siteElem.SiteID != UniSelector.US_GLOBAL_AND_SITE_RECORD))
        {
            // Restrict SKUSiteID only for products not for product section (full listing mode)
            int selectedSiteID = (siteElem.SiteID > 0) ? siteElem.SiteID : 0;
            where.Where(w => w.WhereEquals("SKUSiteID".AsColumn().IsNull(0), selectedSiteID).Or().WhereNull("SKUID"));
        }

        // Append department condition
        if (departmentElem.SelectedID > 0)
        {
            where.WhereEquals("SKUDepartmentID", departmentElem.SelectedID);
        }
        else if (departmentElem.SelectedID == -5)
        {
            where.WhereNull("SKUDepartmentID");
        }

        // Append product type condition
        if ((selectProductTypeElem.Value != null) && (selectProductTypeElem.Value.ToString() != "ALL"))
        {
            where.WhereEquals("SKUProductType", selectProductTypeElem.Value);
        }

        // Manufacturer value
        if (manufacturerElem.SelectedID != UniSelector.US_ALL_RECORDS)
        {
            where.WhereEquals("SKUManufacturerID".AsColumn().IsNull(0), manufacturerElem.SelectedID);
        }

        // Supplier value
        if (supplierElem.SelectedID != UniSelector.US_ALL_RECORDS)
        {
            where.WhereEquals("SKUSupplierID".AsColumn().IsNull(0), supplierElem.SelectedID);
        }

        // Internal status value
        if (internalStatusElem.SelectedID != UniSelector.US_ALL_RECORDS)
        {
            where.WhereEquals("SKUInternalStatusID".AsColumn().IsNull(0), internalStatusElem.SelectedID);
        }

        // Store status value
        if (publicStatusElem.SelectedID != UniSelector.US_ALL_RECORDS)
        {
            where.WhereEquals("SKUPublicStatusID".AsColumn().IsNull(0), publicStatusElem.SelectedID);
        }

        // Append needs shipping condition
        int needsShipping = ValidationHelper.GetInteger(ddlNeedsShipping.SelectedValue, -1);
        if (needsShipping >= 0)
        {
            where.WhereEquals("SKUNeedsShipping".AsColumn().IsNull(0), needsShipping);
        }

        // Append allow for sale condition
        int allowForSale = ValidationHelper.GetInteger(ddlAllowForSale.SelectedValue, -1);
        if (allowForSale >= 0)
        {
            where.WhereEquals("SKUEnabled", allowForSale);
        }

        // When in document mode
        if (ParentNode != null)
        {
            int docTypeId = ValidationHelper.GetInteger(drpDocTypes.SelectedValue, 0);
            if (docTypeId > 0)
            {
                // Append document type condition
                where.WhereEquals("NodeClassID", docTypeId);
            }
        }

        return where;
    }


    /// <summary>
    /// Applies filter to unigrid
    /// </summary>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        if (!IsAdvancedMode)
        {
            ResetAdvancedFilterPart();
        }

        ApplyFilter(sender, e);
    }


    /// <summary>
    /// Applies filter to unigrid.
    /// </summary>
    protected void ApplyFilter(object sender, EventArgs e)
    {
        UniGrid grid = filteredControl as UniGrid;
        if (grid != null)
        {
            grid.ApplyFilter(sender, e);
        }
    }


    /// <summary>
    /// Resets the associated UniGrid control.
    /// </summary>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        UniGrid grid = filteredControl as UniGrid;
        if (grid != null)
        {
            grid.Reset();
        }
    }


    /// <summary>
    /// Sets the advanced mode.
    /// </summary>
    protected void lnkShowAdvancedFilter_Click(object sender, EventArgs e)
    {
        IsAdvancedMode = true;
        SetFieldsVisibility();
    }


    /// <summary>
    /// Sets the simple mode.
    /// </summary>
    protected void lnkShowSimpleFilter_Click(object sender, EventArgs e)
    {
        IsAdvancedMode = false;
        SetFieldsVisibility();
    }


    /// <summary>
    /// Resets filter to the default state.
    /// </summary>
    private void ResetAdvancedFilterPart()
    {
        siteElem.SiteID = UniSelector.US_GLOBAL_AND_SITE_RECORD;
        departmentElem.Value = UniSelector.US_ALL_RECORDS;
        selectProductTypeElem.Value = "ALL";
        manufacturerElem.Value = UniSelector.US_ALL_RECORDS;
        supplierElem.Value = UniSelector.US_ALL_RECORDS;
        internalStatusElem.Value = UniSelector.US_ALL_RECORDS;
        publicStatusElem.Value = UniSelector.US_ALL_RECORDS;
        ddlNeedsShipping.SelectedIndex = 0;
        ddlAllowForSale.SelectedIndex = 0;
        drpDocTypes.SelectedIndex = 0;
    }


    /// <summary>
    /// Shows/hides fields of filter according to simple/advanced mode.
    /// </summary>
    private void SetFieldsVisibility()
    {
        plcSimpleFilter.Visible = !IsAdvancedMode;
        plcAdvancedFilter.Visible = IsAdvancedMode;

        plcAdvancedFilterType.Visible = IsAdvancedMode;
        plcAdvancedFilterGeneral.Visible = IsAdvancedMode;

        bool documentMode = (ParentNode != null);
        plcAdvancedDocumentType.Visible = IsAdvancedMode && documentMode;
    }


    /// <summary>
    /// Fills items 'Yes', 'No' and 'All' to given drop down list.
    /// </summary>
    /// <param name="dropDown">Drop down list to be filled.</param>
    private void FillThreeStateDDL(CMSDropDownList dropDown)
    {
        dropDown.Items.Add(new ListItem(GetString("general.selectall"), "-1"));
        dropDown.Items.Add(new ListItem(GetString("general.yes"), "1"));
        dropDown.Items.Add(new ListItem(GetString("general.no"), "0"));
    }


    /// <summary>
    /// Fills dropdown list with document types.
    /// </summary>
    private void FillDocumentTypesDDL()
    {
        drpDocTypes.Items.Clear();

        // Add (All) record
        drpDocTypes.Items.Add(new ListItem(GetString("general.selectall"), "0"));

        // Select only document types from current site marked as product
        DataSet ds = DocumentTypeHelper.GetDocumentTypeClasses()
            .OnSite(SiteContext.CurrentSiteID)
            .WhereTrue("ClassIsProduct")
            .OrderBy("ClassDisplayName")
            .Columns("ClassID", "ClassDisplayName");

        if (!DataHelper.DataSourceIsEmpty(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string name = ValidationHelper.GetString(dr["ClassDisplayName"], "");
                int id = ValidationHelper.GetInteger(dr["ClassID"], 0);

                if (!string.IsNullOrEmpty(name) && (id > 0))
                {
                    // Handle document name
                    name = ResHelper.LocalizeString(MacroResolver.Resolve(name));

                    drpDocTypes.Items.Add(new ListItem(name, id.ToString()));
                }
            }
        }
    }

    #endregion


    #region "State management"

    /// <summary>
    /// Stores filter state to the specified object.
    /// </summary>
    /// <param name="state">The object that holds the filter state.</param>
    public override void StoreFilterState(FilterState state)
    {
        base.StoreFilterState(state);
        state.AddValue("AdvancedMode", IsAdvancedMode);
    }


    /// <summary>
    /// Restores filter state from the specified object.
    /// </summary>
    /// <param name="state">The object that holds the filter state.</param>
    public override void RestoreFilterState(FilterState state)
    {
        base.RestoreFilterState(state);
        IsAdvancedMode = state.GetBoolean("AdvancedMode");
        SetFieldsVisibility();
    }


    /// <summary>
    /// Resets filter to the default state.
    /// </summary>
    public override void ResetFilter()
    {
        txtNameOrNumber.Text = String.Empty;
        ResetAdvancedFilterPart();
    }

    #endregion
}