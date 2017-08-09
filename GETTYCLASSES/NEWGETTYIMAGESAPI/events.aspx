<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"  Async="true" CodeBehind="events.aspx.cs" Inherits="newgettyimagesAPI.events" %>
<asp:Content ID="Content2" ContentPlaceHolderID="mainmastercontent" runat="server">
     <div class="midwrp">
            <H1>Sports Events</H1>
	<ul class="innersec">
        <H1></H1>
			<%=strtext.ToString()%>
    </ul>
            <div class="loadermore">
        <img src="<%=NewGettyAPIclasses.ConstantAPI.siteURL%>images/loading87.gif" />
    </div>
</div>

    <script type="text/javascript">
        var ajaxurl = "<%=NewGettyAPIclasses.ConstantAPI.siteURL%>imageajax.aspx";
        var track_page = 1;
        var pageInd = 1;
        var inteventid=<%=inteventid%>;
        var multiple = "<%=strmultiple%>";
        var loading = false; //prevents multiple loads
        $(window).scroll(function () {
            console.log(track_page);
            //detect page scroll
            if ($(window).scrollTop() + $(window).height() >= $(document).height()) {
                //if user scrolled to bottom of the page
                track_page++; //page number increment
                //alert("End Of The Page");
                if(inteventid==0)
                    load_contents(track_page); //load content   
            }
        });

        //Ajax load function
        function load_contents(track_page) {
            if (loading == false) {
                loading = true;  //set loading flag on
                console.log("track_page" + track_page);
                $(".loadermore").show();
                if (pageInd == 1)
                    pageInd = 2;

                $.ajax({
                    type: "POST",
                    url: ajaxurl,
                    cache: false,
                    data: { type: "load_multiple_events", page: pageInd},
                    success: function (msg) {
                        if (msg != "") {
                            var accepted;
                            $(".innersec").append(msg)
                            $(".loadermore").hide();
                            pageInd++;
                            loading = false;
                        }
                    },
                    error: function (request, err) {
                        console.log("error");
                    }
                });

            }
        }



    </script>
</asp:Content>
