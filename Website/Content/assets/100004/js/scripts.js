jQuery(document).ready(function($){
	new WOW().init();
	$('.loader_overlay').addClass('loaded');

	/*Back to top*/
	$('.back-to-top').click(function() {
		$('html, body').animate({
			scrollTop: 0
		}, 600);
		return false;
	});
	$("#owl-slider").owlCarousel({
		navigation : true, // Show next and prev buttons
		paginationSpeed : 400,
		pagination : true,
		singleItem:true,
		stopOnHover: true,
		transitionStyle : "fade",
		autoPlay: 5000,
		navigationText : ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
	});
	$("#blog_index_list").owlCarousel({
		items : 4,
		lazyLoad : true,
		pagination : false,
		navigation : false,
		itemsDesktop: [1199, 4],
		itemsDesktopSmall: [979, 3],
		itemsTablet: [768,2],
		itemsTabletSmall: [480, 1],
		itemsMobile: [360, 1],
		paginationSpeed : 400,
		stopOnHover: true,
		transitionStyle : "fade",
		loop: true,
		autoPlay: 4000,
		navigationText : ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
	});
	$("#brand_owl").owlCarousel({
		items : 6,
		lazyLoad : true,
		pagination : false,
		navigation : false,
		itemsDesktop: [1199, 6],
		itemsDesktopSmall: [979, 4],
		itemsTablet: [768,3],
		itemsTabletSmall: [480, 2],
		itemsMobile: [360, 1],
		paginationSpeed : 400,
		stopOnHover: true,
		transitionStyle : "fade",
		loop: true,
		autoPlay: 4000,
		navigationText : ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
	});
	/*contact_email change*/
	$('.newsletter_wrap #contact_email').change(function(){
		$(this).toggleClass("not-empty", "" != $(this).val())
	});
	/*show cartpopup
	$('.cart_header a').click(function(){
		getCartAjax();
		$('#cart_popup').modal('show');
		$('.modal-backdrop').css({'height':$(document).height()});
	});*/

	/*Menu mobile*/
	$("#trigger_click_mobile").click(function(e){
		e.preventDefault();
		$("#mobile_wrap_menu").toggleClass("show");
		$('#opacity').addClass("opacity_body");
		$('body').addClass("overflow_hidden");
	});
	$('#opacity, .close_menu').click(function(){
		$("#mobile_wrap_menu").removeClass("show");
		$('#opacity').removeClass("opacity_body");
		$('body').removeClass("overflow_hidden");
	});
	$(".more").on("click", function() {
		$("i", this).toggleClass("fa-plus fa-minus");
	});
	$('.ajax_qty .btn_plus').click(function(){
		var variant_id = $(this).data('id');
		plus_quantity($(this),variant_id);
	});
	$('.ajax_qty .btn_minus').click(function(){
		var variant_id = $(this).data('id');
		minus_quantity($(this),variant_id);
	});
});
jQuery(window).scroll(function() {
	if (jQuery(this).scrollTop() > 200) {
		jQuery('.back-to-top').fadeIn();
	} else {
		jQuery('.back-to-top').fadeOut();
	}
	if (jQuery(this).scrollTop() > $('#header').height()) {
		$('#header').addClass('header_fixed');
	} else {
		$('#header').removeClass('header_fixed');
	}
});
function add_item_flytocart(variant_id,param){
	/*Add cart product loop*/
	var img_fly = $(param).parents('.product-block').find('.product-image img');
	var cart_mini = $('.cart_header');
	flytocart(img_fly, cart_mini);
	$.ajax({
		url: '/cart/add.js',
		data: 'quantity=1&id=' + variant_id,
		dataType: 'json',
		type: 'POST',
		async: false,
		success: function(){
			//flytocart(img_fly,cart_mini);
			setTimeout(function(){
				getCartAjax();
				//	$('#cart_popup').modal('show');
				//$('.modal-backdrop').css({'height':$(document).height()});
			},600);
		},
		error: function(XMLHttpRequest, txtStatus){
			Haravan.onError(XMLHttpRequest, txtStatus);
		}
	});
}
function changeImageUrl(e,url){
	jQuery(e).find("img").attr( "src", url );
}
function getCartAjax(){
	var cart = null;
	/*$('#cartform').hide();
	$('#myCart #exampleModalLabel').text("Giỏ hàng");*/
	jQuery('.cart_header_top_box').html('');
	jQuery.getJSON('/cart.js', function(cart, txtStatus){
		if(cart.item_count != 0){
			/*$('#CartCount').html(cart.item_count);
			$('#cartform').show();
			$('.line-item:not(.original)').remove();*/
			jQuery('.cart_header_top_box').append('<div class="cart_box_wrap"></div>');
			jQuery.each(cart.items,function(i,item){
				var total_price = 0;
				total_price = item.quantity * item.price;
				div = $('.original').clone().removeClass('original').appendTo('.cart_box_wrap');
				if(item.image != null)
					div.find('.cart_item_image').html("<img src=" + Haravan.resizeImage(item.image,'small') + " alt="+item.product_title+">");
				else
					div.find('.cart_item_image').html("<img src='//hstatic.net/0/0/global/noDefaultImage6_large.gif'>");
				vt = item.variant_options;
				if(vt.indexOf('Default Title') != -1)
					vt = '';
				div.find('.cart_item_info .cart_item_title a').html(item.product_title + '<br><span>' + vt + '</span>').attr('href', item.url).attr('title',item.title);
				div.find('.cart_item_quantity').html("<input type='number' disabled min='1' id='update_"+item.variant_id+"' value='"+item.quantity+"' class='quantity_top_cart'/>");
				if ( typeof(formatMoney) != 'undefined' ){
					div.find('.cart_item_price').html(Haravan.formatMoney(item.price, "")+"₫");
					div.find('.total_cart .total_price').html(Haravan.formatMoney(total_price, "")+"₫");
				}else {
					div.find('.cart_item_price').html(Haravan.formatMoney(item.price, "")+"₫");
					div.find('.total_cart .total_price').html(Haravan.formatMoney(total_price, "")+"₫");
				}
				div.find('.remove').html("<a href='javascript:void(0);' onclick='deletecart(" + item.variant_id + ")' ><i class='fa fa-times'></i></a>");
			});
			/*$('.modal-title b').html(cart.item_count);
			$('.cart_header').find('.cart_header_count span').html('('+cart.item_count+')');
			$('.cart_header').find('.cart_price').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
			$('.item-total').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
			if(cart.item_count == 0){
				$('#cart_popup #cartform').addClass('hidden');
				$('#cart_popup #exampleModalLabel').html('Không có sản phẩm nào trong giỏ hàng của bạn.<p class="text-center">Tiếp tục mua hàng <a href="/collections/all">tại đây</a>.</p>');
			}
			else{
				$('#cart_popup #exampleModalLabel').html('<img src="//hstatic.net/244/1000030244/10/2016/5-31/icon-cart.png" class="icon-cart"/> Giỏ hàng của bạn (' + cart.item_count + ' sản phẩm) <i class="fa fa-play" aria-hidden="true"></i>');
				$('#cart_popup .modal-footer').removeClass('hidden');
				$('#cart_popup #cart-table').removeClass('hidden');
				$('#cart_popup button').removeAttr('disabled');
			}
			$('.btn_plus').click(function(){
				var variant_id = $(this).data('id');
				plus_quantity($(this),variant_id);
			});
			$('.btn_minus').click(function(){
				var variant_id = $(this).data('id');
				minus_quantity($(this),variant_id);
			});*/
			jQuery('.cart_header_top_box').append('<div class="total_cart"><span>Tổng tiền: </span><span class="total_price pull-right">'+Haravan.formatMoney(cart.total_price,'')+ "₫"+'</span></div>');
			jQuery('.cart_header_top_box').append('<div class="cart-buttons clearfix"><a href="/cart" class="btn-cart">Xem giỏ hàng</a><a href="/checkout" class="btn-check-out">Thanh toán</a></div>');
			jQuery('.cart_header').find('.box_text .cart_header_count span').html('('+cart.item_count+')');
			jQuery('.cart_header').find('.box_text .cart_price').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
		}
		else{
			/*$('#cart_popup #exampleModalLabel').html('Giỏ hàng của bạn đang trống. Mời bạn tiếp tục mua hàng.');
			$('#cartform').hide();*/
			jQuery('.cart_header').find('.box_text .cart_header_count span').html('('+cart.item_count+')');
			jQuery('.cart_header').find('.box_text .cart_price').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
			jQuery('.cart_header_top_box').html('<div class="cart_empty">Giỏ hàng của bạn vẫn chưa có sản phẩm nào.</div>');
		}
	});
}
function deletecart(variant_id){
	jQuery.ajax({
		type: 'POST',
		url: '/cart/change.js',
		data: 'quantity=0&id=' +variant_id,
		dataType: 'json',
		async: false,
		success: function(line_item){
			/*jQuery('body').removeClass('modal-open');
			jQuery('body .modal-backdrop').remove();
			jQuery('body #cart_popup').remove();*/
			getCartAjax();

		},
		error: function(XMLHttpRequest, txtStatus){
			Haravan.onError(XMLHttpRequest, txtStatus);
		}
	})
}
function plus_quantity(btn,variant_id){
	var cur_quan = parseInt($(btn).parent('.ajax_qty').find('input.quantity').val());
	var new_quan = cur_quan + 1;
	$(btn).parent('.ajax_qty').find('input.quantity').val(new_quan);
	var price = $(btn).parent('.ajax_qty').find('input.quantity').attr('data-price');
	var new_price = price * new_quan;
	$(btn).parents('.line-item').find('.item-price').html(Haravan.formatMoney(new_price,'') + '₫');
	jQuery.ajax({
		type: 'POST',
		url: '/cart/change.js',
		dataType: 'json',
		async: false,
		data: 'quantity=' + new_quan + '&id=' + variant_id,
		success: function(line_item){
			jQuery.getJSON('/cart.js', function(cart, txtStatus){
				$('.cart_header').find('.cart_header_count span').html('('+cart.item_count+')');
				$('.cart_header').find('.cart_price').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
				$('.item-total').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
			});
		},
		error: function(XMLHttpRequest, txtStatus){
			Haravan.onError(XMLHttpRequest, txtStatus);
		}
	});
}
function minus_quantity(btn,variant_id){
	var cur_quan = parseInt($(btn).parent('.ajax_qty').find('input.quantity').val());
	var new_quan = cur_quan - 1;
	if (new_quan > 0){
		$(btn).parent('.ajax_qty').find('input.quantity').val(new_quan);
		var price = $(btn).parent('.ajax_qty').find('input.quantity').attr('data-price');
		var new_price = price * new_quan;
		$(btn).parents('.line-item').find('.item-price').html(Haravan.formatMoney(new_price,'') + '₫');
		jQuery.ajax({
			type: 'POST',
			url: '/cart/change.js',
			dataType: 'json',
			async: false,
			data: 'quantity=' + new_quan + '&id=' + variant_id,
			success: function(line_item){
				jQuery.getJSON('/cart.js', function(cart, txtStatus){
					$('.cart_header').find('.cart_header_count span').html('('+cart.item_count+')');
					$('.cart_header').find('.cart_price').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
					$('.item-total').html(Haravan.formatMoney(cart.total_price,'')+ "₫");
				});
			},
			error: function(XMLHttpRequest, txtStatus){
				Haravan.onError(XMLHttpRequest, txtStatus);
			}
		});
	}else{

	}
}
function changeview(dataview){
	if( $(".product_list").hasClass("grid")){
		$('.product_list').removeClass("grid");
		$('.product_list').addClass(dataview);
	}
	else{
		$('.product_list').removeClass("list");
		$('.product_list').addClass(dataview);
	}
}
function flytocart(fly, flyingto){
	var flyto = $(flyingto);
	var flyclone = $(fly).clone();
	$(flyclone).css({'position':'absolute',"left":$(fly).offset().left + "px","top":$(fly).offset().top + "px","z-index":"9999"});
	$('body').append($(flyclone));
	var goX = $(flyingto).offset().left + ($(flyingto).width()/2);
	var goY = $(flyingto).offset().top + ($(flyingto).height()/2);

	$(flyclone).animate({
		left: goX,
		top: goY,
		width: $(fly).width()/3,
		height: $(fly).height()/3
	},1000,function(){
		$(flyclone).animate({'width': 0,'height': 0}, function () {$(this).remove()});
	})
}
function get_list_ajax(array, container, view, cb) {
	$.each(array,function(i,url){
		$.ajax({
			url:'/products/'+url+'?view='+view,
			success:function(data){       
				$(container).append(data);
				cb && (i+1) === array.length ? cb(i) : null ;
			}
		});
	});   
}