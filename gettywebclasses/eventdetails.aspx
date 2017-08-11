<%@ Page Title="" Language="C#" MasterPageFile="~/list.Master" AutoEventWireup="true" CodeBehind="eventdetails.aspx.cs" Inherits="gettywebclasses.eventdetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
    </asp:ScriptManager>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        function endRequest(sender, endRequestEventArgs) {
            //alert("trigger");
            //imgPopup();
            $('#ContentPlaceHolder1_UpdateProgress1').hide();
        }
    </script>
   
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <span class='loader'></span>
             <div class="progdiv" id="prog"  >
            <span style="line-height:26px;">Downloading images from Getty</span>
            <span>On the next screen you will be able to edit titles and photo credits.</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         <div class="search-wrap">
         <h1 >Current Events</h1>
         <asp:Button ID="loadmore" CssClass="searchbtn" runat="server" Text="Load more" OnClick="btnmore_Click" />
         <asp:Button ID="prev" CssClass="searchbtn" runat="server" Text="Previous" OnClick="btnprev_Click" />
           </div>
            <div class="datact">
            <asp:DataList ID="gettydata" DataKeyField="ImageId" RepeatColumns="5" RepeatDirection="Horizontal"
                runat="server"  onitemdatabound="gettydata_ItemDataBound" >
                <ItemTemplate>
                    <div class="eventimgdiv">
                        <div class="eventthumimg">
                            <asp:Literal ID="ltdate" runat="server" ></asp:Literal>
                            <asp:Literal ID="lthtml" runat="server" ></asp:Literal>
                              <div class="event-wrap">
                              <asp:Literal ID="lttitle" runat="server" ></asp:Literal>
                                
                            </div>
                         <asp:Literal ID="ltcount" runat="server" ></asp:Literal> 
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            </div> 
            <asp:Literal ID="ltscript" runat="server"></asp:Literal>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="loadmore" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="prev" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
      <script>
      
          $(document).ready(function () {
              $(function () {           // <-- what's this doing here?
                  $("img.lazy").lazyload({
                      //placeholder: "/images/blank.gif",
                      container: $(".datact"),
                      effect: "fadeIn",
                      failurelimit: 1,
                      threshold: -20,
                      event: 'sporty'
                  });

                  $(window).bind("load", function () {

                      setTimeout(function () { $(".lazy").trigger("sporty") }, 500);
                  });
              });

          });
          function opendiv() {
              document.getElementById("prog").style.display = 'block';

          }
          function Singlegettyimage(imagename, imgurl, cookieName, nDays) {
              window.close();
          }
         
	</script>
</asp:Content>
