﻿using System;

using CMS.Community;
using CMS.Membership;
using CMS.Forums;
using CMS.SiteProvider;
using CMS.Helpers;
using CMS.DataEngine;

namespace APIExamples
{
    /// <summary>
    /// Holds forum API examples.
    /// </summary>
    /// <pageTitle>Forums</pageTitle>
    internal class ForumsMain
    {
        /// <summary>
        /// Holds forum group API examples.
        /// </summary>
        /// <groupHeading>Forum groups</groupHeading>
        private class ForumGroups
        {
            /// <heading>Creating a forum group</heading>
            private void CreateForumGroup()
            {
                // Creates a new forum group object
                ForumGroupInfo newGroup = new ForumGroupInfo();

                // Sets the forum group properties
                newGroup.GroupDisplayName = "New group";
                newGroup.GroupName = "NewGroup";
                newGroup.GroupSiteID = SiteContext.CurrentSiteID;
                newGroup.GroupAuthorDelete = true;
                newGroup.GroupAuthorEdit = true;
                newGroup.GroupDisplayEmails = true;

                // Saves the forum group to the database
                ForumGroupInfoProvider.SetForumGroupInfo(newGroup);
            }


            /// <heading>Updating a forum group</heading>
            private void GetAndUpdateForumGroup()
            {
                // Gets the forum group
                ForumGroupInfo updateGroup = ForumGroupInfoProvider.GetForumGroupInfo("NewGroup", SiteContext.CurrentSiteID);
                if (updateGroup != null)
                {
                    // Updates the properties
                    updateGroup.GroupDisplayName = updateGroup.GroupDisplayName.ToLower();

                    // Saves the changes
                    ForumGroupInfoProvider.SetForumGroupInfo(updateGroup);
                }
            }


            /// <heading>Updating multiple forum groups</heading>
            private void GetAndBulkUpdateForumGroups()
            {
                // Prepares parameters for loading all forum groups whose name starts with 'NewGroup'
                string where = "GroupName LIKE N'NewGroup%'";
                string orderBy = "";                

                // Gets the forum groups based on the parameters
                InfoDataSet<ForumGroupInfo> groups = ForumGroupInfoProvider.GetGroups(where, orderBy);

                // Loops through individual groups
                foreach (ForumGroupInfo forumGroup in groups)
                {
                    // Updates the properties
                    forumGroup.GroupDisplayName = forumGroup.GroupDisplayName.ToUpper();

                    // Saves the changes to the database
                    ForumGroupInfoProvider.SetForumGroupInfo(forumGroup);
                }
            }


            /// <heading>Deleting a forum group</heading>
            private void DeleteForumGroup()
            {
                // Gets the forum group
                ForumGroupInfo deleteGroup = ForumGroupInfoProvider.GetForumGroupInfo("NewGroup", SiteContext.CurrentSiteID);

                if (deleteGroup != null)
                {
                    // Deletes the forum group
                    ForumGroupInfoProvider.DeleteForumGroupInfo(deleteGroup);
                }
            }
        }


        /// <summary>
        /// Holds forum API examples.
        /// </summary>
        /// <groupHeading>Forums</groupHeading>
        private class Forums
        {
            /// <heading>Creating a forum</heading>
            private void CreateForum()
            {
                // Gets the parent forum group
                ForumGroupInfo group = ForumGroupInfoProvider.GetForumGroupInfo("NewGroup", SiteContext.CurrentSiteID);

                if (group != null)
                {
                    // Creates a new forum object
                    ForumInfo newForum = new ForumInfo();

                    // Sets the forum properties
                    newForum.ForumDisplayName = "New forum";
                    newForum.ForumName = "NewForum";
                    newForum.ForumGroupID = group.GroupID;
                    newForum.ForumSiteID = group.GroupSiteID;
                    newForum.AllowAccess = SecurityAccessEnum.AllUsers;
                    newForum.AllowAttachFiles = SecurityAccessEnum.AuthenticatedUsers;
                    newForum.AllowPost = SecurityAccessEnum.AllUsers;
                    newForum.AllowReply = SecurityAccessEnum.AllUsers;
                    newForum.AllowSubscribe = SecurityAccessEnum.AllUsers;
                    newForum.ForumOpen = true;
                    newForum.ForumModerated = false;
                    newForum.ForumThreads = 0;
                    newForum.ForumPosts = 0;

                    // Saves the forum to the database
                    ForumInfoProvider.SetForumInfo(newForum);
                }
            }


