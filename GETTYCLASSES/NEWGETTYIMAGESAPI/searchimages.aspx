<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="searchimages.aspx.cs" Inherits="newgettyimagesAPI.searchimages" %>
<asp:Content ID="indexconten" runat="server" ContentPlaceHolderID="mainmastercontent">
   <div class="overlay">
       <span></span>
   </div>
   
    <div class="serchtxt sernew">
                <div class="leftmenuwrap"><span class="leftmenu">Filter and sort <em></em></span>
                       <div class="dvfilter">
 <ul>
    <li class="Sort">
        <span class="listhead">Sort by</span>
        <div class="refinement-radiobutton"><input id="sort_best" name="sort" type="radio" value="best_match"><label class="refinement-label" for="sort_best">Best match</label></div>
        <div class="refinement-radiobutton"><input id="sort_Newest" name="sort" type="radio" checked="checked" value="newest"><label class="refinement-label" for="sort_Newest">Newest</label></div>
            <div class="refinement-radiobutton"><input id="sort_oldest" name="sort" type="radio" value="oldest"><label class="refinement-label" for="sort_oldest">Oldest</label></div>
           <div class="refinement-radiobutton"><input id="sort_popular" name="sort" type="radio" value="most_popular"><label class="refinement-label" for="sort_popular">Most popular</label></div>
        

    </li>
    <li class="Orientation">
        <span class="listhead">Orientation</span>
       <div class="refinement-checkbox"><input type="checkbox"  id="orientations_horizontal" value="Horizontal" checked="checked"><label for="orientations_horizontal">Horizontal</label></div>
       <div class="refinement-checkbox"><input type="checkbox" id="orientations_vertical" value="Vertical" ><label for="orientations_vertical">Vertical</label></div>
       <div class="refinement-checkbox"><input type="checkbox" id="orientations_panoramichorizontal" value="PanoramicHorizontal"><label for="orientations_panoramichorizontal">Panoramic horizontal</label></div>
       <div class="refinement-checkbox"><input type="checkbox" id="orientations_Panoramicvertical" value="PanoramicVertical"><label for="orientations_Panoramicvertical">Panoramic vertical</label></div>
       <div class="refinement-checkbox"><input type="checkbox" id="orientations_Square" value="Square"><label for="orientations_Square">Square</label></div>
        
    </li>
    <li class="Datel">
        <span class="listhead">Date</span>
        <select>
            <option value="0" selected="selected" >Today's date</option>
            <option value="24">Last 24 hours</option>
            <option  value="48">Last 48 hours</option>
            <option value="72" >Last 72 hours</option>
            <option value="7-days">Last 7 days</option>
            <option value="30-days">Last 30 days</option>
            <option value="12-months">Last 12 months</option>
        </select>
    </li>
 </ul>
   </div>
                </div>
        <span><asp:TextBox ID="txtsear" runat="server" placeholder="Search images"></asp:TextBox>   </span> 
        <span><input name="search" type="button"  id="search"   value="Search images" />  </span> 
         <span><input name="btndownload" type="button"  id="btndownload"     value="Download images(0)" />  </span>
    </div> 
   <div class="mainwrap">

    <div class="midwrp">
   
	<ul class="innersec">
			<%=strtext.ToString()%>
    </ul>
  <div id="UpdateProgress1" style="display: none;"  >
<span class="loader"></span>
</div>

   <!--<div class="loadermore">
        <img src="<%=NewGettyAPIclasses.ConstantAPI.siteURL%>images/loading87.gif" />
    </div>-->
