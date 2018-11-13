
jQuery(document).ready(function ($) {
    new WOW().init();
    //$('.loader_overlay').addClass('loaded');

    /*Back to top*/
    $('.back-to-top').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 600);
        return false;
    });
    $("#owl-slider").owlCarousel({
        navigation: true, // Show next and prev buttons
        paginationSpeed: 400,
        pagination: true,
        singleItem: true,
        stopOnHover: true,
        transitionStyle: "fade",
        autoPlay: 5000,
        navigationText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
    });

    //$("#blog_index_list").owlCarousel({
    //    items: 4,
    //    lazyLoad: true,
    //    pagination: false,
    //    navigation: false,
    //    itemsDesktop: [1199, 4],
    //    itemsDesktopSmall: [979, 3],
    //    itemsTablet: [768, 2],
    //    itemsTabletSmall: [480, 1],
    //    itemsMobile: [360, 1],
    //    paginationSpeed: 400,
    //    stopOnHover: true,
    //    transitionStyle: "fade",
    //    loop: true,
    //    autoPlay: 4000,
    //    navigationText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
    //});
    //$("#brand_owl").owlCarousel({
    //    items: 6,
    //    lazyLoad: true,
    //    pagination: false,
    //    navigation: false,
    //    itemsDesktop: [1199, 6],
    //    itemsDesktopSmall: [979, 4],
    //    itemsTablet: [768, 3],
    //    itemsTabletSmall: [480, 2],
    //    itemsMobile: [360, 1],
    //    paginationSpeed: 400,
    //    stopOnHover: true,
    //    transitionStyle: "fade",
    //    loop: true,
    //    autoPlay: 4000,
    //    navigationText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
    //});

    /*contact_email change*/
    $('.newsletter_wrap #contact_email').change(function () {
        $(this).toggleClass("not-empty", "" != $(this).val())
    });
	/*show cartpopup
	$('.cart_header a').click(function(){
		getCartAjax();
		$('#cart_popup').modal('show');
		$('.modal-backdrop').css({'height':$(document).height()});
	});*/

    /*Menu mobile*/
    $("#trigger_click_mobile").click(function (e) {
        e.preventDefault();
        $("#mobile_wrap_menu").toggleClass("show");
        $('#opacity').addClass("opacity_body");
        $('body').addClass("overflow_hidden");
    });
    $('#opacity, .close_menu').click(function () {
        $("#mobile_wrap_menu").removeClass("show");
        $('#opacity').removeClass("opacity_body");
        $('body').removeClass("overflow_hidden");
    });
    $(".more").on("click", function () {
        $("i", this).toggleClass("fa-plus fa-minus");
    });
    $('.ajax_qty .btn_plus').click(function () {
        var variant_id = $(this).data('id');
        plus_quantity($(this), variant_id);
    });
    $('.ajax_qty .btn_minus').click(function () {
        var variant_id = $(this).data('id');
        minus_quantity($(this), variant_id);
    });
});

var heightHeader = $('#header').height();
$('#wr_header').height(heightHeader);

$(window).resize(function () {
    heightHeader = $('#header').height();
    $('#wr_header').height(heightHeader);
});

jQuery(window).scroll(function () {
    if (jQuery(this).scrollTop() > 300) {
        jQuery('.back-to-top').fadeIn();
    } else {
        jQuery('.back-to-top').fadeOut();
    }

    if (jQuery(this).scrollTop() > heightHeader + 200) {
        $('#header').addClass('header_fixed');
    } else {
        $('#header').removeClass('header_fixed');
    }
});

