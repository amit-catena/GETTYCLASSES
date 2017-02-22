<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addnewsliveupdate.aspx.cs"
    Inherits="gettywebclasses.addnewsliveupdate" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../datepicker/jquery.datetimepicker.css" >
     <script src="../datepicker/jquery.datetimepicker.js"></script>
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
    </style>
    <script>

        function closePOP(count, newsid) {
            var retVal = count + '_' + newsid
            parent.postMessage(retVal, '*');
        }        
	

    </script>
</head>
<body>
    <form id="form1" runat="server" onsubmit="return validate();">
    <div class="container">
        <span style='font: size:10px; font-family: Verdana; text-align: center;'><b>Live Update</b></span><br />
       
        <span id="newstitle" style="display: none;"></span>
        <span class="submit-btn">
        <asp:Button ID="btnadd" runat="server" CssClass="buttontext" Text="Add" OnClick="btnadd_click" />
        &nbsp;
        <asp:Button ID="btndelete" runat="server" Text="Delete" class="buttontext" />
        </span>
        <div class="divtheme" id="divadd" runat="server">
            <table cellpadding="3" cellspacing="0">
                <tr>
                    <td>
                        Title
                    </td>
                    <td>
                        <asp:TextBox ID="textTitle" runat="server" Style='width: 500px;'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Time
                    </td>
                    <td>
                        <asp:TextBox ID="txtstartdate" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Image
                    </td>
                    <td>
                        <asp:FileUpload ID="file1" runat="server" /></br>
                        <asp:Literal ID="ltimg" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Description
                    </td>
                    <td>
                        <asp:TextBox ID="textdesc" runat="server" TextMode="MultiLine" Rows="10" Columns="70"></asp:TextBox>
                    </td>
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
    <div class="maintable" id="divlist" runat="server">
        <table cellpadding="3" cellspacing="0" class="tbl-list">
            <tr>
                <td class='headings' width="5px">
                    <input type="checkbox" id="checkAll" />
                </td>
                <td class='headings'>
                    Title
                </td>
                <td class='headings'>
                    Edit
                </td>
                 <td class='headings'>
                   Time
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
    <script>
        /*
        $("#btnsave").click(function () {             
        fncallSaveNewsLiveUpdate();
        });

        function fncallSaveNewsLiveUpdate() {              
             
        var url = "<%=baseurl%>ajaxpost.aspx";
        var _networkid = '<%=networkid %>';
        var _siteid = '<%=siteid %>';
        var _newsid = '<%=newsid %>';
        var _title=$("#textTitle").val();
        var _desc=$("#textdesc").val();
        var _imagename = $("#file1").val();
        var _addedby ='<%=userid %>';
        $.ajax({
        type: 'POST',
        url: url,
        cache: false,
        data: { type: 'newsliveupdate', newtworkid: _networkid, siteid: _siteid, newsid: _newsid, title: _title, desc: _desc, addedby: _addedby, image: _imagename },
        success: function (msg) {                                        
        closePOP(msg,_newsid);
        },
        error: function (request, err) {
        }
        });
        }
        */
        $("#btnadd").bind("click", function () {
            $("#divadd").show();
            $("#divlist").hide();
            $("#btndelete").hide();
            $("#btnadd").hide();


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
                    data: { type: 'deletenewsliveupdate', ids: favoriteids, networkid: _networkid, newsid: _newsid ,siteurl:_siteurl},
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

        function validate() {
            var title = $("#textTitle").val();
            if (title == "") {
                alert("Please Add Title");
                return false;
            }
        }

              

        $('#<%=txtstartdate.ClientID %>').datetimepicker()
	   .datetimepicker({ value: '', step: 1 });
     
        
      
    </script>
    </form>
</body>
</html>
