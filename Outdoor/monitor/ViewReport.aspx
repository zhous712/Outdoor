<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="Outdoor.monitor.ViewReport" %>

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
    <link href="../js/autoComplete/jquery.autocomplete.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/autoComplete/jquery.autocomplete.min.js"></script>
    <style type="text/css">
        .bgBlue {
            background-color: #B2E8FB;
        }

        .abnormal {
            width: 50px;
            background-color: #ff006e;
            color: white;
            margin: 0 auto;
        }

        .normal {
            width: 50px;
            background-color: #0094ff;
            color: white;
            margin: 0 auto;
        }
    </style>
    <script type="text/javascript">
        function CheckCityAll(obj) {
            var inputs = document.getElementsByName('chkCity');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = obj.checked;
                }
            }
        }
        function ChangeCheckCityAll() {
            var all_checked = true;
            var inputs = document.getElementsByName('chkCity');
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
        function searchCity() {
            $.get("/ashx/Action.ashx?r=?m" + Math.random(), { option: 'getCity',key:$("#txtCityKey").val(), sid: <%=SId %> }, function (data) {
                if (data != "") {
                    var JosnItem = eval("(" + data + ")");
                    var html="<li><input type=\"checkbox\" id=\"chkCityAll\" onclick=\"CheckCityAll(this)\" /><font color=\"#489ED2\">全选</font></li>";
                    for (var i = 0; i < JosnItem.length; i++) {
                        html += "<li><input type=\"checkbox\" name=\"chkCity\" value=\"" + JosnItem[i].RegionId + "\" onclick=\"ChangeCheckCityAll()\" /><font color=\"#489ED2\">" + JosnItem[i].RegionName + "</font></li>";
                    }
                    $("#ulCity").html(html);
                    var element = $("#divCity");    
                    var list=element.find("ul");
                    var listItems=element.find("li");
                    var input=document.getElementById('txtCity');
                    var offset = $(input).offset();
                    element.css({
                        width: $(input).width(),
                        top: offset.top + input.offsetHeight,
                        left: offset.left
                    }).show();
                    list.scrollTop(0);
                    list.css({
                        maxHeight: 180,
                        overflow: 'auto'
                    });
                    if ($.browser.msie && typeof document.body.style.maxHeight === "undefined") {
                        var listHeight = 0;
                        listItems.each(function() {
                            listHeight += this.offsetHeight;
                        });
                        var scrollbarsVisible = listHeight > options.scrollHeight;
                        list.css('height', scrollbarsVisible ? options.scrollHeight : listHeight);
                        if (!scrollbarsVisible) {
                            // IE doesn't recalculate width when scrollbar disappears
                            listItems.width(list.width() - parseInt(listItems.css("padding-left")) - parseInt(listItems.css("padding-right")));
                        }
                    }
                    var regionIds=$("#hidCity").val().split(',');
                    var chkCitys = document.getElementsByName('chkCity');
                    for (var i = 0; i < chkCitys.length; i++) {
                        if ($.inArray(chkCitys[i].value, regionIds)!=-1) {
                            chkCitys[i].checked = true;
                        }
                    }
                }
            });
        }
        function CheckCityShow() {
            $("#divBlock").hide();
            var element = $("#divCity");    
            if (element.find("li").size()==0) {
                var html="<input type=\"text\" id=\"txtCityKey\" class=\"sear_txt\" value=\"请输入城市\" onfocus=\"if(this.value=='请输入城市')this.value='';this.focus();\" onblur=\"if(this.value=='')this.value='请输入城市';\"/><img src=\"../images/bg_search.png\" style=\"position:relative;top:5px;cursor:pointer\" onclick=\"searchCity()\" />";
                html+="<ul id=\"ulCity\">";
                html+="</ul>"
                html+="<div class=\"right\"><input type=\"button\" class=\"sear_but\" id=\"btnCityConfirm\" value=\"确定\"  onclick=\"CityConfirm()\" /><input type=\"button\" class=\"sear_but\" id=\"btnCityCancle\" value=\"取消\" onclick=\"CityCancle()\"/></div>";
                element.html(html);
                searchCity();
            } 
            else {
                element.show();
                var regionIds=$("#hidCity").val().split(',');
                var chkCitys = document.getElementsByName('chkCity');
                for (var i = 0; i < chkCitys.length; i++) {
                    if ($.inArray(chkCitys[i].value, regionIds)!=-1) {
                        chkCitys[i].checked = true;
                    }
                }
            }
        }
        function CityConfirm() {
            var inputs = document.getElementsByName('chkCity');
            var regionIds = '';
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (inputs[i].checked) {
                        regionIds += (inputs[i].value + ',');
                    }
                }
            }
            if (document.getElementById('chkCityAll').checked) {
                $("#hidCity").val('');   
            }
            else {
                $("#hidCity").val(regionIds);   
            }
            $("#divCity").hide();
            window.location.href="ViewReport.aspx?sid="+<%=SId %>+"&regionids="+$("#hidCity").val()+"&blocknames="+  $("#hidBlock").val();
        }
        function CityCancle() {
            $("#divCity").hide();
        }

        function CheckBlockAll(obj) {
            var inputs = document.getElementsByName('chkBlock');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = obj.checked;
                }
            }
        }
        function ChangeCheckBlockAll() {
            var all_checked = true;
            var inputs = document.getElementsByName('chkBlock');
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
        function searchBlock() {
            $.get("/ashx/Action.ashx?r=?m" + Math.random(), { option: 'getBlock',key:$("#txtBlockKey").val(), sid: <%=SId %> }, function (data) {
                if (data != "") {
                    var JosnItem = eval("(" + data + ")");
                    var html = "<li><input type=\"checkbox\" id=\"chkBlockAll\" onclick=\"CheckBlockAll(this)\" /><font color=\"#489ED2\">全选</font></li>";
                    for (var i = 0; i < JosnItem.length; i++) {
                        html += "<li><input type=\"checkbox\" name=\"chkBlock\" value=\"" + JosnItem[i].BlockName + "\" onclick=\"ChangeCheckBlockAll()\" /><font color=\"#489ED2\">" + JosnItem[i].BlockName + "</font></li>";
                    }
                    $("#ulBlock").html(html);
                    var element = $("#divBlock");    
                    var list=element.find("ul");
                    var listItems=element.find("li");
                    var input=document.getElementById('txtBlock');
                    var offset = $(input).offset();
                    element.css({
                        width: $(input).width(),
                        top: offset.top + input.offsetHeight,
                        left: offset.left
                    }).show();
                    list.scrollTop(0);
                    list.css({
                        maxHeight: 180,
                        overflow: 'auto'
                    });

                    if ($.browser.msie && typeof document.body.style.maxHeight === "undefined") {
                        var listHeight = 0;
                        listItems.each(function() {
                            listHeight += this.offsetHeight;
                        });
                        var scrollbarsVisible = listHeight > options.scrollHeight;
                        list.css('height', scrollbarsVisible ? options.scrollHeight : listHeight);
                        if (!scrollbarsVisible) {
                            // IE doesn't recalculate width when scrollbar disappears
                            listItems.width(list.width() - parseInt(listItems.css("padding-left")) - parseInt(listItems.css("padding-right")));
                        }
                    }
                    var chkBlocks = document.getElementsByName('chkBlock');
                    var blockNames=$("#hidBlock").val().split(',');
                    for (var i = 0; i < chkBlocks.length; i++) {
                        if ($.inArray(chkBlocks[i].value, blockNames)!=-1) {
                            chkBlocks[i].checked = true;
                        }
                    }
                }
            });
        }
        function CheckBlockShow() {
            $("#divCity").hide();
            var element = $("#divBlock");    
            if (element.find("li").size()==0) {
                var html="<input type=\"text\" id=\"txtBlockKey\" class=\"sear_txt\" value=\"请输入楼宇\" onfocus=\"if(this.value=='请输入楼宇')this.value='';this.focus();\" onblur=\"if(this.value=='')this.value='请输入楼宇';\"/><img src=\"../images/bg_search.png\" style=\"position:relative;top:5px;cursor:pointer\" onclick=\"searchBlock()\" />";
                html+="<ul id=\"ulBlock\">";
                html+="</ul>"
                html+="<div class=\"right\"><input type=\"button\" class=\"sear_but\" id=\"btnBlockConfirm\" value=\"确定\" onclick=\"BlockConfirm()\" /><input type=\"button\" class=\"sear_but\" id=\"btnBlockCancle\" value=\"取消\" onclick=\"BlockCancle()\"/></div>";
                element.html(html);
                searchBlock();
            } 
            else {
                element.show();
                var chkBlocks = document.getElementsByName('chkBlock');
                var blockNames=$("#hidBlock").val().split(',');
                for (var i = 0; i < chkBlocks.length; i++) {
                    if ($.inArray(chkBlocks[i].value, blockNames)!=-1) {
                        chkBlocks[i].checked = true;
                    }
                }
            }
        }
        function BlockConfirm() {
            var inputs = document.getElementsByName('chkBlock');
            var blockNames = '';
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (inputs[i].checked) {
                        blockNames += (inputs[i].value + ',');
                    }
                }
            }
            if (document.getElementById('chkBlockAll').checked) {
                $("#hidBlock").val('');   
            }
            else {
                $("#hidBlock").val(blockNames);   
            }
            $("#divBlock").hide();   
            window.location.href="ViewReport.aspx?sid="+<%=SId %>+"&regionids="+$("#hidCity").val()+"&blocknames="+ $("#hidBlock").val();  
        }
        function BlockCancle() {
            $("#divBlock").hide();
        }

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

        function OpenAndCloseBlock(regionId,o) {
            var trs=$("#tabReport tr[id]");
            if ($(o).text()=='✚') {
                $(o).text('━');
                trs.each(function () {
                    if($(this).attr('id')==('tr'+regionId)&&$(this).attr('class')!='bgBlue'){
                        $(this).show();
                    }
                });
            }
            else if ($(o).text()=='━') {
                $(o).text('✚');
                trs.each(function () {
                    if($(this).attr('id')==('tr'+regionId)&&$(this).attr('class')!='bgBlue'){
                        $(this).hide();
                    }
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top_bg">
            <jianboUI:Header ID="jianbo_Header" runat="server" />
            <div class="border">
                <div class="position">
                    <a href="ScheduleList.aspx">排期列表</a> ><%=scheduleName %>
                </div>
                <div class="my_radio">
                    <h2 class="left">我的RadioBuy</h2>
                    <div class="clear">
                    </div>
                </div>
                <div id="myRadiobuy" class="my_data">
                    <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                    <div class="my_right">
                        <font style="font-size: x-large" id="fontScheduleTitle" runat="server">未查询到数据</font>
                        <div class="search right">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnExportData">
                                <asp:Button ID="btnExportData" runat="server" Text="下载排期报表" CssClass="sear_but" Width="100px" OnClick="btnExportData_Click"
                                    UseSubmitBehavior="false" />
                            </asp:Panel>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="tab clearfix">
                            <table id="tabReport" style="width: 100%; border: 0;">
                                <tr class="tab_tit">
                                    <td style="width: 200px; padding: 5px 0">
                                        <input type="text" readonly="readonly" class="text" style="width: 180px" id="txtCity" value="筛选城市" onclick="CheckCityShow()" />
                                        <input type="hidden" id="hidCity" runat="server" />
                                    </td>
                                    <td style="width: 200px; padding: 5px 0">
                                        <input type="text" readonly="readonly" class="text" style="width: 180px" id="txtBlock" value="筛选楼宇" onclick="CheckBlockShow()" />
                                        <input type="hidden" id="hidBlock" runat="server" />
                                    </td>
                                    <td>未上画
                                    </td>
                                    <td>遮挡
                                    </td>
                                    <td>破损
                                    </td>
                                    <td>正常
                                    </td>
                                    <td>合计
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class="<%#Eval("RegionName").ToString().Contains("Total")?"bgBlue":""%>" id="tr<%#Eval("RegionId").ToString() %>">
                                            <td>
                                                <%#Eval("RegionName").ToString()%>
                                            </td>
                                            <td>
                                                <%#Eval("BlockName").ToString()%>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litNotPaintCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litHiddenCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litBreakCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litNormalCount" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litTotalCount" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td>
                                        <span style="font-size: larger; font-weight: 700; color: black">总合计</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="NotPaintTotal" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="HiddenTotal" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="BreakTotal" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="NormalTotal" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="AllTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <%--                            <div class="page">
                                <%=pageStr%>
                            </div>--%>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
        <jianboUI:Footer ID="jianbo_Footer" runat="server" />
    </form>
    <div id="divCity" class="ac_results" style="position: absolute; display: none"></div>
    <div id="divBlock" class="ac_results" style="position: absolute; display: none"></div>
</body>
</html>
