<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Outdoor.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户登录- RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
    <jianboUI:Keyword ID="jianbo_Keyword" runat="server" />
    <link href="css/base.css" rel="stylesheet" type="text/css" />
    <link href="css/my_radio.css" rel="stylesheet" type="text/css" />
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.artDialog.source.js?skin=blue" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.alert.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="top_bg">
        <jianboUI:Header ID="jianbo_Header" runat="server" />
        <div class="border">
            <div class="position">
                <a href="monitor/ScheduleList.aspx">首页</a> > 登录</div>
            <div id="log" style="width:400px">
                <div class="logStyle">
                    <img src="images/mainStyle.jpg" alt="RadioBuy广告监播 广告监测 电台刊例 广告刊例" /></div>
                <div class="intro" style="top:280px; text-indent:0px">
                    户外广告的执行情况你知道吗？<br />
                    暴风雨过后你的广告还在吗？天黑了灯箱广告亮了吗？你的广告画面展现完整吗？你是否感到对广告投放结果检验鞭长莫及？<br />
                    快用RadioBuy吧，帮你解决户外监测的一切难题
                </div>
                <div class="log">
                    <ul class="item">
                        <li>
                            <label>
                                用户名：</label><input type="text" id="txtUserName" clientidmode="Static" class="text"
                                    size="20" onkeyup="dokeyup(event)" /></li>
                        <li>
                            <label>
                                密&nbsp;&nbsp;码：</label><input type="password" id="txtPwd" clientidmode="Static" class="text"
                                    size="20" onkeyup="dokeyup(event)" /></li>
                    </ul>
                    <div class="btnBox of2">
                        <input id="btnLogin" onclick="floginlogin();" type="button" value="登 录" class="btnBlue" /><input
                            type="reset" value="重 置" class="btnBlue" /></div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <jianboUI:Footer ID="jianbo_Footer" runat="server" />
    </form>
    <script src="js/jquery.cookie.js" type="text/javascript"></script>
    <script type="text/javascript">
        function dokeyup(evt) {
            evt = (evt) ? evt : ((window.event) ? window.event : "");
            keyCode = evt.keyCode ? evt.keyCode : (evt.which ? evt.which : evt.charCode);
            if (keyCode == 13) {
                floginlogin();
            }
        }
        function floginlogin() {
            if ($.trim(document.getElementById("txtUserName").value) == "") { effect.Dialog.alert("请输入用户名."); return };
            if ($.trim(document.getElementById("txtPwd").value) == "") { effect.Dialog.alert("请输入密码."); return };
            $.post("/ashx/login.ashx?m" + Math.random(), { type: "1", username: $("#txtUserName").val(), password: $("#txtPwd").val() }, function (data) {
                if (data != "") {
                    var result = eval("(" + data + ")");
                    if (result != undefined) {
                        if (result.status == "1") {
                            if (request('ref') != null && request('ref').length != 0) {
                                var UrlRef = Url.decode(request('ref')).replace("+", "&");
                                //var UrlRef =request('ref').replace("+", "&");
                                UrlRef = UrlRef.replace("+", "&");
                                location.href = UrlRef;
                            }
                            else {
                                location.href = "monitor/ScheduleList.aspx";
                            }
                        }
                        else if (result.status == "-2") { effect.Dialog.alert("用户名或密码错误！"); }
                        else if (result.status == "-5") { effect.Dialog.alert("用户帐号已停用！"); }
                        else { effect.Dialog.alert("登录错误！"); }
                    }
                    else {
                        effect.Dialog.alert("用户名或密码错误！");
                    }
                }
            });
        }
        //获取Url参数
        function request(paras) {
            var url = location.href;
            var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
            var paraObj = {}
            for (i = 0; j = paraString[i]; i++) {
                paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
            }
            var returnValue = paraObj[paras.toLowerCase()];
            if (typeof (returnValue) == "undefined") {
                return "";
            } else {
                return returnValue;
            }
        }
        //jquery版UrlEncode、UrlDecode
        var Url = {

            // public method for url encoding
            encode: function (string) {
                return escape(this._utf8_encode(string));
            },

            // public method for url decoding
            decode: function (string) {
                return this._utf8_decode(unescape(string));
            },

            // private method for UTF-8 encoding
            _utf8_encode: function (string) {
                string = string.replace('//r/n/g', "/n");

                var utftext = "";

                for (var n = 0; n < string.length; n++) {

                    var c = string.charCodeAt(n);

                    if (c < 128) {
                        utftext += String.fromCharCode(c);
                    }
                    else if ((c > 127) && (c < 2048)) {
                        utftext += String.fromCharCode((c >> 6) | 192);
                        utftext += String.fromCharCode((c & 63) | 128);
                    }
                    else {
                        utftext += String.fromCharCode((c >> 12) | 224);
                        utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                        utftext += String.fromCharCode((c & 63) | 128);
                    }

                }

                return utftext;
            },

            // private method for UTF-8 decoding
            _utf8_decode: function (utftext) {
                var string = "";
                var i = 0;
                var c = c1 = c2 = 0;

                while (i < utftext.length) {

                    c = utftext.charCodeAt(i);

                    if (c < 128) {
                        string += String.fromCharCode(c);
                        i++;
                    }
                    else if ((c > 191) && (c < 224)) {
                        c2 = utftext.charCodeAt(i + 1);
                        string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                        i += 2;
                    }
                    else {
                        c2 = utftext.charCodeAt(i + 1);
                        c3 = utftext.charCodeAt(i + 2);
                        string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                        i += 3;
                    }

                }

                return string;
            }
        }
    </script>
</body>
</html>
