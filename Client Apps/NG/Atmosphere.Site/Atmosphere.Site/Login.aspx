<%@ Page Language="C#" MasterPageFile="~/App_Resource/Atmosphere.Core.dll/Atmosphere.Web.C3.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="C3.Web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script src="//cdn.optimizely.com/js/251330794.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="updLogin" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnContinuePassword" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelPassword" />
            <asp:AsyncPostBackTrigger ControlID="btnLogin" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnLogin">
                <div id="modal_pop_panel" runat="server" class="ui-widget-overlay hide">
                </div>
                <div id="modal_pop" runat="server" style="height: auto; width: 790px;" class="ui-dialog ui-widget-content hide">
                    <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix"
                        unselectable="on">
                        <span runat="server" class="ui-dialog-title" id="ui_dialog_title" unselectable="on">
                        </span>
                        <div id="model_pop_panel_close" class="ui-dialog-titlebar-close ui-corner-all" onclick="document.getElementById('<%=btnCancelPassword.ClientID%>').click();">
                        </div>
                    </div>
                    <div id="ui_dialog_body" class="dialog ui-dialog-content ui-widget-content" style="display: block;
                        width: auto; min-height: 90px; height: auto;">
                        <span runat="server" id="ui_dialog_text" unselectable="on"></span>
                        <br />
                        <br />
                        <asp:Button runat="server" ID="btnContinuePassword" OnClick="Initiate_Login_PasswordChange"
                            Text="Yes" CssClass="button" />
                        <asp:Button runat="server" ID="btnCancelPassword" OnClick="Initiate_Login" Text="No"
                            CssClass="button" />
                        <input runat="server" id="pageRedirect" class="hide" type="hidden" />
                    </div>
                </div>
                <div id="divLogin" runat="server" class="fm_sign_in">
                    <table cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                        <tr>
                            <td>
                                <table cellpadding="0" style="height: 100px;">
                                    <tr>
                                        <td style="height: 5px;" colspan="2">
                                            <asp:ValidationSummary Visible="false" ID="ValidationSummary1" runat="server" CssClass="validationMessageContainer"
                                                ValidationGroup="MainLogin" HeaderText="Please enter UserName and Password." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 68px; white-space: nowrap;">
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="Username" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server" />
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                ToolTip="User Name is required." Text="*" ValidationGroup="MainLogin" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 68px; white-space: nowrap;">
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" />
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                ToolTip="Password is required." ValidationGroup="MainLogin" Text="*" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td style="text-align: right; padding-left: 141px;">
                                            <asp:Button runat="server" Width="65" Text="Sign In" OnClientClick="javascript:UpdateDaisyText('Signing In');"
                                                OnClick="Login_Click" ID="btnLogin" ValidationGroup="MainLogin" CssClass="button" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:HyperLink ID="PasswordRecoveryLink" runat="server" CssClass="forgot_password"
                                                NavigateUrl="~/ForgotPassword.aspx" Text="Forgot Password?" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
