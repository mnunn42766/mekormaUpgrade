using System;

using CMS.Newsletters;
using CMS.SiteProvider;

namespace APIExamples
{
    /// <summary>
    /// Holds email marketing API examples.
    /// </summary>
    /// <pageTitle>Email marketing</pageTitle>
    internal class EmailMarketing
    {
        /// <summary>
        /// Holds email marketing template API examples.
        /// </summary>
        /// <groupHeading>Email marketing templates</groupHeading>
        private class EmailTemplates
        {
            /// <heading>Creating an email template</heading>
            private void CreateEmailTemplate()
            {
                // Creates a new template object
                EmailTemplateInfo newTemplate = new EmailTemplateInfo()
                {
                    // Sets the basic template properties
                    TemplateDisplayName = "New email template",
                    TemplateName = "NewEmailTemplate",
                    TemplateSiteID = SiteContext.CurrentSiteID,

                    // Sets the template type (Campaigns emails in this case)
                    TemplateType = EmailTemplateType.Issue,
                    // Other possible template type values: EmailTemplateType.Subscription, EmailTemplateType.Unsubscription, EmailTemplateType.DoubleOptIn

                    // Defines the content of the template
                    TemplateHeader = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
                                       <head>
                                         <title>Newsletter</title>
                                         <meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"" />
                                       </head>
                                       <body>",
                    TemplateBody = "Email template content",
                    TemplateFooter = "</body></html>"
                };

                // Saves the new template to the database
                EmailTemplateInfoProvider.SetEmailTemplateInfo(newTemplate);
            }


            /// <heading>Updating an email template</heading>
            private void GetAndUpdateIssueTemplate()
            {
                // Gets the email template
                EmailTemplateInfo updateTemplate = EmailTemplateInfoProvider.GetEmailTemplateInfo("NewEmailTemplate", SiteContext.CurrentSiteID);

                if (updateTemplate != null)
                {
                    // Updates the template properties
                    updateTemplate.TemplateDisplayName = updateTemplate.TemplateDisplayName.ToLower();

                    // Saves the updated template to the database
                    EmailTemplateInfoProvider.SetEmailTemplateInfo(updateTemplate);
                }
            }


            /// <heading>Updating multiple email templates</heading>
            private void GetAndBulkUpdateIssueTemplates()
            {
                // Gets all email marketing templates of the "Campaign email" type whose code name starts with 'New'
                var templates = EmailTemplateInfoProvider.GetEmailTemplates()
                                                            .WhereEquals("TemplateType", EmailTemplateType.Issue)
                                                            .WhereStartsWith("TemplateName", "New");

                // Loops through individual email templates
                foreach (EmailTemplateInfo template in templates)
                {
                    // Updates the template properties
                    template.TemplateDisplayName = template.TemplateDisplayName.ToUpper();

                    // Saves the updated template to the database
                    EmailTemplateInfoProvider.SetEmailTemplateInfo(template);
                }
            }


            /// <heading>Deleting an email template</heading>
            private void DeleteIssueTemplate()
            {
                // Gets the email template
                EmailTemplateInfo deleteTemplate = EmailTemplateInfoProvider.GetEmailTemplateInfo("NewEmailTemplate", SiteContext.CurrentSiteID);

                if (deleteTemplate != null)
                {
                    // Deletes the email template
                    EmailTemplateInfoProvider.DeleteEmailTemplateInfo(deleteTemplate);
                }
            }
        }


        /// <summary>
        /// Holds email campaign API examples.
        /// </summary>
        /// <groupHeading>Template-based email campaigns</groupHeading>
        private class EmailCampaigns
        {
            /// <heading>Creating an email campaign</heading>
            private void CreateEmailCampaign()
            {
                // Gets templates for the campaign's emails
                EmailTemplateInfo subscriptionTemplate = EmailTemplateInfoProvider.GetEmailTemplateInfo("SubscriptionTemplate", SiteContext.CurrentSiteID);
                EmailTemplateInfo unsubscriptionTemplate = EmailTemplateInfoProvider.GetEmailTemplateInfo("UnsubscriptionTemplate", SiteContext.CurrentSiteID);
                EmailTemplateInfo emailTemplate = EmailTemplateInfoProvider.GetEmailTemplateInfo("NewEmailTemplate", SiteContext.CurrentSiteID);

                if ((subscriptionTemplate != null) && (unsubscriptionTemplate != null) && (emailTemplate != null))
                {
                    // Creates a new email campaign object
                    NewsletterInfo newEmailCampaign = new NewsletterInfo() 
                    {
                        // Sets the campaign properties
                        NewsletterDisplayName = "New email campaign",
                        NewsletterName = "NewEmailCampaign",
                        NewsletterType = NewsletterType.TemplateBased,
                        NewsletterSenderName = "Sender name",
                        NewsletterSenderEmail = "sender@localhost.local",
                        NewsletterSiteID = SiteContext.CurrentSiteID,

                        // Assigns email templates to the campaign
                        NewsletterSubscriptionTemplateID = subscriptionTemplate.TemplateID,
                        NewsletterUnsubscriptionTemplateID = unsubscriptionTemplate.TemplateID,
                        NewsletterTemplateID = emailTemplate.TemplateID
                    };

                    // Saves the new email campaign to the database
                    NewsletterInfoProvider.SetNewsletterInfo(newEmailCampaign);
                }
            }


