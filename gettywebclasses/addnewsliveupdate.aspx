<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addnewsliveupdate.aspx.cs"
    Inherits="gettywebclasses.addnewsliveupdate"  ValidateRequest="false" %>
 <html> 
<head id="Head1" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../datepicker/jquery.datetimepicker.css" >
    <script src="../scripts/toolbarfuncs8.js" type="text/javascript"></script>   
   
    <title></title>
    <style>
        .container
        {
            display: block;
            padding: 15px;
        }
        .divtheme
        {
            min-height: 94px; /* border: 2px dashed #777; */
            color: #777;
            width: 100%;
            margin: auto;
            
        }
        .headings
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            font-weight: bold;
            color: #333333;
            text-decoration: none;
        }
        .buttontext
        {
            background: -webkit-linear-gradient(top,#fefefe,#e8e8e8);
            background: -moz-linear-gradient(top,#fefefe,#e8e8e8);
            border: solid 1px #999;
            border-radius: 3px;
            padding: 3px 10px;
            cursor: pointer;</style>
    <style>
        TABLE
        {
            border-collapse: collapse;
            width: 100%;
        }
        .tr-list
        {
            border-top: medium none;
        }
        .td-tbl-list
        {
            border-top: medium none;
            border-right: medium none;
            border-bottom: medium none;
            padding-bottom: 0px;
            padding-top: 0px;
            padding-left: 0px;
            margin: 0px;
            border-left: medium none;
            padding-right: 0px;
        }
        .head-maintable TR TD
        {
            border-top: medium none;
            border-right: medium none;
            border-bottom: medium none;
            border-left: medium none;
        }
        .maintable
        {
                margin: 10px 0;
        }
        .submit-btn{ margin-top:5px; display: block;}
        .maintable TR TD
        {
            border-top: #999999 1px solid;
            border-right: #999999 1px solid;
            border-bottom: #999999 1px solid;
            border-left: #999999 1px solid;
            
        }
        .divtheme table tr td{ padding-left:0;}
        .tblliveudt{width: 11%;}
    </style>
    <script>

        

        function closePOP(count, newsid) {
            var retVal = count + '_' + newsid
            parent.postMessage(retVal, '*');
        }        
	

    </script>
</head>

<body>
    <form id="ed" runat="server" onsubmit="return validate();">   
    <input type="hidden" id="hdngettyimg" name="hdngettyimg" />
    <input type="hidden" value="0" name="htm2" id="htm2"> <input type="hidden" value="0" name="htm">
    <asp:HiddenField ID="hdnAU" Value="11" runat="server" />
    <div class="container">
        <div style="LEFT: -100px; VISIBILITY: hidden; WIDTH: 0px; POSITION: absolute; TOP: 0px; HEIGHT: 0px" align="center">
		<TEXTAREA id="templateText8" name="templateText8" rows="4" cols="70"><asp:Literal id='ltback3' runat='server'></asp:Literal></TEXTAREA>
		</div>
        <span style='font: size:10px; font-family: Verdana; text-align: center;'><b>Live Update</b></span><br /><br />
        <span class="headings tblliveudt">Header</span>  <span style="padding-left:58px;"> <asp:TextBox ID="txtHeader" runat="server" style="width:500px;"></asp:TextBox>&nbsp;  <input type="button" id="btnsetheader" value="Set Header" class="buttontext" /></span>
        <span id="newstitle" style="display: none;"></span>
        <span class="submit-btn">      
        <input type="button" id="btnadd" value="Add" class="buttontext" />            
         <input type="button" id="btndelete" value="Delete" class="buttontext" />
         <input type="button" id="btnsethighlight" value="Set Highlight" class="buttontext" />
         <input type="button" id="btnremovehighlight" value="Remove Highlight" class="buttontext" />
         <input type="button" id="btnsetfeatured" value="Set Pin" class="buttontext" />
         <input type="button" id="btnremovefeatured" value="Remove Pin" class="buttontext" />
        </span>
        <div class="divtheme"  id="divadd" style="display:none;">
            <table cellpadding="3" cellspacing="0">
                <tr>
                    <td class="headings tblliveudt">
                      <span class="bold">Title</span>  
                    </td>
                    <td>
                           <asp:TextBox ID="textTitle" runat="server" Style='width: 500px;'></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="headings tblliveudt">
                       <span class="bold">Time</span> 
                    </td>
                    <td>                                 
                      <asp:DropDownList ID="ddlregion" runat="server">
                       <asp:ListItem Text="AU" Value="AU"></asp:ListItem>
                       <asp:ListItem Text="UK" Value="GB" Selected="True"></asp:ListItem>
                       </asp:DropDownList>&nbsp;  <asp:TextBox ID="txtstartdate" runat="server"></asp:TextBox>    
                                       
                    </td>
                </tr>
                 <tr>
                    <td class="headings">
                      <span class="bold">Is Highlight</span>  
                    </td>
                    <td>
                       <asp:CheckBox ID="chkhighlight" runat="server" />   
                    </td>
                </tr>
                <tr>
                    <td class="headings">
                       <span class="bold">Link</span> 
                    </td>
                    <td>                                 
                     <asp:DropDownList ID="sellink" style="width:250px;"  Runat="server"></asp:DropDownList>
                     OR                                          
                                       
                    </td>
                </tr>
                <tr>
                    <td class="headings">
                      <span class="bold">Promotional Link</span>  
                    </td>
                    <td>
                     <select id="ddlpromoregion" name="ddlpromoregion" runat="server" onchange="javascript:Setlink();">
                            <option value="0" selected>-select-</option>
							<option value="GB" >GB</option>
							<option value="AU">AU</option>
						</select>&nbsp;<select id="ddlpromolink" name="ddlpromolink" runat="server" style='width:180px;'>
                        <option>-select promotional link-</option>
                        </select>&nbsp;OR
                    </td>
                </tr>
                  <tr>
                    <td class="headings">
                      <span class="bold">External Link</span>  
                    </td>
                    <td>
                     <asp:TextBox ID="txtexternallink" runat="server" style="width:500px;"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="headings">
                      <span class="bold">Target</span>  
                    </td>
                    <td>
                     <asp:DropDownList ID="ddltargetto" runat="server">
                     <asp:ListItem Value="_blank">new</asp:ListItem>
                     <asp:ListItem Value="_selt">same</asp:ListItem>
                     </asp:DropDownList>
                    </td>
                </tr>
              

                <tr>
                    <td class="headings tblliveudt">
                       <span class="bold">Image</span> 
                    </td>
                    <td>
                       
                        <asp:FileUpload ID="file1" runat="server" />
                        <a href="javascript:void(0);" id="gettyimg">Getty Image</a><br />
                        <asp:Literal ID="ltimg" runat="server"></asp:Literal>
                       
                    </td>
                </tr>
                <tr>
                    <td class="headings">
                      <span class="bold">Image Text</span>  
                    </td>
                    <td>
                       <asp:TextBox ID="txtimgtext" runat="server" style="width:500px;" TextMode="MultiLine" Rows="4" ></asp:TextBox>
                    </td>
                </tr>
               
                <tr>
					<td class="headings tblliveudt" height="30" width="22%" noWrap align="left">&nbsp;<span class="bold">Description</span></td>
					<td width="66%" align="left"> &nbsp;&nbsp;<!-- #Include File="../editor/editor8.inc" --></td>
				</tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnsave" runat="server" Text="submit" OnClick="btnsave_click" class='buttontext' />
                    </td>
                </tr>
            </table>
        </div>
    <div class="maintable" id="divlist">
        <table cellpadding="3" cellspacing="0" class="tbl-list">
            <tr>
                <td class='headings' width="5px">
                    <input type="checkbox" id="checkAll" />
                </td>
                <td class='headings'>
                    Title
                </td>
                <td class='headings' align="center">
                    Edit
                </td>
                 <td class='headings'>
                   Time
                </td>
                <td class='headings' align="center">
                   Highlight
                </td>
                <td class='headings' align="center">
                   Pin
                </td>
                <td class='headings'>
                    Addedon
                </td>
                <td class='headings'>
                    Added by
                </td>
            </tr>
            <asp:Literal ID="ltlist" runat="server"></asp:Literal>
        </table>
    </div>
    </div>
     <script src="../datepicker/jquery.datetimepicker.js"></script>
    <script>

        window.addEventListener('message', function (e) {
            var message = e.data;
            alert(message);
        });

        window.onload = function (e) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');

            if (url[5] == "operation=list") {
                $("#divlist").show();
            }
            else {
                $("#divadd").show();
                $("#divlist").hide();
                $("#btndelete").hide();
                $("#btnadd").hide();
                $("#btnsethighlight").hide();
                $("#btnremovehighlight").hide();
                $("#btnsetfeatured").hide();
                $("#btnremovefeatured").hide();
            }

        }
        $(document).ready(function () {
            $('#<%=txtstartdate.ClientID %>').datetimepicker()
	      .datetimepicker({ value: '', step: 1 });
             
        })
        
        function starter() {
            idContent8.document.write('<body>' + document.forms[0].templateText8.value + '</body>');
            idContent8.document.close();
            idContent8.document.designMode = "On";        
             
        }
   
        
        $("#btnadd").bind("click", function () {
            $("#divadd").show();
            $("#divlist").hide();
            $("#btndelete").hide();
            $("#btnadd").hide();
            $("#btnsethighlight").hide();
            $("#btnremovehighlight").hide();
            $("#btnsetfeatured").hide();
            $("#btnremovefeatured").hide();

        });
        $("#checkAll").change(function () {
            $("input:checkbox").prop('checked', $(this).prop("checked"));
        });


        $("#btndelete").click(function () {          
            var _siteurl = '<%=siteurl %>';
            var _newsid = '<%=newsid %>';
            var url = "<%=baseurl%>ajaxpost.aspx";
            var favoriteids = [];
            var _networkid = '<%=networkid %>';
            $("input:checkbox[class=source]:checked").each(function () {
                favoriteids.push($(this).val());
            });
            favoriteids = favoriteids.join(", ");
            if (favoriteids != "") {
                $.ajax({
                    type: 'POST',
                    url: url,
                    cache: false,
                    data: { type: 'deletenewsliveupdate', ids: favoriteids, networkid: _networkid, newsid: _newsid, siteurl: _siteurl },
                    success: function (msg) {
                        closePOP(msg, _newsid);
                    },
                    error: function (request, err) {
                    }
                });
            }
            else {
                alert("please select article");
                return false;
            }

        });

        $("#btnsethighlight").click(function () {
            var _siteurl = '<%=siteurl %>';
            var _newsid = '<%=newsid %>';
            var url = "<%=baseurl%>ajaxpost.aspx";
            var favoriteids = [];
            var _networkid = '<%=networkid %>';
            $("input:checkbox[class=source]:checked").each(function () {
                favoriteids.push($(this).val());
            });
            favoriteids = favoriteids.join(", ");
            if (favoriteids != "") {
                $.ajax({
                    type: 'POST',
                    url: url,
                    cache: false,
                    data: { type: 'highlightnewsliveupdate', ids: favoriteids, networkid: _networkid,  siteurl: _siteurl,status: 'Y' },
                    success: function (msg) {
                        window.location.reload();
                    },
                    error: function (request, err) {
                    }
                });
            }
            else {
                alert("please select article");
                return false;
            }

        });

        $("#btnremovehighlight").click(function () {
            var _siteurl = '<%=siteurl %>';
            var _newsid = '<%=newsid %>';
            var url = "<%=baseurl%>ajaxpost.aspx";
            var favoriteids = [];
            var _networkid = '<%=networkid %>';
            $("input:checkbox[class=source]:checked").each(function () {
                favoriteids.push($(this).val());
            });
            favoriteids = favoriteids.join(", ");
            if (favoriteids != "") {
                $.ajax({
                    type: 'POST',
                    url: url,
                    cache: false,
                    data: { type: 'highlightnewsliveupdate', ids: favoriteids, networkid: _networkid, siteurl: _siteurl, status: 'N' },
                    success: function (msg) {
                        window.location.reload();
                    },
                    error: function (request, err) {
                    }
                });
            }
            else {
                alert("please select article");
                return false;
            }

        });

        function validate() {
           
            var title = $("#textTitle").val();
            if (title == "") {
                alert("Please Add Title");
                return false;
            }
            
            if (!(isHTMLMode2)) {
                var htmlStr = idContent8.document.body.innerHTML;
                var dEl = $('<div />', { html: htmlStr });
                $('a', dEl).each(function () {
                    var that = this, reg = /(www.caledonianmedia.com|f.ast.bet|newsletterurl)/, hrf = that.href;
                    if (!reg.test(hrf)) {
                        that.href = hrf + '[newsletterurl]';
                    }
                })
                ed.templateText8.value = dEl.html();

            }
            else {

                ed.templateText8.value = idContent8.document.body.innerText;

            }
           
            
        }
        $("#ddlregion").change(function () {
            var region = $("#ddlregion option:selected").val();
            var _autime = '<%=autime %>';
            var _uktime = '<%=uktime %>';
            if (region == "AU") {
                $("#txtstartdate").val(_autime);
            }
            else {
                $("#txtstartdate").val(_uktime);
            }
        })
        starter();



        $("#gettyimg").click(function () {
            showgetty();
        });

               
         function showgetty() {         

             childWindow = window.open("http://anil-pc/gettyclasses/gettywebclasses/newsingleimage.aspx", "NewsImage", "menubar=no,toolbar=no,titlebar=no,location=no,width=1500,height=800,scrollbars=yes");
                     
           
         }


         window.addEventListener("message", receiveMessage, false);
         function receiveMessage(e) {
             Settwitterdat(e.data);
         }

         function Settwitterdat(strkey) {
             if (document.selection && document.selection.createRange) {

                 var range = idContent8.document.selection.createRange();
                 idContent8.focus();
             }
             else if (window.getSelection) {
                 var userSelection;
                 var iframe = document.getElementById('idContent8');
                 var userSelection = iframe.contentWindow.getSelection();
                 idContent8.focus();
             }

             if (document.selection && document.selection.createRange) {
                 range.pasteHTML(strkey);
             }
             else if (window.getSelection) {
                 var range, node;

                 range = getRangeObject(userSelection);
                 node = range.createContextualFragment(strkey);
                 range.deleteContents();
                 range.insertNode(node);


             }
             else {
                 var range = getRangeObject(userSelection);
                 var newSpan = document.createElement('span');
                 newSpan.innerHTML = strkey;

                 range.deleteContents();
                 range.insertNode(newSpan);
             }

         }

         function Setlink() {
             var e = document.getElementById("ddlpromoregion");
             var strUser = e.options[e.selectedIndex].value;
             filllink(strUser);
         }

         function filllink(region) {
             var url = "<%=baseurl%>ajaxpost.aspx";
             $.ajax({
                 type: 'POST',
                 url: url,
                 cache: false,
                 data: { type: 'PROMOTIONALLINK', region: region },
                 success: function (msg) {                
                     
                         $("#ddlpromolink").html(msg);                    
                    
                 },
                 error: function (request, err) {
                     alert(err.status);
                 }
             });
         }


         $("#btnsetfeatured").click(function () {
             var _siteurl = '<%=siteurl %>';
             var _newsid = '<%=newsid %>';
             var url = "<%=baseurl%>ajaxpost.aspx";
             var favoriteids = [];
             var _networkid = '<%=networkid %>';
             $("input:checkbox[class=source]:checked").each(function () {
                 favoriteids.push($(this).val());
             });
             favoriteids = favoriteids.join(", ");
             if (favoriteids != "") {
                 $.ajax({
                     type: 'POST',
                     url: url,
                     cache: false,
                     data: { type: 'featurednewsliveupdate', ids: favoriteids, networkid: _networkid, siteurl: _siteurl, status: 'Y' },
                     success: function (msg) {
                         window.location.reload();
                     },
                     error: function (request, err) {
                     }
                 });
             }
             else {
                 alert("please select article");
                 return false;
             }

         });


         $("#btnremovefeatured").click(function () {
             var _siteurl = '<%=siteurl %>';
             var _newsid = '<%=newsid %>';
             var url = "<%=baseurl%>ajaxpost.aspx";
             var favoriteids = [];
             var _networkid = '<%=networkid %>';
             $("input:checkbox[class=source]:checked").each(function () {
                 favoriteids.push($(this).val());
             });
             favoriteids = favoriteids.join(", ");
             if (favoriteids != "") {
                 $.ajax({
                     type: 'POST',
                     url: url,
                     cache: false,
                     data: { type: 'featurednewsliveupdate', ids: favoriteids, networkid: _networkid, siteurl: _siteurl, status: 'N' },
                     success: function (msg) {
                         window.location.reload();
                     },
                     error: function (request, err) {
                     }
                 });
             }
             else {
                 alert("please select article");
                 return false;
             }

         });


         $("#btnsetheader").click(function () {
             var _siteurl = '<%=siteurl %>';
             var _newsid = '<%=newsid %>';
             var url = "<%=baseurl%>ajaxpost.aspx";
             var _networkid = '<%=networkid %>';
             var _header = $("#txtHeader").val();
             $.ajax({
                 type: 'POST',
                 url: url,
                 cache: false,
                 data: { type: 'liveupdateheader', newsid: _newsid, header: _header, networkid: _networkid, siteurl: _siteurl },
                 success: function (msg) {
                     alert("Header save successfully");
                 },
                 error: function (request, err) {
                 }
             });

         });



    </script>
    </form>
</body>
</html>
