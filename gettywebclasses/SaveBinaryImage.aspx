<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaveBinaryImage.aspx.cs" Inherits="gettywebclasses.SaveBinaryImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function GetImagePath(strimgpath) {
            alert(strimgpath);
        }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <textarea id="txtbinarydata" runat="server" rows="10" cols="100"></textarea>    
     
    </div>
    </form>
    
</body>
</html>