            /// <heading>Updating an email campaign</heading>
            private void GetAndUpdateEmailCampaign()
            {
                // Gets the email campaign
                NewsletterInfo updateEmailCampaign = NewsletterInfoProvider.GetNewsletterInfo("NewEmailCampaign", SiteContext.CurrentSiteID);

                if (updateEmailCampaign != null)
                {
                    // Updates the email campaign properties
                    updateEmailCampaign.NewsletterDisplayName = updateEmailCampaign.NewsletterDisplayName.ToLower();

                    // Saves the updated campaign to the database
                    NewsletterInfoProvider.SetNewsletterInfo(updateEmailCampaign);
                }
            }


            /// <heading>Updating multiple email campaigns</heading>
            private void GetAndBulkUpdateEmailCampaign()
            {
                // Gets all email campaigns on the current site whose code name starts with 'New'
                var emailCampaigns = NewsletterInfoProvider.GetNewsletters()
                                                                .WhereEquals("NewsletterType", NewsletterType.TemplateBased)
                                                                .WhereEquals("NewsletterSiteID", SiteContext.CurrentSiteID)
                                                                .WhereStartsWith("NewsletterName", "New");
                
                // Loops through individual email campaigns
                foreach (NewsletterInfo emailCampaign in emailCampaigns)
                {
                    // Updates the campaign properties
                    emailCampaign.NewsletterDisplayName = emailCampaign.NewsletterDisplayName.ToUpper();

                    // Saves the updated campaign to the database
                    NewsletterInfoProvider.SetNewsletterInfo(emailCampaign);
                }
            }


            /// <heading>Deleting an email campaign</heading>
            private void DeleteEmailCampaign()
            {
                // Gets the email campaign
                NewsletterInfo deleteEmailCampaign = NewsletterInfoProvider.GetNewsletterInfo("NewEmailCampaign", SiteContext.CurrentSiteID);

                if (deleteEmailCampaign != null)
                {
                    // Deletes the email campaign
                    NewsletterInfoProvider.DeleteNewsletterInfo(deleteEmailCampaign);
                }
            }
        }


        /// <summary>
        /// Holds dynamic newsletter API examples.
        /// </summary>
        /// <groupHeading>Dynamic newsletters</groupHeading>
        private class DynamicNewsletters
        {
            /// <heading>Creating a dynamic newsletter</heading>
            private void CreateDynamicNewsletter()
            {
                // Gets templates for the newsletter's subscription messages
                EmailTemplateInfo subscriptionTemplate = EmailTemplateInfoProvider.GetEmailTemplateInfo("SubscriptionTemplate", SiteContext.CurrentSiteID);
                EmailTemplateInfo unsubscriptionTemplate = EmailTemplateInfoProvider.GetEmailTemplateInfo("UnsubscriptionTemplate", SiteContext.CurrentSiteID);

                if ((subscriptionTemplate != null) && (unsubscriptionTemplate != null))
                {
                    // Creates a new dynamic newsletter object
                    NewsletterInfo newNewsletter = new NewsletterInfo()
                    {
                        // Sets the newsletter properties
                        NewsletterDisplayName = "New dynamic newsletter",
                        NewsletterName = "NewDynamicNewsletter",
                        NewsletterType = NewsletterType.Dynamic,                        
                        NewsletterSenderName = "Sender name",
                        NewsletterSenderEmail = "sender@localhost.local",
                        NewsletterDynamicURL = "http://www.MyWebsite.com/NewsletterContent",
                        NewsletterDynamicSubject = "Newsletter issue",
                        NewsletterSiteID = SiteContext.CurrentSiteID,

                        NewsletterSubscriptionTemplateID = subscriptionTemplate.TemplateID,
                        NewsletterUnsubscriptionTemplateID = unsubscriptionTemplate.TemplateID
                    };

                    // Saves the new dynamic newsletter to the database
                    NewsletterInfoProvider.SetNewsletterInfo(newNewsletter);
                }
            }


