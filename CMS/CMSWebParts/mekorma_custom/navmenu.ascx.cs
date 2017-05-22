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
using CMS.CMSHelper;
using CMS.Helpers;

public partial class CMSWebParts_mekorma_custom_navmenu : CMSAbstractWebPart
{

    protected void Page_Load(object sender, EventArgs e)
    {
        TreeProvider tree = new TreeProvider(CurrentUser);
        currentpageId.Value = CurrentDocument.NodeID.ToString();

        string backgroundClass = "";

        if (!IsPostBack)
        {
            DataSet docs = tree.SelectNodes(CurrentSiteName, "/%", "en-us", false, "CMS.MenuItem", "","NodeOrder", 1);           


            menu_render.Text += "<li data-pageid='46' class='mobquicklinks parent-link hidden-desktop'><a href='~/renew-license.aspx'>Renew License</a></li>";
            menu_render.Text += "<li data-pageid='30' class='mobquicklinks parent-link hidden-desktop'><a href='~/request-a-demo.aspx'>Request Demo</a></li>";
            menu_render.Text += "<li data-pageid='34' class='mobquicklinks parent-link hidden-desktop'><a href='~/Downloads.aspx'>Download Now</a></li>";

            if (!CMS.Helpers.DataHelper.DataSourceIsEmpty(docs))
            {
                foreach (DataRow row in docs.Tables[0].Rows)
                {
                    CMS.DocumentEngine.TreeNode thisnode = CMS.DocumentEngine.TreeNode.New(row);

                    if (!ValidationHelper.GetBoolean(thisnode.GetValue("DocumentMenuItemHideInNavigation"), false))
                    {
                        string link = "";
                        backgroundClass = CurrentDocument.NodeID.ToString() == thisnode.NodeID.ToString() ? " pageSelected" : "";

                        if (thisnode.Children.Count > 0)
                        {
                            if (thisnode.DocumentMenuRedirectToFirstChild)
                            {
                                link = "#";
                            }
                            else
                            {
                                link = thisnode.RelativeURL;
                            }
                        }
                        else
                        {
                            link = thisnode.RelativeURL;
                        }
                       
                        if (thisnode.Children.Count > 0)
                        {

                            menu_render.Text += "<li class= 'parent-link haschildren" + backgroundClass + "' data-pageid='" + thisnode.NodeID + "'>";
                            menu_render.Text += "<div><a href='" + link + "'>" + thisnode.DocumentName + "</a></div>";
                           
                            menu_render.Text += "<ul>";
                            foreach (CMS.DocumentEngine.TreeNode child in thisnode.Children.OrderBy(x => x.NodeOrder))
                            {
                                if (!ValidationHelper.GetBoolean(child.GetValue("DocumentMenuItemHideInNavigation"), false))
                                {
                                    menu_render.Text += "<li class='child-link' data-parentpageid='" + thisnode.NodeID + "' data-pageid='" + child.NodeID + "'>";

                                    menu_render.Text += "<a href='" + child.RelativeURL + "'>" + child.DocumentName;
                                    menu_render.Text += "</a></li>";
                                }
                            }
                            menu_render.Text += "</ul>";
                        }
                        else
                        {
                            menu_render.Text += "<li class= 'parent-link " + backgroundClass + "' data-pageid='" + thisnode.NodeID + "'>";
                            menu_render.Text += "<a href='" + link + "'>" + thisnode.DocumentName + "</a>";
                        }
                        menu_render.Text += "</li>";
                    }
                }

                 menu_render.Text += "";
                //menu_render.Text += "<li class=\"parent-link hidden-desktop partner-mobile\"><a href=\"#\">Partner Login</a></li>";
            }
        }
    }

}