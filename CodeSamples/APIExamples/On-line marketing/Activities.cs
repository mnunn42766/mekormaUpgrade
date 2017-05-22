using System;

using CMS.OnlineMarketing;
using CMS.WebAnalytics;
using CMS.SiteProvider;
using CMS.Base;

namespace APIExamples
{
    /// <summary>
    /// Holds activity API examples.
    /// </summary>
    /// <pageTitle>Activities</pageTitle>
    internal class Activities
    {
        /// <heading>Logging an activity for a contact</heading>
        private void CreateActivity()
        {
            // Gets the first contact whose last name is 'Smith'            
            ContactInfo contact = ContactInfoProvider.GetContacts()
                                                .WhereEquals("ContactLastName", "Smith")
                                                .FirstObject;

            if (contact != null)
            {
                // Creates a new activity of the "Forum post" type for the given contact
                ActivityInfo newActivity = new ActivityInfo
                {
                    ActivityType = PredefinedActivityType.FORUM_POST,
                    ActivityTitle = "New forum post",
                    ActivitySiteID = SiteContext.CurrentSiteID,
                    ActivityOriginalContactID = contact.ContactID,
                    ActivityActiveContactID = contact.ContactID
                };

                // Saves the activity to the database
                ActivityInfoProvider.SetActivityInfo(newActivity);
            }
        }


        /// <heading>Updating logged activities</heading>
        private void GetAndUpdateActivity()
        {
            // Gets the first contact whose last name is 'Smith'
            ContactInfo contact = ContactInfoProvider.GetContacts()
                                                .WhereEquals("ContactLastName", "Smith")
                                                .FirstObject;

            if (contact != null)
            {
                // Gets all activities logged for the contact
                var updateActivities = ActivityInfoProvider.GetActivities().WhereEquals("ActivityActiveContactID", contact.ContactID);

                // Loops through individual activities
                foreach (ActivityInfo activity in updateActivities)
                {
                    // Updates the activity title
                    activity.ActivityTitle = activity.ActivityTitle.ToUpper();

                    // Saves the modified activity to the database
                    ActivityInfoProvider.SetActivityInfo(activity);
                }
            }
        }


        /// <heading>Deleting logged activities</heading>
        private void DeleteActivity()
        {
            // Gets the first contact whose last name is 'Smith'
            ContactInfo contact = ContactInfoProvider.GetContacts()
                                                .WhereEquals("ContactLastName", "Smith")
                                                .FirstObject;

            if (contact != null)
            {
                // Gets all activities logged for the contact
                var activities = ActivityInfoProvider.GetActivities().WhereEquals("ActivityActiveContactID", contact.ContactID);

                // Loops through individual activities
                foreach (ActivityInfo activity in activities)
                {
                    // Deletes the activity
                    ActivityInfoProvider.DeleteActivityInfo(activity);
                }
            }
        }
    }
}
