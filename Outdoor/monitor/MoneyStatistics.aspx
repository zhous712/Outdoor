<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoneyStatistics.aspx.cs" Inherits="Outdoor.monitor.MoneyStatistics" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>财务统计-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
    <jianboUI:Keyword ID="jianbo_Keyword" runat="server" />
    <link href="../css/base.css" rel="stylesheet" type="text/css" />
    <link href="../css/my_radio.css" rel="stylesheet" type="text/css" />
    <link href="../css/login.css" rel="stylesheet" type="text/css" />
    <link href="../css/paiqi.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/bgiframe-2.1.2-1/jquery.bgiframe.js" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.artDialog.source.js?skin=blue" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.alert.js" type="text/javascript"></script>
    <script type="text/javascript">
        function PayMoney(userid) {
            effect.Dialog.confirm("确定要支付吗?", function () {
                $.ajax({
                    url: "/ashx/Action.ashx",
                    type: "post",
                    data: "option=payMoney&userid=" + userid,
                    success: function (data) {
                        if (data == "true") {
                            effect.Dialog.alert("支付成功!");
                            location.reload();
                        }
                        else {
                            effect.Dialog.alert("抱歉,操作失败");
                            return false;
                        }
                    }
                });
            })
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="top_bg">
        <jianboUI:Header ID="jianbo_Header" runat="server" />
        <div class="border">
            <div class="position">
                <a href="ScheduleList.aspx">排期列表</a> >财务统计</div>
            <div class="my_radio">
                <h2 class="left">
                    我的RadioBuy</h2>
                <div class="search right" id="YesDivListTop" runat="server">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="bntSearch">
                        <asp:TextBox ID="txt_key" CssClass="sear_txt" runat="server" value="请输入昵称" onfocus="if(this.value=='请输入昵称')this.value='';this.focus();"
                            onblur="if(this.value=='')this.value='请输入昵称';" MaxLength="50"></asp:TextBox>
                        <asp:Button ID="bntSearch" runat="server" Text="搜 索" CssClass="sear_but" OnClick="bntSearch_Click"
                            UseSubmitBehavior="false" />
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
            </div>
            <div id="myRadiobuy" class="my_data">
                <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                <div class="my_right" id="YesDivList" runat="server">
                    <div class="clear">
                    </div>
                    <div class="tab clearfix">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr class="tab_tit">
                                <td>
                                    用户编号
                                </td>
                                <td>
                                    用户昵称
                                </td>
                                <td>
                                    应付金额
                                </td>
                                <td>
                                    操作
                                </td>
                            </tr>
                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                <ItemTemplate>
                                    <tr id="tr<%# Eval("UserId") %>">
                                        <td>
                                            <%#Eval("UserId").ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("NickName").ToString()%>
                                        </td>
                                        <td>
                                            <%# Eval("SumMoney").ToString()%>元
                                        </td>
                                        <td>
                                            <asp:Literal ID="litAction" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <div class="page">
                            <%=pageStr%>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <jianboUI:Footer ID="jianbo_Footer" runat="server" />
    </form>
</body>
</html>
