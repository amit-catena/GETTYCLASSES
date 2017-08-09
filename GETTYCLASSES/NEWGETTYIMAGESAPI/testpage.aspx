<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testpage.aspx.cs" Inherits="newgettyimagesAPI.testpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script src="<%=NewGettyAPIclasses.ConstantAPI.siteURL%>js/jquery.js" ></script> 
</head>
<body>
    <link  href="<%=NewGettyAPIclasses.ConstantAPI.siteURL%>css/layout.css" rel="stylesheet"  type="text/css"  />
    <form id="form1" runat="server">
    <div>
        <input type="hidden" id="rsjson" name="rsjson" /> 
    <span><img src="<%=NewGettyAPIclasses.ConstantAPI.siteURL%>/images/getty.png" </span>
        <span><asp:TextBox ID="txtimage" runat="server"></asp:TextBox></span>
        <span><input type="button"  id="btngetty" value="Get getty images"  title="Get getty images"/></span>
    </div>
        <div class='imgdiv'>
	
	</div>
    </form>

    <script type="text/javascript">
        var multiple = "N";
        var URL = "<%=NewGettyAPIclasses.ConstantAPI.siteURL%>searchimages.aspx?NwtID=Network1&liveupdate=Y&SiteId=1&multiple=" + multiple;
        $("#btngetty").click(function () {
            var newdata = window.open(URL, "imageSearch", "height=1000,width=1500,status=yes,toolbar=no,menubar=no,location=no");
        });
        window.addEventListener('message', function (e) {
            if (e.data[0] == 'getty') {
                getrespondata(e.data[1])
            }
        });

        function getrespondata(t) {
            
            var imgs = JSON.parse(t.replace(/['\n]/g, '')).imagedetails;
            console.log(imgs)
            var k = Object.keys(imgs);
            var urls = [];
            if (multiple == "Y") {
                for (var m in k) {
                    var kk = Object.keys(imgs[k[m]]);
                    for(var n in kk)
                    {
                        if (kk[n] == "URL") {
                            $(".imgdiv").append("<img src='" + imgs[k[m]][[kk[n]]] + "'  />");
                        }
                        if (kk[n] == "DESC") {
                            $(".imgdiv").append("<p>" + imgs[k[m]][[kk[n]]] + "</p>");
                        }
                    }
                }
            }else
            {
                for (var m in k) {
                    if (k[m] == "URL") {
                        $(".imgdiv").append("<img src='" + imgs[k[m]] + "'  />");
                    }
                    if (k[m] == "DESC") {
                        var data = imgs[k[m]].replace(/'/g, '"');
                        $(".imgdiv").append("<p>" + data + "</p>");
                    }
                }

            }
           
        }

      
    </script>
</body>
</html>


