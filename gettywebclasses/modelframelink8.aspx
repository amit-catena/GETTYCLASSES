<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modelframelink8.aspx.cs" Inherits="gettywebclasses.modelframelink8" %>

<html>
	<HEAD>
		<TITLE>Select Links </TITLE>
		<STYLE type="text/css">BODY { FONT-SIZE: 12px; FONT-FAMILY: Verdana; BACKGROUND: #eeeeee; MARGIN: 0px }
	BUTTON { WIDTH: 5em }
	P { TEXT-ALIGN: center }
	.boldtext { FONT-SIZE: 12px; FONT-FAMILY: Verdana; BACKGROUND: #eeeeee; FONT-WEIGHT: bold; MARGIN-LEFT: 10px }
	.text { FONT-SIZE: 12px; FONT-FAMILY: Verdana; BACKGROUND: menu; MARGIN-LEFT: 10px }
	.listbox { FONT-SIZE: 12px; FONT-FAMILY: Verdana; BACKGROUND: white }
	</STYLE>
		<SCRIPT language="JavaScript">
		    function validate() {
		        if (document.getElementById("seltarget").selectedIndex == 0)
		            alert('Please Select the target');
		        else {
		            var tUrl = "//www.caledonianmedia.com";


		            if (document.selection && document.selection.createRange) {
		                var range = window.parent.idContent8.document.selection.createRange();
		                window.parent.idContent8.focus();
		            }
		            else if (window.getSelection) {
		                var userSelection;
		                var iframe = window.parent.document.getElementById('idContent8');
		                var userSelection = iframe.contentWindow.getSelection();
		                window.parent.idContent8.focus();


		            }

		            var retVal = document.getElementById("sellink").value + "," + document.getElementById("seltarget").value + ",2";
		            //alert( document.form1.sellink.value + "," + document.form1.seltarget.value);

		            var tmp = retVal.split(',');
		            retVal = tmp[0];
		            target = tmp[1];
		            var t = tmp[2];


		            if (document.selection && document.selection.createRange) {

		                range.pasteHTML('<a href=\"' + tUrl + '/sitestat.aspx?siteurl=' + retVal + '\" class=\"links2Bold\"  target=\"' + target + '\">' + range.text + '</a>');

		            }
		            else if (window.getSelection) {
		                var range, node;
		                //alert(window.getSelection().getRangeAt);
		                range = getRangeObject(userSelection);
		                node = range.createContextualFragment('<a href=\"' + tUrl + '/sitestat.aspx?siteurl=' + retVal + '\" class=\"links2Bold\"  target=\"' + target + '\">' + userSelection + '</a>');
		                range.deleteContents();
		                range.insertNode(node);



		            }
		            else {
		                var range = getRangeObject(userSelection);
		                var newSpan = window.parent.document.createElement('span');
		                newSpan.innerHTML = '<a href=\"' + tUrl + '/sitestat.aspx?siteurl=' + retVal + '\" class=\"links2Bold\"  target=\"' + target + '\">' + userSelection + '</a>';
		                //alert('<a href=\"' +  tUrl + '/sitestat.aspx?siteurl=' + retVal +'\" class=\"links2Bold\"  target=\"' + target + '\">' + userSelection + '</a>');
		                range.deleteContents();
		                range.insertNode(newSpan);

		            }
		        }
		    }

		    function getRangeObject(selectionObject) {
		        if (selectionObject.getRangeAt) {
		            return selectionObject.getRangeAt(0);
		        } else { // Old Safari!
		            var range = document.createRange();
		            range.setStart(selectionObject.anchorNode, selectionObject.anchorOffset);
		            range.setEnd(selectionObject.focusNode, selectionObject.focusOffset);
		            return range;
		        }
		    }


		</SCRIPT>
		<!-- Copyright 2000 Microsoft Corporation. All rights reserved. -->
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
	</HEAD>
	<BODY>
		<TABLE cellpadding="3" cellspacing="1" width="100%" border="0">
			<form name="form1" id="form1" runat="server">
				<TBODY>
					<tr>
						<td style='width:1%' class="boldtext">
							<asp:DropDownList ID="sellink" Width="210" Runat="server"></asp:DropDownList>
						</td>
						<td width="1%">
							<select id="seltarget" name="seltarget" Class="listbox">
								<option value="">--</option>
								<option selected value="_blank">New</option>
								<option value="_self">Same</option>
							</select>
						</td>
						<td align='left' width="1%">
							<BUTTON ID="Ok" onclick="javascript:validate();" class="text">OK</BUTTON>
						</td>
					</tr>
			</form>
			</TBODY>
		</TABLE>
	</BODY>
</html>