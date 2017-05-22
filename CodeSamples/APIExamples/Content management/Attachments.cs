using System;

using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;

namespace APIExamples
{
    /// <summary>
    /// Holds page attachment API examples.
    /// </summary>
    /// <pageTitle>Attachments</pageTitle>
    internal class Attachments
    {        
        /// <heading>Adding unsorted attachments</heading>
        private void InsertUnsortedAttachment()
        {
            // Creates a new instance of the Tree provider
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            // Gets a page
            TreeNode page = tree.SelectSingleNode(SiteContext.CurrentSiteName, "/Articles", "en-us");

            // Prepares the path of the file
            string file = System.Web.HttpContext.Current.Server.MapPath("/FileFolder/file.png");

            if (page != null)
            {
                // Adds the file as an attachment of the page
                DocumentHelper.AddUnsortedAttachment(page, Guid.NewGuid(), file, tree, ImageHelper.AUTOSIZE, ImageHelper.AUTOSIZE, ImageHelper.AUTOSIZE);
            }
        }


        /// <heading>Inserting attachments into page fields</heading>
        private void InsertFieldAttachment()
        {
            // Creates a new instance of the Tree provider
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            // Gets a page
            TreeNode page = tree.SelectSingleNode(SiteContext.CurrentSiteName, "/Articles", "en-us");

            if (page != null)
            {
                AttachmentInfo attachment = null;

                // Prepares the path of the file
                string file = System.Web.HttpContext.Current.Server.MapPath("/FileFolder/file.png");

                // Inserts the attachment into the "MenuItemTeaserImage" field and updates the page
                attachment = DocumentHelper.AddAttachment(page, "MenuItemTeaserImage", file, tree);
                page.Update();
            }
        }        

        
        /// <heading>Changing the order of unsorted attachments</heading>
        private void MoveAttachmentUpDown()
        {
            // Creates a new instance of the Tree provider
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            // Gets a page
            TreeNode page = tree.SelectSingleNode(SiteContext.CurrentSiteName, "/Articles", "en-us");

            if (page != null)
            {
                // Gets an attachment by file name
                AttachmentInfo attachment = DocumentHelper.GetAttachment(page, "file.png", tree);

                // Moves the attachment down in the list
                DocumentHelper.MoveAttachmentDown(attachment.AttachmentGUID, page);

                // Moves the attachment up in the list
                DocumentHelper.MoveAttachmentUp(attachment.AttachmentGUID, page);
            }
        }


        /// <heading>Modifying attachment metadata</heading>
        private void EditMetadata()
        {
            // Creates a new instance of the Tree provider
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            // Gets a page
            TreeNode page = tree.SelectSingleNode(SiteContext.CurrentSiteName, "/Articles", "en-us");

            if (page != null)
            {
                // Gets an attachment by file name
                AttachmentInfo attachment = DocumentHelper.GetAttachment(page, "file.png", tree);

                // Edits the attachment's metadata (name, title and description)
                attachment.AttachmentName += " - modified";
                attachment.AttachmentTitle = "Attachment title";
                attachment.AttachmentDescription = "Attachment description.";

                // Ensures that the attachment can be updated without supplying its binary data
                attachment.AllowPartialUpdate = true;

                // Saves the modified attachment into the database
                AttachmentInfoProvider.SetAttachmentInfo(attachment);
            }
        }


        /// <heading>Deleting attachments</heading>
        private void DeleteAttachments()
        {
            // Creates a new instance of the Tree provider
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            // Gets a page
            TreeNode page = tree.SelectSingleNode(SiteContext.CurrentSiteName, "/Articles", "en-us");

            if (page != null)
            {
                // Gets an attachment by file name
                AttachmentInfo attachment = DocumentHelper.GetAttachment(page, "file.png", tree);

                // Deletes the attachment
                DocumentHelper.DeleteAttachment(page, attachment.AttachmentGUID, tree);
            }
        }
    }
}