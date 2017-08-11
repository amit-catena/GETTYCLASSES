<%@ Page Language="C#" MasterPageFile="~/Main.Master"  AutoEventWireup="true" CodeBehind="index.aspx.cs" Async="true"  Inherits="newgettyimagesAPI.index" %>
<asp:Content ID="indexconten" runat="server" ContentPlaceHolderID="mainmastercontent">
    
     <div class="loader">
        <img src="<%=NewGettyAPIclasses.ConstantAPI.siteURL%>images/loading87.gif" />
    </div>
    <div class="serchtxt">
        <span >search</span>
        <span><asp:TextBox ID="txtsear" runat="server"></asp:TextBox>   </span> 
        <span><input name="search" type="button"  id="search"   value="Search images" />  </span> 

    </div> 
   
    <div class="midwrp">
	<ul class="innersec">
			<%=strtext.ToString()%>
    </ul>
    <span><input name="ldmore" class="ldmore" type="button"  id="ldmore"   value="Load more images" />  </span> 
</div>
    <script type="text/javascript" >

        var ajaxurl = "<%=NewGettyAPIclasses.ConstantAPI.siteURL%>imageajax.aspx";
        var networkid = "<%=strnetwork%>";
        var siteId = "<%=intsiteId%>";
        var pageInd = 1;

        $("#search").click(function () {
            $(".loader").show();
            var str = $("#<%=txtsear.ClientID%>").val();
            console.log("str" + str);
            $.ajax({
                type: "POST",
                url: ajaxurl,
                cache: false,
                data: { type: "Getserchimages", search: str, page: pageInd },
                success: function (msg) {
                    if (msg != "") {
                        var accepted;
                        $(".innersec").html(msg)
                        $(".loader").hide();
                        pageInd++;
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });

        });

        $("#ldmore").click(function () {
            $(".loader").show();
            var str = $("#<%=txtsear.ClientID%>").val();
            if (pageInd == 1)
                pageInd = 2;
            console.log("str" + str);
            $.ajax({
                type: "POST",
                url: ajaxurl,
                cache: false,
                data: { type: "Getserchimages", search: str, page: pageInd },
                success: function (msg) {
                    if (msg != "") {
                        var accepted;
                        $(".innersec").append(msg)
                        $(".loader").hide();
                        pageInd++;
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });

        });

        function getdownload(ID)
        {
            $(".loader").show();
            $.ajax({
                type: "POST",
                url: ajaxurl,
                cache: false,
                data: { type: "Downloadimage", ID: ID },
                success: function (msg) {
                    if (msg != "") {
                        var accepted;
                        alert(msg);
                        $(".loader").hide();
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });
        }


        function getdownloadfornetwork(ID) {
            window.parent.postMessage(['getty', 'test'], "*");
            console.log("got id" + ID.value)
            $(".loader").show();
            $.ajax({
                type: "POST",
                url: ajaxurl,
                cache: false,
                data: { type: "Downloadimage_Forsite", ID: ID.value, SiteID: siteId, NetworkID: networkid },
                success: function (msg) {
                    if (msg != "") {
                        // $(window.opener).find('#rsjson').val(msg);
                        //window.opener.receiveDataFromPopup(msg);
                        console.log(msg)
                        window.opener.postMessage(['getty',msg], "*");
                        $(".loader").hide();
                        window.close();
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });
        }
        function getimagesave(ID)
        {
            console.log("got id" + ID.value)
            var data = getdownloadfornetwork(ID.value);
            

            
        }
       

    </script> 
</asp:Content>    
