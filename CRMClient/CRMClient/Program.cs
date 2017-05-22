///Requires Windows Identity Foundation be enabled or Microsoft.IdentityModel.dll to be adde to GAC.
using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System.ServiceModel.Description;
using System.Collections.Generic;


namespace CRMClient
{
    class Program
    {
        private static IOrganizationService Service;
        //private static System.Configuration.ConnectionStringSettings crmConnString;
        static void Main(string[] args)
        {
            //retrieve CRM Service
            if(Service == null)
            {
                Service = GetCRMService();
            }

            
            if(Service != null)
            {
                int msAccountNumber = 0;
                //parse args to retrieve account number
                if(args.Length == 0 || args.Length > 1 || !Int32.TryParse(args[0], out msAccountNumber)){
                    msAccountNumber = 3424729;
                }
#if DEBUG
                Console.WriteLine("MS Account Number: " + msAccountNumber);
                Console.WriteLine("RetrieveQueryByAttribute(account, [mek_ms_customer_acct_number, " + msAccountNumber + "], [mek_gp_accountnumber], CRMService, mek_gp_accountnumber, OrderType.Ascending)");
#endif
                //query the account for the ms account return 
                //var retrievedAccounts = RetrieveQueryByAttribute("account", new Dictionary<string, object> { { "mek_ms_customer_acct_number", msAccountNumber } }, new[] { "mek_gp_accountnumber" }, Service, "mek_gp_accountnumber", OrderType.Ascending);
                mLinkEntity link = new mLinkEntity() { link = new LinkEntity("account", "account", "mek_partnercompanyid", "accountid", JoinOperator.Inner), columns = new List<string>() { "mek_ms_customer_acct_number" }, entityAlias =  "partnerAccount"};
                var retrievedAccounts = RetrieveQueryExpressionWithLinkEntities("account", new Dictionary<string, object> { { "mek_ms_customer_acct_number", msAccountNumber } }, new[] { "mek_gp_accountnumber" }, Service, new List<mLinkEntity>() { link });
                if (retrievedAccounts.Entities.Count == 1 && retrievedAccounts.Entities[0].Attributes.Contains("mek_gp_accountnumber"))
                {
                    Console.WriteLine("GP Account Number: " + retrievedAccounts.Entities[0].Attributes["mek_gp_accountnumber"]);
                    Console.WriteLine("Partner MS Account Number: " + (Int32)((AliasedValue)retrievedAccounts.Entities[0]["partnerAccount.mek_ms_customer_acct_number"]).Value);
                }
#if DEBUG
                else {
                    Console.WriteLine("Number of Accounts Retrieved: " + retrievedAccounts.Entities.Count);
                }
#endif
            }

        }

        /// <summary>
        ///     Gets a CRMService for the server defined in AppSettings
        /// </summary>
        /// <returns>OrganizationService</returns>
        internal static IOrganizationService GetCRMService()
        {
            IOrganizationService service = null;
            var connection = new Microsoft.Xrm.Client.CrmConnection("Crm");
#if DEBUG
            Console.WriteLine("Organication Uri: " + connection.ServiceUri);
            Console.WriteLine("Client Credentials User: " + connection.ClientCredentials.Windows.ClientCredential.UserName);
            Console.WriteLine("Client Credentials Password: " + connection.ClientCredentials.Windows.ClientCredential.Password);
#endif
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

        /// <summary>
        /// Retrieves all the records in a given entity, there is a hard cap for returns, this query bypasses that hard cap returning all records.
        /// </summary>
        /// <param name="entityName">entity name of the entity you wish to query</param>
        /// <param name="parameters">columns you want returned</param>
        /// <param name="service">IOrganizationService of the CRM organization you wish to query</param>
        /// <returns></returns>
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

        /// <summary>
        /// Query by attributes
        /// </summary>
        /// <param name="entityName">entity name of the entity you wish to query</param>
        /// <param name="attributeValues">attributes that the query will to filter query</param>
        /// <param name="values">values that those attributes should match</param>
        /// <param name="columns">columns to return in the query</param>
        /// <param name="service">IOrganizationService of the CRM organization you wish to query</param>
        /// <param name="attributeNameOrder">the column you wish to order the results</param>
        /// <param name="orderType">OrderType.Ascending</param>
        /// <returns>business collection of entity...</returns>
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
#if DEBUG
                Console.WriteLine("<<<ERROR>>> RetrieveQueryByAttribute: " + ex.Message);
#endif
                return new EntityCollection();
            }

        }

        internal static EntityCollection RetrieveQueryExpressionWithLinkEntities(String entityName,
            Dictionary<String, Object> attributeValues, //String[] attributes, String[] values,
            String[] columns,
            IOrganizationService service,
            List<mLinkEntity> links)
        {
            var query = new QueryExpression(entityName) { ColumnSet = new ColumnSet(columns) };
            foreach (var attributeValue in attributeValues)
            {
                query.Criteria.AddCondition(attributeValue.Key, ConditionOperator.Equal, attributeValue.Value);
            }
            for(Int32 i = 0; i < links.Count; i++)
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
#if DEBUG
                Console.WriteLine("<<<ERROR>>> RetrieveQueryExpressionWithLinkEntities: " + ex.Message);
#endif
                return new EntityCollection();
            }
        }

    }

    internal class mLinkEntity
    {
        public LinkEntity link = new LinkEntity();
        public List<String> columns = new List<string>();
        public String entityAlias = String.Empty;
    }
}
