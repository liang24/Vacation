$(document).ready(function (e) {

    $(".menu_list li a").hover(function () {
        if ($(this).hasClass("menut2cl")) { } else { $(this).addClass("menulisthover"); }
    }, function () {
        $(this).removeClass("menulisthover");
    });

    $("#top_menu li span").hide();

    $("#top_menu li a").hover(function () {
        if ($(this).hasClass("topmlcl")) { } else { $(this).addClass("topmlhr"); }
    }, function () {
        $(this).removeClass("topmlhr");
    });

    $('[node-type="top-menu"]').click(function () {
        $('[node-type="top-menu"]').removeClass("menucl").filter(this).addClass("menucl");

        var module_id = $(this).attr("module-id");

        if (module_id) {
            var curObj = $('.menu_list').hide().filter('[module-id="' + module_id + '"]');
            $(curObj).show();
            //            $(curObj).children("p").first().children().click();
        }
        else {
            $('.menu_list').hide();
        }
    }).hover(function () {
        if ($(this).hasClass("menucl")) { } else { $(this).addClass("menuchover"); }
    }, function () {
        $(this).removeClass("menuchover");
    });

    $(".menu_left").children(".menu_list:first").children("ul").show();

    $(".menu_list p a").click(function () {
        if (($(this).parent("p").parent(".menu_list").hasClass("xz"))) {
            $(this).parent("p").next("ul").slideUp("fast");
            $(".menu_list p a").removeClass("menutitlecl");
            $(".menu_list").removeClass("xz");
        } else {
            $(".menu_list").removeClass("xz");
            $(this).parent("p").parent(".menu_list").addClass("xz");
            $(".menu_list p a").removeClass("menutitlecl");
            $(".menu_list ul").slideUp("fast");
            $(".menu_list ul li a").removeClass("menut2cl");
            $(this).addClass("menutitlecl");
            $(this).parent().next("ul").slideDown(300);
        }
    });

    $(".menu_list ul li a").click(function () {
        $(".menu_list ul li a").removeClass("menut2cl");
        $(this).addClass("menut2cl");
    });

    $("#top_info").hover(function () {
        $(this).css("cursor", "pointer");
        $("#top_menu_sm").show();
    }, function () {
        $("#top_menu_sm").hide();
    });

    $("#tainfo tr").hover(function () {
        $(this).addClass("trhover");
    }, function () {
        $(this).removeClass("trhover");
    });

    $("#tainfo tr:first").hover(function () {
        $(this).removeClass("trhover");
    });

    // 列表常用函数
    $("body").on("click", '[href="#"]', function (e) {
        e.preventDefault();
    });

    // 全选
    $("#btnCheckAll").click(function () {
        if ($(this).is(".choicecl")) {
            $(this).removeClass("choicecl").addClass("choice");
            $('[node-type="ckb"]').attr("checked", true);
        }
        else {
            $(this).addClass("choicecl").removeClass("choice");
            $('[node-type="ckb"]').attr("checked", false);
        }
    });

    //分页查询
    $("#btnSearch").click(function () {
        $("#pager").pager("load", BaseOP.getParameter("#wrapSearchBar [name]"));
    });

    //页面操作查询
    $("#btnSearchList").click(function () {
        var name = $(".ss_input").val();
        $("#tbList tbody tr").hide();
        if (name == "") {
            $("#tbList tbody").html($("#tmplList").tmpl(DataList));
        } else {
            var obj = $("[search-name*=" + name + "]").parent("tr");
            $.each(obj, function (i, v) {
                $(v).show();
                var child = $("tr[node-value=" + $(v).attr("node-value") + "]");
                $.each(child, function (i, item) {
                    if (!$(item).hasClass("child")) {
                        $(item).show();
                        $(item).children("td:eq(1)").children("a").attr("btn", "btnHide").html("关闭");
                    }
                });
            });
        }
    });
});