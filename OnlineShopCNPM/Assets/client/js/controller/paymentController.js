$(document).ready(function () {
    $("input[required]").blur(function () {
         var value = this.value;
         if (!value) {
             $(this).addClass("required-border");
             $(this).attr("placeholder", "Bạn Phải Nhập Thông tin này");
             return false;
         }
         else {
             $(this).removeClass("required-border");
             $(this).removeAttr("placeholder");
             return true;
         }
    })
    $("#dathang").click(function () {
        var inputRequire = $("#cart-container #left-details input[required]");
        var isValid = true;
        $.each(inputRequire, function (index, input) {
            var s = $(input).val();
            if (!s) {
                isValid = false;
                return false;
            }
        })
        if (isValid) {
            var shipName = String($("#ship-name-second").val()) + ' ' + String($("#ship-name-first").val());
            var mobile = $("#mobile").val();
            var address = $("#address").val();
            var email = $("#email").val();
            var putData = shipName + '-' + mobile + '-' + address + '-' + email;
            $.ajax({
                url: '/Payment/SendOrder',
                data: { shipName: shipName, mobile: mobile, address: address, email: email },
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = "/chi-tiet-don-hang/" + res.orderID;
                    }
                    else {
                        alert("fail");
                    }
                }
            });
        }
        else {
            alert("Bạn cần nhập đủ thông tin yêu cầu.");
        }
    })
    $("#container-message span").click(function () {
        $("#checkout-coupon").slideToggle();
    })
})