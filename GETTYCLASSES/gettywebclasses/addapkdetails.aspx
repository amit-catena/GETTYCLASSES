<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="addapkdetails.aspx.cs" Inherits="gettywebclasses.addapkdetails" %>
<head>
 <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
 <script>

     function closePOP() {        
         parent.postMessage('closewin', '*');
     }
     
 </script>

 <style>
 .headings {
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 11px;
	font-weight: bold;
	color: #333333;
	text-decoration: none;
}
.text {
	PADDING-LEFT: 5px; FONT-SIZE: 11px; COLOR: black; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif; TEXT-ALIGN: left
}
 
 </style>
  </head>
  <body>
  <form id="form1" runat="server">
 <table  cellspacing="1" cellpadding="0" width="100%" bgcolor="#999999">
   <tr height="30px">
   <td class='headings'>Add Apk Details</td>
   </tr>
   <tr  bgColor="#ffffff" height="30px">
   <td class='headings'>
     Apk Name  
   </td>
    <td class='text'><asp:TextBox ID="txtApktitle" runat="server" Columns="50" CssClass="text"></asp:TextBox>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtApktitle"  Display="Dynamic"><span style='color:Red;'>please enter Apk File Name</span></asp:RequiredFieldValidator>
    </td>
   </tr>
   <tr bgColor="#ffffff" height="30px"><td class='headings'>Version</td>
    <td class='text'><asp:TextBox ID="txtversion" runat="server" CssClass="text" Columns="50"></asp:TextBox></td>
   </tr>
   <tr bgColor="#ffffff" height="30px"><td class='headings'>Size</td>
    <td class='text'><asp:TextBox ID="txtSize" runat="server" CssClass="text" Columns="50"></asp:TextBox></td>
   </tr>
   <tr bgColor="#ffffff" height="30px"><td class='headings'>Apk File</td>
    <td class='text'><asp:FileUpload ID="Apkfileupload" runat="server" CssClass="text" />
    
     <br />
      
     
    </td>
    <% if (Request.QueryString["apkid"].ToString() != "0")
       { %>
   <tr bgColor="#ffffff" height="30px">
   <td class='headings'>File Name</td>
   <td class='text'>
   <asp:Literal ID="ltfilename" runat="server"></asp:Literal>
   </td>
   </tr>
   <% } %>
   </tr>
   <tr bgColor="#ffffff" height="30px"><td class='headings'>Description</td>
    <td class='text'><asp:TextBox ID="txtdesc" runat="server" CssClass="text" Columns="70" Rows="10" TextMode="MultiLine"></asp:TextBox></td>
   </tr>
   <tr bgColor="#ffffff" height="30px"><td></td><td class='text'><asp:Button ID="btnsubmit" runat="server" CssClass="submitButton" Text="submit" OnClick="btnsubmit_click" /></td></tr>
 </table>

 <script>
     /*
     var ref = document.referrer.toUpperCase();
     var patt = /addapkdetails/g;
             
		//var pattrn = /list/g.test(ref);
		var pattrn = patt.test(ref);
		console.log(pattrn);
		if(!pattrn){
			parent.document.body.classList.remove('cPop')
		}
        
     */
     
    
  </script>
  </form>
  </body>

