<%@ Page Title="" Language="C#" MasterPageFile="~/list.Master" AutoEventWireup="true" CodeBehind="newsingleimage.aspx.cs" Inherits="gettywebclasses.newsingleimage" %>
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


        function setimage(img) {
            window.opener.document.getElementById("hdngettyimg").value = img;           
            var logo = window.opener.document.getElementById('imggetty');
            logo.src = "../newsliveupdate/" + img;
            logo.setAttribute('width','60');
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(endRequest);
        function endRequest(sender, endRequestEventArgs) {
            //alert("trigger");
            imgPopup();
        }
    </script>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" >
        <ProgressTemplate>
        <span class='loader'></span>
            <div class="progdiv" id="prog">
                <span style="line-height: 26px;">downloading images&nbsp;&nbsp;&nbsp;</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <div class="search-wrap">
                <div class="searchIn">
                    <asp:TextBox ID="txtsearch" CssClass="searchtxt" runat="server" Text="Search here..."></asp:TextBox>
                    <asp:Button ID="btnsearch" CssClass="searchbtn" runat="server" Text="Search" OnClick="btnsearch_Click" />
                    <!--<asp:Button ID="btnclear" CssClass="searchbtn" runat="server" Text="Clear search" OnClick="btnclear_Click" />-->
                    <asp:Button ID="loadmore" CssClass="searchbtn" runat="server" Text="Load more" OnClick="btnmore_Click" />
                    <asp:Button ID="prev" CssClass="searchbtn" runat="server" Text="Previous" OnClick="btnprev_Click" />
                </div>
                <div class="search-rt">
                    <asp:RadioButtonList ID="RDchkbox" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                        RepeatColumns="2" OnSelectedIndexChanged="RDchkbox_SelectedIndexChanged">
                        <asp:ListItem Value="vertical" Text="Vertical"></asp:ListItem>
                        <asp:ListItem Value="horizontal" Selected="True" Text="Horizontal"></asp:ListItem>
                    </asp:RadioButtonList>
                    
                </div>
            </div>
            <div class="datact">
                <asp:DataList ID="gettydata" DataKeyField="ImageId" RepeatColumns="5" RepeatDirection="Horizontal"
                    runat="server" OnItemDataBound="gettydata_ItemDataBound">
                    <ItemTemplate>
                        <div class="imgdiv">
                            <div class="thumimg">
                                <asp:Literal ID="lthtml" runat="server"></asp:Literal>
                                <div class="title-wrap">
                                    <asp:Label ID="lbl" runat="server"></asp:Label>
                                    <asp:RadioButton runat="server" ID="Rdselect" AutoPostBack="true" OnCheckedChanged="rd_selectedbtn" />
                                    <input id="btnselectId" runat="server" type="hidden" value='<%# Eval("ImageId") %>' />
                                    <strong>
                                        <%# Eval("Title")%></strong>
                                </div>
                            </div>
                            <p rel="<%# Eval("Caption")%>">
                                <%# Eval("ShortCaption")%></p>
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
