<%@ Control Language="C#" AutoEventWireup="true" CodeFile="newsletter_signup.ascx.cs" Inherits="CMSWebParts_mekorma_custom_newsletter_signup" %>

<div id="emailDiv">
    <asp:TextBox runat="server" ID="emailTxt" placeholder="email address" ></asp:TextBox>
    <asp:Button OnClick="btn_Click" ID="btn" runat="server" CssClass="newsletter-btn" />
    <asp:RegularExpressionValidator CssClass="newsletter_conf" ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" ControlToValidate="emailTxt" ErrorMessage="Invalid Email"></asp:RegularExpressionValidator>
    <asp:Label ForeColor="Red" ID="lbl" runat="server" CssClass="newsletter_conf"></asp:Label>

</div>
