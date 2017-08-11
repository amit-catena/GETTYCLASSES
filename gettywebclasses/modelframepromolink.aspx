<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modelframepromolink.aspx.cs" Inherits="gettywebclasses.modelframepromolink" %>

<html>
	<HEAD>
		<TITLE>Select Links </TITLE>
		<STYLE type="text/css">BODY { FONT-SIZE: 12px; FONT-FAMILY: Verdana; BACKGROUND: #eeeeee; MARGIN: 0px }
	BUTTON { WIDTH: 5em }
	P { TEXT-ALIGN: center }
	.boldtext { FONT-SIZE: 12px; FONT-FAMILY: Verdana; BACKGROUND: #eeeeee; FONT-WEIGHT: bold; MARGIN-LEFT: 10px }
	.text { FONT-SIZE: 12px; FONT-FAMILY: Verdana; BACKGROUND: menu; MARGIN-LEFT: 10px }
	.select { FONT-SIZE: 11px; FONT-FAMILY: Verdana; WIDTH: 150px; BACKGROUND: white }
	</STYLE>
		<SCRIPT language="JavaScript">
		    function validate() {
		        var tUrl, target;
		        if (document.getElementById("seltarget").selectedIndex == 0)
		            alert('Please Select the target');
		        else {

		            if (document.getElementById("sellink").value == "0") {
		                alert('Please Select promotional link');
		                return false;
		            }

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

		            tUrl = document.getElementById("sellink").value;
		            //alert( document.form1.sellink.value + "," + document.form1.seltarget.value);


		            if (document.getElementById("sellink").value == "_blank")
		                target = '_blank';
		            else
		                target = '_self';


		            if (document.selection && document.selection.createRange) {

		                range.pasteHTML('<a href=\"' + tUrl + '\" class=\"links2Bold\"  target=\"' + target + '\">' + range.text + '</a>');

		            }
		            else if (window.getSelection) {
		                var range, node;
		                range = getRangeObject(userSelection);
		                node = range.createContextualFragment('<a href=\"' + tUrl + '\" class=\"links2Bold\"  target=\"' + target + '\">' + userSelection + '</a>');
		                range.deleteContents();
		                range.insertNode(node);



		            }
		            else {
		                var range = getRangeObject(userSelection);
		                var newSpan = window.parent.document.createElement('span');
		                newSpan.innerHTML = '<a href=\"' + tUrl + '\" class=\"links2Bold\"  target=\"' + target + '\">' + userSelection + '</a>';
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
		<form name="form1" id="form1" runat="server">
			<TABLE cellpadding="2" cellspacing="1" width="100%" border="0">
				<tr>
					<td>
						<select id="ddregion" onchange="javascript:Setlink();">
							<option value="GB" selected>GB</option>
							<option value="AU">AU</option>
						</select>
					</td>
					<td width="50%" class="boldtext">
						<asp:DropDownList ID="sellink" Width="210" Runat="server"></asp:DropDownList>
					</td>
					<td>
						<select id="seltarget" name="seltarget" Class="listbox">
							<option value="">--</option>
							<option selected value="_blank">New</option>
							<option value="_self">Same</option>
						</select>
					</td>
					<td align='left'>
						<BUTTON ID="Ok" onclick="javascript:validate();" class="text">OK</BUTTON>
					</td>
				</tr>
			</TABLE>
			<script src="scripts/jquery-1.4.1.min.js"></script>
			<script>
			    function Setlink() {
			        var e = document.getElementById("ddregion");
			        var strUser = e.options[e.selectedIndex].value;
			        filllink(strUser);
			    }

			    function filllink(region) {
			        var url = "<%= BaseUrl%>ajaxpost.aspx";
			        $.ajax({
			            type: 'POST',
			            url: url,
			            cache: false,
			            data: { type: 'PROMOTIONALLINK', region: region },
			            success: function (msg) {
			                $("#sellink").html(msg);
			            },
			            error: function (request, err) {
			                alert(err.status);
			            }
			        });
			    } 
	
			</script>
		</form>
	</BODY>
</html>
