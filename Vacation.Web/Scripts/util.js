var BaseOP = {};
var CheckBoxs = {};

CheckBoxs.CheckAll = function (checkBox, containerId) {
    if (containerId == undefined)
        $("input[type=checkbox]").each(function () { this.checked = checkBox.checked; });
    else {
        $("#" + containerId + " input[type=checkbox]").each(function () { this.checked = checkBox.checked; });
    }
};

CheckBoxs.GetCheckedIds = function (containerId) {
    if (containerId == undefined)
        return $("input.check-box:checked").map(function () { return $(this).attr("rel"); }).get();
    else
        return $("#" + containerId + " input.check-box:checked").map(function () { return $(this).attr("rel"); }).get();
};

CheckBoxs.GetNewCheckedIds = function (containerId) {
    if (containerId == undefined)
        return $("input[node-type='ckb']:checked").map(function () { return $(this).val(); }).get();
    else
        return $("#" + containerId + " inputt[node-type='ckb']:checked").map(function () { return $(this).val(); }).get();
};


///////////////////////////////////////////////
//  基础删除方法
//  使用art.dialog弹出窗方式
//  使用jQuery
//  url     :   打开的Url
//  id      :   删除记录的ID
//  callback:   执行成功的回调函数
///////////////////////////////////////////////
BaseOP.DeleteById = function (url, id, callback) {
    art.dialog({
        icon: 'warning',
        content: '此操作不可恢复，确定要删除选中的记录吗？',
        ok: function () {
            art.dialog.tips('操作正在执行..');
            $.post(url, { id: id }, function (data) {
                if (data.IsSuccess) {
                    art.dialog.tips('操作成功！');
                    if (callback != undefined && typeof (callback) == "function") {
                        callback();
                    }
                } else {
                    art.dialog({

                        lock: true,
                        icon: 'error',
                        content: data.Message
                    });
                }
            }, "json");
            return true;
        },
        cancelVal: '关闭',
        cancel: true //为true等价于function(){}
    });
};

///////////////////////////////////////////////
//  基础删除多项方法
//  使用art.dialog弹出窗方式
//  使用jQuery
//  url         :   打开的Url
//  parameter   :   回发的参数，Object 或 Function
//  callback    :   执行成功的回调函数
///////////////////////////////////////////////
BaseOP.DeleteByIds = function (url, parameter, callback) {
    var ids, postData;
    ids = CheckBoxs.GetCheckedIds().join(',');
    if (ids.length == 0) {
        art.dialog({
            time: 2,
            lock: true,
            icon: 'warning',
            content: '请选择至少一条记录'
        });
        return;
    }

    if (parameter && typeof parameter == "function") {
        postData = parameter();
    }
    else {
        postData = parameter || {};
    }
    postData["id"] = ids;

    art.dialog({
        icon: 'warning',
        content: '此操作不可恢复，确定要删除选中的记录吗？',
        ok: function () {
            art.dialog.tips('数据正在提交..', 2);
            $.post(url, postData, function (data) {
                if (data == null || data == 0) {
                    art.dialog.tips('权限不足！');
                    return false;
                }

                if (data.IsSuccess) {
                    art.dialog.tips('操作成功！');
                    if (callback && typeof callback == "function") {
                        callback();
                    }
                } else {
                    art.dialog({

                        lock: true,
                        icon: 'error',
                        content: data.Message
                    });
                }
            }, "json");
            return true;
        },
        cancelVal: '关闭',
        cancel: true //为true等价于function(){}
    });
};


///////////////////////////////////////////////
//  基础审核方法
//  使用art.dialog弹出窗方式
//  使用jQuery
//  url     :   打开的Url
//  id      :   删除记录的ID
//  callback:   执行成功的回调函数
///////////////////////////////////////////////
BaseOP.PassById = function (url, id, callback) {
    art.dialog({
        icon: 'warning',
        content: '确认要通过审核所选项？',
        ok: function () {
            art.dialog.tips('操作正在执行..');
            $.post(url, { id: id }, function (data) {
                if (data.IsSuccess) {
                    art.dialog.tips('操作成功！');
                    if (callback != undefined && typeof (callback) == "function") {
                        callback();
                    }
                } else {
                    art.dialog({
                        lock: true,
                        icon: 'error',
                        content: data.Message
                    });
                }
            }, "json");
            return true;
        },
        cancelVal: '关闭',
        cancel: true //为true等价于function(){}
    });
};


