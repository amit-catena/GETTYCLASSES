<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modelframe.aspx.cs" Inherits="gettywebclasses.modelframe" %>
<HTML>
	<HEAD>
		<TITLE>Select Links	</TITLE>
		<STYLE type="text/css">BODY {
	FONT-SIZE: 12px; BACKGROUND: menu; MARGIN-LEFT: 10px; FONT-FAMILY: Verdana
}
BUTTON {
	WIDTH: 5em
}
P {
	TEXT-ALIGN: center
}
.boldtext
{
FONT-SIZE: 12px; BACKGROUND: menu; MARGIN-LEFT: 10px; FONT-FAMILY: Verdana;font-weight:bold;
}

.text
{
FONT-SIZE: 12px; BACKGROUND: menu; MARGIN-LEFT: 10px; FONT-FAMILY: Verdana;
}

.listbox
{
FONT-SIZE: 12px; BACKGROUND: white;  FONT-FAMILY: Verdana;
}

</STYLE>
		
<SCRIPT language="JavaScript">
    function validatemodel() {
        var link = "";
        if (document.getElementById("seltarget").selectedIndex == 0)
            alert('Please Select the target');
        else {
            if (document.getElementById("txturl").value == "") {
                window.returnValue = link = document.getElementById("sellink").value + "," + document.getElementById("seltarget").value + ",2";
            }
            else {
                window.returnValue = link = document.getElementById("txturl").value + "," + document.getElementById("seltarget").value + ",1";
            }

            if (!window.showModalDialog) {
                window.opener.setLink(link);
            }

            window.close();

        }
    }
</SCRIPT>

<script language=javascript >

    function Filllink() {
        //modelframe.filllinknews(document.getElementById("selsite").value,Filllink_Callback);
    }

    function Filllink_Callback(response) {
        if (response == null) return;
        else
            document.all.tdlink.innerHTML = response.value;
    }
		

</script> 
		<!-- Copyright 2000 Microsoft Corporation. All rights reserved. -->
	</HEAD>
	<BODY  onload="Filllink()"  >
	<form  name="form1" id="form1" runat=server   >
		<TABLE  cellpadding="3" cellspacing="1" width="300" border=0>
			<tr>
				<td colspan="2" height="8"></td>
			</tr>
			<tr>
				<td width="35%" class="boldtext" Class="boldtext" nowrap>
					Enter a URL 
				</td>
				<td width="65%">
					<asp:TextBox ID='txturl' Runat=server Class="listbox" Columns="41"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td width="35%" class="boldtext" Class="boldtext" nowrap>&nbsp;
					
				</td>
				<td width="65%">
					<span class='error'><font color=red size=2>e.g.http://www.test.com</font></span>
				</td>
			</tr>
			<tr>
				<td colspan="2" height="8"><font color=red size=2><b>OR</b></font></td>
			</tr>		
			<tr>
				<td width="35%" class="boldtext">
					Link
				</td>
				<td  id=tdlink  width="65%">
					<select id="sellink" name="selsite" onchange="Filllink()"  runat=server   Class="listbox" >
					    
					</select>
				</td>
			</tr>
			<tr>
				<td width="35%" class="boldtext">
					Target
				</td>
				<td width="65%" >
					 <select id="seltarget" name="seltarget"  id="seltarget" Class="listbox" >
						<option value="">-- Select Target --</option>
						<option value="_blank">New Page</option>
						<option value="_self">Same Page</option>						
					 </select>
				</td>
			</tr>
			<tr>
				<td colspan="2" >
					<P>
						<BUTTON ID="Ok" type="button"  class='blogtext' onclick="javascript:validatemodel();"   class="text">OK</BUTTON> 
						<BUTTON class="text" ONCLICK="window.close();" type="button"  class='blogtext' >
						Cancel</BUTTON>
						</P>
				</td>
			</tr>
		</table>
		<BR>
	
				</form>
				
				