            /// <heading>Updating a dynamic newsletter</heading>
            private void GetAndUpdateDynamicNewsletter()
            {
                // Gets the dynamic newsletter
                NewsletterInfo updateNewsletter = NewsletterInfoProvider.GetNewsletterInfo("NewDynamicNewsletter", SiteContext.CurrentSiteID);

                if (updateNewsletter != null)
                {
                    // Updates the newsletter properties
                    updateNewsletter.NewsletterDisplayName = updateNewsletter.NewsletterDisplayName.ToLower();

                    // Saves the updated newsletter to the database
                    NewsletterInfoProvider.SetNewsletterInfo(updateNewsletter);
                }
            }


            /// <heading>Updating multiple dynamic newsletters</heading>
            private void GetAndBulkUpdateDynamicNewsletters()
            {
                // Gets all dynamic newsletters on the current site whose code name starts with 'New'
                var newsletters = NewsletterInfoProvider.GetNewsletters()
                                                                .WhereEquals("NewsletterType", NewsletterType.Dynamic)
                                                                .WhereEquals("NewsletterSiteID", SiteContext.CurrentSiteID)
                                                                .WhereStartsWith("NewsletterName", "New");
                
                // Loops through individual newsletters
                foreach (NewsletterInfo newsletter in newsletters)
                {
                    // Updates the newsletter properties
                    newsletter.NewsletterDisplayName = newsletter.NewsletterDisplayName.ToUpper();

                    // Saves the updated newsletter to the database
                    NewsletterInfoProvider.SetNewsletterInfo(newsletter);
                }
            }


            /// <heading>Deleting a dynamic newsletter</heading>
            private void DeleteDynamicNewsletter()
            {
                // Gets the dynamic newsletter
                NewsletterInfo deleteNewsletter = NewsletterInfoProvider.GetNewsletterInfo("NewDynamicNewsletter", SiteContext.CurrentSiteID);

                if (deleteNewsletter != null)
                {
                    // Deletes the dynamic newsletter
                    NewsletterInfoProvider.DeleteNewsletterInfo(deleteNewsletter);
                }
            }
        }


        /// <summary>
        /// Holds subscriber API examples.
        /// </summary>
        /// <groupHeading>Subscribers</groupHeading>
        private class Subscribers
        {
            /// <heading>Creating an email campaign subscriber</heading>
            private void CreateSubscriber()
            {
                // Creates a new subscriber object
                SubscriberInfo newSubscriber = new SubscriberInfo();

                // Sets the subscriber properties
                newSubscriber.SubscriberFirstName = "Name";
                newSubscriber.SubscriberLastName = "Surname";
                newSubscriber.SubscriberFullName = "Name Surname";
                newSubscriber.SubscriberEmail = "subscriber@localhost.local";
                newSubscriber.SubscriberSiteID = SiteContext.CurrentSiteID;

                // Saves the subscriber to the database
                SubscriberInfoProvider.SetSubscriberInfo(newSubscriber);
            }


            /// <heading>Updating a subscriber</heading>
            private void GetAndUpdateSubscriber()
            {
                // Gets the subscriber based an email address
                SubscriberInfo updateSubscriber = SubscriberInfoProvider.GetSubscriberInfo("subscriber@localhost.local", SiteContext.CurrentSiteID);

                if (updateSubscriber != null)
                {
                    // Updates the subscriber properties
                    updateSubscriber.SubscriberFullName = updateSubscriber.SubscriberFullName.ToLower();

                    // Saves the updated subscriber to the database
                    SubscriberInfoProvider.SetSubscriberInfo(updateSubscriber);
                }
            }


            /// <heading>Updating multiple subscribers</heading>
            private void GetAndBulkUpdateSubscribers()
            {
                // Gets all subscribers on the current site whose email ends with 'localhost.local'
                var subscribers = SubscriberInfoProvider.GetSubscribers()
                                                            .WhereEquals("SubscriberSiteID", SiteContext.CurrentSiteID)
                                                            .WhereEndsWith("SubscriberEmail", "localhost.local");

                // Loops through individual subscribers
                foreach (SubscriberInfo subscriber in subscribers)
                {
                    // Updates the subscriber properties
                    subscriber.SubscriberFullName = subscriber.SubscriberFullName.ToUpper();

                    // Saves the updated subscriber to the database
                    SubscriberInfoProvider.SetSubscriberInfo(subscriber);
                }
            }


