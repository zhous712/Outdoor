(function ($) {
    var initLayout = function () {
        var hash = window.location.hash.replace('#', '');
        var currentTab = $('ul.navigationTabs a')
            .bind('click', showTab)
            .filter('a[rel=' + hash + ']');
        if (currentTab.size() == 0) {
            currentTab = $('ul.navigationTabs a:first');
        }
        showTab.apply(currentTab.get(0));
	var controldate01 = $('#inputDate01');
        var controldate10 = $('#inputDate10');
        var controldate11 = $('#inputDate11');
        var controldate12 = $('#inputDate12');
        var controldate13 = $('#inputDate13');
        var controldate20 = $('#inputDate20');
        var controldate21 = $('#inputDate21');
        var controldate22 = $('#inputDate22');
        var controldate23 = $('#inputDate23');
        var controldate24 = $('#inputDate24');
        var controldate25 = $('#inputDate25');
	var controldate20_monitorFree = $('#inputDate20_monitorFree');//监播免费版

	controldate01.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate01.val()).split("-")[0].toString(), getDates(controldate01.val()).split("-")[1].toString()],
            current: controldate01.val() == "" ? nowdate() : controldate01.val(),
            calendars: 1,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate01.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate01.val() == "") {
                    controldate01.val(nowdate());
                    controldate01.DatePickerSetDate(controldate01.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate01.val(subDate(formated.toString().replace(",", "-")));
                }
            },
            starts: 0
        });
        controldate11.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate11.val()).split("-")[0].toString(), getDates(controldate11.val()).split("-")[1].toString()],
            current: controldate11.val() == "" ? nowdate() : controldate11.val(),
            calendars: 1,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate11.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate11.val() == "") {
                    controldate11.val(nowdate());
                    controldate11.DatePickerSetDate(controldate11.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate11.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate10.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate10.val()).split("-")[0].toString(), getDates(controldate10.val()).split("-")[1].toString()],
            current: controldate10.val() == "" ? nowdate() : controldate10.val(),
            calendars: 1,
            starts: 1,
            mode: 'single',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate10.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate10.val() == "") {
                    controldate10.val(nowdate());
                    controldate10.DatePickerSetDate(controldate10.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate10.val(subDate(formated.toString().replace(",", "-")));
                }
            },
            onRender: function (date) {
                return {
                    disabled: (controldate10.attr("min") != undefined ? (controldate10.attr("min").valueOf() > formatdate(date).valueOf()) : false),
                    className: (controldate10.attr("min") != undefined ? (controldate10.attr("min").valueOf() > formatdate(date).valueOf()) : false) ? 'datepickerNotInMonth' : false
                }
            },
            starts: 0
        });
        controldate11.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate11.val()).split("-")[0].toString(), getDates(controldate11.val()).split("-")[1].toString()],
            current: controldate11.val() == "" ? nowdate() : controldate11.val(),
            calendars: 1,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate11.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate11.val() == "") {
                    controldate11.val(nowdate());
                    controldate11.DatePickerSetDate(controldate11.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate11.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate12.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate12.val()).split("-")[0].toString(), getDates(controldate12.val()).split("-")[1].toString()],
            current: controldate12.val() == "" ? nowdate() : controldate12.val(),
            calendars: 1,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate12.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate12.val() == "") {
                    controldate12.val(nowdate());
                    controldate12.DatePickerSetDate(controldate12.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate12.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate13.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate13.val()).split("-")[0].toString(), getDates(controldate13.val()).split("-")[1].toString()],
            current: controldate13.val() == "" ? nowdate() : controldate13.val(),
            calendars: 1,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate13.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate13.val() == "") {
                    controldate13.val(nowdate());
                    controldate13.DatePickerSetDate(controldate13.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate13.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate20.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate20.val()).split("-")[0].toString(), getDates(controldate20.val()).split("-")[1].toString()],
            current: controldate20.val() == "" ? nowdate() : controldate20.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate20.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate20.val() == "") {
                    controldate20.val(nowdate());
                    controldate20.DatePickerSetDate(controldate20.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate20.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate21.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate21.val()).split("-")[0].toString(), getDates(controldate21.val()).split("-")[1].toString()],
            current: controldate21.val() == "" ? nowdate() : controldate21.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate21.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate21.val() == "") {
                    controldate21.val(nowdate());
                    controldate21.DatePickerSetDate(controldate21.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate21.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate22.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate22.val()).split("-")[0].toString(), getDates(controldate22.val()).split("-")[1].toString()],
            current: controldate22.val() == "" ? nowdate() : controldate22.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate22.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate22.val() == "") {
                    controldate22.val(nowdate());
                    controldate22.DatePickerSetDate(controldate22.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate22.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate23.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate23.val()).split("-")[0].toString(), getDates(controldate23.val()).split("-")[1].toString()],
            current: controldate23.val() == "" ? nowdate() : controldate23.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate23.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate23.val() == "") {
                    controldate23.val(nowdate());
                    controldate23.DatePickerSetDate(controldate23.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate23.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate24.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate24.val()).split("-")[0].toString(), getDates(controldate24.val()).split("-")[1].toString()],
            current: controldate24.val() == "" ? nowdate() : controldate24.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate24.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate24.val() == "") {
                    controldate24.val(nowdate());
                    controldate24.DatePickerSetDate(controldate24.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate24.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
        controldate25.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate25.val()).split("-")[0].toString(), getDates(controldate25.val()).split("-")[1].toString()],
            current: controldate25.val() == "" ? nowdate() : controldate25.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate25.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                if (controldate25.val() == "") {
                    controldate25.val(nowdate());
                    controldate25.DatePickerSetDate(controldate25.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate25.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
	//监播V3.0.2扩展2014.11.6，zhugz
	controldate20_monitorFree.DatePicker({	 
            format: 'Y.m.d',
            date: [getDates(controldate20_monitorFree.val()).split("-")[0].toString(), getDates(controldate20_monitorFree.val()).split("-")[1].toString()],
            current: controldate20_monitorFree.val() == "" ? nowdate() : controldate20_monitorFree.val(),
	    calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {		
                    var can = controldate20_monitorFree.attr("id");
                    $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                    if (controldate20_monitorFree.val() == "") {
                       controldate20_monitorFree.val(nowdate());
		      controldate20_monitorFree.DatePickerSetDate(ShieldDatePicker.DefaultDateBox(), true);
                    }	
		 ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.11.6,免费监播	
            },
            onChange: function (formated) {		
		if (formated.toString().indexOf("NaN") == -1) {
                        controldate20_monitorFree.val(subDate(formated.toString().replace(",", "-")));
                    }		
		ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.11.6,免费监播
            }
        });
        $('#imgDate10').bind('click', function () {
            $('#inputDate10').DatePickerShow();
            return false;
        });
        $('#imgDate11').bind('click', function () {
            $('#inputDate11').DatePickerShow();
            return false;
        });
        $('#imgDate12').bind('click', function () {
            $('#inputDate12').DatePickerShow();
            return false;
        });
        $('#imgDate13').bind('click', function () {
            $('#inputDate13').DatePickerShow();
            return false;
        });
        $('#imgDate20').bind('click', function () {
            $('#inputDate20').DatePickerShow();
            return false;
        });
        //$('#imgDate21').bind('click', function () {
        //    $('#inputDate21').DatePickerShow();
        //    return false;
        //});
	$("#imgDate21").die().live("click",function(){
	    $('#inputDate21').DatePickerShow();
	    return false;
	});
        $('#imgDate22').bind('click', function () {
            $('#inputDate22').DatePickerShow();
            return false;
        });
        $('#imgDate23').bind('click', function () {
            $('#inputDate23').DatePickerShow();
            return false;
        });
        $('#imgDate24').bind('click', function () {
            $('#inputDate24').DatePickerShow();
            return false;
        });
        $('#imgDate25').bind('click', function () {
            $('#inputDate25').DatePickerShow();
            return false;
        });
	//监播V3.0.2扩展2014.11.6，zhugz
	 $('#imgDate20_monitorFree').bind('click', function () {
            $('#inputDate20_monitorFree').DatePickerShow();
	    ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.11.6,免费监播
            return false;
        });
    };
    var showTab = function () {
        var tabIndex = $('ul.navigationTabs a')
            .removeClass('active')
            .index(this);
        $(this)
            .addClass('active')
            .blur();
        $('div.tab')
            .hide()
            .eq(tabIndex)
            .show();
    };

    EYE.register(initLayout, 'init');
})(jQuery);
function nowdate() {
    var today = new Date();
    var day = today.getDate();
    //获取当前日(1-31)                
    var month = today.getMonth() + 1;
    //显示月份比实际月份小1,所以要加1   
    var year = today.getFullYear();
    //获取完整的年份
    month = month < 10 ? "0" + month : month;
    //数字<10，实际显示为，如5，要改成05   
    day = day < 10 ? "0" + day : day;
    var data = year + "." + month + "." + day;
    return data;
}
function formatdate(obj) {
    var today = obj;
    var day = today.getDate();
    //获取当前日(1-31)                
    var month = today.getMonth() + 1;
    //显示月份比实际月份小1,所以要加1   
    var year = today.getFullYear();
    //获取完整的年份
    month = month < 10 ? "0" + month : month;
    //数字<10，实际显示为，如5，要改成05   
    day = day < 10 ? "0" + day : day;
    var data = year + "." + month + "." + day;
    return data;
}
function cleardate(obj) {
    $('#' + obj).DatePickerClear();
    $('#' + obj).val("");
    $('#' + obj).DatePickerHide();
    $('#' + obj).focus();
}
function submitdate(obj) {
    var selectDate = $('#' + obj).DatePickerGetDate();
    if (selectDate == "" || selectDate.toString().indexOf("Invalid") > -1) {
        alert("您没有选择日期。");
        return false;
    } else {
        $('#' + obj).DatePickerHide();
        return true;
    }
}
function subDate(olddate) {
    var date = olddate.toString().indexOf("-");
    if (date > 0) {
        var startdate = olddate.toString().split("-")[0];
        var enddate = olddate.toString().split("-")[1];
        if (startdate == enddate) {
            return startdate;
        }
    }
    return olddate.toString();
}
function getDates(date) {
    if (date == "underfined" || date == null || date == "") {
        var a = nowdate().toString() + "-" + nowdate().toString();
        return a;
    }
    var getdate = date.indexOf("-");
    if (getdate > 0) {
        return date;
    }
    return date + "-" + date;
}