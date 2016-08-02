<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupportFormConfirmation.aspx.cs" Inherits="Atmosphere.Support.SupportFormConfirmation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Phytel Support - Thank you</title>
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
    
    p 
    {
        margin-top:30px;
    }
        .anchorBottomRight 
        {
            position:absolute;
            top:550px;
            right:10px;
            width:80px;
        }
    </style> 
    <script type='text/javascript' src='../Assets/scripts/jquery-1.7.1.min.js'></script>
        <script type="text/javascript">
            $(document).ready(
            function () {
                $('#<%=btnSupportFormClose.ClientID %>').click(
                    function (e) {
                        e.preventDefault();
                        closeWindow();
                    });
                window.setInterval(closeWindow, 5000);
            });

            function closeWindow() {
                $('#supportFormContainer', window.parent.document).css('display', 'none');
                $('#supportFormBlanket', window.parent.document).css('display', 'none');
                $('#supportFormFrame', window.parent.document).attr('src', '<%=Atmosphere.UrlHelper.ResolveUrl(C3.Web.Helpers.UserHelper.GlobalSiteRoot + "Support/SupportForm.aspx") %>');
            }
        </script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSupportFormClose">
    <div>
        <h2>Support Request Submitted</h2>
        <p>Thank you for submitting your request. A Phytel Client Care representative will be in contact with you upon reviewing.</p>
        <p>This window will automatically close in 5 seconds.</p>
        <asp:Button CssClass="button anchorBottomRight" id="btnSupportFormClose" runat="server" Text="Close" /> 
    </div>
    </form>
</body>
</html>