            /// <heading>Adding a subscriber to an email campaign</heading>
            private void SubscribeToCampaign()
            {
                // Gets the subscriber and email campaign
                SubscriberInfo subscriber = SubscriberInfoProvider.GetSubscriberInfo("subscriber@localhost.local", SiteContext.CurrentSiteID);
                NewsletterInfo newsletter = NewsletterInfoProvider.GetNewsletterInfo("NewEmailCampaign", SiteContext.CurrentSiteID);

                if ((subscriber != null) && (newsletter != null))
                {
                    // Adds the subscriber to the specified email campaign
                    // The SubscribeSettings parameter determines which additional actions the system performs during the subscription
                    CMS.Core.Service<ISubscriptionService>.Entry().Subscribe(subscriber.SubscriberID, newsletter.NewsletterID, new SubscribeSettings()
                    {
                        SendConfirmationEmail = true,
                        RequireOptIn = true,

                        // Removes the subscriber from the opt-out list for all of the site's email campaigns if present
                        RemoveAlsoUnsubscriptionFromAllNewsletters = true
                    });
                }
            }


            /// <heading>Deleting a subscriber</heading>
            private void DeleteSubscriber()
            {
                // Gets the subscriber based an email address
                SubscriberInfo deleteSubscriber = SubscriberInfoProvider.GetSubscriberInfo("subscriber@localhost.local", SiteContext.CurrentSiteID);
                
                if (deleteSubscriber != null)
                {
                    // Deletes the subscriber
                    SubscriberInfoProvider.DeleteSubscriberInfo(deleteSubscriber);
                }
            }
        }


        /// <summary>
        /// Holds email API examples.
        /// </summary>
        /// <groupHeading>Emails</groupHeading>
        private class Emails
        {
            /// <heading>Creating a template-based campaign email</heading>
            private void CreateCampaignEmail()
            {
                // Gets the email campaign
                NewsletterInfo emailCampaign = NewsletterInfoProvider.GetNewsletterInfo("NewEmailCampaign", SiteContext.CurrentSiteID);

                if (emailCampaign != null)
                {
                    // Creates a new email object
                    IssueInfo newIssue = new IssueInfo()
                    {
                        // Sets the email properties
                        IssueSubject = "New issue",
                        IssueNewsletterID = emailCampaign.NewsletterID,
                        IssueSiteID = SiteContext.CurrentSiteID,
                        IssueUnsubscribed = 0,
                        IssueSentEmails = 0,
                        IssueTemplateID = emailCampaign.NewsletterTemplateID,
                        IssueShowInNewsletterArchive = false,
                        IssueUseUTM = false,

                        // Defines the email content
                        IssueText = "<?xml version=\"1.0\" encoding=\"utf-16\"?><content><region id=\"content\">Issue text</region></content>"
                    };                    

                    // Saves the campaign email to the database
                    IssueInfoProvider.SetIssueInfo(newIssue);
                }
            }


            /// <heading>Creating a dynamic newsletter issue</heading>
            private void CreateDynamicIssue()
            {
                // Gets the dynamic newsletter
                NewsletterInfo newsletter = NewsletterInfoProvider.GetNewsletterInfo("NewDynamicNewsletter", SiteContext.CurrentSiteID);

                if (newsletter != null)
                {
                    // Generates a new dynamic issue
                    EmailQueueManager.GenerateDynamicIssue(newsletter.NewsletterID);
                }
            }


            /// <heading>Updating campaign emails</heading>
            private void GetAndUpdateCampaignEmails()
            {
                // Gets the email campaign
                NewsletterInfo emailCampaign = NewsletterInfoProvider.GetNewsletterInfo("NewEmailCampaign", SiteContext.CurrentSiteID);

                if (emailCampaign != null)
                {
                    // Gets all of the campaign's emails that have not been sent yet
                    var issues = IssueInfoProvider.GetIssues()
                                                        .WhereEquals("IssueNewsletterID", emailCampaign.NewsletterID)
                                                        .WhereEquals("IssueStatus", IssueStatusEnum.Idle);
                    
                    // Loops through individual emails
                    foreach (IssueInfo issue in issues)
                    {
                        // Updates the email properties
                        issue.IssueSubject = issue.IssueSubject.ToUpper();

                        // Saves the modified email to the database
                        IssueInfoProvider.SetIssueInfo(issue);
                    }
                }
            }


            /// <heading>Deleting campaign emails</heading>
            private void DeleteCampaignEmails()
            {
                // Gets the email campaign
                NewsletterInfo emailCampaign = NewsletterInfoProvider.GetNewsletterInfo("NewEmailCampaign", SiteContext.CurrentSiteID);

                if (emailCampaign != null)
                {
                    // Gets all of the campaign's emails that were already sent
                    var issues = IssueInfoProvider.GetIssues()
                                                        .WhereEquals("IssueNewsletterID", emailCampaign.NewsletterID)
                                                        .WhereEquals("IssueStatus", IssueStatusEnum.Finished);

                    // Loops through individual emails
                    foreach (IssueInfo deleteIssue in issues)
                    {
                        // Deletes the email
                        IssueInfoProvider.DeleteIssueInfo(deleteIssue);
                    }
                }                
            }
        }
    }
}