///////////////////////////////////////////////
//  基础审核多项方法
//  使用art.dialog弹出窗方式
//  使用jQuery
//  url         :   打开的Url
//  parameter   :   回发的参数，Object 或 Function
//  callback    :   执行成功的回调函数
///////////////////////////////////////////////
BaseOP.PassByIDs = function (url, parameter, callback) {
    var ids, postData;
    ids = CheckBoxs.GetNewCheckedIds().join(',');
    if (ids.length == 0) {
        art.dialog({
            time: 2,
            lock: true,
            icon: 'warning',
            content: '请选择至少一条记录'
        });
        return;
    }

    if (parameter && typeof parameter == "function") {
        postData = parameter();
    }
    else {
        postData = parameter || {};
    }
    postData["id"] = ids;

    art.dialog({
        icon: 'warning',
        content: '确认要通过审核所选项？',
        ok: function () {
            art.dialog.tips('数据正在提交..', 2);
            $.post(url, postData, function (data) {
                if (data == null || data == 0) {
                    art.dialog.tips('权限不足！');
                    return false;
                }

                if (data.IsSuccess) {
                    art.dialog.tips('操作成功！');
                    if (callback && typeof callback == "function") {
                        callback();
                    }
                } else {
                    art.dialog({

                        lock: true,
                        icon: 'error',
                        content: data.Message
                    });
                }
            }, "json");
            return true;
        },
        cancelVal: '关闭',
        cancel: true //为true等价于function(){}
    });
};




///////////////////////////////////////////////
//  基础多选方法 
//  使用jQuery
//  url         :   Post的Url
//  parameter   :   回发的参数，Object 或 Function
//  callback    :   执行成功的回调函数
///////////////////////////////////////////////
BaseOP.SelectByIds = function (url, parameter, callback) {
    var ids, postData;
    ids = CheckBoxs.GetCheckedIds();
    ids = ids.join(',');

    if (ids.length == 0) {
        art.dialog({
            time: 2,
            lock: true,
            icon: 'warning',
            content: '请选择至少一条记录'
        });
        return;
    }

    if (parameter && typeof parameter == "function") {
        postData = parameter();
    }
    else {
        postData = parameter || {};
    }
    postData["ids"] = ids;

    art.dialog.tips('数据正在提交..', 2);

    $.post(url, postData, function (response) {
        if (response.IsSuccess) {
            art.dialog.tips('操作成功！');
            if (callback && typeof callback == "function") {
                callback();
            }
        } else {
            art.dialog({

                lock: true,
                icon: 'error',
                content: data.Message
            });
        }
    }, "json");
};

///////////////////////////////////////////////
//  基础修改方法
//  使用art.dialog弹出窗方式
//  使用jQuery
//  url     :   打开的Url
//  options :   选项，art.dialog.open的配置项
//  callback:   关闭窗口的回调函数
///////////////////////////////////////////////
BaseOP.edit = function (url, options, callback) {
    var defaults = {
        title: '修改',
        width: 850,
        height: 500,
        close: function () {
            if (callback != undefined && typeof callback == "function") {
                callback();
            }
        }
    };
    art.dialog.open(url, $.extend(defaults, options));
};

///////////////////////////////////////////////
//  基础添加方法
//  使用art.dialog弹出窗方式
//  使用jQuery
//  url     :   打开的Url
//  options :   选项，art.dialog.open的配置项
//  callback:   关闭窗口的回调函数
///////////////////////////////////////////////
BaseOP.add = function (url, options, callback) {
    var defaults = {
        title: '新增',
        width: 850,
        height: 500,
        close: function () {
            if (callback != undefined && typeof callback == "function") {
                callback();
            }
        }
    };
    art.dialog.open(url, $.extend(defaults, options));
};

///////////////////////////////////////////////
//  基础提交表单方法
//  使用JS框架有：jQuery, art.dialog, easyui
//  url     :   请求的Url
//  callback:   关闭窗口的回调函数
///////////////////////////////////////////////
BaseOP.submitForm = function (url, callback, parameter) {
    if ($(document.forms[0]).form("validate")) {
        art.dialog.tips('操作正在执行..');
        if (!parameter) {
            parameter = BaseOP.getParameter();
        }
        $.post(url, parameter, function (response) {
            if (response.Status) {
                art.dialog.tips('操作成功！');
                if (callback != undefined && typeof (callback) == "function") {
                    callback();
                }
            }
            else {
                art.dialog({

                    lock: true,
                    icon: 'error',
                    content: response.Msg
                });
            }
        }, "json");
    }
};

