<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="Outdoor.monitor.TaskList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>任务列表-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
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
        function DelTask(tid, sid) {
            effect.Dialog.confirm("确定要删除吗?", function () {
                if ('<%=base.Profile.CusId %>' != '0') {
                    $.ajax({
                        url: "/ashx/Action.ashx",
                        type: "post",
                        data: "option=delTask&tid=" + tid + "&sid=" + sid,
                        success: function (data) {
                            if (data == "true") {
                                effect.Dialog.alert("删除成功!");
                                location.reload();
                            }
                            else if (data == "no") {
                                effect.Dialog.alert("任务已进行，不能删除!");
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
        function ReleaseTask(tid) {
            effect.Dialog.confirm("确定要发布吗?", function () {
                $.ajax({
                    url: "/ashx/Action.ashx",
                    type: "post",
                    data: "option=releaseTask&tid=" + tid,
                    success: function (data) {
                        if (data == "true") {
                            effect.Dialog.alert("发布成功!");
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
        function ViewImage(tid) {
            //$('#imgImage').attr('src', 'http://' + PicturePath);
            //jDivShow('divImage', '查看图片');
            window.open('ViewTaskImage.aspx?tid=' + tid);
        }
        function ChooseMediaType(o) {
            $(".fx li").removeClass("on");
            $(o).addClass("on");
        }
    </script>
    <style type="text/css">
        #divImage {
            width: 800px;
            height: 600px;
            border: 1px solid black;
        }

            #divImage img {
                width: 100%;
                height: 100%;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top_bg">
            <jianboUI:Header ID="jianbo_Header" runat="server" />
            <div class="border">
                <div class="position">
                    <a href="ScheduleList.aspx">排期列表</a> >任务列表
                </div>
                <div class="my_radio">
                    <h2 class="left">我的RadioBuy</h2>
                    <div class="search right" id="YesDivListTop" runat="server" visible="false">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="bntSearch">
                            <asp:TextBox ID="txt_key" CssClass="sear_txt" runat="server" value="请输入任务编号或名称" onfocus="if(this.value=='请输入任务编号或名称')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入任务编号或名称';" MaxLength="50"></asp:TextBox>
                            <asp:Button ID="bntSearch" runat="server" Text="搜 索" CssClass="sear_but" OnClick="bntSearch_Click"
                                UseSubmitBehavior="false" />
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="myRadiobuy" class="my_data">
                    <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                    <div class="main" id="NoDivList" runat="server" visible="false">
                        <p class="newTask" id="pTipContent" runat="server">
                        </p>
                    </div>
                    <div class="my_right" id="YesDivList" runat="server" visible="false">
                        <div class="tab clearfix">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="tab_tit">
                                    <td>任务编号
                                    </td>
                                    <td>任务名称
                                    </td>
                                    <td>点位/执行数
                                    </td>
                                    <td>任务状态
                                    </td>
                                    <td>创建人
                                    </td>
                                    <td>任务周期
                                    </td>
                                    <td>样图
                                    </td>
                                    <td>操作
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr id="tr<%# Eval("Tid") %>">
                                            <td>
                                                <%#Eval("TId").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litTaskName" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litPlanCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <%#GetTaskStatus(Eval("Status").ToString())%>
                                            </td>
                                            <td>
                                                <%#Eval("CusName").ToString()%>
                                            </td>
                                            <td>
                                                <%# Eval("DateCycle").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litImage" runat="server"></asp:Literal>
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
    <div id="divImage" style="display: none">
        <img id="imgImage" src="#" alt="找不到图片" />
    </div>
</body>
</html>