$(function () {

    $.fn.osgslide = function (smallThumb, largeThumb, movepx, counttab, countimage) {
        var objId = $(this).attr('id');
        var tabindex = 1;
        var imageindex = 0;
        //Sự kiện next ảnh
        $('#' + objId + ' .slide-next').click(function () {
            if ($(this).hasClass('noneclick')) {
                tabindex = 0;
                imageindex = -1;
                $(this).removeClass('noneclick');
                //return;
            }
            $('#' + objId + ' .slide-prev').removeClass('noneclick');
            imageindex++;
            if ((countimage > counttab) && (tabindex <= (countimage - counttab))) {
                tabindex++;
                var toppx = (tabindex - 1) * (-movepx);
                $('#' + objId + ' .content-listthumb').animate({
                    left: toppx
                }, 500);
            }
            ReplaceImage(imageindex);
            CountImage(imageindex + 1);
            if (imageindex == countimage - 1) {
                $(this).addClass('noneclick');
            }
            else if (imageindex == 0) {
                $('#' + objId + ' .slide-prev').addClass('noneclick');
            }
        });

        //Sự kiện back ảnh
        $('#' + objId + ' .slide-prev').click(function () {
            var countImg = $('#' + objId + ' .list-thumb .osgslideimg').length;
            if ($(this).hasClass('noneclick')) {
                tabindex = countImg - (counttab - 2);
                imageindex = countImg;
                $(this).removeClass('noneclick');
                //return;
            }
            $('#' + objId + ' .slide-next').removeClass('noneclick');
            imageindex--;
            if ((countimage > counttab) && tabindex > 1) {
                tabindex--;
                var toppx = (tabindex - 1) * (-movepx);
                $('#' + objId + ' .content-listthumb').animate({
                    left: toppx
                }, 500);
            }
            ReplaceImage(imageindex);
            CountImage(imageindex + 1);
            if (imageindex == 0) {
                $(this).addClass('noneclick');
            }
            else if (imageindex == countImg - 1) {
                $('#' + objId + ' .slide-next').addClass('noneclick');
            }
        });

        //Sự kiện khi click vào ảnh nhỏ
        $('#' + objId + ' .osgslideimg img').click(function () {
            var currentimage = $(this).parent().index()
            ReplaceImage(currentimage);
            currentimage++;
            CountImage(currentimage);
            $('#' + objId + ' .slide-prev').removeClass('noneclick');
            $('#' + objId + ' .slide-next').removeClass('noneclick');
            if (currentimage == 1) {
                $('#' + objId + ' .slide-prev').addClass('noneclick');
            } else if (currentimage == $('#' + objId + ' .list-thumb .osgslideimg').length) {
                $('#' + objId + ' .slide-next').addClass('noneclick');
            }
        });

        //Sự kiện khi click vào ảnh lớn
        $('#' + objId + ' .show-img img').click(function () {
            var rel = $(this).attr('rel');
            $('#' + objId + ' #' + rel).click();
        });

        //Func thay thế ảnh nhỏ => ảnh lớn
        function ReplaceImage(currentimage) {
            imageindex = currentimage;
            var src = $('#' + objId + ' .osgslideimg:eq(' + currentimage + ') img').attr('src');
            var rel = $('#' + objId + ' .osgslideimg:eq(' + currentimage + ') img').attr('rel');
            var attr = $('#' + objId + ' .osgslideimg:eq(' + currentimage + ') img').attr('class');
            var cls = "";
            if (typeof attr !== typeof undefined && attr !== false) {
                var cls = attr;
            }

            $('#' + objId + ' .show-img img')
                .attr('src', src.replace(smallThumb, largeThumb))
                .attr('rel', rel)
                .attr('class', cls);

            $('#' + objId + ' .osgslideimg').removeClass('osgslide-active');
            $('#' + objId + ' .osgslideimg:eq(' + currentimage + ')').addClass('osgslide-active');
        }

        function CountImage(currentimage) {
            $('#countimage').html(currentimage);
        }

    };

    if ($('#osgslide').length == 1) {
        $("#aniimated-thumbnials").lightGallery({ showThumbByDefault: false });
        $('#osgslide').osgslide($("#hddSmallThumb").val(), $("#hddLargeThumb").val(), 82.5, 4, eval($("#hddCountImage").val()));
    }
    else if ($('#detail_one_image').length == 1) {
        $("#detail_one_image").lightGallery();
    }

    if ($('#map_contact').length > 0)
        initMap('#map_contact');

    product.init();
});

function initMap(element) {
    var lat = $("#hdX").val().replace(',', '.');
    var lon = $("#hdY").val().replace(',', '.');
    $(element).html('<iframe width="100%" height="100%" frameborder= "0" style= "border:0" src="https://www.google.com/maps/embed/v1/place?q=' + lat + ',' + lon + '&key=AIzaSyAT1NB-OnXJdTbW7Gx5M7lH57DH7JbTxKA' + '&zoom=14" allowfullscreen></iframe>');
}

function Arrange(key, arrangeId) {
    //document.cookie = "ArrangeProduct=" + arrangeId + ";path=/;";
    $.cookie(key, arrangeId, { path: '/', expires: 7 });
    var url = location.href;
    url = url.replace(new RegExp("/p([0-9]+)"), "");//bo phan trang di ve trang 1
    location.href = url;
}

var product = {
    init: function () {
        //product.switchInterface();
        if ($('.product_list').length > 0) {
            setTimeout(function () {
                product.setHeightItem('.product_list .product-wrapper', 4, '.title');
            }, 300);
        }
    },

    switchInterface: function () {
        $('.icon-switch-interface span').on('click', function () {
            $('.icon-switch-interface span.active').removeClass('active');
            $(this).addClass('active');

            var layoutId = $(this).data('id');
            if (layoutId == '1') {
                $('#product_list').addClass('large-col');
                product.setHeightItem('#product_list.large-col > li', 3);
            }
            else {
                $('#product_list').removeClass('large-col');
                $('#product_list .title, #product_list .function-list').removeAttr('style');
            }
            $.cookie('LayoutType', layoutId, { path: '/', expires: 3 });
        });
    },
    setHeightItem: function (item, column, objFixHeight) {
        for (var i = 0; i < $(item).length; i += column) {
            var maxHeight = 0;
            var lstItem = '';
            for (var j = 0; j < column; j++) {
                if (j != 0)
                    lstItem += ', ';
                var elementId = (item + ':eq(' + (i + j) + ')');
                lstItem += elementId;

                if ($(elementId).find(objFixHeight).height() > maxHeight) {
                    maxHeight = $(elementId).find(objFixHeight).height();
                }
            }

            //$(lstItem).find(objFixHeight).height(maxHeight);
            // check if object < maxHeight
            $(lstItem).find(objFixHeight).each(function () {
                if ($(this).height() < maxHeight)
                    $(this).height(maxHeight);
            });
        }
    }
};