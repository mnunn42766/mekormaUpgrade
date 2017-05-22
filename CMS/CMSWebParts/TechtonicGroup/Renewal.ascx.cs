using BusinessEntities.Crm;
using CMS.Helpers;
using CMS.Localization;
//using CMS.PortalControls;
using CMS.SalesForce.WebServiceClient;
using CMS.SiteProvider;
using CMS.WebAnalytics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections.Specialized;
using System.Net;
using CMS.PortalEngine.Web.UI;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

public partial class CMSWebParts_TechtonicGroup_Renewal : CMSAbstractWebPart
{
    private static IOrganizationService Service;

    //public string TrackConversionName
    //{
    //    get
    //    {
    //        return ValidationHelper.GetString(GetValue("TrackConversionName"), "");
    //    }
    //    set
    //    {
    //        if (value.Length > 400)
    //        {
    //            value = value.Substring(0, 400);
    //        }
    //        SetValue("TrackConversionName", value);
    //    }
    //}



    private readonly string RENEW_URL = "https://renewal.mekorma.com/default.aspx?guid=";
    protected readonly string ERROR_MSG = "No Renewal Record Found.  Please verify that a Renewal Record was created for Company Number ";
    public int _companyType = 2;

    //public bool recaptcha()
    //{
    //    var response = Request["g-recaptcha-response"];
    //    bool isValid = false;
    //    string key = "";
    //    string secret = "6LdFkgsUAAAAAAw5-dEy8Sr0RnoZCHkQ0PdOP_c-";

    //    WebClient client = new WebClient();
    //    var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
    //    var resultObject = JObject.Parse(result);
    //    isValid = (bool)resultObject.SelectToken("success");

    //    return isValid;
    //}

    //public double ConversionValue
    //{
    //    get
    //    {
    //        return ValidationHelper.GetDoubleSystem(GetValue("ConversionValue"), 0);
    //    }
    //    set
    //    {
    //        SetValue("ConversionValue", value);
    //    }
    //}

    private string _renewalKey { get; set; }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    pnlAccountNumber.Visible = false;
    //    pnlPartnerAccount.Visible = true;

    ////    if (!IsPostBack)
    ////    {
    ////        //pnlAccountNumber.Visible = false;
    ////        pnlPartnerAccount.Visible = true;
    ////    }
    ////}

    protected void lbNo_Click(object sender, EventArgs e)
    {

        

        lbNo.CssClass = "btn btn-secondary active";
        lbYes.CssClass = "btn btn-secondary";
    }
    protected void lbYes_Click(object sender, EventArgs e)
    {

        

        lbNo.CssClass = "btn btn-secondary";
        lbYes.CssClass = "btn btn-secondary active";
    }


    //private void TrackClickEvent()
    //{
    //    if (!string.IsNullOrEmpty(TrackConversionName))
    //    {
    //        string siteName = SiteContext.CurrentSiteName;

    //        //if (AnalyticsHelper.AnalyticsEnabled(siteName) && AnalyticsHelper.TrackConversionsEnabled(siteName) && !AnalyticsHelper.IsIPExcluded(siteName, RequestContext.UserHostAddress))
    //        //{
    //        //    HitLogProvider.LogConversions(SiteContext.CurrentSiteName, LocalizationContext.PreferredCultureCode, TrackConversionName, 0, ConversionValue);
    //        //}
    //    }
    //}

