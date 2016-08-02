<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupportForm.aspx.cs" Inherits="Atmosphere.Support.SupportFormPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-type" content="text/html; charset=UTF-8"/>
    <title>Support Form</title>
    <link rel="stylesheet" href='../Assets/Style/CSS/main.css' type="text/css" media="screen, projection" />    
    <style type="text/css">
    
    html {
        background-color: #ffffff;
        width: 100%;
        height: 100%; 
    }
    
    body {
        background-color: #ffffff;
        width: 100%;
        height: 100%;
    }
    
    h2 
    {
        font-size:18px;
    }
    
    .requiredInput
    {
        border-color: #DCD51C;
        border-width: 2px;
        border-style: solid;
        background-color: #FFFFCC;
    }
        
    .requiredInput table {
            background-color: #FFFFCC; 
    }       
        
    .requiredInput table td.rcbInputCell
    {
        background-image: none;
    }
    
    #supportFormTable, p.supportFormParagraph {
        width:840px;
        margin: 20px auto;
    }
    
    #supportFormTable {
        margin: 12px auto;
    }
    
    p.supportFormParagraph {
        margin: 5px auto;
    }
    
    p.supportFormActions 
    {
        margin-top: 20px;
        margin-right: 20px;
    }
       
    p.supportFormActions button, p.supportFormActions input[type=submit] {
        float: right;
        margin-left: 10px;
    }       
    
    #supportFormTable td {
        padding-top: 10px;
        vertical-align: top;
        border-top: none;
    }
    
    *+html #supportFormTable td 
    {
        padding-top:2px;
    }
    
    #supportFormTable td.input 
    {
        width: 320px;
    }
    
    #supportFormTable td.caption {
        text-align: right;
        padding-right: 5px;
        width: 100px;
    }   
    
    #supportFormTable td input[type=text], 
    #supportFormTable td select  
    {
        width: 300px;
    }
    
    
    #supportFormTable td textarea {
            width: 820px; 
            height:204px;
    }
        
    #supportFormTable label.optional
    {
        font-weight: normal;
    }
    
    #submittingMessage 
    {
        width:200px;
        float:left;        
    }   
    </style>
    <script type='text/javascript' src='../Assets/scripts/jquery-1.7.1.min.js'></script>
    <script type="text/javascript">
        $(document).ready(
            function () {
                $('#<%=btnSupportFormCancel.ClientID %>').click(
                    function (e) {
                        e.preventDefault();
                        $('#supportFormContainer', window.parent.document).css('display', 'none');
                        $('#supportFormBlanket', window.parent.document).css('display', 'none');
                    });
            });
        function validateSupportForm() {
            var invalidFieldCount = 0;
            if ($('#name').val() == '') {
                $('#name').addClass('requiredInput');
                invalidFieldCount++;
            } else {
                $('#name').removeClass('requiredInput');
            }
            if ($('#email').val() == '') {
                $('#email').addClass('requiredInput');
                invalidFieldCount++;
            } else {
                $('#email').removeClass('requiredInput');
            }
            if ($('#phone').val() == '') {
                $('#phone').addClass('requiredInput');
                invalidFieldCount++;
            } else {
                $('#phone').removeClass('requiredInput');
            }
            if ($('#description').val() == '') {
                $('#description').addClass('requiredInput');
                invalidFieldCount++;
            } else {
                $('#description').removeClass('requiredInput');
            }
            if (invalidFieldCount == 0) {
                $('#submittingMessage').css('display','block');
            }
            else {
                $('#submittingMessage').css('display','none');
            }
            return (invalidFieldCount == 0);
        }
    </script>
</head>
<body>
    <form runat="server">
    <div id="supportFormHeader">
            <h2>Support Request</h2>
            <hr/>
    </div>
    <p class="supportFormParagraph">Phytel is committed to providing superior customer service from knowledgeable technical representatives. If you have any questions about or issues with your Phytel service, please let us know. We appreciate your feedback!</p>
    
    <input type="hidden" name="orgid" value="00D500000007VsH"/>
    <input type="hidden" name="retURL" value="<%=SupportConfirmationUrl %>"/>    
    <table id="supportFormTable">
        <tr>
            <td class="caption"><label for="name">Contact Name:</label></td>
            <td class="input"><input id="name" maxlength="80" name="name" size="20" type="text" value="<%= DisplayNameValue %>" tabindex="1" /></td>
            <td class="caption"><label for="00N50000002DQ4I" class="optional">Facility Name:</label> </td>
            <td><input id="00N50000002DQ4I" maxlength="50" name="00N50000002DQ4I" size="20" type="text" tabindex="2" /></td>
        </tr>
        <tr>
            <td class="caption"><label for="phone">Contact Phone:</label></td>
            <td class="input"><input id="phone" maxlength="40" name="phone" size="20" type="text" value="<%= PhoneValue %>" tabindex="3" /></td>
            <td class="caption"><label for="email">Contact Email:</label></td>
            <td class="input"><input id="email" maxlength="80" name="email" size="20" type="text" value="<%= EmailValue %>" tabindex="4"/></td>
        </tr>
        <tr>
            <td class="caption"><label for="subject" class="optional">Subject:</label></td>
            <td class="input"><input id="subject" maxlength="80" name="subject" size="20" type="text" tabindex="5" /></td>
            <td class="caption"><label for="00N500000029yTe" class="optional">Product:</label></td>
            <td class="input"><select id="00N500000029yTe" name="00N500000029yTe" title="Product" tabindex="6"><option
                value="">--None--</option>
                <option value="Coordinate">Nightingale</option>
                <option value="Other">Other</option>                
            </select></td>

        </tr>
        <tr>
            <td colspan="4"> <label for="description">Issue or Request Description:</label></td>                        
        </tr>
        <tr>
            <td colspan="4"><textarea id="description" name="description" rows="12" tabindex="7"></textarea></td>
        </tr>
    </table>

    <p class="supportFormParagraph">
        The protection of personal health information is important to us.  To assist us in our efforts to keep personal health information secure, please do not include in the issue description field any identifiable information about a patient (such as Name, Date of Birth, Social Security Number, or Medical Record Number) or references to a patient&apos;s condition or treatment.  If you need to share protected health information with us, please indicate that and we will contact you directly.
    </p>
    <p class="supportFormActions">
                <input type="hidden" id="external" name="external" value="1" />
                <input id="00N50000002DQ4D" name="00N50000002DQ4D" type="hidden" value="<%= PhytelUsernameValue %>" />
                <input id="company" name="company" type="hidden" value="<%= CompanyNameValue %>" />
                <span id="submittingMessage" style="display:none;"><em>Submitting Support Request...</em></span>
                <asp:Button CssClass="button" id="btnSupportFormCancel" runat="server" Text="Cancel" tabindex="9" />
                <asp:Button CssClass="button" PostBackUrl="https://www.salesforce.com/servlet/servlet.WebToCase?encoding=UTF-8" runat="server" Text="Submit" ID="btnSupportFormSubmit" tabindex="8" OnClientClick="if(validateSupportForm()==false) return false;" />
    </p>
    </form>   
</body>
</html>
