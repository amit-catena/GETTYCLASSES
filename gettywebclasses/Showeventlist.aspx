<%@ Page Title="" Language="C#" MasterPageFile="~/list.Master" AutoEventWireup="true" CodeBehind="Showeventlist.aspx.cs" Inherits="gettywebclasses.Showeventlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<h1>Getty Events</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
    </asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="datact activedata ">
            <asp:DataList ID="gettydata" DataKeyField="gettyID" RepeatColumns="3" RepeatDirection="Horizontal"
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
    </asp:UpdatePanel>
   <script type="text/javascript">
       var fr = parent.document.getElementById("lfLogin3");
      fr.style.height=document.body.scrollHeight+50;
    </script>
</asp:Content>
