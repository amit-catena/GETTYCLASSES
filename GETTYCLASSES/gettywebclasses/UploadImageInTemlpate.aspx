<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImageInTemlpate.aspx.cs" Inherits="gettywebclasses.UploadImageInTemlpate" %>

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
    <input type="hidden" id="id_image" />
    <input type="hidden" id="id_imagesrc" />    
    <input type="hidden" id="idserver_image" runat="server" value="0" />
        <div class="upload-image">  
            <div class="page-header"> Select or Upload an Image</div>
            
            <div class="left-side-menu">
            <div class="select-image"><asp:FileUpload runat="server" ID="FileUpload1" onchange="UploadFile()"  /></div>
            <asp:Literal runat="server" ID="ltsignupimages"></asp:Literal></div>
            <div class="right-side-menu">
            <div><span class="right-side-menu-left">Title: <asp:TextBox runat="server" ID="txttitle" class="right-side-menu-right"></asp:TextBox></span></div>                <div style="clear:both;"></div>   
            <div><span class="right-side-menu-left">Alt Text: <asp:TextBox runat="server" ID="txtalttext" class="right-side-menu-right"></asp:TextBox></span></div><div style="clear:both;"></div>
            <div><span  class="right-side-menu-left">Date Uploaded: <asp:TextBox runat="server" ID="txtdateuploaded" ReadOnly="true" class="right-side-menu-right"></asp:TextBox></span></div>
            <div style="clear:both;"></div>
            <div><input type="button" class="right-side-menu-btn" id="btninsert" onclick="AddImageInParent()" value="Insert Image" />
            <asp:Button runat="server" ID="btndelete" class="right-side-menu-btn-red"  Text="Delete" OnClick="DeleteImage" OnClientClick=" ShowLoading('Y');"/>
            
            </div>
       </div>
            <div style="clear:both;"></div>
           <div id="json-overlay" style="display:none;"></div>      
    </div>
    </form>
    <script lang="javascript" type="text/javascript">

        function UploadFile() {
            var value = $("#FileUpload1").val();
            if (value != '') {
                $("#ImageUploadform1").submit();
            }
            ShowLoading('Y');
        };

        $("#ul_image li").click(function () {
            imageid = this.id;
            imageurl = $(this).attr('data-url');
            $('#ul_image').children().removeClass('li_selected');
            $(this).addClass('li_selected');
            document.getElementById("id_image").value = imageid;
            document.getElementById("idserver_image").value = imageid;
            document.getElementById("id_imagesrc").value = imageurl;

            document.getElementById("txtdateuploaded").value = $(this).attr('data-date');
            document.getElementById("txttitle").value = $(this).attr('data-name');
            document.getElementById("txtalttext").value = $(this).attr('data-alt');
        });

        function AddImageInParent() {
            imageid = document.getElementById("idserver_image").value;
            imagetitle = document.getElementById("txttitle").value;
            imagealttext = document.getElementById("txtalttext").value;

            var url = '<%=baseurl %>ajax_post.aspx';
            $.ajax({
                type: 'POST',
                url: url,
                cache: false,
                data: { type: "UpdateImageDetails", imageid: imageid, imagetitle: imagetitle, imagealttext: imagealttext },
                success: function (msg) {
                    alert('hi');

                },
                error: function (request, err) {

                }
            });

            if (window.opener != null && !window.opener.closed) {
                var txtName = window.opener.document.getElementById("incimage");
                $(txtName).attr("src", document.getElementById("id_imagesrc").value);
            }
            window.close();
        }
        function ShowLoading(boolshow) {
            alert(boolshow);
            if (boolshow == 'Y')
                document.getElementById('json-overlay').style.display = 'block';
            else
                document.getElementById('json-overlay').style.display = 'none';
        }
    </script>
</body>
</html>
