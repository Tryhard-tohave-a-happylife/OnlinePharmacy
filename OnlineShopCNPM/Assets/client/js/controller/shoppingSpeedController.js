$(document).ready(function () {
    $(".button-shopping-speed").click(function () {
        var ind = $(".button-shopping-speed").index(this);
        var orderList = $(".order-id");
        var orderID = $(orderList[ind]).attr("full-id");
        $.ajax({
            data: { orderID: orderID },
            url: '/Order/OrderDetailByOrderID',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status) {
                    $("#model-shopping-speed #contains-products").empty();
                    var listItem = res.data;
                    $.each(listItem, function (index, item) {
                        var eachHTML = $(`<div class="each-product-detail" product-code="` + item.ProductCode +`">
                                                <div class="name" meta-title ="`+ item.MetaTitle +`">`+ item.ProductName+`</div>
                                                <div class="price">Đơn giá : ₫<span>` + item.Price +`</span></div>
                                                <input type="button" value="-" class="substract"/>
                                                <input type="number" value="` +  item.Amount +`" readonly = "readonly" class="amount-each-product" />
                                                <input type="button" value="+" class="plus"/>
                                                <div class = "delete">Xóa</div>
                                                <div class="sum-each-price">Tổng : ₫<span>`+ item.Amount * item.Price +`</span></div>
                                         </div>`);
                        $("#model-shopping-speed #contains-products").append(eachHTML);
                    });
                    $("#model-shopping-speed #sum-all-price span").text(res.amount);
                    $("#cover-model").css("display", "block");
                    $("body").css("overflow", "hidden");
                    $("#model-shopping-speed").attr("order-id", orderID);
                }
            }
        })
    });
    $("#cancel-model-shop").click(function () {
        $("#cover-model").css("display", "none");
        $("body").css("overflow", "scroll");
    });
    $("#model-shopping-speed #payment").click(function () {
        var check = confirm("Xác nhận mua hàng nhanh");
        if (!check) return;
        var orderID = $("#model-shopping-speed").attr("order-id");
        var eachProduct = $("#model-shopping-speed .each-product-detail");
        var nameProduct = $("#model-shopping-speed .name");
        var priceEachProduct = $("#model-shopping-speed .price span");
        var amountEachProduct = $("#model-shopping-speed .amount-each-product");
        var cartList = [];
        for (var i = 0; i < eachProduct.length; i++) {
            cartList.push({
                ProductCode: $(eachProduct[i]).attr("product-code"),
                ProductName: $(nameProduct[i]).text(),
                MetaTitle: $(nameProduct[i]).attr("meta-title"),
                Amount: $(amountEachProduct[i]).val(),
                Price: $(priceEachProduct[i]).text()
            });
        }
        $.ajax({
            url: '/Payment/SendOrderQuick',
            data: { cartModel: JSON.stringify(cartList), orderID: orderID },
            dataType: 'JSON',
            type: 'POST',
            success: function (res) {
                if (res.status) {
                    window.location.href = "/chi-tiet-don-hang/" + res.orderID;
                }
            }
        })
    });
    $("#contains-products").on("click", ".each-product-detail .substract", function () {
        var ind = $(".each-product-detail .substract").index(this);
        var amountList = $(".each-product-detail .amount-each-product");
        var priceList = $(".each-product-detail .price span");
        var sumList = $(".each-product-detail .sum-each-price span");
        var priceIndex = Number($(priceList[ind]).text());
        var num = Number($(amountList[ind]).val());
        if (num == 1) return;
        num -= 1;
        $(amountList[ind]).val(num);
        $(sumList[ind]).text(priceIndex * num);
        var oldSum = Number($("#model-shopping-speed #sum-all-price span").text());
        $("#model-shopping-speed #sum-all-price span").text(oldSum - priceIndex);

    });
    $("#contains-products").on("click", ".each-product-detail .plus", function () {
        var ind = $(".each-product-detail .plus").index(this);
        var amountList = $(".each-product-detail .amount-each-product");
        var priceList = $(".each-product-detail .price span");
        var sumList = $(".each-product-detail .sum-each-price span");
        var priceIndex = Number($(priceList[ind]).text());
        var num = Number($(amountList[ind]).val());
        num += 1;
        $(amountList[ind]).val(num);
        $(sumList[ind]).text(priceIndex * num);
        var oldSum = Number($("#model-shopping-speed #sum-all-price span").text());
        $("#model-shopping-speed #sum-all-price span").text(oldSum + priceIndex);
    });
    $("#contains-products").on("click", ".each-product-detail .delete", function () {
        var ind = $(".each-product-detail .delete").index(this);
        var sumList = $(".each-product-detail .sum-each-price span");
        var substractMoney = Number($(sumList[ind]).text());
        var oldSum = Number($("#model-shopping-speed #sum-all-price span").text());
        $("#model-shopping-speed #sum-all-price span").text(oldSum - substractMoney);
        $(this).parent().remove();
    });
});