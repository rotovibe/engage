<%@ Page Language="C#" MasterPageFile="~/App_Resource/Atmosphere.Core.dll/Atmosphere.Web.C3.Master" AutoEventWireup="true" CodeBehind="ErrorMessage.aspx.cs" Inherits="C3.Web.ErrorMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TopNav" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
<div id="errorMessage">
<!--<h2>System Error</h2>-->
<p>Error Message: <span><%=ViewState["Error"].ToString() %></span></p>
</div>
</asp:Content>
