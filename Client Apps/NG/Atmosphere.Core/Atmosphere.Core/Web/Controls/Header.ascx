<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="C3.Web.Controls.Header" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:UpdatePanel ID="updHeader" runat="server">
    <triggers>
        <asp:AsyncPostBackTrigger ControlID="btnChangePassword" />
        <asp:AsyncPostBackTrigger ControlID="btnSignOut" />
    </triggers>
    <contenttemplate>
    <div id="divHeaderTop" class="section headerTop">
        <asp:Panel ID="pnlContainer" runat="server" CssClass="headerContainer">
            <div class="account-controls" id="divSupportContainer">
                <div class="dropdownMenuContainer" id="divSupportMenuContainer">
                    <div class="dropdownMenuTop help">
                        <div class="dropdownMenuDownArrow whiteArrow"></div>
                        <div class="dropdownMenuHelpIcon lightBlueHelpIcon"></div>
                    </div>
                    <div class="dropdownMenuItemContainer">
                        <a id="supportRequestLink" href="#">Support Request</a>
                        <a href="https://www.yammer.com/populationhealth" target="_blank">Client Community</a>   
                    </div>
                </div>
            </div>
            <div id="loggedInUserContainer">               
                <div id="divAccount" class="account-controls">
                <asp:Panel ID="pnlWelcome" ToolTip="" class="dropdownMenuContainer"  runat="server">
                    <div class="dropdownMenuTop">
                        <div class="dropdownMenuDownArrow whiteArrow"></div>
                        <div class="dropdownMenuText">
                            <asp:Label
                            ID="lblWelcome" 
                            unselectable="on" 
                            runat="server" 
                            Text="Unknown User" />
                        </div>                                      
                    </div>
                    <div id="divAccountPopup" class="dropdownMenuItemContainer">
                    <asp:LinkButton 
                        ID="btnChangePassword" 
                        runat="server" 
                        text="Change Password" 
                        OnClientClick="javascript:UpdateDaisyText('Loading');"
                        onclick="btnChangePassword_Click" 
                        CausesValidation="False" /> 

                    <asp:LinkButton 
                        ID="btnSignOut" 
                        runat="server" 
                        text="Sign Out" 
                        OnClientClick="javascript:UpdateDaisyText('Signing out');"
                        onclick="btnSignOut_Click" 
                        CausesValidation="False" /> 
                </div>
                </asp:Panel>
                </div>
            </div>
        </asp:Panel>
        <h1 id="company-name"></h1>
    </div>
       
    <script type="text/javascript">

        $(document).ready(
            function () {
               
                $('#loggedInUserContainer').css('display', '<%=IsUserLoggedIn ? "block" : "none" %>');
                $('#divSupportMenuContainer .dropdownMenuTop').mouseover(
                    function (e) {
                        e.preventDefault();
                        $('#divSupportMenuContainer .dropdownMenuTop').css('background-color', '#0a436d');
                        $('#divSupportMenuContainer .dropdownMenuTop').css('border', '1px dotted #CCCCCC');
                        $('#divSupportMenuContainer .dropdownMenuItemContainer').show();
                    });
                $('#divSupportMenuContainer').mouseleave(
                    function () {
                        $('#divSupportMenuContainer .dropdownMenuTop').css('background-color', 'transparent');
                        $('#divSupportMenuContainer .dropdownMenuTop').css('border', '1px solid transparent');
                        $('#divSupportMenuContainer .dropdownMenuItemContainer').hide();
                    }
                );
                $('#supportRequestLink').click(
                    function () {
                        $('#supportFormContainer').css('display', 'block');
                        $('#supportFormBlanket').css('display', 'block');
                    }
                );
                $('#' + '<%=pnlWelcome.ClientID%> .dropdownMenuTop').mouseover(
                    function (e) {
                        e.preventDefault();
                        $('#' + '<%=pnlWelcome.ClientID%> .dropdownMenuTop').css('background-color', '#0a436d');
                        $('#' + '<%=pnlWelcome.ClientID%> .dropdownMenuTop').css('border', '1px dotted #CCCCCC');
                        $('#' + '<%=pnlWelcome.ClientID%> .dropdownMenuItemContainer').show();
                    }
                );
                $('#' + '<%=pnlWelcome.ClientID%>').mouseleave(
                    function () {
                        $('#' + '<%=pnlWelcome.ClientID%> .dropdownMenuTop').css('background-color', 'transparent');
                        $('#' + '<%=pnlWelcome.ClientID%> .dropdownMenuTop').css('border', '1px solid transparent');
                        $('#' + '<%=pnlWelcome.ClientID%> .dropdownMenuItemContainer').hide();
                    }
                );

                $('#spnClearSearch').hide();
            });

      


    </script>
    </contenttemplate>
</asp:UpdatePanel>
