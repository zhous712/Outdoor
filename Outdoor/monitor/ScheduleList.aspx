<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleList.aspx.cs" Inherits="Outdoor.monitor.ScheduleList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>排期列表-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
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
        function DelSchedule(sid) {
            effect.Dialog.confirm("确定要删除吗?", function () {
                if ('<%=base.Profile.CusId %>' != '0') {
                    $.ajax({
                        url: "/ashx/Action.ashx",
                        type: "post",
                        data: "option=delSchedule&sid=" + sid,
                        success: function (data) {
                            if (data == "true") {
                                effect.Dialog.alert("删除成功!");
                                location.reload();
                            }
                            else if (data == "no") {
                                effect.Dialog.alert("此排期下存在任务，不能删除!");
                                return false;
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
                    <a href="ScheduleList.aspx">排期列表</a> ><%=showList %>
                </div>
                <div class="my_radio">
                    <h2 class="left">我的RadioBuy</h2>
                    <div class="search right" id="YesDivListTop" runat="server">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="bntSearch">
                            <asp:TextBox ID="txt_key" CssClass="sear_txt" runat="server" value="请输入排期编号或名称" onfocus="if(this.value=='请输入排期编号或名称')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入排期编号或名称';" MaxLength="50"></asp:TextBox>
                            <input type="text" id="inputDate20" runat="server" readonly="readonly" class="sear_txt" />
                            <b class="img_date" id="imgDate20"></b>
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
                        <div class="main_title clearfix" runat="server" id="divMediaType">
                        </div>
                        <div class="clear">
                        </div>
                        <div class="tab clearfix">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="tab_tit">
                                    <td>排期编号
                                    </td>
                                    <td>排期名称
                                    </td>
                                    <td>投放时间
                                    </td>
                                    <td>全部投放量
                                    </td>
                                    <td>抽检量
                                    </td>
                                    <td>抽检率
                                    </td>
                                    <td>合格量
                                    </td>
                                    <td>不合格量
                                    </td>
                                    <td>合格率
                                    </td>
                                    <td>操作
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr id="tr<%# Eval("SId") %>">
                                            <td>
                                                <%#Eval("SId").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litScheduleName" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litPlanCycle" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litPlanCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litAuditCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litSamplingRate" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litQualifiedCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litUnqualifiedCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litQualifiedRate" runat="server"></asp:Literal>
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
        <!--多选日期控件-->
        <link href="../js/datepicker/style/date/datepicker.css" rel="stylesheet" />
        <script type="text/javascript" src="../js/datepicker/js/date/datepicker.js"></script>
        <script type="text/javascript" src="../js/datepicker/js/date/eye.js"></script>
        <script type="text/javascript" src="../js/datepicker/js/date/layout.js"></script>
    </form>
</body>
</html>
