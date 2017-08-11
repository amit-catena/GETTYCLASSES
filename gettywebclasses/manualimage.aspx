<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manualimage.aspx.cs" Inherits="gettywebclasses.manualimage" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function testcall(url) {

            //alert(url);
            parent.postMessage(['update', url], "*");
            //window.frameElement.style.height = ht +'px';

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
    <tr>
    <td> <textarea id="imagetext" name="imagetext"></textarea>
    <input type="text" id="sitefolder" name="sitefolder" />
    <input type="text" id="imagename" name="imagename" />    
    <input type="submit" value="Save" />
    </td>
    </tr>
    </table>
    </div>
    </form>
    <script>
        var img = document.querySelector('#imagetext'), sitename = document.querySelector('#sitefolder'), imageName = document.querySelector('#imagename');
        function getSuccess(e) {
            
            if (e.data[0] == 'addData') { console.log(e.data[1]); img.innerHTML = e.data[1]; sitename.value = e.data[2];imageName.value = e.data[3];document.forms[0].submit(); }
        }
        var events = window.addEventListener ? 'addEventListener' : 'attachEvent',
		listner = window[events],
		message = events == 'addEventListener' ? 'message' : 'onmessage';
        listner(message, getSuccess);
        
    </script> 
</body>
</html>
