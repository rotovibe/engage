<%@ Page Language="C#" MasterPageFile="~/App_Resource/Atmosphere.Core.dll/Atmosphere.Web.C3.Master" 
AutoEventWireup="true" CodeBehind="TermsOfService.aspx.cs" Inherits="C3.Web.TermsOfService" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
    <div class="tos_print_right">
        <a id="TOSPDF" runat="server" href="#" target="_blank"><img src="Assets/Images/print.gif" alt="Print" title="Print" /></a>
    </div>

    <div class="scroll" id="divTOS" runat="server"></div>
    <div class="tos_button_location">
        <asp:Button CssClass="button" Text="I Do Not Agree" runat="server" 
            ID="btnNoAccept" onclick="btnNoAccept_Click" />&nbsp;&nbsp;
        <asp:Button CssClass="button" Text="I Agree" runat="server" ID="btnAccept" onclick="btnAccept_Click" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".supportControls").hide();
        });
    </script>
</asp:Content>
