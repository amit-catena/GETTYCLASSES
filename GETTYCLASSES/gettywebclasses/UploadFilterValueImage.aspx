<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadFilterValueImage.aspx.cs" Inherits="gettywebclasses.UploadFilterValueImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    width: 100%;
    margin: auto;
}
    </style>

    <script>

        function closePOP(imgpath,filtervalueid) {
            var retVal = imgpath + ','+filtervalueid;
            parent.postMessage(retVal, '*');
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
     <td>Image</td>
     <td><asp:FileUpload ID="file1" runat="server" />  <span style='font-family:Verdana;font-size:11px;'>Please Upload Software Image (100 * 70)</span></td> 
     </tr>  
     <tr>  
     <td></td>
        <td> <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btnsave_Click" />     </td>
     </tr>
   
    </table>
    </div>
    </div>
        
    </form>
   
</body>
</html>
