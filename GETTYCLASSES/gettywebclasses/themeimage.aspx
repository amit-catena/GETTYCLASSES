<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="themeimage.aspx.cs" Inherits="gettywebclasses.themeimage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script>

        function closePOP() {
            parent.postMessage('closewin', '*');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-left:50px;padding-top:100px">
    <table cellpadding="3" cellspacing="0">
     <tr>
     <td>Theme Title</td>
     <td>&nbsp;<asp:TextBox  ID="txtthemeName" runat="server"></asp:TextBox></td>
     </tr>       
     <tr>
     <td>Image</td>
     <td><asp:FileUpload ID="file1" runat="server" /> </td>
     </tr>
     <tr>
     <td colspan="2" align="center"> <asp:Button ID="btnSave" runat="server" Text="Submit" OnClick="btnsave_Click" />     </td>
     </tr>
    </table>
    </div>
    </form>
</body>
</html>
