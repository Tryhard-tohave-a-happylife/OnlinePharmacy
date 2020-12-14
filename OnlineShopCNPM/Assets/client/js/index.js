$(document).ready(function() {
    $('#resizing_select').change(function(){
      $("#width_tmp_option").html($('#resizing_select option:selected').text()); 
      $(this).width($("#width_tmp_select").width());
      var ab = $(this).width() + 55;
      //alert(ab);
        document.getElementById("search-bar").style.width = "calc(100% - " + ab + "px)";
    });
    $("#buttonSearch").click(function () {
        var cateID= $("#resizing_select").val();
        var search = $("#search-bar").val();
        var url = '/san-pham/' + cateID + '/' + cateMeta + '/3?search=' + search;
        $(location).attr('href', url);
    });
    $('#resizing_select_res').change(function(){
        $("#width_tmp_option_res").html($('#resizing_select_res option:selected').text()); 
        $(this).width($("#width_tmp_select_res").width());
    });
    $('#res-cancel').click(function(){
       $("#responsive-menu").css("left","-261px");
    });  
    $('#menu-icon').click(function(){
       $("#responsive-menu").css("left","0px");
    });
    $('#icon-cart').click(function () {
        window.location.href = "/gio-hang";
    })
    var firstChange = true;
    window.addEventListener("scroll", function(){
        var checkY = this.window.pageYOffset;
        if(checkY == 0){
            $('#header-details').css({
                "position":"absolute",
                "top" : "0px",
                "transition" : "unset"
            })
            firstChange = true;
        }
        if(checkY > 130 && checkY < 400){
            if(firstChange){
                $('#header-details').css({
                    "position":"fixed",
                    "top" : "-126px",
                })
            }
        }
        else if(checkY >= 400){
            firstChange = false;
            $('#header-details').css({
                "position":"fixed",
                "top" : "0px",
                "transition" : "top 1s"
            })
        }
    });
    $(window).resize(function () {
        var wd = $(window).width();
        var saveMax = maxMarginTime;
        if (wd > 860) {
            maxMarginTime = 1;      
        }
        else if (wd > 550) {
            maxMarginTime = 3;
        }
        else {
            maxMarginTime = 4;
        }
        if (saveMax != maxMarginTime) {
            $(".slide-product").css("margin-left", "0px");
            $(".slide-product").attr("time-slide", "0");
        }
    });
    $(".button-next").click(function () {
        if (!canClickNext) return;
        var ind = $(".button-next").index(this);
        var slideProduct = $(".slide-product");
        var marginLeft = $(slideProduct[ind]).css("margin-left");
        var timeSlide = Number($(slideProduct[ind]).attr("time-slide"));
        marginLeft = Number(marginLeft.substring(0, marginLeft.length - 2));
        var widthContent = $(".overflow-product").width();
        canClickNext = false;
        setTimeout(function () {
            canClickNext = true;
        }, 500);
        if (timeSlide == -(maxMarginTime)){
            $(slideProduct[ind]).css("margin-left", "0px");
            $(slideProduct[ind]).attr("time-slide","0")
            return;
        }
        var percent = marginLeft / widthContent;
        $(slideProduct[ind]).css("margin-left", (100 * (percent - 1)) + "%");
        $(slideProduct[ind]).attr("time-slide", timeSlide - 1);

    });
    $(".button-pre").click(function () {
        if (!canClickPre) return;
        var ind = $(".button-pre").index(this);
        var slideProduct = $(".slide-product");
        var marginLeft = $(slideProduct[ind]).css("margin-left");
        var timeSlide = Number($(slideProduct[ind]).attr("time-slide"));
        var widthContent = $(".overflow-product").width();
        marginLeft = Number(marginLeft.substring(0, marginLeft.length - 2));
        var widthContent = $(".content").width();    
        canClickPre = false;
        setTimeout(function () {
            canClickPre = true;
        }, 500);
        if (timeSlide == 0) {
            $(slideProduct[ind]).css("margin-left", -(100 * maxMarginTime) + "%");
            $(slideProduct[ind]).attr("time-slide", -maxMarginTime);
            return;
        }
        var percent = marginLeft / widthContent;      
        $(slideProduct[ind]).css("margin-left", (100 * (percent + 1)) + "%");
        $(slideProduct[ind]).attr("time-slide", timeSlide + 1);


    });
    $("#slide-review-pre").click(function () {
        if (!canClickPreReview) return;
        var maxTime = Number($("#overflow-review").attr("total-review"));
        var ind = Number($("#overflow-review").attr("time-slide"));
        canClickPreReview = false;
        setTimeout(function () {
            canClickPreReview = true;
        }, 500);
        if (ind == 0) {
            $("#overflow-review").css("margin-left", (-(maxTime - 1) * 100) + "%");
            $("#overflow-review").attr("time-slide", maxTime - 1);
            return;
        }
        $("#overflow-review").css("margin-left", (-(ind - 1) * 100) + "%");
        $("#overflow-review").attr("time-slide", ind - 1)

    });
    $("#slide-review-next").click(function () {
        if (!canClickNextReview) return;
        var maxTime = Number($("#overflow-review").attr("total-review"));
        var ind = Number($("#overflow-review").attr("time-slide"));
        canClickNextReview = false;
        setTimeout(function () {
            canClickNextReview = true;
        }, 500);
        if (ind == maxTime - 1) {
            $("#overflow-review").css("margin-left", "0%");
            $("#overflow-review").attr("time-slide", 0);
            return;
        }
        $("#overflow-review").css("margin-left", (-(ind + 1) * 100) + "%");
        $("#overflow-review").attr("time-slide", ind + 1)


    });
    $('.quick-view').click(function () {
        var productCode = Number($(this).attr("product-id"));
        $.ajax({
            url: '/Product/ReturnProduct',
            type: 'POST',
            dataType: 'json',
            data: {
                productCode: productCode
            },
            success: function (res) {
                if (res.status == true) {
                    $('.dialog').css({
                        "opacity": "1",
                        "visibility": "visible"
                    })
                    $('.fog-background').css({
                        "opacity": "1",
                        "visibility": "visible"
                    })
                    $('body').css("overflow-y", "hidden");
                    $(".dialog-left img").attr("src", res.data.ImageFirst);
                    var link = "/chi-tiet/" + res.data.ProductCode + "/" + res.data.MetaTitle + "/1";
                    $(".dialog-right a").text(res.data.Name);
                    $(".dialog-right a").attr("href", link);
                    $(".dialog-right a").attr("product-code", productCode);
                    if (res.data.PercentSale != 0) {
                        var strPrice = res.data.Price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                        $(".dialog-right #price-promotion").text("₫" + strPrice);
                        var priceEach = res.data.Price - (res.data.Price * res.data.PercentSale / 100);
                        var strPriceEach = priceEach.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                        $(".dialog-right #price-present").text("₫" + strPriceEach);
                    }
                    else {
                        var strPrice = res.data.Price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                        $(".dialog-right #price-present").text("₫" + strPrice);
                    }
                    var str = res.data.Description;
                    var linkCate = "/san-pham/" + res.categoryID + "/" + res.metaTitleCate + "/1"; 
                    $(".dialog-right #product-description").html(str).text();
                    $(".dialog-right #category-product span").text(res.nameCategory);
                    $(".dialog-right #category-product span").click(function () {
                        window.location.href = linkCate;
                    });
                }
                else {
                    window.alert("False");
                }
            }
        });
    })
    $('.button-close').click(function () {
        $('.dialog').css({
            "opacity": "0",
            "visibility": "hidden"
        })
        $('.fog-background').css({
            "opacity": "0",
            "visibility": "hidden"
        })
        $('body').css("overflow-y", "visible");
    })
    $("#cart-buy").click(function () {
        var num = $("#amount-product").val();
        if (num == "" || !Number.isInteger(Number(num))) {
            alert("Bạn phải nhập số lượng mua hàng");
            return;
        }
        else if (Number(num) <= 0 || Number(num) > 100) {
            alert("Số lượng mua hàng chỉ từ 1 - 100 sản phẩm");
            return;
        }
        num = Number(num);
        var productCode = $(".dialog-right a").attr("product-code");
        $.ajax({
            url: '/Cart/AddItem',
            data: { productCode: productCode, Amount: num },
            type: 'Post',
            //dataType: 'JSON',
            success: function (res) {
                if (res.status == true) {
                    location.reload();
                }
                else {
                    alert("Bạn phải đăng nhập trước khi mua hàng");
                }
            }
        });
    })
});
var cateMeta = "default";
var maxMarginTime = 1;
var canClickPre = true;
var canClickNext = true;
var canClickPreReview = true;
var canClickNextReview = true;




 