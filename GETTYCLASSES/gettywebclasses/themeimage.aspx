<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="themeimage.aspx.cs" Inherits="gettywebclasses.themeimage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
    .container {
    display: block;
    padding: 15px;
}
.divtheme
{
    min-height: 94px;
    border: 2px dashed #777;
    color: #777;
    width: 44%;
    margin: auto;
}
    </style>

    <script>

        function closePOP() {
            parent.postMessage('closewin', '*');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
   
   <!-- <div style="padding-left:50px;padding-top:100px"> -->
   <div class="container">
  <div class="divtheme">
    <table cellpadding="3" cellspacing="0">
     <tr>
     <td>Theme Title</td>
     <td><asp:TextBox  ID="txtthemeName" runat="server" Columns="50" style='border: 1px solid #ccc;
    padding-left: 5px;'></asp:TextBox></td>
     </tr>       
     <tr>
     <td>Image</td>
     <td><asp:FileUpload ID="file1" runat="server" /> </td>
     </tr>
      <tr>
    <td>Exist Image</td>
    <td><asp:Literal ID="ltimage" runat="server"></asp:Literal></td>
    </tr>
    <tr><td>Hyperlink</td>
    <td><asp:DropDownList ID="ddllink" runat="server" style='width:250px;'></asp:DropDownList></td>
    </tr>
    <tr>   
  <td>Select your favorite color:</td>  
  <td> <input type="color" name="favcolor" value="#ff0000" id="myColor">
    
    </tr>
    <tr height='30px'></tr>
     <tr>
     
     <td></td>
        <td> <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btnsave_Click" />     </td>
     </tr>
   
    </table>
    </div>
    </div>
   <!-- </div> -->
    </form>
    <script>
        var color = '<%=_favcolor %>'
        document.getElementById("myColor").value = color;
    </script>
</body>
</html>
