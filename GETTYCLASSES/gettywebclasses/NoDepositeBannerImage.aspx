<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoDepositeBannerImage.aspx.cs" Inherits="gettywebclasses.NoDepositeBannerImage" %>

<HTML>
	<HEAD>
		<title>Uplaod Image</title>		
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

		    function closePOP(imgname) {
		        parent.postMessage(imgname, '*');
		    }

		    var topImagepath = '<%=topImagepath %>';
		</script>
			
				<style>
			SPAN[data-set]:before { COLOR: #ccc; MARGIN: 0px 10px; DISPLAY: inline-block }
			TABLE ~ SPAN[data-set] { MARGIN-BOTTOM: 10px; PADDING-LEFT: 0px; DISPLAY: inline-block }
			TABLE ~ SPAN[data-set]:before { PADDING-LEFT: 20px; DISPLAY: block }
			.addlive { CURSOR: hand }
			TD TABLE TR TD SELECT { WIDTH: 200px }
			
			htm, body{margin:0; padding:0; background:none;}
.container{max-width:1300px; margin:0 auto; padding-top:20px; text-align:center;}
.fileUpload{display:inline-block;padding:20px 20px 20px 42px;background: #0192DD url(<%=baseurl%>/images/camera.png) no-repeat 20px 19px; color:#fff; min-width:60px; position:relative; cursor:pointer; font-family:Arial, Helvetica, sans-serif; font-size:14px; text-transform:uppercase;}
.fileUpload::before{content:'Upload Image'}
.fileUpload input[type="file"]{opacity:0; position:absolute; left:0; top:0; height:56px;}
.fileUpload .error{padding:7px; background:#F5CAD6; box-shadow: 0 0 3px rgba(0, 0, 0, 0.4); border: 1px solid #FF94B2; color:#900; font:12px Arial, Helvetica, sans-serif; position:absolute; bottom:0; left:50%; -webkit-transform:translateX(-50%);-moz-transform:translateX(-50%);transform:translateX(-50%); opacity:0; visibility:hidden;transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s; min-width:180px; text-transform:none;}
.errorCrop{padding:7px; background:#F5CAD6; box-shadow: 0 0 3px rgba(0, 0, 0, 0.4); border: 1px solid #FF94B2; color:#900; font:12px Arial, Helvetica, sans-serif; position:absolute; bottom:0; left:50%; -webkit-transform:translateX(-50%);-moz-transform:translateX(-50%);transform:translateX(-50%); opacity:0; visibility:hidden;transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s; min-width:180px; text-transform:none;}
.fileUpload .error::after{content:''; width:0; height:0; position: absolute;bottom: -8px;left: 20%; border-top: 7px solid #FF94B2;border-left: -3px solid transparent;border-right: 13px solid rgba(0, 0, 0, 0); }
.errorCrop::after{content:''; width:0; height:0; position: absolute;bottom: -10px;left: 50%; border-top: 10px solid #FF94B2;border-left: 7px solid transparent;border-right: 7px solid rgba(0, 0, 0, 0);}
.fileUpload .error.show, .errorCrop.show{opacity:1; visibility:visible; bottom:35px;}
.errorCrop.show{bottom:42px;}
.uploadPop{max-width:1300px; min-width:850px; margin-top:0; position:relative; background:#fff; color: #808080;font-size: 14px;-webkit-user-select: none;-khtml-user-select: none;-moz-user-select: none;-ms-user-select: none;user-select: none;cursor: default; visibility:hidden; opacity:0;transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s;z-index:99; height:700px;overflow-y:auto;}
#uploadOverlay{width:100%; height:100%; position:fixed; top:0; left:0; background:rgba(255,255,255,0.5);transition: all 0.3s; -webkit-transition: all 0.3s;-moz-transition: all 0.3s; visibility:hidden; opacity:0; z-index:98;}
.uploadPop #imgTarget{text-align:center;}
.uploadPop #imgTarget img{}
.uploadPop .darkroom-image-container{display:inline-block; overflow:auto;}
.uploadPop i.remove{width: 24px;height: 24px;position: absolute;top: 5px;right: -5px;cursor: pointer; font-style:normal; font-size:18px;}
.uploadPop i.remove::before{content:'\2715';}
body.crop .uploadPop{visibility:visible; opacity:1;}
#cropPreview{margin-top:50px; position:absolute; left:50%; transform:translateX(-50%);-webkit-transform:translateX(-50%);-moz-transform:translateX(-50%);}
body.crop #uploadOverlay{visibility:visible; opacity:1;}
body.crop .fileUpload{display:none;}
.actionBar{padding:0 10px 15px; text-align:center;}
.actionBar input[id$="btnSave"]{display:inline-block;padding: 10px 15px;background: #0192DD; color:#fff; min-width:60px; position:relative; cursor:pointer; font-family:Arial, Helvetica, sans-serif; font-size:14px; text-transform:uppercase; border:0;}
.btnsaveimg{display:inline-block;padding: 10px 15px;background: #0192DD; color:#fff; min-width:60px; position:relative; cursor:pointer; font-family:Arial, Helvetica, sans-serif; font-size:14px; text-transform:uppercase; border:0;}
.actionBar #cancel{display:inline-block;padding: 10px 15px;background: #555; color:#fff; min-width:60px; position:relative; cursor:pointer; font-family:Arial, Helvetica, sans-serif; font-size:14px; text-transform:uppercase;border:0;}
#loader {position: fixed;top:50%;left: 50%;width: 50px;height: 50px;-webkit-transition: -webkit-transform 0.3s;transition: transform 0.3s;pointer-events: none; display:none; top:50%; left:50%;  -webkit-transform: translateX(-50%) translateY(-50%);
-moz-transform: translateX(-50%) translateY(-50%);
-ms-transform: translateX(-50%) translateY(-50%);
transform: translateX(-50%) translateY(-50%); z-index:9999; border-radius:50%; background:#fff; border:5px solid #fff;}
.darkroom-toolbar{display:none;}
body.load #loader{display:block;}
body.load #loader::before,#loader.loading::after {position: absolute;left:0; top:0;display: block;border: 5px solid rgba(0,0,0,0.1);border-radius: 50%;content: '';}
body.load #loader::before {width: 40px;height: 40px;border-right-color:rgba(22, 145, 190, 0.8);border-left-color:rgba(22, 145, 190, 0.8);-webkit-animation: rotation 3s linear infinite;animation: rotation 3s linear infinite;}
body.load #loader::after {margin-left: -30px;width: 20px;height: 20px;border-top-color:rgba(22, 145, 190, 0.8);border-bottom-color: rgba(22, 145, 190, 0.8);-webkit-animation: rotation 1s linear infinite;animation: rotation 1s linear infinite;}
.coords{display:block; text-align:center; font-family:Verdana; font-size:12px; padding-top:5px;}
.coords label{font-size:14px; font-family:Arial; font-weight:bold;}

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
    
	<span class="fileUpload">
						<label class="error"></label>
						<input type="file" id="upload" />
					</span>
	<div id="cropPreview" ></div>
	<script>	    if (topImagepath != '') { var _img = document.createElement('img'); _img.src = topImagepath; document.querySelector('#cropPreview').appendChild(_img); _img.onload = function () { /*parent.updateHeight(); */ } }</script>
	<div class="uploadPop">
        <div class="actionBar">
			<asp:button id="btnSave" OnClick="btnSave_Click" Runat="server" CssClass="buttontext" Text="Save"
						CausesValidation="False" style="display:none"></asp:button>
            <input type="button" name="dummySave" id="dummySave" value="Save"   class="btnsaveimg" />
			<input type="button" name="cancel" id="cancel" value="Cancel" />
            <span class="coords">Crop current width and height : <label id="coords"></label></span>
		</div>
		<i class="remove" style='display:none;'></i>
		<div class="target" id="imgTarget">
			<img src="" id="target" />
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
		    var imgBasket = document.querySelector('#templateText3');
		    var err = document.querySelector('.fileUpload .error');
		    var getImage = function (e) {
		        var preview = document.querySelector('#imgTarget img');
		        preview.id = 'target';
		        var file = this.files[0];
		        this.value = '';
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
		                if (w > 356 && h > 225) {
		                    var wh = h / w;
		                    h = w > 1300 ? 1300 * wh : h;
		                    w = w > 1300 ? 1300 : w;
		                    var pw = w < 600 ? w - 20 : 600, ph = pw * .67;
		                    var al = (w - pw) / 2;
		                    var at = (h - ph) / 2;
		                    console.log(pw, al, at)
		                    //var arr = [pw, ph];
		                    _body.className = 'crop';
		                    preview.src = reader.result;
		                    // cropper = new Darkroom('#target', { minWidth: 900, minHeight: 310, maxWidth: 2500, maxHeight: 1200, plugins: { crop: { quickCropKey: 67, minHeight: 310, minWidth: 900} }, init: function () { var cropPlugin = this.getPlugin('crop'); cropPlugin.selectZone(25, 15, 850, 310); }
		                    //  cropper = new Darkroom('#target', { minWidth: 300, minHeight: 201, maxWidth: 1300, maxHeight: h, plugins: { crop: { quickCropKey: 67, minHeight: 201, minWidth: 300} }, init: function () { var cropPlugin = this.getPlugin('crop'); cropPlugin.selectZone(al, at, pw, ph); }

		                    cropper = new Darkroom('#target', { minWidth: 800, minHeight: 310, maxWidth: 1300, maxHeight: 600, plugins: { crop: { quickCropKey: 67, minHeight:225 , minWidth:356 } }, init: function () { var cropPlugin = this.getPlugin('crop'); cropPlugin.selectZone(25, 15, 356, 225); }
		                    });
		                    //parent.updateHeight();
		                } else {

		                    _body.className = 'crop';
		                    preview.src = reader.result;

		                    cropper = new Darkroom('#target', { minWidth: w, minHeight: h, maxWidth: 1300, maxHeight: 600, plugins: { crop: { quickCropKey: 67, minHeight: h, minWidth: w} }, init: function () { var cropPlugin = this.getPlugin('crop'); cropPlugin.selectZone(25, 15, w, h); }
		                    });

		                    /*
		                    err.innerHTML = 'Please upload minimum width 900px and height 300px.';
		                    err.classList.add('show');
		                    errTimer = setTimeout(function () { err.classList.remove('show'); }, 4000)
		                    */

		                }
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
		    var remove = document.querySelector('.uploadPop .remove'), _cancel = document.querySelector('#cancel'), _upload = document.querySelector('input[id$="btnSave"]'), _dummy = document.querySelector('#dummySave');
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
		    _dummy.addEventListener('click', function (e) {
		        //this.darkroom.selfDestroy(); // Cleanup
		        //var newImage = dkrm.canvas.toDataURL();
		        e.preventDefault();
		        //cropper.selfDestroy();

		        console.log(cropper)
		        cropper.plugins.crop.okButton.element.click();
		        _body.classList.add('load');
		        //cropper.selfDestroy();
		        return false;


		    }, false);
		    var hTimer, coords = document.querySelector('#coords');
		    function updateHeightWidth() {
		        var cz = cropper.canvas._activeObject;
		        coords.innerHTML = cz.currentWidth.toFixed() + " x " + cz.currentHeight.toFixed();
		        //console.log(cz.currentWidth)
		    }

		    window.addEventListener('image:change', function () { cropper.selfDestroy(); var img = cropper.canvas.toDataURL(); imgBasket.innerHTML = img; _body.classList.remove('load'); document.forms[0].btnSave.click(); clearTimeout(errTimer); })
		    window.addEventListener('crop:update', function () { clearTimeout(hTimer); hTimer = setTimeout(updateHeightWidth, 400); })
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