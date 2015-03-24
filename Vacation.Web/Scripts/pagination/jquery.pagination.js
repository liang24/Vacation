﻿/**
* This jQuery plugin displays pagination links inside the selected elements.
*
* @author Gabriel Birke (birke *at* d-scribe *dot* de)
* @version 1.2
* @param {int} maxentries Number of entries to paginate
* @param {Object} opts Several options (see README for documentation)
* @return {Object} jQuery Object
*/
(function ($) {
    $.fn.pagination = function (maxentries, opts) {
        opts = jQuery.extend({
            items_per_page: 10,
            num_display_entries: 7, 
            current_page: 0,
            num_edge_entries: 1, 
            link_to: "#",
            prev_text: "上一页",
            next_text: "下一页",
            ellipse_text: "...",
            prev_show_always: true,
            next_show_always: true,
            callback: function () { return false; },
            default_page_class: "",
            default_page_prev_class: "prev",
            default_page_next_class: "next"            
        }, opts || {});

        return this.each(function () {
            /**
            * Calculate the maximum number of pages
            */
            function numPages() {
                return Math.ceil(maxentries / opts.items_per_page);
            }

            /**
            * Calculate start and end point of pagination links depending on 
            * current_page and num_display_entries.
            * @return {Array}
            */
            function getInterval() {
                var ne_half = Math.ceil(opts.num_display_entries / 2);
                var np = numPages();
                var upper_limit = np - opts.num_display_entries;
                var start = current_page > ne_half ? Math.max(Math.min(current_page - ne_half, upper_limit), 0) : 0;
                var end = current_page > ne_half ? Math.min(current_page + ne_half, np) : Math.min(opts.num_display_entries, np);
                return [start, end];
            }

            /**
            * This is the event handling function for the pagination links. 
            * @param {int} page_id The new page number
            */
            function pageSelected(page_id, evt) {
                current_page = page_id;
                drawLinks();
                var continuePropagation = opts.callback(page_id, panel);
                if (!continuePropagation) {
                    if (evt.stopPropagation) {
                        evt.stopPropagation();
                    }
                    else {
                        evt.cancelBubble = true;
                    }
                }
                return continuePropagation;
            }

            /**
            * This function inserts the pagination links into the container element
            */
            function drawLinks() {
                panel.empty();
                var interval = getInterval();
                var np = numPages();
                // This helper function returns a handler function that calls pageSelected with the right page_id
                var getClickHandler = function (page_id) {
                    return function (evt) { return pageSelected(page_id, evt); }
                }
                // Helper function for generating a single link (or a span tag if it's the current page)
                var appendItem = function (page_id, appendopts) {
                    page_id = page_id < 0 ? 0 : (page_id < np ? page_id : np - 1); // Normalize page id to sane value
                    appendopts = jQuery.extend({ text: page_id + 1, classes: opts.default_page_class }, appendopts || {});
                    if (page_id == current_page) {
                        var lnk = jQuery("<span class='act'><a>" + (appendopts.text) + "</a></span>");
                    }
                    else {
                        var lnk = jQuery("<a>" + (appendopts.text) + "</a>")
						    .bind("click", getClickHandler(page_id))
						    .attr('href', opts.link_to.replace(/__id__/, page_id));

                        lnk = jQuery("<span></span>").append(lnk);
                    } 

                    if (appendopts.classes) { lnk.addClass(appendopts.classes); }
                    panel.append(lnk);
                }
                // Generate "Previous"-Link
                if (opts.prev_text && (current_page > 0 || opts.prev_show_always)) {
                    appendItem(current_page - 1, { text: opts.prev_text, classes: opts.default_page_prev_class });
                }
                // Generate starting points
                if (interval[0] > 0 && opts.num_edge_entries > 0) {
                    var end = Math.min(opts.num_edge_entries, interval[0]);
                    for (var i = 0; i < end; i++) {
                        appendItem(i);
                    }
                    if (opts.num_edge_entries < interval[0] && opts.ellipse_text) {
                        jQuery("<span>" + opts.ellipse_text + "</span>").appendTo(panel);
                    }
                }
                // Generate interval links
                for (var i = interval[0]; i < interval[1]; i++) {
                    appendItem(i);
                }
                // Generate ending points
                if (interval[1] < np && opts.num_edge_entries > 0) {
                    if (np - opts.num_edge_entries > interval[1] && opts.ellipse_text) {
                        jQuery("<span>" + opts.ellipse_text + "</span>").appendTo(panel);
                    }
                    var begin = Math.max(np - opts.num_edge_entries, interval[1]);
                    for (var i = begin; i < np; i++) {
                        appendItem(i);
                    }

                }
                // Generate "Next"-Link
                if (opts.next_text && (current_page < np - 1 || opts.next_show_always)) {
                    appendItem(current_page + 1, { text: opts.next_text, classes: opts.default_page_next_class });
                }
            }

            // Extract current_page from options
            var current_page = opts.current_page;
            // Create a sane value for maxentries and items_per_page
            maxentries = (!maxentries || maxentries < 0) ? 1 : maxentries;
            opts.items_per_page = (!opts.items_per_page || opts.items_per_page < 0) ? 1 : opts.items_per_page;
            // Store DOM element for easy access from all inner functions
            var panel = jQuery(this);
            // Attach control functions to the DOM element 
            this.selectPage = function (page_id) { pageSelected(page_id); }
            this.prevPage = function () {
                if (current_page > 0) {
                    pageSelected(current_page - 1);
                    return true;
                }
                else {
                    return false;
                }
            }
            this.nextPage = function () {
                if (current_page < numPages() - 1) {
                    pageSelected(current_page + 1);
                    return true;
                }
                else {
                    return false;
                }
            }
            // When all initialisation is done, draw the links
            drawLinks();
            // call callback function
            //opts.callback(current_page, this);
        });
    }
})(jQuery);

