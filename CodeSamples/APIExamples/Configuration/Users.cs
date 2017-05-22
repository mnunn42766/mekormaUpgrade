using System;
using System.Data;

using CMS.Membership;
using CMS.Base;
using CMS.SiteProvider;
using CMS.DocumentEngine;
using CMS.DataEngine;
using CMS.Helpers;

namespace APIExamples
{
    /// <summary>
    /// Holds user-related API examples.
    /// </summary>
    /// <pageTitle>Users</pageTitle>
    internal class UsersMain
    {
        /// <summary>
        /// Holds user API examples.
        /// </summary>
        /// <groupHeading>Users</groupHeading>
        private class Users
        {
            /// <heading>Creating a new user</heading>
            private void CreateUser()
            {
                // Creates a new user object
                UserInfo newUser = new UserInfo();

                // Sets the user properties
                newUser.FullName = "New user";
                newUser.UserName = "NewUser";
                newUser.Email = "new.user@domain.com";
                newUser.PreferredCultureCode = "en-us";
                
                // Sets the user's privilege level to 'Editor'
                newUser.SetPrivilegeLevel(UserPrivilegeLevelEnum.Editor);

                // Saves the user to the database
                UserInfoProvider.SetUserInfo(newUser);
            }

            /// <heading>Updating an existing user</heading>
            private void GetAndUpdateUser()
            {
                // Gets the user
                UserInfo updateUser = UserInfoProvider.GetUserInfo("NewUser");
                if (updateUser != null)
                {
                    // Updates the user's properties
                    updateUser.FullName = updateUser.FullName.ToLowerCSafe();

                    // Saves the changes
                    UserInfoProvider.SetUserInfo(updateUser);                    
                }
            }

            /// <heading>Updating multiple users</heading>
            private void GetAndBulkUpdateUsers()
            {
                // Gets all users whose username starts with 'NewUser'
                var users = UserInfoProvider.GetUsers().WhereStartsWith("UserName", "NewUser");

                // Loops through individual users
                foreach (UserInfo modifyUser in users)
                {
                    // Updates the user properties
                    modifyUser.FullName = modifyUser.FullName.ToUpper();

                    // Saves the changes
                    UserInfoProvider.SetUserInfo(modifyUser);
                }                
            }

            /// <heading>Deleting a user</heading>
            private void DeleteUser()
            {
                // Gets the user
                UserInfo deleteUser = UserInfoProvider.GetUserInfo("NewUser");

                if (deleteUser != null)
                {
                    // Deletes the user
                    UserInfoProvider.DeleteUser(deleteUser);
                }
            }

            /// <heading>Authenticating a user</heading>
            private void AuthenticateUser()
            {
                UserInfo user = null;

                // Attempts to log in to the current site using a username and password
                user = AuthenticationHelper.AuthenticateUser("Username", "password", SiteContext.CurrentSiteName);

                if (user != null)
                {
                    // Authentication was successful
                }
            }
        }

        /// <summary>
        /// Holds user-site API examples.
        /// </summary>
        /// <groupHeading>User-site relationships</groupHeading>
        private class UserSite
        {
            /// <heading>Getting all sites to which a user is assigned</heading>
            private void GetUserSites()
            {
                // Gets the user
                UserInfo user = UserInfoProvider.GetUserInfo("NewUser");                

                if (user != null)
                {
                    // Gets the sites to which the user is assigned
                    var userSiteIDs = UserSiteInfoProvider.GetUserSites().Column("SiteID").WhereEquals("UserID", user.UserID);
                    var sites = SiteInfoProvider.GetSites().WhereIn("SiteID", userSiteIDs);
                    
                    // Loops through the sites
                    foreach (SiteInfo site in sites)
                    {                                                    
                        // Process the site
                    }
                }
            }

            /// <heading>Assigning a user to a site</heading>
            private void AddUserToSite()
            {
                // Gets the user
                UserInfo user = UserInfoProvider.GetUserInfo("NewUser");
                if (user != null)
                {                    
                    // Adds the user to the site
                    UserInfoProvider.AddUserToSite(user.UserName, SiteContext.CurrentSiteName);
                }
            }

            /// <heading>Removing a user from a site</heading>
            private void RemoveUserFromSite()
            {
                // Gets the user
                UserInfo removeUser = UserInfoProvider.GetUserInfo("NewUser");
                if (removeUser != null)
                {
                    // Removes the user from the site
                    UserInfoProvider.RemoveUserFromSite(removeUser.UserName, SiteContext.CurrentSiteName);
                }
            }
        }


