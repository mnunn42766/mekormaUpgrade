<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSAdminControls_UI_UIProfiles_UIElementEdit"
     CodeFile="UIElementEdit.ascx.cs" %>
<cms:CMSUpdatePanel runat="server" ID="pnlUpdate">
    <ContentTemplate>
        <cms:UIForm runat="server" ID="EditForm" ObjectType="CMS.UIElement" RedirectUrlAfterCreate="" SetDefaultValuesToDisabledFields="false"
            IsLiveSite="false" />
    </ContentTemplate>
</cms:CMSUpdatePanel>
