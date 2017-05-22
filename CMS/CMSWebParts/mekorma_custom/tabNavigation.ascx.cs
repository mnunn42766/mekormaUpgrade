using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.PortalControls;
using System.Data;
using CMS.DataEngine;
using CMS.GlobalHelper;
using CMS.DocumentEngine;
using CMS.GlobalHelper;
using CMS.DocumentEngine;
using CMS.CMSHelper;
using CMS.Helpers;

public partial class CMSWebParts_mekorma_custom_tabNavigation : CMSAbstractWebPart
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TreeProvider tree = new TreeProvider(CurrentUser);
        currentpageId.Value = CurrentDocument.NodeID.ToString();

        string backgroundClass = "";

        if (!IsPostBack)
        {
            string parentPath = this.GetValue("Path").ToString();
            DataSet docs = tree.SelectNodes(CurrentSiteName, parentPath, "en-us", false, "CMS.MenuItem", "", "NodeOrder", 1);

            if (!CMS.Helpers.DataHelper.DataSourceIsEmpty(docs))
            {
                menu_render.Text += "<div class='tabNavigation'>";

                menu_render.Text += "<div class='container'>";
                foreach (DataRow row in docs.Tables[0].Rows)
                {
                    CMS.DocumentEngine.TreeNode thisnode = CMS.DocumentEngine.TreeNode.New(row);

                    bool hideInNav = true;
                    try { hideInNav = (bool)thisnode.GetValue("DocumentMenuItemHideInNavigation"); }
                    catch { hideInNav = false; }

                    if (!ValidationHelper.GetBoolean(thisnode.GetValue("DocumentMenuItemHideInNavigation"), false))
                    {

                        if (thisnode.NodeLevel >= 3)
                        {
                            if (CurrentDocument.Parent.NodeID.ToString() == thisnode.NodeID.ToString())
                            {
                               
                                menu_render.Text += "<li class= 'item tabSelected' data-pageid='" + thisnode.NodeID + "'>";
                                menu_render.Text += "<div><a href='" + thisnode.RelativeURL + "'>" + thisnode.DocumentName + "</a></div>";
                                menu_render.Text += "</li>";
                            }
                            else
                            {
                                menu_render.Text += "<li class= 'item' data-pageid='" + thisnode.NodeID + "'>";
                                menu_render.Text += "<div><a href='" + thisnode.RelativeURL + "'>" + thisnode.DocumentName + "</a></div>";
                                menu_render.Text += "</li>";
                            }
                        }
                        else
                        {
                            if (CurrentDocument.NodeID.ToString() == thisnode.NodeID.ToString() || CurrentDocument.Parent.NodeID.ToString() == thisnode.NodeID.ToString())
                            {
                                menu_render.Text += "<li class= 'item tabSelected' data-pageid='" + thisnode.NodeID + "'>";
                                menu_render.Text += "<div><a href='" + thisnode.RelativeURL + "'>" + thisnode.DocumentName + "</a></div>";
                                menu_render.Text += "</li>";
                            }
                            else
                            {
                                menu_render.Text += "<li class= 'item' data-pageid='" + thisnode.NodeID + "'>";
                                menu_render.Text += "<div><a href='" + thisnode.RelativeURL + "'>" + thisnode.DocumentName + "</a></div>";
                                menu_render.Text += "</li>";
                            }
                        }

                       
                        
                    }
                }
                menu_render.Text += "</div>";
                menu_render.Text += "</div>";
            }
        }
    }
}