        /// <summary>
        /// Holds user authorization API examples.
        /// </summary>
        /// <groupHeading>User authorization</groupHeading>
        private class UserAuthorization
        {
            /// <heading>Checking the user privilege level</heading>
            private void CheckPrivilegeLevel()
            {
                // Gets the user
                UserInfo user = UserInfoProvider.GetUserInfo("NewUser");

                if (user != null)
                {
                    // Checks whether the user has the Editor privilege level or higher
                    if (user.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Editor, SiteContext.CurrentSiteName))
                    {
                        // Perform an action (the user has the required privilege level)
                    }
                }
            }

            /// <heading>Checking permissions for a module</heading>
            private void CheckModulePermissions()
            {
                // Gets the user
                UserInfo user = UserInfoProvider.GetUserInfo("NewUser");

                if (user != null)
                {
                    // Checks whether the user has the Read permission for the Content module
                    if (UserInfoProvider.IsAuthorizedPerResource("CMS.Content", "Read", SiteContext.CurrentSiteName, user))
                    {
                        // Perform an action (the user has the required module permission)
                    }
                }
            }

            /// <heading>Checking permissions for a page type or custom table</heading>
            private void CheckPageTypePermissions()
            {
                // Gets the user
                UserInfo user = UserInfoProvider.GetUserInfo("NewUser");

                if (user != null)
                {
                    // Checks whether the user has the Read permission for the CMS.MenuItem page type
                    if (UserInfoProvider.IsAuthorizedPerClass(SystemDocumentTypes.MenuItem, "Read", SiteContext.CurrentSiteName, user))
                    {
                        // Perform an action (the user is authorized to read CMS.MenuItem page types)
                    }
                }
            }

            /// <heading>Checking permissions for specific pages (ACLs)</heading>
            private void IsAuthorizedToDocument()
            {
                // Creates a TreeProvider instance
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

                // Gets the Example page
                TreeNode page = tree.SelectSingleNode(SiteContext.CurrentSiteName, "/Example", "en-US");

                if (page != null)
                {
                    // Gets the user
                    UserInfo user = UserInfoProvider.GetUserInfo("NewUser");

                    if (user != null)
                    {
                        // Checks whether the user is authorized to read the page
                        if (user.IsAuthorizedPerTreeNode(page, NodePermissionsEnum.Read) == AuthorizationResultEnum.Allowed)
                        {
                            // Perform an action (the user is allowed to read the page)
                        }
                    }
                }
            }            
        }


        /// <summary>
        /// Holds online user API examples.
        /// </summary>
        /// <groupHeading>Online users</groupHeading>
        private class UserOnline
        {
            /// <heading>Getting and updating online users</heading>
            private void GetOnlineUsers()
            {
                string where = "";
                int topN = 10;
                string orderBy = "";
                string location = "";
                string siteName = SiteContext.CurrentSiteName;
                bool includeHidden = true;
                bool includeKicked = false;

                // Gets DataSet of online users
                DataSet users = SessionManager.GetOnlineUsers(where, orderBy, topN, location, siteName, includeHidden, includeKicked);
                if (!DataHelper.DataSourceIsEmpty(users))
                {
                    // Loops through the online user data
                    foreach (DataRow userDr in users.Tables[0].Rows)
                    {
                        // Creates a user from the DataRow
                        UserInfo modifyUser = new UserInfo(userDr);

                        // Updates the user's properties
                        modifyUser.FullName = modifyUser.FullName.ToUpper();

                        // Saves the changes to the database
                        UserInfoProvider.SetUserInfo(modifyUser);
                    }
                }
            }

            /// <heading>Checking if a user is online</heading>
            private void IsUserOnline()
            {
                bool includeHidden = true;

                // Gets user and site objects
                UserInfo user = UserInfoProvider.GetUserInfo("NewUser");
                SiteInfo site = SiteInfoProvider.GetSiteInfo(SiteContext.CurrentSiteName);

                if ((user != null) && (site != null))
                {
                    // Checks if the user is online
                    bool userIsOnline = SessionManager.IsUserOnline(site.SiteName, user.UserID, includeHidden);
                }
            }

            /// <heading>Kicking an online user</heading>
            private void KickUser()
            {
                // Gets the user 
                UserInfo kickedUser = UserInfoProvider.GetUserInfo("NewUser");

                if (kickedUser != null)
                {
                    // Kicks the user
                    SessionManager.KickUser(kickedUser.UserID);
                }
            }
        }
    }
}
