<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerStatistics.aspx.cs" Inherits="Outdoor.monitor.CustomerStatistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>任务统计-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
    <jianboUI:Keyword ID="jianbo_Keyword" runat="server" />
    <link href="../css/base.css" rel="stylesheet" type="text/css" />
    <link href="../css/my_radio.css" rel="stylesheet" type="text/css" />
    <link href="../css/login.css" rel="stylesheet" type="text/css" />
    <link href="../css/paiqi.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top_bg">
            <jianboUI:Header ID="jianbo_Header" runat="server" />
            <div class="border">
                <div class="position">
                    <a href="ScheduleList.aspx">排期列表</a> >任务统计
                </div>
                <div class="my_radio">
                    <h2 class="left">我的RadioBuy</h2>
                    <!--多选日期控件-->
                    <link href="../js/DatePicker/style/date/datepicker.css" rel="stylesheet" type="text/css" />
                    <script type="text/javascript" src="../js/DatePicker/js/date/datepicker.js"></script>
                    <script type="text/javascript" src="../js/DatePicker/js/date/eye.js"></script>
                    <script type="text/javascript" src="../js/DatePicker/js/date/layout.js"></script>
                    <div class="search right" id="YesDivListTop" runat="server">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="bntSearch">
                            <input type="text" id="inputDate20" runat="server" readonly="readonly" class="input inp_week" style="width: 150px" />
                            <img class="control" src="../images/date.gif" alt="选择日期" id="imgDate20" />
                            <asp:TextBox ID="txtCusName" CssClass="sear_txt" runat="server" value="请输入账号" onfocus="if(this.value=='请输入账号')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入账号';" MaxLength="50"></asp:TextBox>
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
                        <div class="main_title clearfix">
                            <ul class="fx">
                                <li><a href="TaskStatistics.aspx">客户统计</a></li>
                                <li class="on">账号统计</li>
                            </ul>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="tab clearfix">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="tab_tit">
                                    <td>账号
                                    </td>
                                    <td>所属地区
                                    </td>
                                    <td>领取数/完成数
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Eval("CusName").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("FullName").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litPlanCount" runat="server"></asp:Literal>
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
