<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addappscreenshots.aspx.cs"
    Inherits="gettywebclasses.addappscreenshots" %>

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
        .text {
            margin: 6px 0px 0px 7px;
            padding: 2px;
            padding-left: 5px;
            font-size: 11px;
            Color: black;
            font-family: Verdana,Arial,Helvetica,sans-serif;
        }
        
        .textbold {
            padding-left: 5px;
            Font-size: 11px;
            color: black;
            font-family: Verdana,Arial,Helvetica,sans-serif;
            text-align: left;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table bgcolor="#cccccc" cellpadding="3" cellspacing="1" style="width: 90%">
        <tr bgcolor="#FFFFFF" style="height: 25px">
            <td colspan="2">
                <asp:ValidationSummary ID="validationsummary1" runat="server" DisplayMode="List" />
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image (1)</span>
            </td>
            <td>
                <asp:FileUpload ID="fup1" runat="server" Width="650px" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only gif, jpeg, png files are allowed!"
                    ValidationExpression="^(.*?)\.((jpg)|(png)|(gif)|(jpeg)|(JPG)|(PNG)|(GIF)|(JPEG))$"
                    ControlToValidate="fup1">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Title (1)</span>
            </td>
            <td>
                <asp:TextBox ID="txttitle1" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image URL</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgurl1" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Credit</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgcredit1" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td colspan="2">
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image (2)</span>
            </td>
            <td>
                <asp:FileUpload ID="fup2" runat="server" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only gif, jpeg, png files are allowed!"
                    ValidationExpression="^(.*?)\.((jpg)|(png)|(gif)|(jpeg)|(JPG)|(PNG)|(GIF)|(JPEG))$"
                    ControlToValidate="fup2">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Title (2)</span>
            </td>
            <td>
                <asp:TextBox ID="txttitle2" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image URL</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgurl2" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Credit</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgcredit2" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td colspan="2">
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image (3)</span>
            </td>
            <td>
                <asp:FileUpload ID="fup3" runat="server" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Only gif, jpeg, png files are allowed!"
                    ValidationExpression="^(.*?)\.((jpg)|(png)|(gif)|(jpeg)|(JPG)|(PNG)|(GIF)|(JPEG))$"
                    ControlToValidate="fup3">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Title (3)</span>
            </td>
            <td>
                <asp:TextBox ID="txttitle3" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image URL</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgurl3" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Credit</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgcredit3" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td colspan="2">
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image (4)</span>
            </td>
            <td>
                <asp:FileUpload ID="fup4" runat="server" Width="505px" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Only gif, jpeg, png files are allowed!"
                    ValidationExpression="^(.*?)\.((jpg)|(png)|(gif)|(jpeg)|(JPG)|(PNG)|(GIF)|(JPEG))$"
                    ControlToValidate="fup4">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Title (4)</span>
            </td>
            <td>
                <asp:TextBox ID="txttitle4" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image URL</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgurl4" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Credit</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgcredit4" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td colspan="2">
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image (5)</span>
            </td>
            <td>
                <asp:FileUpload ID="fup5" runat="server" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Only gif, jpeg, png files are allowed!"
                    ValidationExpression="^(.*?)\.((jpg)|(png)|(gif)|(jpeg)|(JPG)|(PNG)|(GIF)|(JPEG))$"
                    ControlToValidate="fup5">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Title (5)</span>
            </td>
            <td>
                <asp:TextBox ID="txttitle5" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Image URL</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgurl5" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
                <span class="textbold">Credit</span>
            </td>
            <td>
                <asp:TextBox ID="txtimgcredit5" runat="server" Width="350px" CssClass="text"></asp:TextBox>
            </td>
        </tr>
        <tr bgcolor="#FFFFFF">
            <td style="width: 1500px">
            </td>
            <td style="width: 857px">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="Cnt_btn_rt_sub"
                    OnClick="btnSubmit_Click" />
                <input id="btnreset" class="Cnt_btn_rt_sub" name="btnreset" type="reset" value="Reset" />
                <input type="button" id="btnBack" value="Cancel" class="Cnt_btn_rt_sub" onclick="showlist();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html> 