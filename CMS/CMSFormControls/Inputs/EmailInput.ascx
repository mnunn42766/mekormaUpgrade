<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSFormControls_Inputs_EmailInput"  CodeFile="EmailInput.ascx.cs" %>

<cms:CMSTextBox runat="server" ID="txtEmailInput" />
<cms:CMSRegularExpressionValidator ID="revEmailValid" runat="server" ControlToValidate="txtEmailInput" Display="Dynamic" EnableViewState="false" Enabled="False" />