<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="twitterimages.aspx.cs" Inherits="gettywebclasses.twitterimages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script>

        function closePOP() {
            parent.postMessage('closewin', '*');
        }
     
    </script>
    <style>
        .headings
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            font-weight: bold;
            color: #333333;
            text-decoration: none;
        }
        .text
        {
            padding-left: 5px;
            font-size: 11px;
            color: black;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table cellspacing="1" cellpadding="0" width="100%" bgcolor="#999999">
        <tr height="30px">
            <td class='headings'>
                Add Tweet Image
            </td>
        </tr>
        <tr bgcolor="#ffffff" height="30px">
            <td class='headings'>
                Image File
            </td>
            <td class='text'>
                <asp:FileUpload ID="imagefileupload" runat="server" CssClass="text" />
                <br />
            </td>
        </tr>
        <tr bgcolor="#ffffff" height="30px">
            <td>
            </td>
            <td class='text'>
                <asp:Button ID="btnsubmit" runat="server" CssClass="submitButton" Text="submit" OnClick="btnsubmit_click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
