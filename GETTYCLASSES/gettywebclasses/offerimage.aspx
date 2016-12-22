<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="offerimage.aspx.cs" Inherits="gettywebclasses.offerimage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
        html,body{margin:0; padding:0;}
        .darkroom-image-container .canvas-container{margin:0 auto;}
        .msg{display:none;position:absolute; width:100%; padding:50px 0; background:#fff; color:#666; font-size:21px; font-family:Arial; font-weight:bold; z-index:2; text-align:center;}
        .wait .msg{display:block;}
        #loader {position: absolute;top:35%;left: 50%;width: 200px;height: 200px;-webkit-transition: -webkit-transform 0.3s;transition: transform 0.3s;pointer-events: none; display:none; top:35%; left:50%;  -webkit-transform: translateX(-50%) translateY(-35%);
-moz-transform: translateX(-50%) translateY(-35%);
-ms-transform: translateX(-50%) translateY(-35%);
transform: translateX(-50%) translateY(-35%); z-index:9999;}
#loader.loading{display:block;}
#loader.loading::before,#loader.loading::after {position: absolute;bottom: 30px;left: 50%;display: block;border: 5px solid rgba(0,0,0,0.1);border-radius: 50%;content: '';}
#loader.loading::before {margin-left: -40px;width: 40px;height: 40px;border-right-color:rgba(22, 145, 190, 0.8);border-left-color:rgba(22, 145, 190, 0.8);-webkit-animation: rotation 3s linear infinite;animation: rotation 3s linear infinite;}
#loader.loading::after {bottom:40px;margin-left: -30px;width: 20px;height: 20px;border-top-color:rgba(22, 145, 190, 0.8);border-bottom-color: rgba(22, 145, 190, 0.8);-webkit-animation: rotation 1s linear infinite;animation: rotation 1s linear infinite;}
@-webkit-keyframes rotation {0%{ -webkit-transform: rotate(0deg); }50%{ -webkit-transform: rotate(180deg); }100%{ -webkit-transform: rotate(360deg); }}

@keyframes rotation {0%{ transform: rotate(0deg); }50%{ transform: rotate(180deg); }100%{ transform: rotate(360deg); }}

.darkroom-container .darkroom-toolbar{position:fixed; top:0; left:48.5%;border: 12px solid rgba(255,255,255,1);border-radius: 0;border-width: 1px 25px 12px 25px;}
#btcrop{display:inline-block; padding:10px 20px; margin-top:15px; font-family:Arial; font-size:16px; text-transform:uppercase; background:#d94c20; color:#fff; border:0; cursor:pointer;}
#btcrop:disabled{opacity:0.5;}
    </style>
    <link rel="stylesheet" type="text/css" href="js/darkroomjs/build/darkroom.css" />
     <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
     <script src="js/fabric/fabric.min.js"></script>
     <!-- <script src="https://cdnjs.cloudflare.com/ajax/libs/fabric.js/1.7.2/fabric.min.js"></script> -->
     <script src="js/darkroomjs/build/darkroom.js"></script>    
     
     <script>
         var _hash = location.search; 
         var fresh=true;        
        function closePOP(image) {
           fresh=false;
            parent.postMessage(['close',image], '*');
        }
        function updateImage(s) {
            
            document.body.classList.remove('wait');
        
        var dkrm = new Darkroom('#img1', {
          minWidth: 200,
          minHeight: 100,
          maxWidth: 1000,
          maxHeight: 750,
          //ratio: 4/3,
          backgroundColor: '#000',
          plugins: { crop: { quickCropKey: 67} },
          initialize: function() {var cropPlugin = this.plugins['crop'];cropPlugin.requireFocus();},
          init:function(){document.querySelector('#loader').className = '';}
        });
    
        }
        function imageCropped() {
            setTimeout(function () {                
                document.querySelector('#img64').value = document.querySelector('#croppedImg').src;
            document.querySelector('#btcrop').disabled = false;
        }, 500)
        }
    </script>
</head>
<body class="wait">
    <div class="msg">Grabbing screenshot please wait..</div>
 <div id="loader" class="loading"></div>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div style="text-align:center;">
   
    
        <div style="text-align:center;" >
        <img ID="img1" src="" /> 
        
            <!--<asp:Literal ID ="ltimg" runat="server"></asp:Literal>-->
        </div>
        <script>
            var _img = document.querySelector('#img1');            
            _img.onload = function () {
                //parent.postMessage(['close', 'test'], '*');
                updateImage();
            }
            _img.onerror = function () {
                alert('Sorry, something goes wrong please select another bookmaker.');
                document.body.classList.remove('wait');
                closePOP();
            }
            _img.crossOrigin = "Anonymous";
            if(fresh)
                _img.src = "images/newsimages/2013Nov12053730_187744267.jpg" // _img.src = "newimage.ashx" + _hash;

            
        </script>
        <input disabled type="submit"  id="btcrop" value="Upload"/>
        
    </div>
     <asp:HiddenField ID="img64" runat="server" Value="" />
    </form>
    <script>
            document.forms[0].addEventListener('submit',function(){
                console.log('submit')
                document.quer