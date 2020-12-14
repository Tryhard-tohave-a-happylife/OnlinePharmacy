$(document).ready(function () {
  
    var cart = {
        init: function () {
            cart.regEvents();
        },
        regEvents: function () {
            $(".product-remove > i").on("click", function () {
                var self = this;
                $.ajax({
                    data: { productCode: Number($(self).parent().parent().attr('item-id')) },
                    url: '/Cart/Delete',
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status) {
                            $(self).parent().parent().remove();
                            var num = Number($("#amount-display").text());
                            $("#amount-display").text(Number(num - res.minusAmount));
                            if (Number(num - res.minusAmount) == 0) {
                                $(".cart-content").css("display", "none");
                                $("#alert-empty").css("display", "block");
                            }
                            $("#item-name-restore").text('"'+res.nameItem+'"');
                            $("#div-restore").css("display", "block");
                        }
                    }
                })
            });
            $(".button-up-down").on("click", function () {
                var ind = $(".button-up-down").index(this);
                var tssl = $(".product-quantity .quantity");
                var price = $(".product-price .display-price");
                var sumOfPrice = $(".product-subtotal .display-sumOfAll");
                var quantity = Number($(tssl[ind]).val());
                quantity += 1;
                $(tssl[ind]).val(quantity);
                $(sumOfPrice[ind]).text(Number($(price[ind]).text()) * quantity);
            });
            $(".button-up-up").on("click", function () {
                var ind = $(".button-up-up").index(this);
                var tssl = $(".product-quantity .quantity");
                var price = $(".product-price .display-price");
                var sumOfPrice = $(".product-subtotal .display-sumOfAll");
                var quantity = Number($(tssl[ind]).val());
                if (quantity > 1) quantity -= 1;
                $(tssl[ind]).val(quantity);
                $(tssl[ind]).val(quantity);
                $(sumOfPrice[ind]).text(Number($(price[ind]).text()) * quantity);
            });
            //$('#btnContinue').off('click').on('click', function () {
            //    window.location.href = "/";
            //});
            //$('#btnPayment').off('click').on('click', function () {
            //    window.location.href = "/thanh-toan";
            //});
            $('.button-update-cart').off('click').on('click', function () {
                var listProduct = $('.list-view-product');
                var cartList = [];
                var amount = $(".product-quantity .quantity");
                $.each(listProduct, function (i, item) {
                    cartList.push({
                        ProductCode: Number($(item).attr("item-id")),
                        MetaTitle: $(item).attr("item-metaTitle"),
                        Name: $(item).attr("item-name"),
                        Image: $(item).attr("item-img"),
                        Price: $(item).attr("item-price"),
                        Amount: Number($(amount[i]).val())
                    });
                });

                $.ajax({
                    url: '/Cart/UpdateCart',
                    data: { cartModel: JSON.stringify(cartList) },
                    dataType: 'JSON',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/gio-hang";
                        }
                    }
                })
            });
        }
    }
    cart.init();
})
