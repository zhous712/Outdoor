<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskStatistics.aspx.cs" Inherits="Outdoor.monitor.TaskStatistics" %>

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
                            <asp:TextBox ID="txtCusName" CssClass="sear_txt" runat="server" value="请输入客户" onfocus="if(this.value=='请输入客户')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入客户';" MaxLength="50"></asp:TextBox>
                            <asp:TextBox ID="txtMediaType" CssClass="sear_txt" runat="server" value="请输入媒体类型" onfocus="if(this.value=='请输入媒体类型')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入媒体类型';" MaxLength="50"></asp:TextBox>
                            <asp:TextBox ID="txtCity" CssClass="sear_txt" runat="server" value="请输入城市" onfocus="if(this.value=='请输入城市')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入城市';" MaxLength="50"></asp:TextBox>
                            <input type="text" id="inputDate20" runat="server" readonly="readonly" class="input inp_week" style="width: 150px" />
                            <img class="control" src="../images/date.gif" alt="选择日期" id="imgDate20" />
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
                                <li class="on">客户统计</li>
                                <li><a href="CustomerStatistics.aspx">账号统计</a></li>
                            </ul>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="tab clearfix">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="tab_tit">
                                    <td>客户
                                    </td>
                                    <td>媒体类型
                                    </td>
                                    <td>城市
                                    </td>
                                    <td>发放数/完成数
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Eval("CusName").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("MediaType").ToString()%>
                                            </td>
                                            <td>
                                                <%# Eval("RegionName").ToString()%>
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