            /// <heading>Updating a forum</heading>
            private void GetAndUpdateForum()
            {
                // Gets the forum
                ForumInfo updateForum = ForumInfoProvider.GetForumInfo("NewForum", SiteContext.CurrentSiteID);
                if (updateForum != null)
                {
                    // Updates the properties
                    updateForum.ForumDisplayName = updateForum.ForumDisplayName.ToLower();

                    // Saves the changes to the database
                    ForumInfoProvider.SetForumInfo(updateForum);
                }
            }


            /// <heading>Updating multiple forums</heading>
            private void GetAndBulkUpdateForums()
            {
                // Prepares parameters for loading the first 10 forums whose name starts with 'NewForum'
                string where = "ForumName LIKE N'NewForum%'";
                string orderBy = "";
                string columns = "";
                int topN = 10;

                // Gets the forums based on the parameters
                InfoDataSet<ForumInfo> forums = ForumInfoProvider.GetForums(where, orderBy, topN, columns);

                // Loops through individual forums
                foreach (ForumInfo forum in forums)
                {
                    // Updates the properties
                    forum.ForumDisplayName = forum.ForumDisplayName.ToUpper();

                    // Saves the changes to the database
                    ForumInfoProvider.SetForumInfo(forum);
                }
            }

            /// <heading>Deleting a forum</heading>
            private void DeleteForum()
            {
                // Gets the forum
                ForumInfo deleteForum = ForumInfoProvider.GetForumInfo("NewForum", SiteContext.CurrentSiteID);

                if (deleteForum != null)
                {
                    // Deletes the forum
                    ForumInfoProvider.DeleteForumInfo(deleteForum);
                }
            }
        }


        /// <summary>
        /// Holds forum post API examples.
        /// </summary>
        /// <groupHeading>Forum posts</groupHeading>
        private class ForumPosts
        {
            /// <heading>Creating a forum post</heading>
            private void CreateForumPost()
            {
                // Gets the forum
                ForumInfo forum = ForumInfoProvider.GetForumInfo("NewForum", SiteContext.CurrentSiteID);
                if (forum != null)
                {
                    // Creates a new forum post object
                    ForumPostInfo newPost = new ForumPostInfo();

                    // Sets the forum post properties
                    newPost.PostUserID = MembershipContext.AuthenticatedUser.UserID;
                    newPost.PostUserMail = MembershipContext.AuthenticatedUser.Email;
                    newPost.PostUserName = MembershipContext.AuthenticatedUser.UserName;
                    newPost.PostForumID = forum.ForumID;
                    newPost.SiteId = forum.ForumSiteID;
                    newPost.PostTime = DateTime.Now;
                    newPost.PostApproved = true;
                    newPost.PostSubject = "New post";
                    newPost.PostText = "This is a new post";

                    // Saves the forum post to the database
                    ForumPostInfoProvider.SetForumPostInfo(newPost);
                }
            }


            /// <heading>Updating multiple forum posts</heading>
            private void GetAndBulkUpdateForumPosts()
            {
                // Prepares a condition for loading all forum posts whose subject starts with 'New post'
                string where = "PostSubject LIKE N'New post%'";

                // Gets the forum posts based on the condition
                InfoDataSet<ForumPostInfo> posts = ForumPostInfoProvider.GetForumPosts(where);

                // Loops through individual forum posts
                foreach (ForumPostInfo forumPost in posts)
                {
                    // Updates the forum post properties
                    forumPost.PostSubject = forumPost.PostSubject.ToUpper();

                    // Saves the changes to the database
                    ForumPostInfoProvider.SetForumPostInfo(forumPost);
                }
            }


            /// <heading>Deleting forum posts</heading>
            private void GetAndBulkDeleteForumPosts()
            {
                // Prepares a condition for loading all unapproved forum posts
                string where = "PostApproved = 0";

                // Gets all unapproved forum posts
                InfoDataSet<ForumPostInfo> posts = ForumPostInfoProvider.GetForumPosts(where);

                // Loops through individual forum posts
                foreach (ForumPostInfo forumPost in posts)
                {
                    if (forumPost != null)
                    {
                        // Deletes the forum post
                        ForumPostInfoProvider.DeleteForumPostInfo(forumPost);
                    }
                }
            }
        }
    }
}
