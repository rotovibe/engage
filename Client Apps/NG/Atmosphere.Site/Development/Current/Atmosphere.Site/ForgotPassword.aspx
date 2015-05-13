<%@ Page Language="C#" MasterPageFile="~/App_Resource/Atmosphere.Core.dll/Atmosphere.Web.C3.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="C3.Web.ForgotPassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<%--    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="UserNameCancelButton" />
            <asp:PostBackTrigger ControlID="QuestionCancelButton" />
            <asp:PostBackTrigger ControlID="QuestionSubmitButton" />
            <asp:AsyncPostBackTrigger ControlID="UserNameSubmitButton" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="UserNamePanel" runat="server" CssClass="fp_panel">
                <table cellpadding="0" cellspacing="0" class="fp_usernametable">
                    <tr>
                        <td>
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="txtUserName" Text="Username:" CssClass="fp_labels"></asp:Label>
                        </td>
                        <td class="fp_usernametable_col2">
                            <asp:TextBox ID="txtUserName" runat="server" TabIndex="1" CssClass="fp_usernametextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="fp_usernametable_col2" style="padding-right:5px;">
                            <asp:Button ID="UserNameSubmitButton" runat="server" CssClass="button" Text="Submit" 
                                TabIndex="2" OnClick="UserNameSubmitButton_Click"/>&nbsp;
                            <asp:Button ID="UserNameCancelButton" runat="server" CssClass="button" 
                                Text="Cancel" TabIndex="3"  OnClick="UserNameCancelButton_OnClick"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="QuestionPanel" runat="server" CssClass="fp_panel">   
                <table cellpadding="0" cellspacing="0"  class="fp_questiontable">                      
                    <tr>
                        <td>
                            <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question" Text="Security Question:" CssClass="fp_labels"></asp:Label>
                        </td>
                        <td class="fp_questiontable_col2">
                            <asp:Label ID="Question" runat="server" Text="Question" CssClass="fp_questionliterals"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="txtAnswer" CssClass="fp_labels" Text="Answer:"></asp:Label>
                        </td>
                        <td class="fp_questiontable_col2">
                            <asp:TextBox ID="txtAnswer" runat="server" TabIndex="4" CssClass="fp_answertextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="fp_questiontable_buttoncell">
                            <asp:Button ID="QuestionSubmitButton" runat="server" CssClass="button" Text="Submit" 
                                TabIndex="5" OnClick="QuestionSubmitButton_OnClick"/>&nbsp;
                            <asp:Button ID="QuestionCancelButton" runat="server" CssClass="button" 
                                Text="Cancel" TabIndex="6"  OnClick="QuestionCancelButton_OnClick"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="PasswordInstructionsDiv" class="fp_instructions">
        <asp:Label ID="InstructionsLabel" runat="server" 
            Text="To obtain your username, please contact your administrator or Phytel Client Care." 
            CssClass="fp_instructions_body">
        </asp:Label>
    </div>   
</asp:Content>
