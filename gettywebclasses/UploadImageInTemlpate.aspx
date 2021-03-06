﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImageInTemlpate.aspx.cs" Inherits="gettywebclasses.UploadImageInTemlpate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <asp:Literal runat="server" ID="ltcss"></asp:Literal>
    <style>
        .right-side-menu-btn-red{padding:6px; border-radius:4px; background:#cc0000; color:#fff; border-style:none; }
        .li_selected 
{
    border: solid 1px #007eff;
}
    </style>
</head>
<body>
    <form id="ImageUploadform1" runat="server">
    <div>
    <input type="hidden" id="id_image" runat="server" />
    <input type="hidden" id="id_imagesrc" runat="server"/>    
    <input type="hidden" id="id_imagesrcbig" runat="server"/>    
    <input type="hidden" id="idserver_image" runat="server" value="0" />
    <input type="hidden" id="idserver_templateid" runat="server" />
        <div class="upload-image">  
            <div class="page-header"><a href="javascript:void(0);" onclick="ShowImageOptions('U');">Select or Upload an Image</a><a href="javascript:void(0);" onclick="ShowImageOptions('G');">Search getty images</a></div>
            
            <div class="left-side-menu" id="divuploadimg">
                <div class="select-image"><asp:FileUpload runat="server" ID="FileUpload1" onchange="UploadFile()"  /></div>
                <div style="float:right;"><input type="checkbox" name="chkall" id="chkall" onchange="toggle(this)" />Toggle All</div>
                <div style="height:30px;"></div>
                <div class='imagediv'>            
                <asp:Literal runat="server" ID="ltsignupimages"></asp:Literal></div>
            </div>
            <div class="left-side-menu" id="divgettyimg" style="display:none;">
                <div class="select-image"><asp:TextBox runat="server" ID="txtgettysearch" placeholder='Search'></asp:TextBox><asp:Button runat="server" ID="btngetty" Text="Search" OnClientClick="ShowLoading('Y');" /></div>       
                <div><asp:RadioButton runat="server" ID="rdlandscape" Text="Landscape" Checked="true"  />
                <asp:RadioButton runat="server" ID="rdportrait" Text="Portrait"  />
                <asp:RadioButton runat="server" ID="rdboth" Text="Both"  /></div>         
                <div style="height:30px;"></div>
                <div class='imagediv'>            
                <asp:Literal runat="server" ID="ltgettyimages"></asp:Literal></div>
            </div>
            <div class="right-side-menu">
            <div><span class="right-side-menu-left">Title: <asp:TextBox runat="server" ID="txttitle" class="right-side-menu-right"></asp:TextBox></span></div>                <div style="clear:both;"></div>   
            <div><span class="right-side-menu-left">Alt Text: <asp:TextBox runat="server" ID="txtalttext" class="right-side-menu-right"></asp:TextBox></span></div><div style="clear:both;"></div>
            <div><span  class="right-side-menu-left">Date Uploaded: <asp:TextBox runat="server" ID="txtdateuploaded" ReadOnly="true" class="right-side-menu-right"></asp:TextBox></span></div>
            <div style="clear:both;"></div>
            <div><input type="button" class="right-side-menu-btn" id="btninsert" onclick="AddImageInParent()" value="Insert Image" />
            <asp:Button runat="server" ID="btndelete" class="right-side-menu-btn-red"  Text="Delete" OnClick="DeleteImage" OnClientClick="return ShowAlertDelete();"/>
            
            </div>
       </div>
            <div style="clear:both;"></div>
           <div id="json-overlay" style="display:none;"></div>      
    </div>
    </form>
    <script lang="javascript" type="text/javascript">
        function toggle(source) {            
            checkboxes = document.getElementsByName('fcheck[]');
            for (var i = 0, n = checkboxes.length; i < n; i++) {                
                checkboxes[i].checked = source.checked;
            }
        }

        function ShowAlertDelete() {
            var result = confirm("Are you sure to delete selected picture(s)?");            
            if (result == true) {
                ShowLoading('Y')
                return true;
            }
            else {
                return false;
            }
        }
        function UploadFile() {
            var value = $("#FileUpload1").val();
            if (value != '') {
                $("#ImageUploadform1").submit();
            }
            ShowLoading('Y');
        };

        $("#ul_image li").click(function (e) {
            if (e.target.nodeName == 'INPUT') { return}
            imageid = this.id;
            imageurl = $(this).attr('data-url');
            imageurlbig = $(this).attr('data-bigurl');
            $('#ul_image').children().removeClass('li_selected');
            $(this).addClass('li_selected');
            document.getElementById("id_image").value = imageid;
            document.getElementById("idserver_image").value = imageid;
            document.getElementById("id_imagesrc").value = imageurl;
            document.getElementById("id_imagesrcbig").value = imageurlbig;

            document.getElementById("txtdateuploaded").value = $(this).attr('data-date');
            document.getElementById("txttitle").value = $(this).attr('data-name');
            document.getElementById("txtalttext").value = $(this).attr('data-alt');
        });
        $("#ul_image li input[type='checkbox']").change(function (e) {
            e.preventDefault();
        })

        function AddImageInParent() {
            imageid = document.getElementById("idserver_image").value;
            imagetitle = document.getElementById("txttitle").value;
            imagealttext = document.getElementById("txtalttext").value;
            netid = '<%=networkid %>';

            var url = '<%=baseurl %>ajax_post.aspx';
            $.ajax({
                type: 'POST',
                url: url,
                cache: false,
                data: { type: "UpdateImageDetails", imageid: imageid, imagetitle: imagetitle, imagealttext: imagealttext , netid:netid},
                success: function (msg) {
                    //alert('hi');

                },
                error: function (request, err) {

                }
            });

            if (window.opener != null && !window.opener.closed) {
                //var txtName = window.opener.document.getElementById("incimage");
                //$(txtName).attr("src", document.getElementById("id_imagesrc").value);
                templateid = document.getElementById("idserver_templateid").value;
                //alert("templateid.." + templateid);
                var img = document.getElementById("id_imagesrc").value;
                if ( templateid == "2")
                    img = document.getElementById("id_imagesrcbig").value;
                window.opener.postMessage('incimage,' + img, 'http://www.developersllc.com');
            }
            window.close();
        }
        function ShowLoading(boolshow) {
            
            if (boolshow == 'Y')
                document.getElementById('json-overlay').style.display = 'block';
            else
                document.getElementById('json-overlay').style.display = 'none';
        }
        function ShowImageOptions(showdiv) {            
            if (showdiv == 'G') {
                document.getElementById('divgettyimg').style.display = 'block';
                document.getElementById('divuploadimg').style.display = 'none';   
            }
            else {
                document.getElementById('divuploadimg').style.display = 'block';
                document.getElementById('divgettyimg').style.display = 'none';
            }
        }
    </script>
</body>
</html>