</div></div>
    <script type="text/javascript" >
        var ajaxurl = "<%=NewGettyAPIclasses.ConstantAPI.siteURL%>imageajax.aspx";
        var pageInd = 1;
        var multiple = "<%=strmultiple%>";
        var networkid = "<%=strnetwork%>";
        var siteId = "<%=intsiteId%>";
        var strliveup = "<%=strliveupdate%>";
        var arr = new Array();
        var filterJSON;
        $("#search").on('click', searchGetty);
        function searchGetty() {
            pageInd = 1;
            $("#UpdateProgress1").show();
            var str = $("#<%=txtsear.ClientID%>").val();
            $.ajax({
                type: "POST",
                url: ajaxurl,
                cache: false,
                data: { type: "Getserchimages", search: str, page: pageInd, _muptilpe: multiple,sort:filterJSON.sort,orient:filterJSON.orientation,date:filterJSON.date},
                success: function (msg) {
                    if (msg != "") {
                        var accepted;
                        $(".innersec").html(msg)
                        if (multiple === "Y") {
                            //console.log($(".downdetail ul input[type=checkbox]").length);
                        }
                        $("#UpdateProgress1").hide();
                        pageInd++;
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });
        }

        $("#ldmore").click(function () {
            $("#UpdateProgress1").show();
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
                        $("#UpdateProgress1").hide();
                        pageInd++;
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });

        });

        function getdownload(ID) {
            $("#UpdateProgress1").show();
            $.ajax({
                type: "POST",
                url: ajaxurl,
                cache: false,
                data: { type: "Downloadimage", ID: ID },
                success: function (msg) {
                    if (msg != "") {
                        var accepted;
                        alert(msg);
                        $("#UpdateProgress1").hide();
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });
        }


        var track_page = 1; //track user scroll as page number, right now page number is 1
        var loading = false; //prevents multiple loads
        $(window).scroll(function () {
            console.log(track_page);
            //detect page scroll
            if ($(window).scrollTop() + $(window).height() >= $(document).height()) {
                //if user scrolled to bottom of the page
                track_page++; //page number increment
                //alert("End Of The Page");
                load_contents(track_page); //load content   
            }
        });
        //Ajax load function
        function load_contents(track_page) {
            if (loading == false) {
                loading = true;  //set loading flag on
                console.log("track_page" + track_page);
                $("#UpdateProgress1").show();
                $(".overlay").show();
                $(".overlay span").html("<p>Loading more images.</p>")
                var str = $("#<%=txtsear.ClientID%>").val();
				if (pageInd == 1)
				    pageInd = 2;

				$.ajax({
				    type: "POST",
				    url: ajaxurl,
				    cache: false,
				    data: { type: "Getserchimages", search: str, page: pageInd, _muptilpe: multiple, sort: filterJSON.sort, orient: filterJSON.orientation, date: filterJSON.date },
				    success: function (msg) {
				        if (msg != "") {
				            var accepted;
				            console.log("innerstr" + str);
				            $(".innersec").append(msg)
				            $("#UpdateProgress1").hide();
				            $(".overlay").hide();
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


        $("#btndownload").click(function () {
            if (arr.length > 0) {

                var jsstr = "{";
                var t = 1;
                for (var i = 0; i < arr.length; i++) {
                    var strcaption = $("#cap" + arr[i]).text();
                    console.log("strcaption" + arr[i] + "---" + strcaption)
                    jsstr += "\"image" + t + "\":{"
                    jsstr += "\"ID\":\"" + arr[i] + "\",\"caption\":\"" + strcaption + "\"";
                    if (t < arr.length)
                        jsstr += "},";
                    else
                        jsstr += "}";

                    t++;
                }
                jsstr += "}"
                $("#UpdateProgress1").show();
                $(".overlay").show();
                $(".overlay span").html("<p>Downloading images from Getty On the next screen you will be able to edit titles and photo credits.</p>")
                $.ajax({
                    type: "POST",
                    url: ajaxurl,
                    cache: false,
                    data: { type: "Download_multiple_image_Forsite", ID: arr.toString(), SiteID: siteId, NetworkID: networkid, Strjson: jsstr },
                    success: function (msg) {
                        if (msg != "") {
                            // $(window.opener).find('#rsjson').val(msg);
                            //window.opener.receiveDataFromPopup(msg);
                            jsstr = "";
                            console.log(msg)
                            window.opener.postMessage(['getty', msg], "*");
                            $("#UpdateProgress1").hide();
                            window.close();
                        }
                    },
                    error: function (request, err) {
                        console.log("error");
                    }
                });
            }
            else {

                alert("Please select images!")
            }
        })
        function getImagecheck(ID) {
            var appval = "";
            if ($(ID).is(":checked")) {
                $(".serchtxt span input[type=button][id^='btndownload']").show();
                $(".serchtxt span input[type=button][id^='btndownload']").attr("Data-item")
                arr.push($(ID).val())
            } else {
                if (arr.length > 0) {
                    console.log(arr.indexOf($(ID).val()));
                    if (arr.indexOf($(ID).val()) > -1) {
                        arr = arr.filter(function (elem) {
                            return elem != $(ID).val();
                        });
                    } else { arr = new Array() }
                }
            }
            console.log(arr.length);
            if (arr.length > 0) {
                for (i = 0; i < arr.length; i++) {
                    console.log(arr[i]);
                }
            } else { arr = new Array() }

            $(".serchtxt span input[type=button][id^='btndownload']").val("Download images(" + arr.length + ")")
        }

        function getdownloadfornetwork(ID) {
            window.parent.postMessage(['getty', 'test'], "*");
            var strcaption = $("#cap" + ID.value).text();
            $("#UpdateProgress1").show();
            $(".overlay").show();
            $.ajax({
                type: "POST",
                url: ajaxurl,
                cache: false,
                data: { type: "Downloadimage_Forsite", ID: ID.value, SiteID: siteId, NetworkID: networkid, caption: strcaption,liveup:strliveup },
                success: function (msg) {
                    if (msg != "") {
                        // $(window.opener).find('#rsjson').val(msg);
                        //window.opener.receiveDataFromPopup(msg);
                        //console.log(msg)
                        window.opener.postMessage(['getty', msg], "*");
                        $("#UpdateProgress1").hide();
                        window.close();
                    }
                },
                error: function (request, err) {
                    console.log("error");
                }
            });
        }
        function getimagesave(ID) {
            console.log("got id" + ID.value)
            var data = getdownloadfornetwork(ID.value);



        }

        
        var filter = $('.dvfilter'), init = false,mapf = { Sort: 'sort', Orientation: 'orientation', Datel: 'date' }
        $('input, select', filter).on('change', generateJSON)
        function generateJSON(e) {
            var el = this, obj = { "sort": "newest", "orientation": [], "date": 0 },selected = $('input:checked,select', filter);                        
            selected.each(function () {
                var parent = $(this).parents('li')[0].className,val = this.value,node = obj[mapf[parent]];                
                if (Array.isArray(node))
                    node.push(val)
                else
                    obj[mapf[parent]] = val
            });
            
            filterJSON = obj;
            filterJSON.orientation = filterJSON.orientation.join(',');
            init && searchGetty()
            init =true;
        }
        generateJSON();

        console.log("filterJSON" + filterJSON);
    </script> 



<script>
$(".serchtxt.sernew span.leftmenu").click(function(){
        $(".serchtxt.sernew span.leftmenu").toggleClass("show");
        $(".leftmenuwrap .dvfilter").toggleClass("show");

    })
</script>
</asp:Content>    
