<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerUploadImage.aspx.cs" Inherits="Outdoor.monitor.CustomerUploadImage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>上传图片-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
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
    <script type="text/javascript">
        $(document).ready(function () {
            new AjaxUpload('btnUploadImageFile', {
                action: '../ashx/CustomerUploadImageFile.ashx',
                name: 'myImageFile',
                multiple: true,
                responseType: 'json',
                onSubmit: function (file, ext) {
                    if (!(ext && /^(jpg|jpeg|png)$/.test(ext.toLowerCase()))) {
                        effect.Dialog.alert("只能上传图片文件");
                        return false;
                    }
                    var cusid = '<%=base.Profile.CusId %>';
                    this.setData({
                        'cusid': cusid
                    });
                    $('#divUploadImageFileStatus').html('<img src=\"../js/ajaxupload/images/sloading.gif\" width=\"15\" height=\"15\" /> ');
                    this.disable();
                },
                onComplete: function (file, response) {
                    this.enable();
                    if (response.status == '1') {
                        $('#divUploadImageFileStatus').html('');
                        var filePaths = response.filePath.split('|');
                        var fileNames = response.fileName.split('|');
                        for (var i = 0; i < filePaths.length; i++) {
                            if (filePaths[i] != null && filePaths[i] != '') {
                                appendToFileList(filePaths[i].split('/')[2], filePaths[i], fileNames[i]);
                            }
                        }
                        if ($("#hfImagePath").val() == ',') {
                            $("#hfImagePath").val('');
                        }
                        $("#hfImagePath").val($("#hfImagePath").val() + response.filePath);
                        if ($("#hfImageName").val() == ',') {
                            $("#hfImageName").val('');
                        }
                        $("#hfImageName").val($("#hfImageName").val() + response.fileName);
                    }
                    else {
                        $('#divUploadImageFileStatus').html('');
                        effect.Dialog.alert(response.note);
                    }
                }
            });
        });

        function appendToFileList(num, path, name) {
            var newLine = "<li id=\"divImage_" + num + "\">";
            newLine += "<p class=\"left rdname\" title=\"" + name + "\">" + name + "</p>";
            newLine += "<div class=\"right\" style=\"padding-right: 10px;\">";
            newLine += "<a href=\"javascript:void(0)\" class=\"blue\" onclick=\"delImage('" + num + "','" + path + "')\">删除</a>";
            newLine += "</div>";
            newLine += "</li>";
            $("#divImageList").append(newLine);
            InitDivImageListVisibility();

            //计算总数
            $("#DivImageCount").html("共计 <em class=\"s_red\">" + $("#divImageList > li").size() + "</em>个");
        }

        function delImage(imageId, imagePath) {
            $.get("/ashx/Action.ashx?m" + Math.random(), { option: "imageDel", imagepath: imagePath }, function (data) {
                if (data == "true") {
                    confirmDelImage(imageId, imagePath);
                }
            });
        }

        function confirmDelImage(imageId, imagePath) {
            $("#divImage_" + imageId).remove();
            var arrImagePath = $("#hfImagePath").val().split('|');
            var arrImageName = $("#hfImageName").val().split('|');
            $.each(arrImagePath, function (i) {
                if (this == imagePath) {
                    arrImagePath.splice(i, 1);
                    arrImageName.splice(i, 1);
                }
            });
            $("#hfImagePath").val(arrImagePath.join('|'));
            $("#hfImageName").val(arrImageName.join('|'));
            InitDivImageListVisibility();
            //计算监测音频总数
            $("#DivImageCount").html("共计 <em class=\"s_red\">" + $("#divImageList > li").size() + "</em>个");
        }

        function InitDivImageListVisibility() {
            if ($("#divImageList>li").length != 0) {
                $("#divImageList").show();
            }
            else {
                $("#divImageList").hide();
            }
        }

        function validConfirm() {
            if ($("#hfImagePath").val() == ",") {
                effect.Dialog.alert('请上传图片文件!');
                return false;
            }
            var abnormalType = $('input:radio[name="abnormal"]:checked').val();
            $("#hfAbnormalType").val(abnormalType);
            return true;
        }
    </script>
    <style type="text/css">
        .check_radio_content {
            background: rgba(0, 0, 0, 0) url("../images/bor_zx.gif") repeat-y scroll center bottom;
            border-bottom: 1px solid #d4d4d4;
            border-left: 1px solid #d4d4d4;
            border-right: 1px solid #d4d4d4;
            clear: both;
            display: block;
        }

            .check_radio_content li {
                border-bottom: 1px solid #d4d4d4;
                display: inline;
                float: left;
                padding: 6px 0;
                width: 50%;
            }

                .check_radio_content li .rdname {
                    font-family: Arial;
                    line-height: inherit;
                    padding-left: 10px;
                }

        .rdname {
            line-height: 20px;
            overflow: hidden;
            text-overflow: ellipsis;
            width: 160px;
        }
    </style>
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
                    <div class="creat_jy clearfix" style="padding-top: 70px;">
                        <div>
                            <span class="creat_tit left fb label_wid">上传图片：</span>
                            <input type="button" id="btnUploadImageFile" style="height: 25px; line-height: 25px; *line-height: 18px; margin-left: 10px; float: left;"
                                value="上传图片" class="lispan btnGray" />
                            <div id="divUploadImageFileStatus" class="theFont left" style="margin: 5px">
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="bjm_paln">
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
                        <div class="check_radio mtp10">
                            <div class="check_radio_tit clearfix">
                                <div class="left lh22 bold">
                                    已上传的图片
                                </div>
                                <div class="right" id="DivImageCount">
                                    共计 <em class="s_red">0</em> 个
                                </div>
                            </div>
                            <div class="check_radio_content">
                                <ul class="clearfix" id="divImageList">
                                </ul>
                            </div>
                        </div>
                        <div class="ty_btnbox">
                            <asp:Button ID="btnNext" runat="server" Text="确定" CssClass="btnBlue" OnClick="btnNext_Click"
                                OnClientClick="return validConfirm();" />
                            <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btnBlue" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <jianboUI:Footer ID="jianbo_Footer" runat="server" />
        <!--上传的图片文件路径-->
        <asp:HiddenField ID="hfImagePath" runat="server" Value="," />
        <asp:HiddenField ID="hfImageName" runat="server" Value="," />
        <asp:HiddenField ID="hfAbnormalType" runat="server" Value="" />
    </form>
</body>
</html>