(function ($) {

    var defaults = {
        url: "",                                                    //加载数据Url
        postData: {},                                               //Post参数
        currPageIndex: 1,                                           //当前显示页
        pageSize: 10,                                               //每页显示行数
        onLoaded: function (data, currPage, total, totalPage) { },     //加载完成事件   
        onLoadStart: function () { },                                   //加载前事件
        default_page_class: "",                                     //默认分页项样式
        default_page_prev_class: "pre",                            //默认上一页样式
        default_page_next_class: "next",                            //默认下一页样式
        prev_text: "上一页",
        next_text: "下一页",
        prev_show_always: true,
        next_show_always: true
    };

    var Digit = {
        /**
        * 进一法截取一个小数
        * @param float digit 要格式化的数字
        * @param integer length 要保留的小数位数
        * @return float
        */
        ceil: function (digit, length) {
            length = length ? parseInt(length) : 0;
            if (length <= 0) return Math.ceil(digit);
            digit = Math.ceil(digit * Math.pow(10, length)) / Math.pow(10, length);
            return digit;
        }
    };

    var methods = {
        //初始化
        init: function (options) {

            return this.each(function () {
                var $this = $(this);

                // build main options before element iteration
                var settings = $.extend({}, defaults, $this.data("settings"), options);
                // iterate and reformat each matched element  

                $this.data("settings", settings);
                handlers.onLoadStart.call($this);
                handlers.onLoading.call($this, settings.currPageIndex, true);
            });
        },
        //加载
        load: function (postData) {
            return methods.init.call(this, { postData: postData, currPageIndex: 1 });
        },
        //重新加载
        reload: function () {
            return methods.init.call(this);
        }
    };

    var handlers = {
        onLoadStart: function () {
            var $this = $(this), settings = $this.data("settings");
            if (settings.onLoadStart) settings.onLoadStart.call(this);
        },
        //获取数据
        onLoading: function (pageIndex, isReload) {
            var $this = $(this), settings = $this.data("settings");

            var postData = $.extend({
                pageSize: settings.pageSize,
                pageIndex: pageIndex
            }, settings.postData);

            $.post(settings.url, postData, function (responseData) {
                if (isReload) {
                    if (!responseData.IsSuccess) {
                        art.dialog({ content: responseData.Message, icon: "error", ok: true });

                        responseData.Total = 0;
                    }

                    $this.pagination(responseData.Total, {
                        items_per_page: settings.pageSize,
                        callback: $.proxy(handlers.onSelect, $this),
                        current_page: pageIndex - 1,
                        prev_text:settings.prev_text,
                        next_text:settings.next_text,
                        default_page_class: settings.default_page_class,
                        default_page_prev_class: settings.default_page_prev_class,
                        default_page_next_class: settings.default_page_next_class,
                        prev_show_always: settings.prev_show_always,
                        next_show_always: settings.next_show_always
                    });
                }

                //加载后
                handlers.onLoadComplete.apply(this);

                if (responseData.IsSuccess) {
                    if (settings.onLoaded) {
                        var totalPage = Digit.ceil(responseData.Total / settings.pageSize, 0);
                        if (totalPage == 0) totalPage = 1;
                        settings.onLoaded(responseData.Data, pageIndex, responseData.Total, totalPage);
                    }
                }


            }, "json");
        },
        onLoadComplete: function () {

        },
        //选择页码事件
        onSelect: function (pageIndex) {
            var $this = $(this), settings = $this.data("settings");

            settings.currPageIndex = pageIndex + 1;
            //$this.data("settings", settings);

            //加载前
            handlers.onLoadStart.apply(this);

            handlers.onLoading.call(this, (pageIndex + 1));
            return false;
        }
    };

    $.fn.pager = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('The method ' + method + ' does not exist in $.pager');
        }
    };
})(jQuery); 