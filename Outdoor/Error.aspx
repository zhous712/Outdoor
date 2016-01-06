<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Outdoor.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出错了- RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
    <jianboUI:Keyword ID="jianbo_Keyword" runat="server" />
    <link href="css/base.css" rel="stylesheet" type="text/css" />
    <link href="css/error.css" rel="stylesheet" type="text/css" />
    <script src="../jquery-1.7.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            function redirect(t) {
                //整体就是一个setTimeout的用法
                window.setTimeout(function () {
                    t--; //自减
                    if (t > 0) {
                        document.getElementById("tnum").innerHTML = t;
                        redirect(t);
                    } else { location.href = "/"; }
                }, 1000);
            }
            redirect(10);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div class="top_bg">
	<jianboUI:Header ID="jianbo_Header" runat="server" />
    <div class="border">
        <div class="error_box">
                <div class="err_left left"><img src="images/error.jpg" width="170" height="190" /></div>
                <div class="err_right left">
                  <p><img src="images/error_font.gif" width="255" height="29" /></p>
                  <div class="err_wid">
                    <p class="err_jump left">页面在 <span id="tnum">10</span> 秒之后，自动跳转到 <span><a href="index.aspx">首页</a></span> 或选择进入 </p>
            		<div class="err_menu"><a href="/monitor/add.aspx" class="btnBlue"  />快速监播</a></div>
                    <div class="clear"></div>
				</div>
                    <div class="err_result">
                        <h3>出错原因</h3>
                        <p>
                            1、您访问的内容不存在了！<br />
    
                            2、您访问的链接地址出错了，请检查！<br />
    
                            3、您的访问已超过权限！                   
                        </p>
                        <p class="err_intr">最终解释权归RadioBuy所有</p>
                    </div>
                </div>
                <div class="clear"></div>
</div>
    </div>
</div>
<jianboUI:Footer ID="jianbo_Footer" runat="server" />
    </form>
</body>
</html>
