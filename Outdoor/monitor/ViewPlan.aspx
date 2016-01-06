<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPlan.aspx.cs" Inherits="Outdoor.monitor.ViewPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>查看点位-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
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
    <link href="../js/autoComplete/jquery.autocomplete.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/autoComplete/jquery.autocomplete.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#divAudit").find("input[id='Cancel']").click(function () {
                jDivHide('divAudit');
            });
            $('input:radio[name="audit"]').click(function () {
                if ($(this).val() == '3') {
                    $("#divAbnormal").show();
                }
                if ($(this).val() == '0') {
                    $("#divAbnormal").hide();
                }
            });
            var current = 0;
            $("#imgImage").click(function () {
                current = (current + 90) % 360;
                this.style.transform = 'rotate(' + current + 'deg)';
            });
            $("#btnCancelPosition").click(function () {
                jDivHide('divPosition');
            });
            $("#btnCancelBind").click(function () {
                jDivHide('divBindTaskPlan');
            });
            //账号
            $("#txtCustomer").bind("input.autocomplete", function () {
                $(this).trigger('keydown.autocomplete');
            });
            $("#txtCustomer").autocomplete("/ashx/Action.ashx?r=" + Math.random(), {
                width: 140,
                dataType: "json",
                cacheLength: 0,
                extraParams: {
                    option: "getCustomer",
                    customerType: 2
                },
                parse: function (data) {
                    return $.map(data, function (row) {
                        return {
                            data: row,
                            value: row.CusName,
                            result: row.CusName
                        };
                    });
                },
                formatItem: function (item) {
                    return "<font color=\"#489ED2\">" + item.CusName + "</font>";
                }
            }).result(function (event, data, formatted) {
                var _CusName = data.CusName;
                $("#txtCustomer").val(_CusName);
            });
        })
        //二次发布任务
        function ReDo(tpid) {
            effect.Dialog.confirm("确认要重做吗？", function () {
                if ('<%=base.Profile.CusId %>' != '0') {
                    $.ajax({
                        url: "/ashx/Action.ashx",
                        type: "post",
                        data: "option=reDo&tpid=" + tpid,
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

        function AuditTask(tpid, tpuid, blockName, type, thumbnailImgPath, shootTime, shootPosition, gpsType, imgCount) {
            var radios = $('input:radio[name="abnormal"]');
            radios.each(function () {
                if ($(this).val() == type) {
                    $(this).attr("checked", "checked");
                }
            });
            var arrPosition = new Array();
            arrPosition = shootPosition.split(",");
            if (arrPosition.length > 1) {
                if (gpsType == 1) {
                    var htmlString = "<lbs-map id='lbsMap' height='240px' width='258px' center='" + shootPosition + "' map-type='static-map' zoom='18'><lbs-poi name='' id='lbsPoi' location='" + shootPosition + "' addr=''></lbs-poi></lbs-map>";
                    $("#divMap").html(htmlString);
                    $("#lableShootPosition").text(shootPosition);
                }
                else {
                    var gpsPoint = new BMap.Point(arrPosition[0], arrPosition[1]);

                    translateCallback = function (point) {
                        shootPosition = point.lng + "," + point.lat;
                        var htmlString = "<lbs-map id='lbsMap' height='240px' width='258px' center='" + shootPosition + "' map-type='static-map' zoom='18'><lbs-poi name='' id='lbsPoi' location='" + shootPosition + "' addr=''></lbs-poi></lbs-map>";
                        $("#divMap").html(htmlString);
                        $("#lableShootPosition").text(shootPosition);
                    }
                    BMap.Convertor.translate(gpsPoint, 0, translateCallback);
                }
            }
            else {
                var htmlString = "";
                $("#divMap").html(htmlString);
                $("#lableShootPosition").text(shootPosition);
            }
            $("#divThumbnailImage").unbind("click");
            $("#divThumbnailImage").click(function () {
                //                $('#imgImage').attr('src', 'http://' + thumbnailImgPath.substring(0, thumbnailImgPath.lastIndexOf('s')) + thumbnailImgPath.substring(thumbnailImgPath.lastIndexOf('.')));
                //                jDivShow('divImage', '查看图片')
                window.open('ViewImage.aspx?tpuid=' + tpuid);
            });
            //操作审核
            $("#divAudit").find("input[id='Determine']").unbind("click");
            $("#divAudit").find("input[id='Determine']").click(function () {
                var val = $('input:radio[name="audit"]:checked').val();
                var abnormalType = $('input:radio[name="abnormal"]:checked').val();
                if (val == null || val == '') {
                    effect.Dialog.alert("请选择审核状态！");
                }
                else {
                    var relation = val == '3' ? 1 : 2;
                    $.ajax({
                        url: "/ashx/Action.ashx",
                        type: "post",
                        data: "option=auditTask&tpid=" + tpid + "&status=" + val + "&relation=" + relation + "&abnormalType=" + abnormalType + "&reason=" + $("#txtReason").val() + "&position=" + $("#txtShootPosition").val(),
                        success: function (data) {
                            if (data == "true") {
                                jDivHide('divAudit');
                                window.location.reload();
                            }
                            else {
                                effect.Dialog.alert("抱歉,操作失败");
                                return false;
                            }
                        }
                    });
                }

            });
            $("#imgThumbnailImage").attr('src', 'http://' + thumbnailImgPath);
            jDivShow('divAudit', '审核报告');
            $("#divShootDate").text(shootTime);
            $("#divImageCount").text(imgCount);
            $("#aBlockName").text(blockName);
        }
        function ViewImage(tpuid, thumbnailImgPath, shootTime, shootPosition, gpsType, imgCount) {
            $("#divThumbnailImageView").unbind("click");
            $("#divThumbnailImageView").click(function () {
                //                $('#imgImage').attr('src', 'http://' + thumbnailImgPath.substring(0, thumbnailImgPath.lastIndexOf('s')) + thumbnailImgPath.substring(thumbnailImgPath.lastIndexOf('.')));
                //                jDivShow('divImage', '查看图片')
                window.open('ViewImage.aspx?tpuid=' + tpuid);
            });
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
        //修改图片GPS
        function UpdateGps(tpid, tpuid) {
            if (tpid > 0 && tpuid > 0) {
                jDivShow('divPosition', '修改坐标');
                // 百度地图API功能
                var map = new BMap.Map("allmap");
                var point = new BMap.Point(116.331398, 39.897445);
                map.centerAndZoom(point, 12);
                $("#btnGetPosition").unbind("click");
                $("#btnGetPosition").click(function () {
                    var address = $("#txtAddress").val();
                    if (address == '' || address == null || address == undefined) {
                        effect.Dialog.alert("请输入您要获取的地址!");
                    }
                    else {
                        // 创建地址解析器实例
                        var myGeo = new BMap.Geocoder();
                        // 将地址解析结果显示在地图上,并调整地图视野
                        myGeo.getPoint(address, function (point) {
                            if (point) {
                                map.centerAndZoom(point, 16);
                                map.addOverlay(new BMap.Marker(point));
                                $("#txtPosition").val(point.lng + ',' + point.lat);
                            } else {
                                effect.Dialog.alert("您输入的地址没有解析到结果!");
                            }
                        }, "");
                        function showInfo(e) {
                            $("#txtPosition").val(e.point.lng + ", " + e.point.lat);
                        }
                        map.addEventListener("click", showInfo);
                    }
                });
                $("#btnGetPositionByGps").unbind("click");
                $("#btnGetPositionByGps").click(function () {
                    var position = $("#txtPosition").val();
                    var positionArr = position.split(',');
                    if (position == '' || position == null || position == undefined) {
                        effect.Dialog.alert("请先获取坐标");
                    }
                    else if (positionArr.length > 1) {
                        map.clearOverlays();
                        var new_point = new BMap.Point(positionArr[0], positionArr[1]);
                        var marker = new BMap.Marker(new_point);  // 创建标注
                        map.addOverlay(marker);              // 将标注添加到地图中
                        map.panTo(new_point);
                    }
                });

                $("#btnConfirmPosition").click(function () {
                    var position = $("#txtPosition").val();
                    if (position == '' || position == null || position == undefined) {
                        effect.Dialog.alert("请先获取坐标");
                    }
                    else {
                        $.ajax({
                            url: "/ashx/Action.ashx",
                            type: "post",
                            data: "option=UpdateGps&tpid=" + tpid + "&tpuid=" + tpuid + "&gps=" + position,
                            success: function (data) {
                                if (data == "true") {
                                    window.location.reload();
                                }
                                else {
                                    effect.Dialog.alert("抱歉,操作失败");
                                    return false;
                                }
                            }
                        });
                    }
                });
            }
            else {
                effect.Dialog.alert("抱歉,操作失败");
            }
        }

        function CheckAll(obj) {
            var inputs = document.getElementsByName('chkTaskPlan');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = obj.checked;
                }
            }
        }

        function changeCheckAll() {
            var all_checked = true;
            var inputs = document.getElementsByName('chkTaskPlan');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (!inputs[i].checked) {
                        all_checked = false;
                        break;
                    }
                }
            }
            if (all_checked) {
                $("#checkAll").attr("checked", true);
            }
            else {
                $("#checkAll").attr("checked", false);
            }
        }

        function bindTaskPlan() {
            var inputs = document.getElementsByName('chkTaskPlan');
            var tpids = '';
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (inputs[i].checked) {
                        tpids += (inputs[i].value + ',');
                    }
                }
            }
            //if (tpids == '') {
            //    effect.Dialog.alert("请选择要分发的任务！");
            //    return false;
            //}
            //else {
            jDivShow('divBindTaskPlan', '分发任务');
            $("#btnConfirmBind").unbind("click");
            $("#btnConfirmBind").click(function () {
                $("#btnConfirmBind").val('分发中');
                if ($("#txtCustomer").val() == '') {
                    effect.Dialog.alert("请填写账号！");
                }
                else {
                    $.ajax({
                        url: "/ashx/Action.ashx",
                        type: "post",
                        data: "option=BindTaskPlan&cusname=" + $("#txtCustomer").val() + "&tpids=" + tpids + "&tid=" + $("#hidTid").val(),
                        success: function (data) {
                            if (data == "true") {
                                $("#btnConfirmBind").val('确定');
                                window.location.reload();
                            }
                            else {
                                effect.Dialog.alert(data);
                                return false;
                            }
                        }
                    });
                }
            });
            //}
        }

        function UpdateShootPosition() {
            $("#spanShootPositionOld").hide();
            $("#spanShootPositionNew").show();
        }
        function CancleShootPosition() {
            $("#spanShootPositionOld").show();
            $("#spanShootPositionNew").hide();
            $("#txtShootPosition").val('');
        }
    </script>
    <style type="text/css">
        #divImage {
            width: 600px;
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
                    <a href="<%=base.Profile.CustomerType==1?"TaskList.aspx?sid="+SId:"ScheduleList.aspx" %>"><%=base.Profile.CustomerType==1?"任务列表":"排期列表" %></a> >
                <%=TaskName %>
                </div>
                <div class="my_radio">
                    <div class="search left" style="margin-left: 10px" id="DivCheckAll" runat="server" visible="false">
                        <label>
                            <input id="checkAll" type="checkbox" onclick="CheckAll(this)" />&nbsp;本页全选</label>
                        <input type="hidden" id="hidTid" value="<%=Tid %>" />
                        <input class="sear_but" style="margin-left: 5px" type="button" value="发任务" onclick="bindTaskPlan()" />
                    </div>
                    <div class="search right" id="YesDivListTop" runat="server" visible="false">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="bntSearch">
                            <asp:TextBox ID="txtCity" CssClass="sear_txt" runat="server" value="请输入城市" onfocus="if(this.value=='请输入城市')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入城市';" MaxLength="50" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="txtArea" CssClass="sear_txt" runat="server" value="请输入区域" onfocus="if(this.value=='请输入区域')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入区域';" MaxLength="50" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="txtMediaType" CssClass="sear_txt" runat="server" value="请输入媒体类型" onfocus="if(this.value=='请输入媒体类型')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入媒体类型';" MaxLength="50" Width="100px"></asp:TextBox>
                            <asp:TextBox ID="txt_key" CssClass="sear_txt" runat="server" value="请输入编号或楼宇名称" onfocus="if(this.value=='请输入编号或楼宇名称')this.value='';this.focus();"
                                onblur="if(this.value=='')this.value='请输入编号或楼宇名称';" MaxLength="50"></asp:TextBox>
                            <input type="text" id="inputDate20" runat="server" readonly="readonly" class="sear_txt" />
                            <b class="img_date" id="imgDate20"></b>
                            <asp:Button ID="bntSearch" runat="server" Text="搜 索" CssClass="sear_but" OnClick="bntSearch_Click"
                                UseSubmitBehavior="false" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="radioCity" Text="城市" runat="server" GroupName="radiorule" Checked="true" />
                            <asp:RadioButton ID="radioBlock" Text="楼宇" runat="server" GroupName="radiorule" />
                            <asp:RadioButton ID="radioPoint" Text="点位" runat="server" GroupName="radiorule" />
                            <asp:RadioButton ID="radioAdProduct" Text="广告" runat="server" GroupName="radiorule" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="chkIsImage" Text="是否下载图片" runat="server" />
                            <asp:Button ID="btnExportData" runat="server" Text="下 载" CssClass="sear_but" OnClick="btnExportData_Click"
                                UseSubmitBehavior="false" />
                        </asp:Panel>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="myRadiobuy">
                    <div id="YesDivList" runat="server">
                        <div class="main_title clearfix" style="width: 900px">
                            <ul class="fx">
                                <li id="LiTypeAll" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=All&sid=<%=SId %>">全部</a></li>
                                <li id="LiTypeNormal" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Normal&sid=<%=SId %>">正常</a></li>
                                <li id="LiTypeBlack" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Black&sid=<%=SId %>">黑屏</a></li>
                                <li id="LiTypeBlur" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Blur&sid=<%=SId %>">花屏</a></li>
                                <li id="LiTypeNotPaint" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=NotPaint&sid=<%=SId %>">未上画</a></li>
                                <li id="LiTypeBreak" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Break&sid=<%=SId %>">破损</a></li>
                                <li id="LiTypeRepair" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Repair&sid=<%=SId %>">电梯维修</a></li>
                                <li id="LiTypeHidden" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Hidden&sid=<%=SId %>">遮挡</a></li>
                                <li id="LiTypeAbnormal" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Abnormal&sid=<%=SId %>">异常</a></li>
                                <li id="LiTypeDraft" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Draft&sid=<%=SId %>">草稿</a></li>
                                <li id="LiTypeReceive" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Receive&sid=<%=SId %>">已领取</a></li>
                                <li id="LiTypeUpload" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Upload&sid=<%=SId %>">已上传</a></li>
                                <li id="LiTypeAudit" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Audit&sid=<%=SId %>">审核通过</a></li>
                                <li id="LiTypeOverdue" runat="server"><a href="ViewPlan.aspx?tid=<%=Tid %>&type=Overdue&sid=<%=SId %>">已过期</a></li>
                            </ul>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="tab clearfix">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="tab_tit">
                                    <td id="tdCheckBox" runat="server" visible="false">全选
                                    </td>
                                    <td>编号
                                    </td>
                                    <td>城市
                                    </td>
                                    <td>区域
                                    </td>
                                    <td>街道地址
                                    </td>
                                    <td>楼宇名称
                                    </td>
                                    <td width="180px">点位名称
                                    </td>
                                    <td>媒体类型
                                    </td>
                                    <td>广告产品名
                                    </td>
                                    <td>任务周期
                                    </td>
                                    <td>排期编号
                                    </td>
                                    <td>备注
                                    </td>
                                    <td>状态
                                    </td>
                                    <td id="tdPrice" runat="server" visible="false">金额
                                    </td>
                                    <td id="tdIsPay" runat="server" visible="false">支付状态
                                    </td>
                                    <td id="tdReceiptor" runat="server" visible="false">领取人
                                    </td>
                                    <td>操作
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr id="tr<%# Eval("TPId") %>">
                                            <td id="tdCheckBox" runat="server" visible="false">
                                                <asp:Literal ID="litCheckBox" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <%#Eval("TPId").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("RegionName").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("AreaName").ToString()%>
                                            </td>
                                            <td title="<%#Eval("StreetAddress").ToString()%>">
                                                <%#AutoRadio.RadioSmart.Common.StringHelper.SubString(Eval("StreetAddress").ToString(), 10, false)%>
                                            </td>
                                            <td>
                                                <%#Eval("BlockName").ToString()%>
                                            </td>
                                            <td title="<%#Eval("PointName").ToString()%>">
                                                <%#Eval("PointName").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("MediaType").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("AdProductName").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litPlanCycle" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <%#Eval("SpareOne").ToString()%>
                                            </td>
                                            <td title="<%#Eval("AuditReason").ToString()%>">
                                                <%#AutoRadio.RadioSmart.Common.StringHelper.SubString(Eval("AuditReason").ToString(), 10, false)%>
                                            </td>
                                            <td>
                                                <%#GetTaskPlanStatus(Eval("Status").ToString(), Eval("AbnormalType").ToString())%>
                                            </td>
                                            <td id="tdPrice" runat="server" visible="false">
                                                <%#Eval("Price").ToString()%>
                                            </td>
                                            <td id="tdIsPay" runat="server" visible="false">
                                                <%#(Eval("IsPay").ToString()=="0"?"未支付":"已支付")%>
                                            </td>
                                            <td id="tdReceiptor" runat="server" visible="false">
                                                <%#AutoRadio.RadioSmart.Common.ConvertHelper.GetString(Eval("CusName").ToString()==""?Eval("NickName"):Eval("CusName"))%>
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
    <div id="divImage" style="display: none">
        <img id="imgImage" src="#" alt="找不到图片" />
    </div>
    <div class="bjm_box" id="divAudit" style="display: none; width: 520px">
        <div class="bjm_main">
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    照片数量：</label>
                <div class="bjm_lam left" id="divImageCount">
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    楼宇名称：</label>
                <div class="bjm_lam left">
                    <a class="blue" href="http://s.suixingpay.com/static/baiduMap/baiduMap.jsp" target="_blank" id="aBlockName"></a>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    拍摄时间：</label>
                <div class="bjm_lam left" id="divShootDate">
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    地理位置：</label>
                <div class="bjm_lam left" id="divShootPosition">
                    <span id="spanShootPositionOld">
                        <label id="lableShootPosition"></label>
                        <input type="button" value="修 改" class="btnBlue" style="margin-left: 10px" onclick="UpdateShootPosition()" />
                    </span>
                    <span style="display: none" id="spanShootPositionNew">
                        <input type="text" class="text" id="txtShootPosition" style="width: 210px" />
                        <input type="button" value="取 消" class="btnBlue" style="margin-left: 10px" onclick="CancleShootPosition()" /></span>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bjm_paln">
                <div style="height: 10px">
                </div>
                <span style="margin-right: 40px">
                    <input type="radio" checked="checked" name="audit" value="3" style="margin-right: 10px" />审核通过</span>
                <span>
                    <input type="radio" name="audit" value="0" style="margin-right: 10px" />审核不通过</span>
                <div class="clear">
                </div>
            </div>
            <div id="divAbnormal" class="bjm_paln">
                <div style="height: 10px">
                </div>
                <span style="margin-right: 10px">
                    <input type="radio" checked="checked" name="abnormal" value="0" style="margin-right: 5px" />正常</span>
                <span style="margin-right: 10px">
                    <input type="radio" name="abnormal" value="1" style="margin-right: 5px" />黑屏</span>
                <span style="margin-right: 10px">
                    <input type="radio" name="abnormal" value="2" style="margin-right: 5px" />花屏</span>
                <span style="margin-right: 10px">
                    <input type="radio" name="abnormal" value="3" style="margin-right: 5px" />未上画</span>
                <span style="margin-right: 10px">
                    <input type="radio" name="abnormal" value="4" style="margin-right: 5px" />破损</span>
                <span style="margin-right: 10px">
                    <input type="radio" name="abnormal" value="5" style="margin-right: 5px" />电梯维修</span>
                <span style="margin-right: 10px">
                    <input type="radio" name="abnormal" value="6" style="margin-right: 5px" />遮挡</span>
                <span style="margin-right: 10px">
                    <input type="radio" name="abnormal" value="7" style="margin-right: 5px" />异常</span>
                <div class="clear">
                </div>
            </div>
            <div id="divReason" class="bjm_paln" style="margin-top: 10px;">
                <label class="bjm_lab left">
                    审核备注：</label>
                <div>
                    <textarea id="txtReason" style="border: 1px solid #cfcfcf;" rows="2" cols="50"></textarea>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div style="width: 100%; overflow: hidden;">
            <div id="divThumbnailImage" style="float: left; overflow: hidden; margin-right: 13px; width: 240px; height: 240px; cursor: pointer">
                <img id="imgThumbnailImage" src="#" alt="找不到图片" title="点击浏览所有图片" />
            </div>
            <div id="divMap" style="float: left; margin-left: 3px; overflow: hidden;">
            </div>
        </div>
        <div class="dwb_bg2 sc_btn">
            <input type="button" id="Determine" value="确 定" class="btnBlue" />
            <input type="button" id="Cancel" value="取 消" class="btnBlue" />
        </div>
    </div>
    <div class="bjm_box" id="divPosition" style="display: none; width: 600px">
        <div class="bjm_main">
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    地址：</label>
                <div class="bjm_lam left" id="div2">
                    <input type="text" class="text" id="txtAddress" /><input type="button" id="btnGetPosition"
                        class="btnBlue" style="padding-bottom: 0px; margin: 0 0 0 10px" value="获 取" />
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    坐标：</label>
                <div class="bjm_lam left" id="div3">
                    <input type="text" class="text" id="txtPosition" /><input type="button" id="btnGetPositionByGps" class="btnBlue" style="padding-bottom: 0px; margin: 0 0 0 10px" value="获 取" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="allmap" style="width: 600px; height: 400px; overflow: hidden; margin: 0 autu 0 auto; font-family: 微软雅黑;">
        </div>
        <div class="dwb_bg2 sc_btn">
            <input type="button" value="确 定" class="btnBlue" id="btnConfirmPosition" />
            <input type="button" id="btnCancelPosition" value="取 消" class="btnBlue" />
        </div>
    </div>

    <div class="bjm_box" id="divBindTaskPlan" style="display: none; width: 400px">
        <div class="bjm_main">
            <div style="height: 10px">
            </div>
            <div class="bjm_paln">
                <label class="bjm_lab left">
                    账号名：</label>
                <div class="bjm_lam left">
                    <input type="text" class="text" id="txtCustomer" />
                </div>
                <div class="clear">
                </div>
            </div>
            <div style="height: 60px">
            </div>
            <div class="dwb_bg2 sc_btn">
                <input type="button" value="确 定" class="btnBlue" id="btnConfirmBind" />
                <input type="button" id="btnCancelBind" value="取 消" class="btnBlue" />
            </div>
        </div>
    </div>
</body>
</html>
