<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyTaskList.aspx.cs" Inherits="Outdoor.monitor.MyTaskList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>我的任务</title>
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
    <script src="http://api.map.baidu.com/components?ak=D550932d199664a711f9b2d42c9253d1&v=1.0"
        type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=D550932d199664a711f9b2d42c9253d1"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/getscript?v=2.0&amp;ak=D550932d199664a711f9b2d42c9253d1&amp;services=&amp;t=20150609132457"></script>
    <script type="text/javascript" src="http://developer.baidu.com/map/jsdemo/demo/convertor.js"></script>
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

        function ViewImage(tpuid, thumbnailImgPath, shootTime, shootPosition, gpsType, imgCount) {
            $("#divShootDateView").text(shootTime);
            $("#divImageCountView").text(imgCount);
            $('#imgThumbnailImageView').attr('src', 'http://' + thumbnailImgPath);
            var arrPosition = new Array();
            arrPosition = shootPosition.split(",");
            if (arrPosition.length > 1) {
                if (gpsType == 1) {
                    var htmlString = "<lbs-map id='lbsMap' height='240px' width='258px' center='" + shootPosition + "' map-type='static-map' zoom='18'><lbs-poi name='' id='lbsPoi' location='" + shootPosition + "' addr=''></lbs-poi></lbs-map>";
                    $("#divMapView").html(htmlString);
                    $("#divShootPositionView").text(shootPosition);
                }
                else {
                    var gpsPoint = new BMap.Point(arrPosition[0], arrPosition[1]);
                    translateCallback = function (point) {
                        shootPosition = point.lng + "," + point.lat;
                        var htmlString = "<lbs-map id='lbsMap' height='240px' width='258px' center='" + shootPosition + "' map-type='static-map' zoom='18'><lbs-poi name='' id='lbsPoi' location='" + shootPosition + "' addr=''></lbs-poi></lbs-map>";
                        $("#divMapView").html(htmlString);
                        $("#divShootPositionView").text(shootPosition);
                    }
                    BMap.Convertor.translate(gpsPoint, 0, translateCallback);
                }

            }
            else {
                var htmlString = "";
                $("#divMapView").html(htmlString);
                $("#divShootPositionView").text(shootPosition);
            }
            jDivShow('divView', '查看');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top_bg">
            <jianboUI:Header ID="jianbo_Header" runat="server" />
            <div class="border">
                <div class="position">
                    <a href="MyTaskList.aspx">我的任务</a> >
                我的任务列表
                </div>
                <div class="my_radio">
                    <h2 class="left">我的RadioBuy</h2>
                    <div class="search right" id="YesDivListTop" runat="server">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="bntSearch">
                            <asp:TextBox ID="txt_key" CssClass="sear_txt" runat="server" value="请输入楼宇名称" onfocus="if(this.value=='请输入楼宇名称')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入楼宇名称';" MaxLength="50"></asp:TextBox>
                            <asp:Button ID="bntSearch" runat="server" Text="搜 索" CssClass="sear_but" OnClick="bntSearch_Click"
                                UseSubmitBehavior="false" />
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="myRadiobuy" class="my_data">
                    <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                    <div class="my_right">
                        <div class="main_title clearfix">
                            <ul class="fx">
                                <li id="LiTypeIn" runat="server"><a href="MyTaskList.aspx?type=In&tid=<%=Tid %>">已领取</a></li>
                                <li id="LiTypeUpload" runat="server"><a href="MyTaskList.aspx?type=Upload&tid=<%=Tid %>">已上传</a></li>
                                <li id="LiTypeAuditPass" runat="server"><a href="MyTaskList.aspx?type=AuditPass&tid=<%=Tid %>">审核通过</a></li>
                                <li id="LiTypeAuditNoPass" runat="server"><a href="MyTaskList.aspx?type=AuditNoPass&tid=<%=Tid %>">审核不通过</a></li>
                                <li id="LiTypeOverdue" runat="server"><a href="MyTaskList.aspx?type=Overdue&tid=<%=Tid %>">已过期</a></li>
                            </ul>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="tab clearfix">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="tab_tit">
                                    <td>编号</td>
                                    <td>城市
                                    </td>
                                    <td>区域
                                    </td>
                                    <td>街道地址
                                    </td>
                                    <td>楼宇名称
                                    </td>
                                    <td>点位名称
                                    </td>
                                    <td>广告产品名
                                    </td>
                                    <td>任务周期
                                    </td>
                                    <td>拍照要求
                                    </td>
                                    <td>状态
                                    </td>
                                    <td>操作
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Eval("TPId").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litCity" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <%#Eval("AreaName").ToString()%>
                                            </td>
                                            <td title="<%#Eval("StreetAddress").ToString()%>">
                                                <%#StringHelper.SubString(Eval("StreetAddress").ToString(), 10, true)%>
                                            </td>
                                            <td>
                                                <%#Eval("BlockName").ToString()%>
                                            </td>
                                            <td title="<%#Eval("PointName").ToString()%>">
                                                <%#StringHelper.SubString(Eval("PointName").ToString(), 12, true)%>
                                            </td>
                                            <td>
                                                <%#Eval("AdProductName").ToString()%>
                                            </td>
                                            <td>
                                                <%# Eval("BeginDate","{0:yyyy-MM-dd}")+"至"+Eval("EndDate","{0:yyyy-MM-dd}")%>
                                            </td>
                                            <td title="<%#Eval("PhotoRequire").ToString()%>">
                                                <%#StringHelper.SubString(Eval("PhotoRequire").ToString(), 12, true)%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
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
    <div class="bjm_box" id="divView" style="display: none">
        <div class="bjm_main">
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    照片数量：</label>
                <div class="bjm_lam left" id="divImageCountView">
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    拍摄时间：</label>
                <div class="bjm_lam left" id="divShootDateView">
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    地理位置：</label>
                <div class="bjm_lam left" id="divShootPositionView">
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div style="width: 100%; overflow: hidden;">
            <div id="divThumbnailImageView" style="float: left; overflow: hidden; margin-right: 3px; width: 240px; height: 240px; cursor: pointer">
                <img id="imgThumbnailImageView" src="#" alt="找不到图片" title="点击浏览所有大图" />
            </div>
            <div id="divMapView" style="float: left; margin-left: 3px; overflow: hidden;">
            </div>
        </div>
    </div>
</body>
</html>
