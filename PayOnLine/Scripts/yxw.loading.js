jQuery.extend({
    loading: function () {
        if ($("#divmask").attr("id") == undefined) {
            $("<div id=\"masker\" style=\"\"></div>").appendTo($("body")); // 黑幕
            $("#masker").css("width", "100%");
            $("#masker").css("height", screen.height);
            $("#masker").css("background-color", "rgba(0, 0, 0, 0.3)");
            $("#masker").css("left", "0");
            $("#masker").css("top", "0");
            $("#masker").css("margin", "0 0 0 0");
            $("#masker").css("position", "fixed");
            $("#masker").css("float", "left");
            $("#masker").css("z-index", "100");
        }

       if ($("#loadinggif").attr("id") == undefined) {
            $("<div id=\"loadinggif\" style=\"\"></div>").appendTo($("body")); // 加载动图
            $("#loadinggif").css('background-image', 'url(/images/loading.gif)');
            $("#loadinggif").css("position", "absolute");
            $("#loadinggif").css("top", "300px");
            $("#loadinggif").css("left", "50%");
            $("#loadinggif").css("height", "18px");
            $("#loadinggif").css("width", "18px");
            //$("#loadinggif").css("margin", "50% auto 50% auto");
            //$("#masker").css("left", "0");
            //$("#masker").css("top", "0");
            //$("#masker").css("margin", "0 0 0 0");
            //$("#masker").css("position", "fixed");
            //$("#masker").css("float", "left");
            $("#loadinggif").css("z-index", "9999");
            $("#loadinggif").show();
        }

    }
});