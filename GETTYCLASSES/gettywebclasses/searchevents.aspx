<%@ Page Title="" Language="C#" MasterPageFile="~/list.Master" AutoEventWireup="true" CodeBehind="searchevents.aspx.cs" Inherits="gettywebclasses.searchevents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.searchtxt').addClass('inputTxt');
            $('.searchtxt').focus(function () {
                var input = $(this);
                if (input.val() == "Search here...") {
                    input.val("");
                }
                input.removeClass('inputTxt');
            }).blur(function () {
                var input = $(this);
                if (input.val() == "") {
                    input.val("Search here...");
                    input.addClass('inputTxt')
                }
            })
        })
        function imgPopup() {
            $(function () {
                $('[id$="UpdatePanel1"] .imgdiv .thumimg img').deepPop({ width: 'auto', topOffset: 25, leftOffset: 50, path: '.thumimg' });
                // <-- what's this doing here?
                $("img.lazy").show().lazyload({
                    //placeholder: "/images/blank.gif",
                    effect: "fadeIn",
                    failurelimit: 1,
                    threshold: -20,
                    event: 'sporty'
                });

                setTimeout(function () { $("img.lazy").trigger("sporty") }, 200);

            })


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
    </asp:ScriptManager>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        function endRequest(sender, endRequestEventArgs) {
            //alert("trigger");
            imgPopup();
        }
    </script>
   
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>

            <img src="images/loading87old.gif" />
             <div class="progdiv" id="prog"  >
            <span style="line-height:26px;">Downloading images from Getty</span>
            <span>On the next screen you will be able to edit titles and photo credits.</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         <div class="search-wrap">
        <div class="searchIn">
        <asp:TextBox ID="txtsearch" CssClass="searchtxt" runat="server" Text="Search here..."></asp:TextBox>
        <asp:Button ID="btnsearch" CssClass="searchbtn" runat="server"  Text="Search" 
            onclick="btnsearch_Click" />
            <asp:Button ID="btnclear" CssClass="searchbtn" runat="server" Text="Clear search" OnClick="btnclear_Click" />
            </div>
             <div class="search-rt">
                    <asp:RadioButtonList ID="RDchkbox" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                        RepeatColumns="2" OnSelectedIndexChanged="RDchkbox_SelectedIndexChanged">
                        <asp:ListItem Value="vertical" Text="Vertical"></asp:ListItem>
                        <asp:ListItem Value="horizontal" Selected="True" Text="Horizontal"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Button ID="loadmore" CssClass="searchbtn" runat="server" Text="Load more" OnClick="btnmore_Click" />
                    <asp:Button ID="prev" CssClass="searchbtn" runat="server" Text="Previous" OnClick="btnprev_Click" />
                </div>
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
                              <%# Eval("Title")%>
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
            <asp:AsyncPostBackTrigger ControlID="btnsearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnclear" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="RDchkbox" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        imgPopup();
    </script>

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
