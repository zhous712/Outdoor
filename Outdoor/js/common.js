//验证email
function emailCheck(str) {
    var emailStr = str;
    //var emailPat = /^(.+)@(.+)$/;
    var emailPat = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    var matchArray = emailStr.match(emailPat);
    if (matchArray == null) {
        return false;
    }
    return true;
}

//CharMode函数 
//测试某个字符是属于哪一类. 
function CharMode(iN) {
    if (iN >= 48 && iN <= 57) //数字 
        return 1;
    if (iN >= 65 && iN <= 90) //大写字母 
        return 2;
    if (iN >= 97 && iN <= 122) //小写 
        return 4;
    else
        return 8; //特殊字符 
}

//bitTotal函数 
//计算出当前密码当中一共有多少种模式 
function bitTotal(num) {
    modes = 0;
    for (i = 0; i < 4; i++) {
        if (num & 1) modes++;
        num >>>= 1;
    }
    return modes;
}

//checkStrong函数 
//返回密码的强度级别 
function checkStrong(sPW) {
    if (sPW.length <= 3)
        return 0; //密码太短
    if (sPW.length > 16)
        return 16; 
    Modes = 0;
    for (i = 0; i < sPW.length; i++) {
        //测试每一个字符的类别并统计一共有多少种模式. 
        Modes |= CharMode(sPW.charCodeAt(i));
    }
    return bitTotal(Modes);
}

function phonecheck(s) {
    var str = s;
    var reg = /(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}1[0-9]{10}$)/;
    if (reg.test(str) == false) {
        return false;
    }
    else {
        return true;
    }
}

