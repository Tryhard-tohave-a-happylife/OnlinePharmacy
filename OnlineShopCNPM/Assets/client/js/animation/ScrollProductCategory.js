$(document).ready(function () {
    var num_category = new Array(1, 1, 1, 1, 1);
    $('.select-item').click(function () {
        var index = $(".select-item").index(this);
        var ts1 = $('.select-item');
        var ts2 = $('.children-product-category');
        if (num_category[index] == 1) {
            ///$(ts2[index]).css("display", "block");
            $(ts2[index]).slideDown();
            num_category[index] = 0;
            $(ts1[index]).css({ "transform": "rotate(180deg)" })
        }
        else {
            $(ts2[index]).slideUp();
            num_category[index] = 1;
            $(ts1[index]).css({ "transform": "rotate(360deg)" })
        }
    });
    $('.second-content-1-1-item').mouseenter(function () {
        $('.icon-up').css("display", "block");
    })
    $('.second-content-1-1-item').mouseleave(function () {
        $('.icon-up').css("display", "none");
    })
    $('#description-tab').click(function () {
        $('.feedback-page').css({
            "display": "none",
            });
        $('.description-meta').css({
            "display": "block",
        });;
        $('#description-tab').css("border-top", "green solid 3px");
        $('#feedback-tab').css("border-top", "none");
    })
    $('#feedback-tab').click(function () {
        $('.description-meta').css({
            "display": "none",
        });
        $('.feedback-page').css({
            "display": "block",
        });;
        $('#feedback-tab').css("border-top", "green solid 3px");
        $('#description-tab').css("border-top", "none");
    })
});