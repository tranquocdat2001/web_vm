// các function dùng chung cho web và mobile
$(function () {
    

});

function CallMe(pId, uId) {
    $.post("/Handler/listen.ashx", { action: "callme", SDT: $("#txtCallMe").val(), productId: pId, userId: uId }, function (data) {
        alert("Veganmart sẽ liên hệ với bạn sớm nhất có thể!");
    });
}

function Loading() {
    $('#loadding').html('<div class="bg-popup"> <img src="/Content/Web/images/loading300.gif" class="loading"/></div>');
    NoScroll();
}
function Loaded() {
    $('#loadding').empty();
    AutoScroll();
}
function ClosePopup() {
    $('.field-validation-error').removeClass().addClass('.field-validation-valid').empty();
    $('#popup').empty();
    AutoScroll();
}
function NoScroll() {
    if ($('body').attr('style') == undefined) {
        var width = $('body').width();
        $('body').css('overflow', 'hidden');
        var scrollWidth = $('body').width() - width;
        $('body').css('margin-right', scrollWidth + 'px');
    }
}
function AutoScroll() {
    $('body').removeAttr('style');
}

function OpenPopupMessage(strMessage) {
    var htmlPopup = '<div class="bg_popup"></div>';
    htmlPopup += '<div class="popup_container">';
    htmlPopup += '    <div class="content">';
    //htmlPopup += '     <div class="titlebox">Đăng ký thành viên thành công</div>';
    //htmlPopup += '     <p class="contentbox">Chúng tôi đã gửi link kích hoạt đến địa chỉ email mà quý khách đã dùng để đăng ký tài khoản. Trong một số trường hợp email có thể rơi vào hòm thư rác, quý khách vui lòng kiểm tra kỹ.</p>';
    htmlPopup += '       <div>' + strMessage + '</div>';
    htmlPopup += '       <div class="control pd-top-20 no-mg">';
    htmlPopup += '           <span class="btn btn-login close-popup">OK</span>';
    htmlPopup += '       </div>';
    htmlPopup += '    </div>';
    htmlPopup += '</div>';
    $('#popup').html(htmlPopup);
    NoScroll();
}

function ConfirmMessageLogout(message) {
    var htmlPopup = '<div class="bg_popup"></div>';
    htmlPopup += '<div class="popup_container">';
    htmlPopup += '    <div class="content">';
    htmlPopup += message;
    htmlPopup += '    </div>';
    htmlPopup += '</div>';
    $('#popup').html(htmlPopup);
    NoScroll();
}
function Logout() {
    $('#logoutForm').submit();
}


//function CreateMessageError(strMessage) {
//    var message = $("#lblMessage");
//    message.html("<div class=\"field-validation-error\">" + strMessage + "</div>");
//}

function CreateMessageError(objId, strMessage) {
    objId.html("<div class=\"field-validation-error\">" + strMessage + "</div>");
}
function CreateMessageSucces(objId, strMessage) {
    objId.html("<div class=\"field-validation-error\">" + strMessage + "</div>");
}

// tạo model form
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
//console.log($("#frmContact").serializeObject());

