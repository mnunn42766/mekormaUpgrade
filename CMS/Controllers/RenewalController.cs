using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ServiceModel.Description;

namespace CMS.Mvc.Controllers
{
    public class RenewalController : Controller
    {
        private static IOrganizationService Service;

        public ActionResult GetAccountData(int id)
        {
            if (Service == null)
            {
                Service = GetCRMService();
            }

            EntityCollection records = null;


            if (Service != null)
            {
                int msAccountNumber = id;
 
                //query the account for the ms account return 
                //var retrievedAccounts = RetrieveQueryByAttribute("account", new Dictionary<string, object> { { "mek_ms_customer_acct_number", msAccountNumber } }, new[] { "mek_gp_accountnumber" }, Service, "mek_gp_accountnumber", OrderType.Ascending);
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

            return Json(records, JsonRequestBehavior.AllowGet);
        }

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
}