    protected void lbRenew_Click(object sender, EventArgs e)
    {
        bool isPartner = true;

        if (txtMicrosoftAccountNumber.Text.Length > 0 && txtPartnerAccountNumber.Text.ToString()!="")
        {
            lbNo.CssClass = "btn btn-secondary";
            lbYes.CssClass = "btn btn-secondary active";
        }
        else if (txtMicrosoftAccountNumber.Text.Length > 0 && txtPartnerAccountNumber.Text.ToString() == "")
        {
            lbNo.CssClass = "btn btn-secondary active";
            lbYes.CssClass = "btn btn-secondary";
        }

       

            string partnerNumber = "";
            string accountNumber = "";
            if (lbNo.CssClass.Contains("active"))
            {
                accountNumber = txtMicrosoftAccountNumber.Text;



            }
            else if (lbYes.CssClass.Contains("active"))
            {

                partnerNumber = txtPartnerAccountNumber.Text;
                accountNumber = txtMicrosoftAccountNumber.Text;

            }


            int id = 0;

            if (txtMicrosoftAccountNumber.Text.Length > 0 && txtPartnerAccountNumber.Text.Length > 0)
            {
                isPartner = true;
            }
            else if (txtMicrosoftAccountNumber.Text.Length > 0 && txtPartnerAccountNumber.Text.ToString() == "")
            {
                isPartner = false;
            }


            var msAccountNumber = accountNumber;
             isPartner = true;


            
            if (lbNo.CssClass.Contains("active")) // This is the the end user who is renewing by themselves
            {

                isPartner = false;
            }


            accountNumber = accountNumber.Replace(",", "");

            int.TryParse(accountNumber, out id);



            var partQueryVal = QueryCrm(id, isPartner);
            if (partQueryVal != null)
            {
                if (partQueryVal.Entities.Count > 0)
                {

                    if (partQueryVal.Entities.Count == 1 && partQueryVal.Entities[0].Attributes.Contains("mek_gp_accountnumber"))
                    {

                        var partNumShow = (Int32)((AliasedValue)partQueryVal.Entities[0]["partnerAccount.mek_ms_customer_acct_number"]).Value;



                        lblErrorMessage.Visible = false;


                        if (lbNo.CssClass.Contains("active"))

                        {

                            partnerNumber = partNumShow.ToString();
                        }



                        accountNumber = accountNumber.Replace(",", "");
                        partnerNumber = partnerNumber.Replace(",", "");

                        if (int.TryParse(accountNumber, out id))
                        {

                            var records = QueryCrm(id, isPartner);


                            if (records != null)
                            {
                                if (records.Entities.Count > 0)
                                {
                                    if (records.Entities.Count == 1 && records.Entities[0].Attributes.Contains("mek_gp_accountnumber"))
                                    {
                                        var gpAccount = records.Entities[0].Attributes["mek_gp_accountnumber"];


                                        if (lbNo.CssClass.Contains("active"))
                                        {
                                            _companyType = 1;

                                            var partnerAccount = (Int32)((AliasedValue)records.Entities[0]["partnerAccount.mek_ms_customer_acct_number"]).Value;

                                            if (partnerNumber == partnerAccount.ToString())
                                            {

                                                URLHelper.Redirect(RENEW_URL + GenerateRenewalID(int.Parse(gpAccount.ToString()), _companyType));

                                            }
                                            else if(partnerNumber != partnerAccount.ToString())
                                            {
                                                lblErrorMessage.Visible = true;
                                                lblErrorMessage.Text = "You are not listed as the Partner of Record for this End User. Please email sales@mekorma.com, to update our records before accessing this link.";
                                               

                                            }

                                           else if (partnerNumber == null)
                                            {

                                                lblErrorMessage.Visible = true;
                                                lblErrorMessage.Text = "Partner Microsoft Account Number or the End User Microsoft Account Number you have entered could not be found in our system. Please check your information and try again";
                                               

                                            }

                                        }
                                        else if (lbYes.CssClass.Contains("active"))
                                        {

                                            _companyType = 2;
                                            var partnerAccount = (Int32)((AliasedValue)records.Entities[0]["partnerAccount.mek_ms_customer_acct_number"]).Value;

                                            if (partnerNumber == partnerAccount.ToString())
                                            {
                                                URLHelper.Redirect(RENEW_URL + GenerateRenewalID(int.Parse(gpAccount.ToString()), _companyType));
                                            }
                                            else
                                            {
                                                lblErrorMessage.Visible = true;
                                                lblErrorMessage.Text = "You are not listed as the Partner of Record for this End User. Please email sales@mekorma.com, to update our records before accessing this link.";
                                                

                                            }
                                        }

                                        else
                                        {
                                            lblErrorMessage.Visible = true;
                                            lblErrorMessage.Text = ERROR_MSG;
                                        }
                                    }

                                }
                                else
                                {
                                    lblErrorMessage.Visible = true;
                                    lblErrorMessage.Text = ERROR_MSG;
                                }
                            }//
                            else
                            {
                                lblErrorMessage.Visible = true;
                                lblErrorMessage.Text = ERROR_MSG;
                            }
                        }
                    }
                    else
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = "Partner Microsoft Account Number or the End User Microsoft Account Number you have entered could not be found in our system. Please check your information and try again";
                        
                    }
                    //txtYourMicrosoftNumber.Text ="";
                    //txtPartnerAccountNumber.Text = "";
                }


                else
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Partner Microsoft Account Number or the End User Microsoft Account Number you have entered could not be found in our system. Please check your information and try again";

                    txtPartnerAccountNumber.Visible=false;
                    lblPartnershow.Visible = false;
                    
                    
              
                }
                
                
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = ERROR_MSG;
            }

        
        

        //      var response = Http.Post("https//:www.google.com/recaptcha/api/i", new NameValueCollection() {
        //    { "secret", "6LdFkgsUAAAAAAw5-dEy8Sr0RnoZCHkQ0PdOP_c-" },
        //    { "response", "The value of 'g-recaptcha-response'." }
        //});

  }
    
    public string GenerateRenewalID(int companyNumber, int _companyType)
    {
        string renewalId = string.Empty;

        try
        {
            renewalId = string.Format("{0}{1}{2}", _companyType, companyNumber.ToString("D7"), CheckDigitCalculation(_companyType + companyNumber.ToString("D7")).ToString("D2"));
            _renewalKey = renewalId;
        }
        catch (ApplicationException ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

        return renewalId;
    }


    private static int CheckDigitCalculation(string renewalId)
    {
        var renewalString = renewalId;
        var checkDigit = 0;

        for (var i = 0; i < renewalString.Length; i++)
        {
            try
            {
                if (!Char.IsDigit(renewalString[i]))
                {
                    return -1;
                }

                var digit = (i % 2 == 1)
                                ? (Int32)Char.GetNumericValue(renewalString[i])
                                : (Int32)Char.GetNumericValue(renewalString[i]) * 2;

                checkDigit = ((checkDigit * 10) + digit) % 97;
            }
            catch (Exception ex)
            {
                var appException = new ApplicationException("71001")
                {
                    Source = "CheckDigitCalculation",
                    HelpLink = ex.Message
                };

                throw appException;
            }
        }

        return checkDigit;
    }

    private EntityCollection QueryCrm(int id, bool isPartner)
    {
        if (Service == null)
        {
            Service = GetCRMService();
        }

        EntityCollection records = null;
        if (Service != null)
        {
            int msAccountNumber = id;

            if (isPartner)
            {
                BusinessEntities.Crm.Entity link = new BusinessEntities.Crm.Entity()
                {
                    link = new LinkEntity("account", "account", "mek_partnercompanyid", "accountid", JoinOperator.Inner),
                    columns = new List<string>() { "mek_ms_customer_acct_number" },
                    entityAlias = "partnerAccount"
                };
                records = RetrieveQueryExpressionWithLinkEntities(
                    "account",
                    new Dictionary<string, object> { { "mek_ms_customer_acct_number", msAccountNumber } },
                    new[] { "mek_gp_accountnumber" },
                    Service,
                    new List<BusinessEntities.Crm.Entity>() 
                    { 
                        link 
                    }
                );
            }
            else if(!isPartner)
            {
                BusinessEntities.Crm.Entity link = new BusinessEntities.Crm.Entity()
                {
                    link = new LinkEntity("account", "account", "mek_partnercompanyid", "accountid", JoinOperator.Inner),
                    columns = new List<string>() { "mek_ms_customer_acct_number" },
                    entityAlias = "partnerAccount"
                };

                records = RetrieveQueryExpressionWithLinkEntities(
                    "account",
                    new Dictionary<string, object> { { "mek_ms_customer_acct_number", msAccountNumber } },
                    new[] { "mek_gp_accountnumber" },
                    Service,
                    new List<BusinessEntities.Crm.Entity>()
                    {
                        link
                    }
                );
            }

            return records;

        }
        return null;
    }

    //public EntityCollection PartnerQueryCrm(int id)

    //{
    //    var msAccountNumber = id;
    
    //    var records = RetrieveQueryByAttribute("account", new Dictionary<string, object> { { "mek_ms_customer_acct_number", msAccountNumber } }, new[] { "mek_gp_accountnumber" }, Service, "mek_gp_accountnumber", OrderType.Ascending);  //no link is set for retrieval

    //    return records;

    //}
    private static IOrganizationService GetCRMService()
    {
        IOrganizationService service = null;
        var connection = new Microsoft.Xrm.Client.CrmConnection("Crm");

        Uri organizationUriIFD = connection.ServiceUri;

        ClientCredentials credentials = new ClientCredentials();
        credentials.UserName.UserName = connection.ClientCredentials.Windows.ClientCredential.UserName;
        credentials.UserName.Password = connection.ClientCredentials.Windows.ClientCredential.Password;

        IServiceConfiguration<IOrganizationService> config = ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(organizationUriIFD);

        using (OrganizationServiceProxy _serviceProxy = new OrganizationServiceProxy(config, credentials))
        {
            // This statement is required to enable early-bound type support.
            _serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
            service = (IOrganizationService)_serviceProxy;
            Microsoft.Crm.Sdk.Messages.WhoAmIResponse response = (Microsoft.Crm.Sdk.Messages.WhoAmIResponse)service.Execute(new Microsoft.Crm.Sdk.Messages.WhoAmIRequest());
        }
        return service;
    }

    internal static EntityCollection RetrieveAllEntityRecords(String entityName, String[] parameters, IOrganizationService service)
    {

        //Set query 
        var query = new QueryExpression(entityName) { ColumnSet = new ColumnSet(parameters) };
        var pageNumber = 1;
        EntityCollection allRecords = new EntityCollection();
        var request = new RetrieveMultipleRequest();
        var response = new RetrieveMultipleResponse();
        //Execute query
        do
        {
            query.PageInfo.Count = 5000;
            query.PageInfo.PagingCookie = (pageNumber == 1) ? null : response.EntityCollection.PagingCookie;
            query.PageInfo.PageNumber = pageNumber++;
            request = new RetrieveMultipleRequest();
            request.Query = query;
            response = (RetrieveMultipleResponse)service.Execute(request);
            allRecords.Entities.AddRange(response.EntityCollection.Entities);

        } while (response.EntityCollection.MoreRecords);

        return allRecords;
    }

    internal static EntityCollection RetrieveQueryByAttribute(String entityName,
        Dictionary<String, Object> attributeValues, //String[] attributes, String[] values,
        String[] columns,
        IOrganizationService service,
        String attributeNameOrder,
        OrderType orderType)
    {
        var query = new QueryByAttribute(entityName) { ColumnSet = new ColumnSet(columns) };
        query.AddOrder(attributeNameOrder, orderType);
        foreach (var attributeValue in attributeValues)
        {
            query.AddAttributeValue(attributeValue.Key, attributeValue.Value);
        }
        //Execute the query
        try
        {
            var retrieveMultiResponse = service.RetrieveMultiple(query);
            return retrieveMultiResponse;
        }
        catch (Exception ex)
        {
            return new EntityCollection();
        }

    }

    internal static EntityCollection RetrieveQueryExpressionWithLinkEntities(String entityName,
        Dictionary<String, Object> attributeValues, //String[] attributes, String[] values,
        String[] columns,
        IOrganizationService service,
        List<BusinessEntities.Crm.Entity> links)
    {
        var query = new QueryExpression(entityName) { ColumnSet = new ColumnSet(columns) };
        foreach (var attributeValue in attributeValues)
        {
            query.Criteria.AddCondition(attributeValue.Key, ConditionOperator.Equal, attributeValue.Value);
        }
        for (Int32 i = 0; i < links.Count; i++)
        {
            query.LinkEntities.Add(links[i].link);
            query.LinkEntities[i].Columns.AddColumns(links[i].columns.ToArray());
            query.LinkEntities[i].EntityAlias = links[i].entityAlias;
        }
        //Execute the query
        try
        {
            var retrieveMultiResponse = service.RetrieveMultiple(query);
            return retrieveMultiResponse;
        }
        catch (Exception ex)
        {
            return new EntityCollection();
        }
    }

}