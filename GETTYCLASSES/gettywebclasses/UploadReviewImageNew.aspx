<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadReviewImageNew.aspx.cs" Inherits="gettywebclasses.UploadReviewImageNew" %>

<HTML>
	<HEAD>
		<title>Slot Home Page Image</title>		
		<link rel="stylesheet" href="js/lib/darkroom.min.css">
		<script type="text/javascript">
		    /*
		    var ref = document.referrer.toLowerCase();
		    var pattrn = /list/g.test(ref);

		    console.log(pattrn);
		    if (!pattrn) {
		    parent.document.body.classList.remove('cPop')
		    }
		    */

		    function closePOP(imgpath, linkid) {
		        var retVal = imgpath + '_' + linkid
		        parent.postMessage(retVal, '*');
		    }
            /*
		    function closepop() {
		        alert('call');
		        return false;
		    }
            */
		    var topImagepath = '<%=topImagepath %>';
		</script>
			
				<style>
				   
				    .darkroom-toolbar-actions li:nth-child(-n+2){ display:none;}
				    .darkroom-toolbar-actions li:nth-child(3) .darkroom-button:nth-child(1),.darkroom-toolbar-actions li:nth-child(3) .darkroom-button:nth-child(3){ display:none;}
			         
            SPAN[data-set]:before { COLOR: #ccc; MARGIN: 0px 10px; DISPLAY: inline-block }
			TABLE ~ SPAN[data-set] { MARGIN-BOTTOM: 10px; PADDING-LEFT: 0px; DISPLAY: inline-block }
			TABLE ~ SPAN[data-set]:before { PADDING-LEFT: 20px; DISPLAY: block }
			.addlive { CURSOR: hand }
			TD TABLE TR TD SELECT { WIDTH: 200px }
			
			htm, body{margin:0; padding:0; background:none;}
.container{width:940px; margin:0 auto; padding-top:55px; text-align:center;}
/*.fileUpload{display:inline-block;padding:20px 20px 20px 42px;background: #0192DD url(<%=baseurl%>/images/camera.png) no-repeat 20px 19px; color:#fff; min-width:60px; position:relative; cursor:pointer; font-family:Arial, Helvetica, sans-serif; font-size:14px; text-transform:uppercase;} */
/*.fileUpload::before{content:'Upload Image'} */
/*.fileUpload input[type="file"]{opacity:0; position:absolute; left:0; top:0; height:56px;} */
/*.fileUpload .error{padding:7px; background:#F5CAD6; box-shadow: 0 0 3px rgba(0, 0, 0, 0.4); border: 1px solid #FF94B2; color:#900; font:12px Arial, Helvetica, sans-serif; position:absolute; bottom:0; left:50%; -webkit-transform:translateX(-50%);-moz-transform:translateX(-50%);transform:translateX(-50%); opacity:0; visibility:hidden;transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s; min-width:180px; text-transform:none;} */
.errorCrop{padding:7px; background:#F5CAD6; box-shadow: 0 0 3px rgba(0, 0, 0, 0.4); border: 1px solid #FF94B2; color:#900; font:12px Arial, Helvetica, sans-serif; position:absolute; bottom:0; left:50%; -webkit-transform:translateX(-50%);-moz-transform:translateX(-50%);transform:translateX(-50%); opacity:0; visibility:hidden;transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s; min-width:180px; text-transform:none;}
.fileUpload .error::after{content:''; width:0; height:0; position: absolute;bottom: -8px;left: 20%; border-top: 7px solid #FF94B2;border-left: -3px solid transparent;border-right: 13px solid rgba(0, 0, 0, 0); }
.errorCrop::after{content:''; width:0; height:0; position: absolute;bottom: -10px;left: 50%; border-top: 10px solid #FF94B2;border-left: 7px solid transparent;border-right: 7px solid rgba(0, 0, 0, 0);}
.fileUpload .error.show, .errorCrop.show{opacity:1; visibility:visible; bottom:35px;}
.errorCrop.show{bottom:42px;}
.uploadPop{width:300px; min-width:300px; margin-top:50px; position:relative; background:#fff; color: #808080;font-size: 14px;-webkit-user-select: none;-khtml-user-select: none;-moz-user-select: none;-ms-user-select: none;user-select: none;cursor: default; visibility:hidden; opacity:0;transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s;z-index:99;}
#uploadOverlay{width:90px; height:45px; position:fixed; top:0; left:0; background:rgba(255,255,255,0.5);transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s; visibility:hidden; opacity:0; z-index:98;}
.uploadPop #imgTarget img{width:90px;}
.uploadPop .darkroom-image-container{display:block; overflow:auto;}
.uploadPop i.remove{width: 24px;height: 24px;position: absolute;top: 5px;right: -5px;cursor: pointer; font-style:normal; font-size:18px;}
.uploadPop i.remove::before{content:'\2715';}
body.crop .uploadPop{visibility:visible; opacity:1;}
#cropPreview{margin-top:50px; position:absolute; left:50%; transform:translateX(-50%);-webkit-transform:translateX(-50%);-moz-transform:translateX(-50%);}
body.crop #uploadOverlay{visibility:visible; opacity:1;}
.actionBar{padding:15px 10px 0; text-align:center;}
.actionBar input[id$="btnSave"]{display:inline-block;padding: 10px 15px;background: #0192DD; color:#fff; min-width:60px; position:relative; cursor:pointer; font-family:Arial, Helvetica, sans-serif; font-size:14px; text-transform:uppercase; border:0;}
.actionBar #cancel{display:inline-block;padding: 10px 15px;background: #555; color:#fff; min-width:60px; position:relative; cursor:pointer; font-family:Arial, Helvetica, sans-serif; font-size:14px; text-transform:uppercase;border:0;}
#loader {position: fixed;top:50%;left: 50%;width: 50px;height: 50px;-webkit-transition: -webkit-transform 0.3s;transition: transform 0.3s;pointer-events: none; display:none; top:50%; left:50%;  -webkit-transform: translateX(-50%) translateY(-50%);
-moz-transform: translateX(-50%) translateY(-50%);
-ms-transform: translateX(-50%) translateY(-50%);
transform: translateX(-50%) translateY(-50%); z-index:9999; border-radius:50%; background:#fff; border:5px solid #fff;}
body.load #loader{display:block;}
body.load #loader::before,#loader.loading::after {position: absolute;left:0; top:0;display: block;border: 5px solid rgba(0,0,0,0.1);border-radius: 50%;content: '';}
body.load #loader::before {width: 40px;height: 40px;border-right-color:rgba(22, 145, 190, 0.8);border-left-color:rgba(22, 145, 190, 0.8);-webkit-animation: rotation 3s linear infinite;animation: rotation 3s linear infinite;}
body.load #loader::after {margin-left: -30px;width: 20px;height: 20px;border-top-color:rgba(22, 145, 190, 0.8);border-bottom-color: rgba(22, 145, 190, 0.8);-webkit-animation: rotation 1s linear infinite;animation: rotation 1s linear infinite;}
@-webkit-keyframes rotation {0%{ -webkit-transform: rotate(0deg); }50%{ -webkit-transform: rotate(180deg); }100%{ -webkit-transform: rotate(360deg); }}

@keyframes rotation {0%{ transform: rotate(0deg); }50%{ transform: rotate(180deg); }100%{ transform: rotate(360deg); }}
			</style>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" bgColor="#eeeeee">
<div class="container">

<form id="ed" onsubmit="return validate(this);" encType="multipart/form-data" method="post" runat="server">
	<input type="hidden" value="0" name="htm2"> <input type="hidden" value="0" name="htm">
	<div style="HEIGHT: 0px; WIDTH: 0px; POSITION: absolute; LEFT: -100px; TOP: 0px; VISIBILITY: hidden"
		align="center"><TEXTAREA id="templateText3" rows="4" cols="70" name="templateText3"></TEXTAREA>
	</div>
    <table>
    <tr>
    <td>Hyperlink</td><td><asp:DropDownList ID="ddllink" runat="server" style='width:250px;'></asp:DropDownList></td> </tr>
    <tr><td>Image</td><td><span class="fileUpload">						
						<input type="file" id="upload" /> <span style="font-family:Verdana;font-size:11px;color:Red;">please upload image (90 * 45) </span>
                        <label class="error"></label>
					</span></td> </tr>
     
	
    </table>
	<div id="cropPreview" ></div>
	<script>	    if (topImagepath != '') { var _img = document.createElement('img'); _img.src = topImagepath; document.querySelector('#cropPreview').appendChild(_img); _img.onload = function () { parent.updateHeight(); } }</script>
	<div class="uploadPop">
		<i class="remove"></i>
		<div class="target" id="imgTarget">
			<img src="" id="target" />
		</div>
		<div class="actionBar">
			<asp:button id="btnSave" OnClick="btnSave_Click" Runat="server" CssClass="buttontext" Text="Save"
						CausesValidation="False"></asp:button>
			<input type="button" name="cancel" id="cancel" value="Cancel" />
		</div>
	</div>
	
</FORM>
	
	<div id="loader"></div>
	<div id="uploadOverlay"></div>

</div>
		
<script src="js/vendor/fabric.js"></script>
<script src="js/lib/js/darkroom.min.js"></script>
		<script>
		    var errTimer, _body = document.body, cropper;
		    window.saved = false; window.changed = false;
		    var err = document.querySelector('.fileUpload .error');
		    var getImage = function (e) {
		        var preview = document.querySelector('#imgTarget img');

		        preview.id = 'target';
		        var file = this.files[0];
		        var reader = new FileReader();
		        _body.classList.add('load');
		        reader.onabort = function () {
		            _body.classList.remove('load');
		        }
		        reader.onerror = function () {
		            _body.classList.remove('load');
		        }
		        reader.onloadend = function () {
		            var img = new Image();
		            img.onload = function () {
		                var w = img.width, h = img.height;
		                _body.classList.remove('load')
		              //  if (w > 300 && h > 280) {

		                    _body.className = 'crop';
		                    preview.src = reader.result;
		                    // cropper = new Darkroom('#target', { minWidth: 900, minHeight: 310, maxWidth: 2500, maxHeight: 1200, plugins: { crop: { quickCropKey: 67, minHeight: 310, minWidth: 900} }, init: function () { var cropPlugin = this.getPlugin('crop'); cropPlugin.selectZone(25, 15, 850, 310); }
		                    cropper = new Darkroom('#target', { minWidth: 200, minHeight: 310, maxWidth: 250, maxHeight: 600, plugins: { crop: { quickCropKey: 67, minHeight:45, minWidth:90} }, init: function () { var cropPlugin = this.getPlugin('crop'); cropPlugin.selectZone(25, 15,90, 45); }

		                    });
		                    parent.updateHeight();
		             //   } 
//                        else {
//		                    err.innerHTML = 'Please upload minimum width 900px and height 300px.';
//		                    err.classList.add('show');
//		                    errTimer = setTimeout(function () { err.classList.remove('show'); }, 4000)
//		                }
		            }
		            img.src = reader.result;

		        }
		        if (file) {
		            reader.readAsDataURL(file);
		        } else {
		            preview.src = "";
		        }
		    }
		    var cancel = function () {
		        _body.className = '';
		        cropper.selfDestroy();
		        parent.updateHeight();
		    }
		    var remove = document.querySelector('.uploadPop .remove'), _cancel = document.querySelector('#cancel'), _upload = document.querySelector('input[id$="btnSave"]');
		    remove.addEventListener('click', cancel, false);
		    err.addEventListener('click', function () { this.classList.remove('show'); }, false)
		    _cancel.addEventListener('click', cancel, false);
		    var showError = function (el, msg) {
		        var cErr = document.querySelector('.errorCrop');
		        cErr.innerHTML = msg;
		        var which = el == 'accept' ? document.querySelector('.darkroom-icon-accept') : document.querySelector('.darkroom-icon-save');
		        cErr.style.left = which.offsetLeft + 'px';
		        cErr.classList.add('show');
		        parent.scrollPage();
		        errTimer = setTimeout(function () { cErr.classList.remove('show'); }, 4000)
		    }
		    _upload.addEventListener('click4', function () {
		        !window.changed && showError('accept', 'Please do click here to crop your selection.');
		        window.changed && !window.saved && showError('save', 'Please do click here to save your cropped image.')

		        if (window.saved && window.changed) {
		            var source = document.querySelector('#imgTarget img').src;
		            var img = document.createElement('img');
		            img.src = source;
		            var p = document.querySelector('#cropPreview'); p.innerHTML = ''; p.appendChild(img);

		            _body.classList.remove('crop');
		        }
		    }, false);
		    window.addEventListener('image:change', function () { window.saved = false; window.changed = true; var cErr = document.querySelector('.errorCrop'); cErr.classList.remove('show'); parent.updateHeight(); clearTimeout(errTimer); })
		    window.addEventListener('crop:update', function () { window.saved = false; window.changed = false; var cErr = document.querySelector('.errorCrop'); cErr.classList.remove('show'); clearTimeout(errTimer); })
		    var upload = document.querySelector('#upload');
		    upload.addEventListener('change', getImage, false);

		    function validate() {
		        !window.changed && showError('accept', 'Please do click here to crop your selection.');
		        window.changed && !window.saved && showError('save', 'Please do click here to save your cropped image.')

		        if (window.saved && window.changed) {
		            var source = document.querySelector('#imgTarget img').src;
		            var img = document.createElement('img');
		            img.src = source;
		            var p = document.querySelector('#cropPreview'); p.innerHTML = ''; p.appendChild(img);
		            templateText3.innerHTML = source;
		            //_body.classList.remove('crop');
		            return true;
		        } else {
		            return false;
		        }
		        /*var strImg = $('#imgCasino').attr('src');
		        //"<%=_mstrImag64%>"=strImg;
		        templateText3.innerHTML = strImg;
		        //document.getElementById('#_mstrImag64').value = strImg;
		        return true;*/
		    }

		    function readURL(input) {
		        if (input.files && input.files[0]) {
		            //alert(input.files[0].size + " Mb");
		            if (input.files[0].size <= 1048576) {
		                var reader = new FileReader();
		                reader.onload = function (e) {
		                    $("#imgCasino")
								.attr('src', e.target.result)
								.width(200)
								.height(200);
		                };

		                reader.readAsDataURL(input.files[0]);
		            }
		            else {
		                $("#imgCasino").attr('src', '');
		                input.form.reset();
		                alert("Can not load image greater than size 1 Mb");
		            }
		        }
		    } 
		</script>
	</body>
	
</HTML>