function InitForm() {
    $('button[type=reset]').click(function () {
        $('.field-validation-error').empty();
    });
    $('button[type=submit]').click(function () {
        var form = $(this).parents('form:first');
        if (form.valid()) {
            var data = form.serialize();
            //if (this.beenSubmitted == data)
            //    return false;
            //else {   
            //this.beenSubmitted = data;
            //}
            Loading();
            $.post(form.attr('action'), data)
                .done(function (response) {
                    $('.lblMessage').empty();
                    var objId = form.find(".lblMessage");
                    if (response.Error == undefined) {
                        CreateMessageError(objId, "Có lỗi, bạn vui lòng thử lại!");
                    }
                    else if (response.Error) {
                        CreateMessageError(objId, response.Message);
                    }
                    else {
                        switch (response.NextAction) {
                            case 1:
                                location.href = location.pathname;
                                break;
                            case 2:
                                location.href = response.Message;
                                break;
                            case 3:
                                form[0].reset();
                                if (form.attr('id') == 'frmLostPass') {
                                    $("#ForgotPassPopup").hide();
                                }
                                else if (form.attr('id') == 'frmRegisterEvent') {
                                    //$('#btnEditRegisterEvent').click();
                                    $('#btnRegisterEvert').hide();
                                    $('#btnHome').show();
                                }
                                else if (form.attr('id') == 'frmRegister') {
                                    $(".chosen-select:not(.disable_search)").trigger("chosen:updated");
                                }
                                else if (form.attr('id') == 'frmReceiveEmail') {
                                    $('#ReceiveEmailPopup').hide();
                                }
                                OpenPopupMessage(response.Message);
                                break;
                            case 5:
                                form[0].reset();
                                CreateMessageSucces(objId, response.Message);
                                break;
                            default:
                                CreateMessageSucces(objId, response.Message);
                                break;
                        }
                    }
                    Loaded();
                    refreshCaptcha();
                }).fail(function (jqXhr, textStatus, errorThrown) {
                    var msg = '';
                    if (jqXhr.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (jqXhr.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (jqXhr.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else if (textStatus === 'parsererror') {
                        msg = 'Requested JSON parse failed.';
                    } else if (textStatus === 'timeout') {
                        msg = 'Time out error.';
                    } else if (textStatus === 'abort') {
                        msg = 'Ajax request aborted.';
                    } else {
                        msg = 'Uncaught Error.\n' + jqXhr.responseText;
                    }
                    Loaded();
                    alert(msg);
                });
            return false;
        }
        else {
            $('.lblMessage').empty();
            return false;
        }
    });
}


function refreshCaptcha() {
    $('.imgCaptcha').attr('src', '/HandlerWeb/CaptchaHandler.ashx?t=' + new Date().getMilliseconds());
}

function EncodeTextSearch() {
    var keyword = $.trim($('#txtKeyword').val());
    if (keyword != '')
        keyword = keyword.replace(/[<|>\*\"\#\\\?]/ig, '');

    if ((keyword == null || keyword === "")) {
        $("#txtKeyword").val("");
        return false;
    }
    $("#txtKeyword").val(keyword);
    return true;
}


Avatar = {
    processGif: function () {
        var me = this;
        var timer = 0;
        var screenHeight = $(window).height();
        var arrImages = $('img[data-original],img[data-src]');
        me.playGif(arrImages, screenHeight);
        $(window).scroll(function () {
            clearTimeout(timer);
            timer = setTimeout(function () {
                me.playGif(arrImages, screenHeight);
            }, 50);
        });
    },
    playGif: function (arrImages, screenHeight) {
        var me = this;
        for (var i = 0; i < arrImages.length; i++) {
            var curItem = $(arrImages[i]);
            if (me.checkInViewport(curItem, screenHeight, 100)) {
                if (!curItem.hasClass('playing')) {
                    // Set new gif for play
                    curItem.addClass('playing');
                    me.changeGifPlay(curItem);
                }
            } else if (curItem.hasClass('playing')) {
                // Remove playing gif
                curItem.removeClass('playing');
                me.changeGifPlay(curItem);
            }
        }
    },
    changeGifPlay: function (item) {
        var temp = item.attr('src');
        dataSrc = item.data('original');
        if (dataSrc == undefined || dataSrc == "")
            dataSrc = item.data('src');

        item.attr('src', dataSrc);
        item.data('original', temp);
    },
    checkInViewport: function (src, screenHeight, scale) {
        var screenScrollTop = $(document).scrollTop();
        var imageHeight = src.outerHeight();
        var imageOffsetTop = src.offset().top;
        return screenScrollTop + screenHeight - scale >= imageOffsetTop
            && screenScrollTop + scale <= imageOffsetTop + imageHeight;
    }
};

String.prototype.format = function () {
    var text = this;
    //decrement to move to the second argument in the array
    var tokenCount = arguments.length;
    //check if there are two arguments in the arguments list
    if (tokenCount < 1) {
        //if there are not 2 or more arguments there's nothing to replace
        //just return the original text
        return text;
    }
    for (var token = 0; token < tokenCount; token++) {
        //iterate through the tokens and replace their placeholders from the original text in order
        text = text.replace(new RegExp("\\{" + token + "\\}", "gi"), arguments[token]);
    }
    return text;
};


function BindingModel(branddivid, eml) {
    if (eml != null && eml != undefined) {
        $('.brand-grid li .image').removeClass('active');
        $(eml).addClass('active');

        var subHtml = $(branddivid).find("ul").html();
        var html = $(eml).parent().find("ul").html();

        if (subHtml != html) {
            $(".sub-brand-menu-all").empty();

            html = '<ul>' + html + '</ul>';
            $(branddivid).html(html).slideDown(500);
        }
        else {
            if ($(branddivid).is(':visible')) {
                $(eml).removeClass('active');
                $(branddivid).slideUp(500);
            }
            else {
                $(branddivid).slideDown(500);
            }
        }


    }
}

// Custome ajax call /
var ajax_feed = ajax_feed || {};
ajax_feed = {
    post_json_extention: function (url, data, beforeCall, callbackSucess, callBackError) {
        $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            //processData: false,
            //cache: false,
            data: data,
            async: false,
            beforeSend: function () {
                //Gọi hàm sử lý trước khi bắt đầu request(show loading icon....)
                if (beforeCall != null && beforeCall != undefined) {
                    beforeCall();
                }
            },
            success: function (resultData) {
                //1. kiểm tra nếu chưa login thì nhảy về trang đăng nhập
                var pathname = window.location.pathname;
                var urlLogin = data != null && data.url != undefined ? data.url : '/dang-nhap';
                if (data !== null && data.login !== undefined && !data.login) {
                    window.location.href = urlLogin + "?returnUrl=" + pathname;
                } else if (data !== null && data.access !== undefined && !data.access) {
                    window.location.href = urlLogin;
                }

                //2. Thực thi gọi hàm sử lý sau khi request sucess.
                if (callbackSucess != null && callbackSucess != undefined) {
                    callbackSucess(resultData);
                }

            },
            error: function (xhr) {
                // Handle errors here
                console.log(xhr.statusText + xhr.responseText);
                //2. Thực thi gọi hàm sử lý sau khi request sucess.
                if (callBackError != null && callBackError != undefined) {
                    callBackError();
                }
            }
        });
    },
    post_html_extention: function (url, data, beforeCall, callbackSucess, callBackError) {
        $.ajax({
            url: url,
            type: "POST",
            dataType: "html",
            contentType: "application/json",
            processData: false,
            cache: false,
            data: data,
            async: false,
            beforeSend: function () {
                //Gọi hàm sử lý trước khi bắt đầu request(show loading icon....)
                if (beforeCall != null && beforeCall != undefined) {
                    beforeCall();
                }
            },
            success: function (resultData) {

                //1. kiểm tra nếu chưa login thì nhảy về trang đăng nhập
                var pathname = window.location.pathname;
                var urlLogin = data != null && data.url != undefined ? data.url : '/dang-nhap';
                if (data !== null && data.login !== undefined && !data.login) {
                    window.location.href = urlLogin + "?returnUrl=" + pathname;
                } else if (data !== null && data.access !== undefined && !data.access) {
                    window.location.href = urlLogin;
                }

                //2. Thực thi gọi hàm sử lý sau khi request sucess.
                if (callbackSucess != null && callbackSucess != undefined) {
                    callbackSucess(resultData);
                }

            },
            error: function (xhr) {
                // Handle errors here
                console.log(xhr.statusText + xhr.responseText);
                //2. Thực thi gọi hàm sử lý sau khi request sucess.
                if (callBackError != null && callBackError != undefined) {
                    callBackError();
                }
            }
        });
    }
};



var lightboxInDetail =
    {
        slideLightBox: function () {
            //Phần dành cho ảnh trong nội dung
            if ($('.group-image-lightbox').length > 0) {

                $('.group-image-lightbox img').each(function (i, lem) {
                    var title = $(this).attr("title");
                    var src = $(this).attr("data-src") !== undefined && $(this).attr("data-src") !== null ? $(this).attr("data-src") : $(this).attr("src");
                    if (!$(this).parent().hasClass('dvsvideo') && src) {
                        $(this).parent().attr("id", "imageGalleryId-" + i)
                            //.attr("class", "imageGallery")
                            .addClass("imageGalleryId")
                            .attr("data-src", src.replace("crop/147x82/", ""))
                            .attr("data-desc", title);
                    }
                });

                var $gl = $(".group-image-lightbox").lightGallery({
                    desc: true,
                    loop: true,
                    escKey: true,
                    zoom: true,
                    download: false,
                    counter: false,
                    selector: '.imageGalleryId',
                    showThumbByDefault: false,
                    lang: { allPhotos: 'Tất cả ảnh' }
                });

                $gl.on('onCloseAfter.lg', function (event) {
                    var url = window.location.href;
                    if (url.indexOf("#view-gallery1") != -1) {
                        url = url.replace("#view-gallery1", '');
                        window.history.pushState(null, null, url);
                    }
                });

                //Dùng để back lại 
                $(".group-image-lightbox img").click(function () {
                    var url = window.location.href;
                    if (url.indexOf("#view-gallery1") == -1) {
                        url += "#view-gallery1";
                        window.history.pushState(null, null, url);
                    }

                    $(window).on('popstate', function () {
                        $(".lg-close").trigger("click");
                    });
                });
            }
        }

    }