<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="slotscreenshotimage.aspx.cs" Inherits="gettywebclasses.superevent.slotscreenshotimage" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="app/bower_components/html5-boilerplate/dist/css/normalize.css">
  <link rel="stylesheet" href="app/bower_components/html5-boilerplate/dist/css/main.css">
  <link rel="stylesheet" href="app/app.css">
  <script src="app/bower_components/html5-boilerplate/dist/js/vendor/modernizr-2.8.3.min.js"></script>
    <script type="text/javascript">
   function closePOP() {        
         parent.postMessage('closewin', '*');
         }
</script>
</head>
<body ng-app="myApp" flow-prevent-drop>
    <form id="form1" runat="server">
    <div ng-view></div>
    <div>
     <asp:HiddenField ID="hdnimage1" runat="server" />
     <asp:HiddenField ID="hdnimage2" runat="server" />
     <asp:HiddenField ID="hdnimage3" runat="server" />
     <asp:HiddenField ID="hdnimage4" runat="server" />
     <asp:HiddenField ID="hdnimage5" runat="server" />
     
     <asp:Button ID="btnSave" runat="server" Text="Submit" OnClick="btnsave_Click" />     
    </div>
    </form>
    <script>
       
        var jsondata = null;
        var baseurl = "<%=baseurl%>";
       // var sport=window.location.search.split('sport=')[1].toLowerCase();
         var sport='';
        
        function DeleteImages(_supereventids) {
         
            
            $.ajax({
                type: 'POST',
                url: baseurl,
                cache: false,
                data: { type: 'supereventimages', supereventids: _supereventids},
                success: function (msg) {
                    if (msg != 0) {
                      
                    }
                    else {
                       
                    }
                },
                error: function (request, err) {
                    alert(err.status);
                }
            });
        }

       

    </script>
   <script src="app/bower_components/angular/slotangular.min.js"></script>
  <script src="app/bower_components/angular-route/angular-route.min.js"></script>
  <script src="app/js/app-slot.js"></script>
  <script src="app/bower_components/fabric/dist/all.min.js"></script>
  <script src="app/bower_components/darkroomjs/build/slotdarkroom.js"></script>
  <script src="app/bower_components/flow.js/dist/ng-flow.js"></script>
  <script src="app/bower_components/angular-darkroom/dist/js/slotangular-darkroom.min.js"></script>
  <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.4.0/angular-animate.min.js"></script>
  <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.4.0/angular-aria.min.js"></script>
  <script src="app/js/angular-material.js"></script>
   <link rel="stylesheet" href="app/js/angular-material.min.css">
  <link rel="stylesheet" href="app/bower_components/darkroomjs/build/darkroom.css">
</body>
</html>