//验证特殊字符串的方法
function Validation(str) {   //str被验证的字符串
   var reg = /([<_%>'])+/;   
    if(reg.test(str))   
    {   
        return true;   
    }   
    else  
    {   
      return false;   
    } 
}

/**
* 验证邮政编码
*/
function checkTextDataForPOST(strValue) {
    var regTextPost = /^(\d){6}$/;
    return regTextPost.test(strValue);
}


/**
* 得到字符串的长度，汉字按两个字节计算
*/
function getStrActualLen(sChars) {
    return sChars.replace(/[^\x00-\xff]/g, "xx").length;
}

/*by zhangzhi*/
$(function () {
    if ($("#page_text").size() > 0) {
        userBrowser();
    }
});
function userBrowser() {
    var browserName = navigator.userAgent.toLowerCase();
    if (/msie/i.test(browserName) && !/opera/.test(browserName)) {
        if ($.browser.version == "9.0") {
            $("#page_text").removeAttr("onkeypress").attr("onkeypress", "return dokeyup(event)");
        }
        return;
    } else if (/firefox/i.test(browserName)) {
        $("#page_text").removeAttr("onkeypress").attr("onkeyup", "return dokeyup(event)");
        return;
    } else if (/chrome/i.test(browserName) && /webkit/i.test(browserName) && /mozilla/i.test(browserName)) {
        $("#page_text").removeAttr("onkeypress").attr("onkeyup", "return dokeyup(event)");
        return;
    } else if (/opera/i.test(browserName)) {
        $("#page_text").removeAttr("onkeypress").attr("onkeydown", "return dokeyup(event)");
        return;
    } else if (/webkit/i.test(browserName) && !(/chrome/i.test(browserName) && /webkit/i.test(browserName) && /mozilla/i.test(browserName))) {
        $("#page_text").removeAttr("onkeypress").attr("onkeyup", "return dokeyup(event)");
        return;
    } else {
        $("#page_text").removeAttr("onkeypress").attr("onkeydown", "return dokeyup(event)");
        return;
    }
}

function dokeyup(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "")
    keyCode = evt.keyCode ? evt.keyCode : (evt.which ? evt.which : evt.charCode);
    if (keyCode == 13) {
        if (Number($("#page_text").val()) > Number($("#page_text").attr("pcount"))) {
            effect.Dialog.alert("请输入正确页数！");
            return false;
         }
        else {
            location.href = $("#page_text").attr("rel") + $("#page_text").val();
            if ($.browser.msie) {
                event.keyCode = 0;
            }
            if ($.browser.version == "9.0") {
                return false;
            }
        }
    }
    if (!$.browser.version == "9.0") {
        return false;
    }
}


function DoNumber(obj) {
    obj.value = obj.value.replace(/[^\d*]/g, '')
}

function get3MonthBefor() {
    var resultDate, year, month, date, hms;
    var currDate = new Date();
    year = currDate.getFullYear();
    month = currDate.getMonth();
    date = currDate.getDate();
    hms = currDate.getHours() + ':' + currDate.getMinutes() + ':' + (currDate.getSeconds() < 10 ? '0' + currDate.getSeconds() : currDate.getSeconds());
    switch (month) {
        case 1:
        case 2:
        case 3:
            month += 9;
            year--;
            break;
        default:
            month -= 3;
            break;
    }
    month = (month < 10) ? ('0' + month) : month;
    //resultDate = year + '-' + month + '-' + date + ' ' + hms;
    return new Date(year, month, date);
}

//比较日期
function DateCompare(BeginDate, EndDate) {
    if (BeginDate == "") {
        effect.Dialog.alert('开始时间不能为空!');
        return false;
    }
    if (EndDate == "") {
        effect.Dialog.alert('结束时间不能为空!');
        return false;
    }
    var b_date = BeginDate.split('-');
    var e_date = EndDate.split('-');
    d1 = new Date(b_date[0], b_date[1] - 1, b_date[2]); //js时间月份是从0开始的 
    d2 = new Date(e_date[0], e_date[1] - 1, e_date[2]);
    if (d1 > d2) {
        effect.Dialog.alert('结束时间必须大于开始时间!');
        return false;
    }
    else {
        $("#spDateError").empty();
    }
//    var threeMonthBefor = get3MonthBefor();
//    if (d1 < threeMonthBefor) {
//        effect.Dialog.alert('只能搜索最近三个月的广告!');
//        return false;
//    }
    var time = parseInt(Math.abs(d1 - d2) / 1000 / 60 / 60 / 24);
    if (time > 365) {
        effect.Dialog.alert('监测周期最长不能大于一年!');
        return false;
    }
    else {
        $("#spDateError").empty();
        return true;
    }
}

//获取Js文件名后的参数
function getJsArgs(paras) {
    var sc = document.getElementsByTagName('script');
    var paramsArr = sc[sc.length - 1].src.split('?')[1].split('&');
    var args = {}, argsStr = [], param, t, name, value;
    for (var ii = 0, len = paramsArr.length; ii < len; ii++) {
        param = paramsArr[ii].split('=');
        name = param[0], value = param[1];
        if (typeof args[name] == "undefined") { //参数尚不存在
            args[name] = value;
        } else if (typeof args[name] == "string") { //参数已经存在则保存为数组
            args[name] = [args[name]]
            args[name].push(value);
        } else {  //已经是数组的
            args[name].push(value);
        }
    }
    var returnValue = args[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}

/**
* 判断时间点位格式是否正确
*/
function CheckTimePoint(hourTime, minuteTime) {
    if (hourTime == "" || hourTime == "0") hourTime = "00";
    if (minuteTime == "" || minuteTime == "0") minuteTime = "00";
    if (parseInt(hourTime) < 10) hourTime = "0" + parseInt(hourTime);
    if (parseInt(minuteTime) < 10) minuteTime = "0" + parseInt(minuteTime);
    var time = hourTime + ":" + minuteTime;
    var reg = /^((20|21|22|23|[0-1]\d)\:[0-5][0-9])?$/;
    if (!reg.test(time)) {
        return false;
    }
    else {
        return true;
    }
}
/* 
用途：检查输入字符串是否符合正整数格式 
输入：s：字符串 
返回：如果通过验证返回true,否则返回false 
*/
function isNumber(s) {
    var regu = "^[0-9]+$";
    var re = new RegExp(regu);
    if (s.search(re) != -1) {
        return true;
    }
    else {
        return false;
    }
};

function GetHour(hourStr) {
    if (hourStr == "" || hourStr == "0") hourStr = "00";
    if (parseInt(hourStr) < 10) hourStr = "0" + parseInt(hourStr);
    return hourStr;
}

function GetMinute(minuteStr) {
    if (minuteStr == "" || minuteStr == "0") minuteStr = "00";
    if (parseInt(minuteStr) < 10) minuteStr = "0" + parseInt(minuteStr);
    return minuteStr;
}

function DateAdd(dtTmp, strInterval, Number) {
    switch (strInterval) {
        case 's': return new Date(dtTmp.getFullYear(), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds() + Number);

        case 'n': return new Date(dtTmp.getFullYear(), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes() + Number, dtTmp.getSeconds());

        case 'h': return new Date(dtTmp.getFullYear(), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours() + Number, dtTmp.getMinutes(), dtTmp.getSeconds());

        case 'd': return new Date(dtTmp.getFullYear(), dtTmp.getMonth(), dtTmp.getDate() + Number, dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());

        case 'w': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()), dtTmp.getDate() + Number * 7, dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());

        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());

        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());

        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}