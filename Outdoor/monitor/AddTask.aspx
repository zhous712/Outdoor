<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTask.aspx.cs" Inherits="Outdoor.monitor.AddTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>新建任务-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
    <jianboUI:Keyword ID="jianbo_Keyword" runat="server" />
    <link href="../css/base.css" rel="stylesheet" type="text/css" />
    <link href="../css/my_radio.css" rel="stylesheet" type="text/css" />
    <link href="../css/paiqi.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/ajaxupload/ajaxupload.js"></script>
    <script src="../js/bgiframe-2.1.2-1/jquery.bgiframe.js" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.artDialog.source.js?skin=blue" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.alert.js" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/plugins/iframeTools.js" type="text/javascript"></script>
    <link href="../js/autoComplete/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="../js/autoComplete/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            new AjaxUpload('btnUploadExcelFile', {
                action: '../ashx/UploadExcelFile.ashx',
                name: 'myExcelFile',
                filetypes: 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
                responseType: 'json',
                multiple: true,
                onSubmit: function (file, ext) {
                    if (!(ext && /^(xls|xlsx)$/.test(ext.toLowerCase()))) {
                        effect.Dialog.alert("只能上传Excel文件");
                        return false;
                    }
                    var cusid = '<%=base.Profile.CusId %>';
                    this.setData({
                        'cusid': cusid
                    });
                    $('#divUploadExcelFileStatus').html('<img src=\"../js/ajaxupload/images/sloading.gif\" width=\"15\" height=\"15\" /> ');
                    this.disable();
                },
                onComplete: function (file, response) {
                    this.enable();
                    if (response.status == '1') {
                        $('#divUploadExcelFileStatus').html('');
                        if ($("#hfExcelPath").val() == ',') {
                            $("#hfExcelPath").val('');
                        }
                        $("#hfExcelPath").val($("#hfExcelPath").val() + response.filePath);
                        $("#txtUploadExcelFile").val($("#txtUploadExcelFile").val() + response.fileName);
                    }
                    else {
                        $('#divUploadExcelFileStatus').html('');
                        effect.Dialog.alert(response.note);
                    }
                }
            });
            new AjaxUpload('btnUploadImageFile', {
                action: '../ashx/CustomerUploadImageFile.ashx',
                name: 'myImageFile',
                responseType: 'json',
                multiple: true,
                onSubmit: function (file, ext) {
                    if (!(ext && /^(jpg|jpeg|png)$/.test(ext.toLowerCase()))) {
                        effect.Dialog.alert("只能上传图片文件");
                        return false;
                    }
                    var cusid = '<%=base.Profile.CusId %>';
                    this.setData({
                        'cusid': cusid,
                        'uploadtype': 'addtask'
                    });
                    $('#divUploadImageFileStatus').html('<img src=\"../js/ajaxupload/images/sloading.gif\" width=\"15\" height=\"15\" /> ');
                    this.disable();
                },
                onComplete: function (file, response) {
                    this.enable();
                    if (response.status == '1') {
                        $('#divUploadImageFileStatus').html('');
                        if ($("#hfImagePath").val() == ',') {
                            $("#hfImagePath").val('');
                        }
                        $("#hfImagePath").val($("#hfImagePath").val() + response.filePath);
                        $("#txtUploadImageFile").val($("#txtUploadImageFile").val() + response.fileName);
                    }
                    else {
                        $('#divUploadImageFileStatus').html('');
                        effect.Dialog.alert(response.note);
                    }
                }
            });
            //客户名称
            $("#txtCusName").bind("input.autocomplete", function () {
                $(this).trigger('keydown.autocomplete');
            });
            $("#txtCusName").autocomplete("../ashx/Action.ashx?r=" + Math.random(), {
                width: 200,
                dataType: "json",
                cacheLength: 0,
                extraParams: {
                    option: "getCustomer",
                    customerType: 0
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
                var _CuName = data.CusName;
                $("#txtCusName").val(_CuName);
            });
        });

        function validConfirm() {
            if ($("#txtScheduleName").val() == "") {
                effect.Dialog.alert('请输入排期名称!');
                return false;
            }
            if ($("#hfExcelPath").val() == ",") {
                effect.Dialog.alert('请上传排期文件!');
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top_bg">
            <jianboUI:Header ID="jianbo_Header" runat="server" />
            <div id="bodyRadio" class="border">
                <div class="position">
                    <a href="/">首页</a> &gt; 上传排期
                </div>
                <div id="myRadiobuy" class="my_data">
                    <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                    <div class="creat_jy clearfix" style="padding-top: 30px; width: 600px">
                        <div runat="server" id="divCustomer" style="height: 60px">
                            <span class="creat_tit left fb label_wid">客户名称：</span>
                            <input type="text" id="txtCusName" runat="server" class="input ty_txt left" />
                            <div class="clear">
                            </div>
                        </div>
                        <div runat="server" style="height: 60px">
                            <span class="creat_tit left fb label_wid">排期名称：</span>
                            <input type="text" id="txtScheduleName" runat="server" class="input ty_txt left" />
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 60px">
                            <span class="creat_tit left fb label_wid">上传图片：</span>
                            <input type="text" id="txtUploadImageFile" style="width: 350px" runat="server" class="input ty_txt left" readonly="readonly" />
                            <input type="button" id="btnUploadImageFile" style="height: 25px; line-height: 25px; *line-height: 18px; margin-left: 10px; float: left;"
                                value="上传图片" class="lispan btnGray" />
                            <div id="divUploadImageFileStatus" class="theFont left" style="margin: 5px">
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <span class="creat_tit left fb label_wid">上传排期表：</span>
                        <input type="text" id="txtUploadExcelFile" style="width: 350px" runat="server" class="input ty_txt left" readonly="readonly" />
                        <input type="button" id="btnUploadExcelFile" style="height: 25px; line-height: 25px; *line-height: 18px; margin-left: 10px; float: left;"
                            value="上传排期" class="lispan btnGray" />
                        <div id="divUploadExcelFileStatus" class="theFont left" style="margin: 5px">
                        </div>
                        <div class="clear">
                        </div>
                        <div class="ty_btnbox">
                            <asp:Button ID="btnNext" runat="server" Text="下一步" CssClass="btnBlue" OnClick="btnNext_Click"
                                OnClientClick="return validConfirm();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btnBlue" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <jianboUI:Footer ID="jianbo_Footer" runat="server" />
        <!--上传的排期文件路径-->
        <asp:HiddenField ID="hfExcelPath" runat="server" Value="," />
        <!--上传的图片文件路径-->
        <asp:HiddenField ID="hfImagePath" runat="server" Value="," />
    </form>
</body>
</html>
