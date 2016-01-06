<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="Outdoor.monitor.CustomerList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>账号管理-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
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
        function IsDisabled(cusid, type) {
            effect.Dialog.confirm(type == 0 ? "确认要启用吗？" : "确认要停用吗？", function () {
                if ('<%=base.Profile.CusId %>' != '0') {
                    $.ajax({
                        url: "/ashx/Action.ashx",
                        type: "post",
                        data: "option=isDisabled&cusid=" + cusid + "&isdisabled=" + type,
                        success: function (data) {
                            if (data == "true") {
                                effect.Dialog.alert("操作成功!");
                                location.reload();
                            }
                            else {
                                effect.Dialog.alert("抱歉,操作失败");
                                return false;
                            }
                        }
                    });
                }
            })
        }

        function UnBind(uid) {
            effect.Dialog.confirm("确认要解绑吗？", function () {
                if ('<%=base.Profile.CusId %>' != '0') {
                    $.ajax({
                        url: "/ashx/Action.ashx",
                        type: "post",
                        data: "option=unBind&userid=" + uid,
                        success: function (data) {
                            if (data == "true") {
                                effect.Dialog.alert("操作成功!");
                                location.reload();
                            }
                            else {
                                effect.Dialog.alert("抱歉,操作失败");
                                return false;
                            }
                        }
                    });
                }
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
                    <a href="CustomerList.aspx">账号管理</a> >
                账号列表
                </div>
                <div class="my_radio">
                    <h2 class="left">我的RadioBuy</h2>
                    <div class="search right" id="YesDivListTop" runat="server">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="bntSearch">
                            <asp:TextBox ID="txt_key" CssClass="sear_txt" runat="server" value="请输入账号" onfocus="if(this.value=='请输入账号')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入账号';" MaxLength="50"></asp:TextBox>
                            <asp:Button ID="bntSearch" runat="server" Text="搜 索" CssClass="sear_but" OnClick="bntSearch_Click"
                                UseSubmitBehavior="false" />
                            <asp:Button ID="btnAddCustomer" runat="server" Text="添 加" CssClass="sear_but" OnClick="btnAddCustomer_Click"
                                UseSubmitBehavior="false" />
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="myRadiobuy" class="my_data">
                    <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                    <div class="my_right">
                        <div class="tab clearfix">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="tab_tit">
                                    <td>账号
                                    </td>
                                    <td>邮箱
                                    </td>
                                    <td>手机
                                    </td>
                                    <td>所属地区
                                    </td>
                                    <td>状态
                                    </td>
                                    <td>创建时间
                                    </td>
                                    <td>微信昵称
                                    </td>
                                    <td>操作
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr id="tr<%# Eval("CusId") %>">
                                            <td>
                                                <%#Eval("CusName").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("Email").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("Phone").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litRegionName" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <%#ConvertHelper.GetInteger(Eval("IsDisabled").ToString())==0?"启用":"禁用"%>
                                            </td>
                                            <td>
                                                <%# Eval("CreateTime","{0:yyyy-MM-dd HH:mm}")%>
                                            </td>
                                            <td>
                                                <%#Eval("NickName").ToString()%>
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
