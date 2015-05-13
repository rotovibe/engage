<%@ Page Language="C#" MasterPageFile="~/App_Resource/Atmosphere.Core.dll/Atmosphere.Web.C3.Master" AutoEventWireup="true" CodeBehind="SecurityQuestion.aspx.cs" Inherits="C3.Web.SecurityQuestion" %>
<%@ Register Assembly="Atmosphere.Core" Namespace="C3.WebControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:Panel ID="Panel1" runat="server" class="sq_panel" DefaultButton="SubmitButton">
        <div>
            <table cellpadding="0" cellspacing="0" class="sq_table">
                <tr>
                    <td class="sq_labelcol">
                        <asp:Label ID="SecurityQuestionListLabel" runat="server" CssClass="sq_labels" Text="Security Question"></asp:Label>
                    </td>
                    <td class="sq_dropdowncell">
                        <cc1:MultiDropDown ID="SecurityQuestionList" SelectedListCount="1" Text="All" MultiSelectType="SingleSelect"
                        ValueMember="Question" DisplayMember="Question" MatchOnDisplayMember="True"  runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td class="sq_labelcol">
                        <asp:Label ID="SecurityQuestionAnswerLabel" runat="server" 
                            AssociatedControlID="SecurityQuestionAnswer" CssClass="sq_labels" Text="Answer"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="SecurityQuestionAnswer" runat="server" CssClass="sq_textboxes" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="fp_questiontable_buttoncell">                                 
                        <asp:Button ID="SubmitButton" runat="server" CssClass="button" Text="Submit" 
                            TabIndex="3" OnClick="SubmitButton_OnClick"/>&nbsp;
                        <asp:Button ID="CancelButton" runat="server" CssClass="button" 
                            Text="Cancel" TabIndex="4"  OnClick="CancelButton_OnClick"/>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>        
    <div id="PasswordInstructionsDiv" class="sq_instructions">
        <asp:Label ID="instructions" runat="server" Text="" CssClass="sq_instructions_body" />
    </div>
</asp:Content>

