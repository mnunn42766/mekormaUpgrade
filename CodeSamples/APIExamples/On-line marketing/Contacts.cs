using System;

using CMS.OnlineMarketing;
using CMS.SiteProvider;
using CMS.Base;
using CMS.Membership;
using CMS.DataEngine;

namespace APIExamples
{
    /// <summary>
    /// Holds contact-related API examples.
    /// </summary>
    /// <pageTitle>Contacts</pageTitle>
    internal class ContactsMain
    {
        /// <summary>
        /// Holds contact API examples.
        /// </summary>
        /// <groupHeading>Contacts</groupHeading>
        private class Contacts
        {
            /// <heading>Creating a contact</heading>
            private void CreateContact()
            {
                // Creates a new contact object for the current site
                ContactInfo newContact = new ContactInfo()
                {
                    ContactLastName = "Smith",
                    ContactFirstName = "John",
                    ContactSiteID = SiteContext.CurrentSiteID,
                    ContactIsAnonymous = true
                };

                // Saves the contact to the database
                ContactInfoProvider.SetContactInfo(newContact);
            }


            /// <heading>Updating a contact</heading>
            private void GetAndUpdateContact()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo updateContact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (updateContact != null)
                {
                    // Updates the contact's properties
                    updateContact.ContactCompanyName = "Company Inc.";

                    // Saves the updated contact to the database
                    ContactInfoProvider.SetContactInfo(updateContact);
                }                
            }


            /// <heading>Updating multiple contacts</heading>
            private void GetAndBulkUpdateContacts()
            {
                // Gets all contacts on the current site whose last name is 'Smith'
                var contacts = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID);

                // Loops through individual contacts
                foreach (ContactInfo contact in contacts)
                {
                    // Updates the properties of the contact
                    contact.ContactCompanyName = "Company Inc.";

                    // Saves the updated contact to the database
                    ContactInfoProvider.SetContactInfo(contact);
                }
            }


