<%@ Page Language="C#" MasterPageFile="~/App_Resource/Atmosphere.Core.dll/Atmosphere.Web.C3.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="C3.Web.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .cp_instructions_local
        {
            float: left;
            margin-left: 16px;
            font-size: 12px;
            width: 340px;
        }
        
        .cp_Body_local
        {
            font-size: 12px;
            height: 100px;
            float: left;
            margin-left: 120px;
        }
        .cp_labels_local
        {
            float: right;
            font-size: 12px;
            line-height: 15px;
            font-weight: bold;
            color: #333333;
            padding-bottom: 8px;
            padding-right: 10px;
            vertical-align: middle;
        }
        .cp_table_local tr td
        {
            border: 0 solid;
        }
        
        .cp_table_col2_local
        {
            width: 240px;
            text-align: right;
        }
        .cp_table_colbuttons_local
        {
            padding: 0px 10px 0px 0px;
            width: 200px;
            text-align: right;
        }
        .cp_validator_local
        {
            color: Red;
            float: left;
            vertical-align: middle;
        }
        
        .setHeaderText
        {
            font-weight: bold;
            margin-bottom: 5px;
        }
        
        .requiredFieldStyle
        {
            border-color: #DCD51C !important;
            border-width: 2px !important;
            background-color: #FFFFCC !important;
        }
        
        #filter-update-local
        {
            top: 90px;
            left: 0px;
            right: 0px;
            background: #ffffcc;
            line-height: 2em;
            padding: 1em;
            border-top: solid 2px #dcd51c;
            border-bottom: solid 2px #dcd51c;
            z-index: 2;
        }
        
        .label-style-local
        {
            float: left;
            font-size: large;
            font-weight: bold;
        }
        
        .textBox
        {
            border: 1px solid #999999;
            width: 195px;
        }
        
        .blueMessage
        {
            position: static;
            background: #B6D3DD;
            line-height: 2em;
            padding: 4px;
            padding-left: 16px;
            border-top: solid 2px #7896A9;
            border-bottom: solid 2px #7896A9;
            z-index: 2;
            margin-bottom: 10px;
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
    <asp:Panel ID="pnlChangePassword" runat="server" DefaultButton="SubmitButton">
        <asp:UpdatePanel ID="updChangePassword" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="SubmitButton" />
                <asp:AsyncPostBackTrigger ControlID="CancelButton" />
            </Triggers>
            <ContentTemplate>
                <div id="MainDiv" class="search-container" style="width: 990px;">
                    <asp:HiddenField ID="hdnValidationFlag" runat="server" Value="" />
                    <div class="master_FormHeader" style="margin-top: 12px; height: 25px;">
                        <asp:Label ID="Label1" runat="server" Text="Change Password" CssClass="label-style-local"></asp:Label>
                    </div>
                    <div id="filter-update-local" style="display: none; margin-top: -1px;">
                        <span id="lblFilterChange"></span>
                    </div>
                    <asp:Panel ID="divPasswordExpired" runat="server" class="blueMessage" Visible="False">
                        <span id="passwordExpiredMessage">Your password has expired. Please enter a new password
                            to continue.</span>
                    </asp:Panel>
                    <asp:Panel ID="divNewUserPasswordExpired" runat="server" class="blueMessage" Visible="False">
                        <span id="newUserpasswordExpiredMessage">You must change your initial password to continue.</span>
                    </asp:Panel>
                    <div style="float: none; margin-right: 12px; margin-top: 20px;">
                        <div id="PasswordInstructionsDiv" class="cp_instructions_local">
                            <ul class="cp_instructions_hdr">
                                <li class="setHeaderText">Minimum Password Requirements</li>
                                <li>
                                    <ul>
                                        <li>
                                            <asp:Label ID="lblMin6Char" runat="server" Text="At least 6 characters in length" /></li>
                                        <li>
                                            <asp:Label ID="lblOneUpper" runat="server" Text="Includes at least one upper case letter" /></li>
                                        <li>
                                            <asp:Label ID="lblOneLower" runat="server" Text="Includes at least one lower case letter" /></li>
                                        <li>
                                            <asp:Label ID="lblOneNumeric" runat="server" Text="Includes at least one numeric character" /></li>
                                        <li>
                                            <asp:Label ID="lblOneAlpha" runat="server" Text="Includes at least one non-alphanumeric character" /></li>
                                        <li>
                                            <asp:Label ID="lblMatchUserName" runat="server" Text="Cannot be the same as your username" /></li>
                                        <li>
                                            <asp:Label ID="lblPastHistory" runat="server" Text="Must be different from your previous 5 passwords" /></li>
                                    </ul>
                                </li>
                            </ul>
                        </div>
                        <div id="ChangePasswordDiv" class="cp_Body_local">
                            <ul class="cp_instructions_hdr">
                                <li class="setHeaderText">What is your new password?</li>
                                <li>
                                    <table cellpadding="1" cellspacing="0" class="cp_table_local" style="width: 400px;">
                                        <tr>
                                            <td style="border: none;">
                                                <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword"
                                                    CssClass="cp_labels_local" Wrap="false">New Password:</asp:Label>
                                            </td>
                                            <td class="cp_table_col2_local" nowrap="nowrap">
                                                <asp:TextBox ID="NewPassword" runat="server" CssClass="textBox" Style="float: left;"
                                                    TextMode="Password" TabIndex="1" onKeyUp="onKeyPressPwd(this.value);"></asp:TextBox><asp:Label
                                                        ID="NewPasswordValidator" runat="server" Text="*" CssClass="cp_validator_local"
                                                        Visible="false"></asp:Label>
                                                <asp:HiddenField ID="hdnNewPassword" runat="server" Value="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword"
                                                    CssClass="cp_labels_local" Wrap="false">Reenter New Password:</asp:Label>
                                            </td>
                                            <td class="cp_table_col2_local" nowrap="nowrap">
                                                <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="textBox" Style="float: left;"
                                                    TextMode="Password" TabIndex="2" onKeyUp="onKeyPressCfmPwd(this.value);"></asp:TextBox>
                                                <asp:Label ID="ConfirmPasswordValidator" runat="server" Text="*" CssClass="cp_validator_local"
                                                    Visible="false"></asp:Label>
                                                <asp:HiddenField ID="hdnConfirmNewPassword" runat="server" Value="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="margin-left: 125px; margin-top: 10px;">
                                                    <asp:Button ID="SubmitButton" runat="server" CommandName="ChangePassword" CssClass="button"
                                                        Text="Save Change" TabIndex="3" OnClick="SubmitButton_OnClick" CausesValidation="False"
                                                        Style="margin-right: 8px;" OnClientClick="javascript:return onSaveChangesClick('Save');" />
                                                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                                        CssClass="button" Text="Cancel" TabIndex="4" OnClick="CancelButton_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        var fieldsToBeFilled = "";
        var fieldsToBeValidateServer = "";
        var saveButtonClicked = false;
        function EndRequestHandler(sender, args) {      //Method called when Update panel postback.
            OnLoad();
            if (saveButtonClicked) {
                HideMessage();
                fieldValidation();
                serverSideValidation();
                if (fieldsToBeValidateServer != "") {
                    ShowMessage(fieldsToBeValidateServer);
                    return false;
                }
                else if (fieldsToBeFilled != "") {
                    ShowMessage(fieldsToBeFilled);
                    return false;
                }
                saveButtonClicked = false;
            }
        }


        function onSaveChangesClick(flag) {
            saveButtonClicked = true;
            fieldValidation();
            if (fieldsToBeFilled != "") {
                ShowMessage(fieldsToBeFilled);
                return false;
            }
            else if (serverValidation != "") {
                ShowMessage("<span style=\"font-weight:bold;border-color: #DCD51C;border-width: 2px 0px 2px 0px\">" + fieldsToBeValidateServer + " </span>" + invalidMessage);
                return false;
            }
            else {
                HideMessage();
                UpdateDaisyText('Processing');
            }
            return true;
        }


        function fieldValidation() {
            fieldsToBeFilled = "";
            var txtNewPwd = document.getElementById("<%= NewPassword.ClientID %>").value;
            var txtConfirmNewPwd = document.getElementById("<%= ConfirmNewPassword.ClientID %>").value;
            serverValidation = "";


            if (txtNewPwd == "") {
                fieldsToBeFilled += " New Password,";
                $('#<%=NewPassword.ClientID%>').addClass("requiredFieldStyle");
            }
            else {
                $('#<%=NewPassword.ClientID%>').removeClass("requiredFieldStyle");
            }

            if (txtConfirmNewPwd == "") {
                fieldsToBeFilled += " Reenter New Password,";
                $('#<%=ConfirmNewPassword.ClientID%>').addClass("requiredFieldStyle");
            }
            else {
                $('#<%=ConfirmNewPassword.ClientID%>').removeClass("requiredFieldStyle");
            }

            if (fieldsToBeFilled != "") {
                if (fieldsToBeFilled.length > 0) {
                    fieldsToBeFilled = fieldsToBeFilled.substring(0, fieldsToBeFilled.length - 1);
                }
                fieldsToBeFilled = "The following fields are required: <span style=\"font-weight:bold;border-color: #DCD51C;border-width: 2px 0px 2px 0px\">" + fieldsToBeFilled + " </span>"
            }
            else if (txtNewPwd != txtConfirmNewPwd) {
                fieldsToBeFilled += "The passwords entered do not match. Please try again."
                $('#<%=NewPassword.ClientID%>').addClass("requiredFieldStyle");
                $('#<%=ConfirmNewPassword.ClientID%>').addClass("requiredFieldStyle");
            }

            if (fieldsToBeFilled.length > 0) {
                fieldsToBeFilled = fieldsToBeFilled.substring(0, fieldsToBeFilled.length - 1);
            }
        }


        function serverSideValidation() {

            fieldsToBeValidateServer = "";
            fieldsToBeFilled = "";
            var serverValidation = document.getElementById("<%= hdnValidationFlag.ClientID %>").value;
            $('#<%=NewPassword.ClientID%>').removeClass("requiredFieldStyle");
            $('#<%=ConfirmNewPassword.ClientID%>').removeClass("requiredFieldStyle");

            if (serverValidation == "NewPasswordMinReq") {
                fieldsToBeValidateServer += "Password must conform to the minimum requirements.";
                $('#<%=NewPassword.ClientID%>').addClass("requiredFieldStyle");
            }
            if (serverValidation == "ConfirmNewPasswordMinReq") {
                fieldsToBeValidateServer += "Password must conform to the minimum requirements.";
                $('#<%=ConfirmNewPassword.ClientID%>').addClass("requiredFieldStyle");
            }
            if (serverValidation == "HistryExists") {
                fieldsToBeValidateServer += "Password must be different from your previous 5 passwords";
                $('#<%=NewPassword.ClientID%>').addClass("requiredFieldStyle");
                $('#<%=ConfirmNewPassword.ClientID%>').addClass("requiredFieldStyle");
            }
        }

        function ShowMessage(message) {
            $("#lblFilterChange").html(message);
            if ($('#filter-update-local').is(':hidden')) {
                $('#filter-update-local').slideDown('fast');
                $("#lblFilterChange").html(message);
                $('#spanPrintLevel1').hide();
            }
        }

        function HideMessage() {
            if ($('#filter-update-local').is(':visible')) {
                $('#filter-update-local').slideUp('fast');
                $('#spanPrintLevel1').show();
            }
        }

        function onKeyPressPwd(_value) {
            document.getElementById("<%= NewPassword.ClientID %>").value = _value;
        }
        function onKeyPressCfmPwd(_value) {
            document.getElementById("<%= hdnNewPassword.ClientID %>").value = _value;
        }

        function OnLoad() {
            SetPWdOnPB();
        }

        function SetPWdOnPB() {
            if (document.getElementById("<%= NewPassword.ClientID %>") != null && document.getElementById("<%= ConfirmNewPassword.ClientID %>") != null) {
                if (document.getElementById("<%= hdnNewPassword.ClientID %>").value != '') {
                    if (document.getElementById("<%= NewPassword.ClientID %>").value == '') {
                        document.getElementById("<%= NewPassword.ClientID %>").value = document.getElementById("<%= hdnNewPassword.ClientID %>").value;
                    }
                    else if (document.getElementById("<%= hdnNewPassword.ClientID %>").value != document.getElementById("<%= NewPassword.ClientID %>").value) {
                        document.getElementById("<%= NewPassword.ClientID %>").value = document.getElementById("<%= hdnNewPassword.ClientID %>").value;
                    }
                }
                if (document.getElementById("<%= hdnConfirmNewPassword.ClientID %>").value != '') {
                    if (document.getElementById("<%= ConfirmNewPassword.ClientID %>").value == '') {
                        document.getElementById("<%= ConfirmNewPassword.ClientID %>").value = document.getElementById("<%= hdnConfirmNewPassword.ClientID %>").value;
                    }
                    else if (document.getElementById("<%= hdnConfirmNewPassword.ClientID %>").value != document.getElementById("<%= ConfirmNewPassword.ClientID %>").value) {
                        document.getElementById("<%= ConfirmNewPassword.ClientID %>").value = document.getElementById("<%= hdnConfirmNewPassword.ClientID %>").value;
                    }
                }
            }
        }

    </script>
</asp:Content>
