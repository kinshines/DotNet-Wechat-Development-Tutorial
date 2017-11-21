    var minvalue=0;
    var maxvalue=0;
    var presion=0;

    jQuery.fn.extend({
        //输入数字的文本框
        mynumberbox: function (iSet) {
            /*
             * Minus:点击元素--减小
             * Add:点击元素--增加
             * Input:表单元素
             * Min:表单的最小值，非负整数
             * Max:表单的最大值，正整数
             */
            classname = $(this).attr("class");
            minvalue=0;
            maxvalue = 100;
            presion = 0;
            if (iSet != null)
            {
                minvalue = iSet.minvalue;
                maxvalue = iSet.maxvalue;
                presion = iSet.presion;
            }
            if (classname == undefined)
                classname = "";
            if (minvalue == undefined)
                minvalue = 0;
            if (maxvalue == undefined)
                maxvalue = 100;
            if (presion == undefined)
                presion = 0;
                $(this).attr("tag", minvalue + "," + maxvalue + "," + presion)

            //外面包一个div
            container_id = $(this).attr("id") + "_container";
            var str = "<div id='" + container_id + "' class='numbox_container' ></div>";
            $(this).wrapAll(str);
            $("#" + container_id).css("display", "block");
            $("#" + container_id).css("height", "40");
            $("#" + container_id).css("width", "400");
            $("#" + container_id).css("border", "0px solid red");

            
            //if (classname == "")//如果为指定class，则自动套用默认样式
            //{
            $(this).css("border", "1px solid  #aaaaaa");
                $(this).css("height", "30");
                $(this).css("width", "100");
                $(this).css("text-align", "right");

            //}
            $(this).css("float", "left");
            id_add = "add" + '_' + $(this).attr("id");
            id_minus = "minus" + '_' + $(this).attr("id");
            $(this).after('<div id="' + id_add + '" class="btn_add" onselectstart="return false;">+</div>');
            $("#" + id_add).css("display", "block");
            $("#" + id_add).css("width", "30");
            $("#" + id_add).css("height", "30");
            $("#" + id_add).css("border", "1px solid #aaaaaa");
            $("#" + id_add).css("border-left-width", "0");
            $("#" + id_add).css("text-align", "center");
            $("#" + id_add).css("padding-top", "4px");
            $("#" + id_add).css("float", "left");
            $("#" + id_add).css("margin-left", "0");
            $("#" + id_add).css("margin-right", "5px");
            $("#" + id_add).css("cursor", "pointer");
            $("#" + id_add).css("-moz-user-select", "-moz-none");


            $("#" + id_add).on("click", function () {
                parse_tag($(this).prev())
                value = parseFloat($(this).prev().val());
                if (isNaN(value))
                    value = 0;
                value = value + 1 < maxvalue ? value + 1 : maxvalue;
                $(this).prev().val(value);

                chkLast($(this).prev()[0]);
                chkNumber($(this).prev()[0]);
                chkPresion($(this).prev()[0]);

            })



            $(this).before('<div id="' + id_minus + '" class="btn_minus" onselectstart="return false;">-</div>');
            $("#" + id_minus).css("display", "block");
            $("#" + id_minus).css("width", "30");
            $("#" + id_minus).css("height", "30");
            $("#" + id_minus).css("border", "1px solid #aaaaaa");
            $("#" + id_minus).css("border-right-width", "0");
            $("#" + id_minus).css("text-align", "center");
            $("#" + id_minus).css("padding-top", "4px");
            $("#" + id_minus).css("float", "left");
            $("#" + id_minus).css("margin-left", "0");
            $("#" + id_minus).css("margin-right", "0");
            $("#" + id_minus).css("cursor", "pointer");
            $("#" + id_minus).css("-moz-user-select", "-moz-none");
            $("#" + id_minus).on("click", function () {
                parse_tag($(this).next())
                value = parseFloat($(this).next().val());
                if (isNaN(value))
                    value = 0;

                value = value - 1 > minvalue ? value - 1 : minvalue;
                $(this).next().val(value);
                chkLast($(this).next()[0]);
                chkNumber($(this).next()[0]);
                chkPresion($(this).next()[0]);

            })
            $(this).attr("display", "block");
            $(this).attr("float", "left");
            $(this).css("text-align", "center");
            width = maxvalue.toString().length * 10 + 20;
            $(this).css("width", width);
            $(this).css("font-family", "Arial");
            $(this).css("font-size", "20");
            
            //$(this).on("mouseenter", function () {
            //    $(this).css("border", "1px solid #ff6600");
            //    $(this).css("color", "#ff6600");
            //})
            //$(this).on("mouseleave", function () {
            //    $(this).css("border", "1px solid #aaaaaa");
            //    $(this).css("color", "#aaaaaa");
            //})

            //$(".btn_minus").on("mouseenter", function () {
            //   $(this).css("border", "1px solid #ff6600");
            //   $(this).css("color", "#ff6600");
            //})
            //$(".btn_minus").on("mouseleave", function () {
            //    $(this).css("border", "1px solid #aaaaaa");
            //    $(this).css("color", "#aaaaaa");
            //})
            //$(".btn_add").on("mouseenter", function () {
            //    $(this).css("border", "1px solid #ff6600");
            //    $(this).css("color", "#ff6600");
            //})
            //$(".btn_add").on("mouseleave", function () {
            //    $(this).css("border", "1px solid #aaaaaa");
            //    $(this).css("color", "#aaaaaa");
            //})

            $(this).change(function () {
                chkLast(this);
                chkPresion(this);
                chkNumber(this);
            })
            $(this).keyup(function () {
                //chkNumber(this);
            })

            $(this).blur(function () {
                chkLast(this);
                chkPresion(this);
                chkNumber(this);
            })
        }
    });

