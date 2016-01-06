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
	var controldate20_monitor = $('#inputDate20_monitor');//监播扩展2014.1.14，zhugz
	var controldate20_monitorPlan = $('#inputDate20_monitorPlan');//监播扩展2014.3.10，zhugz
	var controldate20_monitorPlanV3 = $('#inputDate20_monitorPlanV3');//监播扩展2014.3.10，zhugz
	var controldate20_monitorPlanV3Report = $('#inputDate20_monitorPlanV3Report');//监播扩展2014.7.28，zhugz,V3.01监播报告页面
        var controldate20_filterRun = $('#inputDate20_filterRun');//确认报告筛选运行
        controldate10.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate10.val()).split("-")[0].toString(), getDates(controldate10.val()).split("-")[1].toString()],
            current: controldate10.val() == "" ? nowdate() : controldate10.val(),
            calendars: 1,
            starts: 1,
            mode: 'range',
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
            }
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
	//监播扩展2014.1.14，zhugz
	controldate20_monitor.DatePicker({
            format: 'Y.m.d',
            date: [getDates(controldate20_monitor.val()).split("-")[0].toString(), getDates(controldate20_monitor.val()).split("-")[1].toString()],
            current: controldate20_monitor.val() == "" ? nowdate() : controldate20_monitor.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {
                var can = controldate20_monitor.attr("id");
                $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate_monitor('" + can + "')\"  class=\"date_confirm\">");
                if (controldate20_monitor.val() == "") {
                    controldate20_monitor.val(nowdate());
                    controldate20_monitor.DatePickerSetDate(controldate20_monitor.val(), true);
                }
            },
            onChange: function (formated) {
                if (formated.toString().indexOf("NaN") == -1) {
                    controldate20_monitor.val(subDate(formated.toString().replace(",", "-")));
                }
            }
        });
	//监播扩展2014.3.10，zhugz
	controldate20_monitorPlan.DatePicker({	 
            format: 'Y.m.d',
            date: [getDates(controldate20_monitorPlan.val()).split("-")[0].toString(), getDates(controldate20_monitorPlan.val()).split("-")[1].toString()],
            current: controldate20_monitorPlan.val() == "" ? nowdate() : controldate20_monitorPlan.val(),
            calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {		
                    var can = controldate20_monitorPlan.attr("id");
                    $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate_monitorPlan('" + can + "')\"  class=\"date_confirm\">");
                    if (controldate20_monitorPlan.val() == "") {
                       controldate20_monitorPlan.val(nowdate());
                       controldate20_monitorPlan.DatePickerSetDate(controldate20_monitorPlan.val(), true);
                    }		
            },
            onChange: function (formated) {
		if (formated.toString().indexOf("NaN") == -1) {
                        controldate20_monitorPlan.val(subDate(formated.toString().replace(",", "-")));
                    }		
            }
       });
	//监播V3.0扩展2014.5.12，zhugz
	controldate20_monitorPlanV3.DatePicker({	 
            format: 'Y.m.d',
            date: [getDates(controldate20_monitorPlanV3.val()).split("-")[0].toString(), getDates(controldate20_monitorPlanV3.val()).split("-")[1].toString()],
            current: controldate20_monitorPlanV3.val() == "" ? nowdate() : controldate20_monitorPlanV3.val(),
	    calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {		
                    var can = controldate20_monitorPlanV3.attr("id");
                    $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate_monitorPlanV3('" + can + "')\"  class=\"date_confirm\">");
                    if (controldate20_monitorPlanV3.val() == "") {
                       controldate20_monitorPlanV3.val(nowdate());
                       //controldate20_monitorPlanV3.DatePickerSetDate(controldate20_monitorPlanV3.val(), true);
		      controldate20_monitorPlanV3.DatePickerSetDate(ShieldDatePicker.DefaultDateBox(), true);
                    }	
		 ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.7.21,免费监播	
            },
            onChange: function (formated) {		
		if (formated.toString().indexOf("NaN") == -1) {
                        controldate20_monitorPlanV3.val(subDate(formated.toString().replace(",", "-")));
                    }		
		ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.7.21,免费监播
            }
        });
	//监播V3.1扩展2014.7.28，zhugz
	controldate20_monitorPlanV3Report.DatePicker({	 
            format: 'Y.m.d',
            date: [getDates(controldate20_monitorPlanV3Report.val()).split("-")[0].toString(), getDates(controldate20_monitorPlanV3Report.val()).split("-")[1].toString()],
            current: controldate20_monitorPlanV3Report.val() == "" ? nowdate() : $("#hfPlanV3ReportDefaultDateBox").val(),
	    calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {		
                    var can = controldate20_monitorPlanV3Report.attr("id");
                    $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate_monitorPlanV3Report('" + can + "')\"  class=\"date_confirm\">");
                    if (controldate20_monitorPlanV3Report.val() == "") {
                       controldate20_monitorPlanV3Report.val(nowdate());
		      controldate20_monitorPlanV3Report.DatePickerSetDate(ShieldDatePicker.DefaultDateBox(), true);
                    }	
		 ShieldDatePicker.DateSelect();//屏蔽日期选择
            },
            onChange: function (formated) {		
		if (formated.toString().indexOf("NaN") == -1) {
                        controldate20_monitorPlanV3Report.val(subDate(formated.toString().replace(",", "-")));
                    }		
		ShieldDatePicker.DateSelect();//屏蔽日期选择
            }
        });
	//Vb1.0.3，zhugz，2014.12.25,确认报告筛选运行
	controldate20_filterRun.DatePicker({	 
            format: 'Y.m.d',
            date: [getDates(controldate20_filterRun.val()).split("-")[0].toString(), getDates(controldate20_filterRun.val()).split("-")[1].toString()],
            current: controldate20_filterRun.val() == "" ? nowdate() : controldate20_filterRun.val(),
	    calendars: 2,
            starts: 1,
            mode: 'range',
            position: 'bottom',
            onBeforeShow: function () {		
                    var can = controldate20_filterRun.attr("id");
                    $('.datepickerBorderDD').html("<input id=\"btndateclear\" type=\"button\" value=\"清空\" onclick=\"cleardate('" + can + "')\" class=\"date_reset\"><input id=\"btndatesubmit\" type=\"button\" value=\"确定\" onclick=\"submitdate('" + can + "')\"  class=\"date_confirm\">");
                    if (controldate20_filterRun.val() == "") {
                       controldate20_filterRun.val(nowdate());
		      controldate20_filterRun.DatePickerSetDate(ShieldDatePicker.DefaultDateBox(), true);
                    }	
		 ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.11.6,免费监播	
            },
            onChange: function (formated) {		
		if (formated.toString().indexOf("NaN") == -1) {
                        controldate20_filterRun.val(subDate(formated.toString().replace(",", "-")));
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
        $('#imgDate21').bind('click', function () {
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
	//监播扩展2014.1.14，zhugz
	 $('#imgDate20_monitor').bind('click', function () {
            $('#inputDate20_monitor').DatePickerShow();
            return false;
        });
	//监播扩展2014.3.10，zhugz
	 $('#imgDate20_monitorPlan').bind('click', function () {
		if($("#hfEditChid").val()=="0")
   		{	
		    effect.Dialog.alert("请选择电台！");
        	    return false;	
		}
		else
		{
            	    $('#inputDate20_monitorPlan').DatePickerShow();
            	    return false;
		}
        });
	//监播V3.0扩展2014.5.12，zhugz
	 $('#imgDate20_monitorPlanV3').bind('click', function () {
            $('#inputDate20_monitorPlanV3').DatePickerShow();
	    ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.7.21,免费监播
            return false;
        });
	//监播V3.1扩展2014.7.28，zhugz
	 $('#imgDate20_monitorPlanV3Report').bind('click', function () {
            $('#inputDate20_monitorPlanV3Report').DatePickerShow();
	    ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.7.21,免费监播
            return false;
        });
        //监播Vb1.0.3扩展2014.12.25，zhugz,确认报告筛选运行
	 $('#imgDate20_filterRun').bind('click', function () {
            $('#inputDate20_filterRun').DatePickerShow();
	    ShieldDatePicker.DateSelect();//屏蔽日期选择,2014.12.25,免费监播
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
//监播扩展2014.1.14，zhugz
function submitdate_monitor(obj) {
    var selectDate = $('#' + obj).DatePickerGetDate();
    if (selectDate == "" || selectDate.toString().indexOf("Invalid") > -1) {
        alert("您没有选择日期。");
        return false;
    } else {
        $('#' + obj).DatePickerHide();
	var url=window.location.href;
        var DateVal=$('#' + obj).val();
        var strdate=getQueryString("strdate");
        if(strdate=="")
        {
         strdate="strdate=";
         DateVal="strdate="+DateVal;
	 window.location.href=url.replace(strdate,DateVal);  
         }
        else
         {
	 window.location.href=url+"&strdate="+DateVal;
	}
	      
    }
}
//监播扩展2014.3.10，zhugz
function submitdate_monitorPlan(obj) {
   if($("#hfEditChid").val()=="0")
   {	effect.Dialog.alert("请选择电台！");
        return false;	
	}
	else
{
    var selectDate = $('#' + obj).DatePickerGetDate();
    if (selectDate == "" || selectDate.toString().indexOf("Invalid") > -1) {
        alert("您没有选择日期。");
        return false;
    } else {
        $('#' + obj).DatePickerHide();	
	var DateVal=$('#' + obj).val();	
	var beginDate=DateVal.split('-')[0];
	var endDate=DateVal.split('-')[0];
	if(DateVal.split('-').length>1)
	{
		beginDate=DateVal.split('-')[0];
		endDate=DateVal.split('-')[1];
	}
	GetPlanDay();//该方法对应监播前台确认排期页面
        return true;
    }
	}
}
//监播V3.1扩展搜索，2014.7.28
function submitdate_monitorPlanV3Report(obj)
{
    var selectDate = $('#' + obj).DatePickerGetDate();    
    if (selectDate == "" || selectDate.toString().indexOf("Invalid") > -1) {
        effect.Dialog.alert("您没有选择日期。");
        return false;
    } else {	
        $('#' + obj).DatePickerHide();	
	__doPostBack("hidButton", "");
        return true;
    }
}
//监播V3.0扩展周期只能为31天，2014.5.12，zhugz
function submitdate_monitorPlanV3(obj) {
    $("div.fSubmit p.fPay").hide();
    var selectDate = $('#' + obj).DatePickerGetDate();    
    if (selectDate == "" || selectDate.toString().indexOf("Invalid") > -1) {
        alert("您没有选择日期。");
        return false;
    } else {	
	var DateVal=$('#' + obj).val();
	var BeginDate=DateVal.split('-')[0];
	var EndDate=DateVal.split('-')[1];
	if(EndDate=='undefined'||EndDate==""||EndDate==null) EndDate=BeginDate;	
	var day=daysBetween(BeginDate.replace('.','-').replace('.','-'),EndDate.replace('.','-').replace('.','-'));
	if((day+1)>31)
	{
	  $('#' + obj).DatePickerHide();
          effect.Dialog.alert("监播周期最多只能选择31天！",function(){
	  	
                $('#' + obj).DatePickerShow();
	  });
          return false;
        }
	else
	{	
		$('#' + obj).parent().find("p.fWarn").hide();
		var orgMonitorPlan = $("#divPlanTable thead tr.trDate td").eq(1).text();
	  	var currDate = new Date();
                day = daysBetween(BeginDate.replace('.', '-').replace('.', '-'), currDate.getFullYear() + "-" + (currDate.getMonth() + 1) + "-" + currDate.getDate());
                if ((day + 1) > 90 && (day + 1) <= 365) {
                    $("div.fSubmit p.fPay").show();
                }
                if ((day + 1) > 365) {
		    $('#' + obj).DatePickerHide();
                    effect.Dialog.alert("您的监播任务已超过一年，请联系客服进行人工建单！",function(){	  	
                    	$('#' + obj).DatePickerShow();
	  	    });
                    return false;
                }
		if (orgMonitorPlan != BeginDate+"-"+EndDate && orgMonitorPlan != "") {
                	var points = "";
                	$("#divPlanTable tbody tr.trDays").each(function () {
                    		points += $(this).attr("point") + ",";
                	});
                	//if (points != "") {
				$('#' + obj).DatePickerHide();
                    		effect.Dialog.confirm("修改监测周期，点位排期将重置，确定修改吗？", function () {
                        		Plan.Initial();
                    		},function(){
					$('#' + obj).val(orgMonitorPlan);
				});
                    		return false;
                	//}
			//else{
			//	$('#' + obj).DatePickerHide();	
          		//	return true;
			//}
            	}
		else{
          		$('#' + obj).DatePickerHide();	
          		return true;
		}
	}
    }
}
//js获取url参数值
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
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
function daysBetween(DateOne,DateTwo)  
    {   
        var OneMonth = DateOne.substring(5,DateOne.lastIndexOf('-'));  
        var OneDay = DateOne.substring(DateOne.length,DateOne.lastIndexOf('-')+1);  
        var OneYear = DateOne.substring(0,DateOne.indexOf('-'));  
  
        var TwoMonth = DateTwo.substring(5,DateTwo.lastIndexOf('-'));  
        var TwoDay = DateTwo.substring(DateTwo.length,DateTwo.lastIndexOf('-')+1);  
        var TwoYear = DateTwo.substring(0,DateTwo.indexOf('-'));  
  
        var cha=((Date.parse(OneMonth+'/'+OneDay+'/'+OneYear)- Date.parse(TwoMonth+'/'+TwoDay+'/'+TwoYear))/86400000);   
        return Math.abs(cha);  
    }
