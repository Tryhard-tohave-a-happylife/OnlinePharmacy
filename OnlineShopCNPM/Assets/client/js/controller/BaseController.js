var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#search-bar").autocomplete({
            minLength: 3,
            source: function (request, response) {
                var categoryMeta = $("#resizing_select").val();
                $.ajax({
                    url: "/search?category=" + categoryMeta,
                    dataType: "json",
                    data: {
                        q: String(request.term)
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            select: function (event, ui) {
                window.location.href = "/chi-tiet/" + ui.item.ID + "/" + ui.item.MetaTitle + "/1";
                return false;
            }
        })
        .autocomplete("instance")._renderItem = function (ul, item) {
            var price = item.Price;
            var promotionPrice = price - (price * item.PercentSale) / 100;
            price = price.toLocaleString();
            var divPrice = "";
            if (item.PercentSale != 0) {
                promotionPrice = promotionPrice.toLocaleString();
                divPrice = "<span class='infor-price'><span class='price'><span class='vnd'>đ</span>" + promotionPrice + "</span><span class='promotion'><span class='vnd'>đ</span>" + price + "</span></span>"
            }
            else {
                divPrice = "<span class='infor-price'><span class='price'><span class='vnd'>đ</span>" + price + "</span></span>"
            }
            return $("<li><div><img src = '" + item.Image + "'><span class='name'>" + item.Name + "</span>" +divPrice+"</div></li>")
                    .appendTo(ul);
         };
    }
}
common.init();