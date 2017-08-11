

// JavaScript Document
/****************************************************************
"deepPop Javascript image popup" - Version 1
License: http://www.invitratech.com
Author: Suryakant Patil
Copyright (c) 2010 Suryakant Patil - suryakant.patil77@gmail.com, suryakant@invitratech.com

Last update: 2012-28-05
*****************************************************************/

/*
User Refrence
		wrapper:'.gallery', ---Wrapper Name
		width:  400, --Image Width
		height: 400, -- Image Height
		zIndexActive: 999,
		topOffset: 20, --Offset top 
		leftOffeset:20, -- offset left
		path:'' --Popup image path should be "self", "a", or ""
*/

(function ($) {

    $.fn.deepPop = function (options) {
        var auto;
        var opts = $.extend({}, $.fn.deepPop.defaults, options);
        var imgPop = $("<div class='imgPop' />").css({ zIndex: opts.zIndexActive, position: 'absolute', display: 'none', borderRadius: 10, padding: 15, maxWidth: 600 });
        imgPop.append("<img id='loader' src='images/fb-load.gif' style='margin-top:50px' />");
        var popPeople = $("<img>").addClass('popThumb').css({ display: 'none' });
        var t = $("<h4></h4>");
        var d = $("<p class='desc'></p>");
        var a = $("<span class='author'></span>");
        var dt = $("<div class='dttext'></div>")
        console.log(opts.wrapper);
        $wrapper = $('[id$="' + opts.wrapper + '"]');
        imgPop.append(popPeople);
        $wrapper.css('position', 'relative').append(imgPop);
        var data = $('<div class="data" />')
        imgPop.append(data);
        console.log('init');
        this.each(function () {

            //var uRl = this.deepPop();

            var $this = $(this);
            var uRl;

            uRl = $this.parent().attr('href');




            var popel = popPeople;
            var hold = $wrapper.width();
            var holdHt = $wrapper.height();

            $this.mouseover(function () {
                //alert('up');
                $('.data').html("");
                var data = $this.parent().parent().parent();
                $('#loader').show();
                t.text(data.find('.title-wrap strong').text());
                dt.html(data.find('.dttxt').html());
                d.text(data.find('p').attr('rel'));
                a.html(data.find('.photographer').html());
                $('.data').append(t, dt, d, a);

                var ltpos = $this.parents('.imgdiv').position().left;
                var rtpos = $this.parents('.imgdiv').position().right;

                var tppos = $this.parents('.imgdiv').position().top;
                if (ltpos) {
                    ltpos = ltpos - opts.leftOffset;
                    tppos = tppos - opts.topOffset;
                }

                if (tppos == 0) {
                    tppos = tppos - opts.topOffset;
                }
                if (tppos >= holdHt - opts.height) {
                    tppos = "";
                    imgPop.css({ bottom: 0 });
                } else {
                    imgPop.css({ bottom: 'inherit' });
                }
                if (ltpos >= hold - 550) {
                    imgPop.css({ right: 0 });
                    ltpos = "";
                }
                popel.attr('src', uRl).load(function () { $('#loader').hide(); $(this).fadeIn() });

                //imgPop.css({top:tppos, left:ltpos}).fadeIn('fast', function(){return false;});
                //imgPop.mouseout(function(){$(this).hide()});

                imgPop.css({ top: tppos, left: ltpos }).fadeIn('fast');

            });
            $('.imgPop').mouseleave(function () { $(this).hide(); $('.data').html(" "); $('.popThumb, #loader').hide(); });

        });

    }
    $.fn.deepPop.defaults = {
        wrapper: 'UpdatePanel1',
        width: 460,
        height: 100,
        zIndexActive: 999,
        topOffset: 20,
        leftOffset: 20,
        path: ''
    };
})(jQuery)