function chkNumber(obj) {
    parse_tag($(obj))

    obj.value = obj.value.replace(/[^\d.]/g, "");
    //必须保证第一位为数字而不是. 
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个.而没有多个. 
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证.只出现一次，而不能出现两次以上 
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");

    value = parseFloat(obj.value);

    if (value > maxvalue)
        obj.value = maxvalue;
    if (value < minvalue)
        obj.value = minvalue;
    
    }

function chkPresion(obj) {
    parse_tag($(obj))

    if (presion < 0)
        return;
    //判断小数点后数字
    if (presion > 0) {
        var pos = parseInt(obj.value.lastIndexOf('.'))

        if (obj.value.length - pos - 1 > presion) {
            obj.value = obj.value.substr(0, pos + presion + 1);
        }
        if (obj.value.length - pos - 1 < presion) {

            if (pos > 0) {
                for (var i = 0; i < presion + pos + 1 - obj.value.length; i++) {
                    obj.value += '0';
                }
            }
            if (pos < 0) {
                obj.value += '.';
                for (var i = 0; i < presion; i++) {
                    obj.value += '0';
                }
            }
        }
    }
    if (presion == 0) {
        //判断小数点后数字
        var pos = parseInt(obj.value.lastIndexOf('.'))
        if (pos > 0) {
            obj.value = obj.value.substr(0, pos);
        }
        if (pos == 0) {
            obj.value = '0';
        }
    }
}

function chkLast(obj) {
    if (obj.value == null || obj.value == undefined)
        return;
    // 如果出现非法字符就截取掉 
    if (obj.value.substr((obj.value.length - 1), 1) == '.')
    {
        obj.value = obj.value.substr(0, (obj.value.length - 1));
    }
}

function parse_tag($obj) {
    // 如果出现非法字符就截取掉 
    var tag = $obj.attr("tag");
    if (tag == null)
        return;
    arr = tag.split(',');
    if(arr.length>=3)
    {
        minvalue = parseFloat(arr[0]);
        maxvalue = parseFloat(arr[1]);
        presion = parseFloat(arr[2]);
    }
}