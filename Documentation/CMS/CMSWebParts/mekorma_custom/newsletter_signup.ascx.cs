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
using System.Net;
using System.IO;
using System.Text;
using CTCT;
using CTCT.Components;
using CTCT.Components.Contacts;
using CTCT.Components.EmailCampaigns;
using CTCT.Exceptions;
using CTCT.Services;


public partial class CMSWebParts_mekorma_custom_newsletter_signup : CMSAbstractWebPart
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_Click(object sender, EventArgs e)
    {
        //string oauth_token = "52ce6c74-9a57-40df-a61e-587c975decde"; //52ce6c74-9a57-40df-a61e-587c975decde
        //string apikey = "k2mgrwdp2dqk36t9j32rss4j";
        string email = emailTxt.Text;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.constantcontact.com/v2/contacts?action_by=ACTION_BY_VISITOR&api_key=k2mgrwdp2dqk36t9j32rss4j&access_token=52ce6c74-9a57-40df-a61e-587c975decde");
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            //69 is the ID of the Constant Contact mailing list to add users to. 
            string json = "{\"lists\": [{\"id\": \"69\"}],\"email_addresses\": [{\"email_address\":\"" + email + "\"}]}";//request body
            try
            {
                streamWriter.Write(json);
            }
            catch { }
        }
        
        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    lbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#329946");
                    lbl.Text = "Your email was added to our newsletter sign-up list.";
                }
            }
        }
        catch(WebException ex)
        {
            using (WebResponse response = ex.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                if (httpResponse.StatusCode == HttpStatusCode.Conflict)
                {
                    lbl.Text = "This email address already exists in our newsletter sign-up list.";
                }
                else
                {
                    lbl.Text = "There was an error in adding your email address.";
                }
            }
            
        }
    }
}