            /// <heading>Linking contacts with user accounts</heading>
            private void AddMembership()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo contact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (contact != null)
                {
                    // Links the contact with the current user
                    ContactMembershipInfoProvider.SetRelationship(
                        MembershipContext.AuthenticatedUser.UserID,
                        MemberTypeEnum.CmsUser,
                        contact.ContactID,
                        contact.ContactID,
                        false);
                }
            }


            /// <heading>Removing user accounts from contacts</heading>
            private void RemoveMembership()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo contact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (contact != null)
                {
                    // Gets the relationship between the contact and the current user
                    ContactMembershipInfo contactMembership = ContactMembershipInfoProvider.GetMembershipInfo(
                        contact.ContactID,
                        contact.ContactID,
                        MembershipContext.AuthenticatedUser.UserID,
                        MemberTypeEnum.CmsUser);

                    // Deletes the contact-user relationship
                    ContactMembershipInfoProvider.DeleteRelationship(contactMembership.MembershipID);
                }
            }


            /// <heading>Assigning IP addresses to contacts</heading>
            private void AddIPAddress()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo contact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (contact != null)
                {
                    // Creates a new IP address and assigns it to the contact
                    IPInfo newIP = new IPInfo()
                    {
                        IPAddress = "127.0.0.1",
                        IPOriginalContactID = contact.ContactID,
                        IPActiveContactID = contact.ContactID
                    };

                    // Saves the IP address to the database
                    IPInfoProvider.SetIPInfo(newIP);
                }
            }


            /// <heading>Removing IP addresses from contacts</heading>
            private void RemoveIPAddress()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo contact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (contact != null)
                {
                    // Gets all IP addresses assigned to the contact
                    var deleteIPs = IPInfoProvider.GetIps().WhereEquals("IPOriginalContactID", contact.ContactID);

                    // Loops through individual IP adresses
                    foreach (IPInfo ipAddress in deleteIPs)
                    {
                        // Deletes the IP address
                        IPInfoProvider.DeleteIPInfo(ipAddress);
                    }
                }
            }


            /// <heading>Assigning user agents to contacts</heading>
            private void AddUserAgent()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo contact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (contact != null)
                {
                    // Creates a new agent object and assigns it to the contact
                    UserAgentInfo agentInfo = new UserAgentInfo()
                    {
                        UserAgentActiveContactID = contact.ContactID,
                        UserAgentOriginalContactID = contact.ContactID,
                        UserAgentString = "Custom User Agent"
                    };

                    // Saves the user agent object to the database
                    UserAgentInfoProvider.SetUserAgentInfo(agentInfo);
                }
            }


            /// <heading>Removing user agents from contacts</heading>
            private void RemoveUserAgent()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo contact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (contact != null)
                {
                    // Gets all user agents assigned to the contact
                    var deleteAgents = UserAgentInfoProvider.GetUserAgents().WhereEquals("UserAgentOriginalContactID", contact.ContactID);

                    // Loops through individual user agents
                    foreach (UserAgentInfo agent in deleteAgents)
                    {
                        // Deletes the user agent
                        UserAgentInfoProvider.DeleteUserAgentInfo(agent);
                    }
                }
            }


            /// <heading>Deleting a contact</heading>
            private void DeleteContact()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo deleteContact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                if (deleteContact != null)
                {
                    // Deletes the contact
                    ContactInfoProvider.DeleteContactInfo(deleteContact);
                }
            }
        }


        /// <summary>
        /// Holds contact status API examples.
        /// </summary>
        /// <groupHeading>Contact statuses</groupHeading>
        private class ContactStatuses
        {
            /// <heading>Creating a contact status</heading>
            private void CreateContactStatus()
            {
                // Creates a new contact status object for the current site
                ContactStatusInfo newStatus = new ContactStatusInfo()
                {
                    ContactStatusDisplayName = "New contact status",
                    ContactStatusName = "NewContactStatus",
                    ContactStatusSiteID = SiteContext.CurrentSiteID
                };

                // Saves the contact status to the database
                ContactStatusInfoProvider.SetContactStatusInfo(newStatus);
            }


            /// <heading>Updating a contact status</heading>
            private void GetAndUpdateContactStatus()
            {
                // Gets the contact status
                ContactStatusInfo updateStatus = ContactStatusInfoProvider.GetContactStatusInfo("NewContactStatus", SiteContext.CurrentSiteName);

                if (updateStatus != null)
                {
                    // Updates the contact status properties
                    updateStatus.ContactStatusDisplayName = updateStatus.ContactStatusDisplayName.ToLowerCSafe();

                    // Saves the modified contact status to the database
                    ContactStatusInfoProvider.SetContactStatusInfo(updateStatus);
                }
            }


            /// <heading>Updating multiple contact statuses</heading>
            private void GetAndBulkUpdateContactStatuses()
            {
                // Gets all contact statuses from the current site whose code name starts with 'New'
                var statuses = ContactStatusInfoProvider.GetContactStatuses()
                                                                .WhereEquals("ContactStatusSiteID", SiteContext.CurrentSiteID)
                                                                .WhereStartsWith("ContactStatusName","New");

                // Loops through individual contact statuses
                foreach (ContactStatusInfo contactStatus in statuses)
                {
                    // Updates the contact status properties
                    contactStatus.ContactStatusDisplayName = contactStatus.ContactStatusDisplayName.ToUpper();

                    // Saves the modified contact status to the database
                    ContactStatusInfoProvider.SetContactStatusInfo(contactStatus);
                }
            }


            /// <heading>Assigning a status to a contact</heading>
            private void AddContactStatusToContact()
            {
                // Gets the first contact on the current site whose last name is 'Smith'
                ContactInfo contact = ContactInfoProvider.GetContacts()
                                                    .WhereEquals("ContactLastName", "Smith")
                                                    .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                    .FirstObject;

                // Gets the contact status
                ContactStatusInfo contactStatus = ContactStatusInfoProvider.GetContactStatusInfo("NewContactStatus", SiteContext.CurrentSiteName);

                if ((contact != null) && (contactStatus != null))
                {
                    // Checks that the contact doesn't already have the given status
                    if (contact.ContactStatusID != contactStatus.ContactStatusID)
                    {
                        // Assigns the status to the contact
                        contact.ContactStatusID = contactStatus.ContactStatusID;

                        // Saves the updated contact to the database
                        ContactInfoProvider.SetContactInfo(contact);
                    }
                }
            }


            /// <heading>Removing statuses from contacts</heading>
            private void RemoveContactStatusFromContact()
            {
                // Gets the contact status
                ContactStatusInfo contactStatus = ContactStatusInfoProvider.GetContactStatusInfo("NewContactStatus", SiteContext.CurrentSiteName);

                if (contactStatus != null)
                {
                    // Gets all contacts from the current site that have the specified status
                    var contacts = ContactInfoProvider.GetContacts()
                                                        .WhereEquals("ContactSiteID", SiteContext.CurrentSiteID)
                                                        .WhereEquals("ContactStatusID", contactStatus.ContactStatusID);

                    // Loops through the contacts
                    foreach (ContactInfo contact in contacts)
                    {
                        // Sets the status to 'None' for each contact
                        contact.ContactStatusID = 0;

                        // Saves the updated contact to the database
                        ContactInfoProvider.SetContactInfo(contact);
                    }
                }
            }


            /// <heading>Deleting a contact status</heading>
            private void DeleteContactStatus()
            {
                // Gets the contact status
                ContactStatusInfo deleteStatus = ContactStatusInfoProvider.GetContactStatusInfo("NewContactStatus", SiteContext.CurrentSiteName);

                if (deleteStatus != null)
                {
                    // Deletes the contact status
                    ContactStatusInfoProvider.DeleteContactStatusInfo(deleteStatus);
                }
            }
        }
    }
}
