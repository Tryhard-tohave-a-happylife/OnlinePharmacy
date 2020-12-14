$(document).ready(function () {
    $(".minus").click(function () {
        var num = Number($(".input-quantity").val());
        if (num == 1) return;
        num -= 1;
        $(".input-quantity").val(num);
    })
    $(".plus").click(function () {
        var num = Number($(".input-quantity").val());
        if (num == 100) return;
        num += 1;
        $(".input-quantity").val(num);
    })
    $(".buying-button").click(function () {
        var num = $(".input-quantity").val();
        if (num == "" || !Number.isInteger(Number(num))) {
            alert("Bạn phải nhập số lượng mua hàng");
            return;
        }
        else if (Number(num) <= 0 || Number(num) > 100) {
            alert("Số lượng mua hàng chỉ từ 1 - 100 sản phẩm");
            return;
        }
        num = Number(num);
        var productCode = $(".product-name").attr("product-code");
        var price = Number($(".new-price").val());
        $.ajax({
            url: '/Cart/AddItem',
            data: { productCode: productCode, Amount: num},
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
    $('.stars').click(function () {
        var listStar = $('.stars');
        for (var i = 0; i < 5; i++) {
            $(listStar[i]).css("color", "gray");
        }
        var ind = $('.stars').index(this);
        reviewPoint = 5 - ind;
        for (var i = ind; i < 5; i++) {
            $(listStar[i]).css("color", "yellow");
        }
    });
    $(".feedback-page #submit-button").click(function () {
        if (reviewPoint == -1) {
            alert("Bạn phải chọn điểm đánh giá của mình");
            return;
        }
        var textReview = $(".feedback-page #feedback-textarea").val();
        if (!textReview || textReview == "") {
            alert("Bạn phải nhập lời đánh giá");
            return;
        }
        var productCode = $(".second-content-1-2 .product-name").attr("product-code");
        $.ajax({
            url: '/Review/AddReview',
            data: { productCode: productCode, textReview: textReview, reviewPoint: reviewPoint },
            type : 'POST',
            success: function (res) {
                if (res.status) {
                    var reviewHTML = $(`<div class = "each-review">
                            <img src = "/Assets/client/img/common/User_Circle.png"/>
                            <div class = "user-name">` + res.nameUser + `<span class="create-date"> ` + res.date + `</span></div>
                            <div class = "review-point"><span>`+ reviewPoint + `/5</span><i class="fas fa-star" style="color: yellow;"></i></div>
                            <div class="text-review">` + textReview + `</div>
                        </div>`);
                    $(".contains-review").append(reviewHTML);
                    reviewPoint = -1;
                    $(".feedback-page #feedback-textarea").val("");
                    var listStar = $('.stars');
                    for (var i = 0; i < 5; i++) {
                        $(listStar[i]).css("color", "gray");
                    }
                    var amount = Number($("#feedback-tab").attr("amount-review"));
                    if (amount == 0) {
                        var spanHTML = $(`<span>(1)</span>`);
                        $("#feedback-tab").append(spanHTML);
                        $("#feedback-tab").attr("amount-review", 1);
                    }
                    else {
                        $("#feedback-tab span").text('(' + (amount + 1) + ')');
                        $("#feedback-tab").attr("amount-review", amount + 1);
                    }
                }
                else {
                    alert("Bạn cần đăng nhập trước khi nhận xét");
                }
            }
        });
    });
})
var reviewPoint = -1;