///////////////////////////////////////////////
//  获取参数对象 
//  selector    :   jQuery选择器 
///////////////////////////////////////////////
BaseOP.getParameter = function (selector) {
    var parameter = {};
    if (selector == undefined) {
        selector = "[sign]";
    }
    $(selector).each(function () {
        if (typeof this == "checkbox") {
            parameter[$(this).attr("name")] = $(this)[0].checked;
        }
        else if ($(this).is(":radio")) {
            parameter[$(this).attr("name")] = $("[name='" + $(this).attr("name") + "']:checked").val();
        }
        else {
            parameter[$(this).attr("name")] = $(this).val();
        }
    });
    return parameter;
};

///////////////////////////////////////////////
//  参数对象数组 
//  container : 容器jQuery选择器 
//  selector : 对象jQuery选择器
///////////////////////////////////////////////
BaseOP.getParameters = function (container, selector) {
    var parameters = new Array();
    if (container == undefined) {
        container = "[items]";
    }
    if (selector == undefined) {
        selector = "[item]";
    }
    $(container).each(function () {
        parameters.push(BaseOP.getParameter($(this).find(selector)));
    });

    return parameters;
};


///////////////////////////////////////////////
//  基础post方法
//  使用jQuery、art.dialog 
//  url         :   调用的Url 
//  parameter   :   Post参数
//  callback    :   执行成功的回调函数
///////////////////////////////////////////////
BaseOP.post = function (url, parameter, callback) {
    art.dialog.tips('操作正在执行..');
    $.post(url, parameter, function (data) {
        if (data.IsSuccess) {
            art.dialog.tips('操作成功！');
            if (callback != undefined && typeof (callback) == "function") {
                callback();
            }
        } else {
            art.dialog({
                time: 10,
                lock: true,
                icon: 'error',
                content: data.Message
            });
        }
    }, "json");
};

//截取字符串
BaseOP.cutStr = function (str, len) {

    var result = str || '';
    if (str != null && str.length > len) {

        result = str.substring(0, len);
        //        /\n|\r|(\r\n)|(\u0085)|(\u2028)|(\u2029)/g
        result += "...<a href=\"javascript:void(0);\" class=\"update\" onclick=\"BaseOP.alert('" + str.replace(/(\r\n)/g, "<br/>") + "')\">详细>></a>";
    }

    return result;
};

BaseOP.alert = function (text) {
    art.dialog({
        title: '详细',
        content: text,
        width: 600,
        ok: true,
        lock: true
    });
}


BaseOP.notice = function (options, top) {
    var opt = options || {},
        api, aConfig, hide, wrap, top,
        duration = 800;

    var config = {
        id: 'Notice',
        left: '100%',
        top: '100%',
        fixed: true,
        drag: false,
        resize: false,
        follow: null,
        lock: false,
        init: function (here) {
            api = this;
            aConfig = api.config;
            wrap = api.DOM.wrap;
            top = parseInt(wrap[0].style.top) + top;
            hide = top + wrap[0].offsetHeight;

            wrap.css('top', hide + 'px')
                .animate({ top: top + 'px' }, duration, function () {
                    opt.init && opt.init.call(api, here);
                });
        },
        close: function (here) {
            wrap.animate({ top: hide + 'px' }, duration, function () {
                opt.close && opt.close.call(this, here);
                aConfig.close = $.noop;
                api.close();
            });

            return false;
        }
    };

    for (var i in opt) {
        if (config[i] === undefined) config[i] = opt[i];
    };

    return artDialog(config);
};

BaseOP.dialog = {};

BaseOP.dialog.warning = function (msg) {
    art.dialog({
        content: msg,
        icon: "warning",
        ok: function () { },
        cancel: true
    });
};

BaseOP.dialog.error = function (msg) {
    art.dialog({
        content: msg,
        icon: "error",
        ok: true
    });
};

BaseOP.dialog.success = function (msg) {
    art.dialog({
        content: msg,
        icon: "succeed",
        ok: true
    });
};