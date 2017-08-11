<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="insertscript.aspx.cs" Inherits="gettywebclasses.insertscript" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Script</title>
        <style type="text/css">
        BODY
        {
            font-size: 12px;
            background: menu;
            margin-left: 10px;
            font-family: Verdana;
        }
        BUTTON
        {
            width: 5em;
        }
        P
        {
            text-align: center;
        }
        .boldtext
        {
            font-size: 12px;
            background: menu;
            margin-left: 10px;
            font-family: Verdana;
            font-weight: bold;
        }
        .text
        {
            font-size: 12px;
            background: menu;
            margin-left: 10px;
            font-family: Verdana;
        }
        .listbox
        {
            font-size: 12px;
            background: white;
            font-family: Verdana;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table cellpadding="3" cellspacing="1" width="300" border="0">
        <tr>
            <td colspan="2" height="8">
            </td>
        </tr>
        <tr>
            <td width="35%" class="boldtext" class="boldtext" nowrap>
                Video script</br>
            </td>
            <td width="65%">
                <asp:TextBox ID="txttwitterScript" runat="server" TextMode="MultiLine" placeholder="Iframe width should be 400"
                    Columns="40" Rows="8"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <p>
                    &nbsp;<input type="submit" class='buttontext' value="ok" name="Submit">
                    &nbsp;<input type="reset" class='buttontext' value="close" name="reset" onclick="window.self.close();">
                </p>
            </td>
        </tr>
    </div>
    </form>
    <script>
        function validate() {
            var strkey = document.getElementById("txttwitterScript").val();
            if (strkey != "") {
                window.returnValue = strkey;
                window.opener.Settwitterdat(strkey);
                //alert(this.window.returnValue);
                window.close();
            }
            else {
                alert("Please insert a embed video script.");
            }

        }

    </script>
</body>
</html>
