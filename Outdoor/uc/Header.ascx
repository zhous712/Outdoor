<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Outdoor.uc.Header" %>
<div class="menu">
    <div class="logo left">
        <a href="javascript:void(0)">
            <img src="../images/logo.gif" width="120" height="47" alt="RadioBuy广告监播 广告监测 电台刊例 广告刊例" onclick="defaultPage()" /></a></div>
    <div class="menu_r right">
        <div id="divLexus" runat="server" class="lexuslogo left">
            <a href="javascript:void(0)">
                <img src="../images/lexus.jpg" width="286" height="50" alt="Lexus雷克萨斯" /></a></div>
        <div class="load">
            <ul>
                <%if (IsLogin)
                  { %>
                <li>您好，<span class="name" title="<%=userManager.Profile.CusName%>"><%=userManager.Profile.CusName%></span>！</li>
                <li><a href="javascript:void(0)" onclick="userLogout()">退出</a></li>
                <%}
                  else
                  {%>
                <li><a href="../Login.aspx">登录</a></li>
                <%} %>
            </ul>
        </div>
        <div class="menu_tel clearfix">
            <div class="tel" id="divTel" runat="server">
                <img src="../images/tel.png" width="172" height="20" alt="400-890-6365电话" /></div>
            <div class="clear">
            </div>
        </div>
    </div>
</div>
<div class="sub_ban">
    <div class="sub_banner">
        <img src="../images/sub_banner.jpg" />
    </div>
</div>
<script type="text/javascript">
    function userLogout() {
        $.ajax(
                { type: "POST", dataType: "json", asyn: true,
                    url: "/ashx/login.ashx",
                    data: { type: "4", "username": "", "password": "", "code": "" },
                    complete: function (XHR, TS) {
                        location.href = "/Login.aspx";
                    }
                }
                );
    }
    function defaultPage() {
        location.href = "/monitor/ScheduleList.aspx";
    }
</script>
