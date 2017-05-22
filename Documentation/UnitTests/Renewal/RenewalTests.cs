using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMS.Mvc.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using BusinessEntities.Crm;
//using Newtonsoft.Json;
using Microsoft.Xrm.Sdk;

namespace UnitTests.Renewal
{
    //[TestClass]
    //public class RenewalTests
    //{
        //[TestMethod]
        //public void EndUser()
        //{
        //    // RSM - West Region
        //    List<string> testIds = new List<string>()
        //    {
        //        "3424729",
        //        //"4912239",
        //        "4704129",
        //        "5306902",
        //        "3068087",
        //        "5708837"
        //    };

        //    RenewalController renewal = new RenewalController();

        //    int foundClientCount = 0;
        //    int foundPartnerCount = 0;

        //    foreach (string id in testIds)
        //    {
        //        var result = (JsonResult)renewal.GetAccountData(int.Parse(id));

        //        var json = JsonConvert.SerializeObject(result.Data);

        //        EntityCollection results = (EntityCollection)result.Data;

        //        SearchResult entities = JsonConvert.DeserializeObject<SearchResult>(json);

        //        if (entities.Entities.Count > 0)
        //        { 
        //            if (results.Entities.Count == 1 && results.Entities[0].Attributes.Contains("mek_gp_accountnumber"))
        //            {
        //                var gpAccount = results.Entities[0].Attributes["mek_gp_accountnumber"];
        //                var partnerAccount = (Int32)((AliasedValue)results.Entities[0]["partnerAccount.mek_ms_customer_acct_number"]).Value;

        //                foundPartnerCount++;
        //            }
        //            else
        //            {
        //                foundClientCount++;
        //            } 
        //        }
        //    } 

        //    Assert.IsTrue(foundClientCount + foundPartnerCount == testIds.Count, "didn't find them all");
        //}

        //[TestMethod]
        //public void Partner()
        //{

        //}
    //}
}
