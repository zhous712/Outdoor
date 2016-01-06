<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImage.aspx.cs" Inherits="Outdoor.monitor.UploadImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                action: '../ashx/UploadImageFile.ashx',
                name: 'myImageFile',
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
                        $("#hfImagePath").val(response.filePath);
                        $("#txtUploadImageFile").val(response.fileName);
                    }
                    else {
                        $('#divUploadImageFileStatus').html('');
                        effect.Dialog.alert(response.note);
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="top_bg">
        <jianboUI:Header ID="jianbo_Header" runat="server" />
        <div id="bodyRadio" class="border">
            <div class="position">
                <a href="/">首页</a> &gt; 上传排期</div>
            <div id="myRadiobuy" class="my_data">
                <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                <!--通用新加代码开始-->
                <div class="creat_jy clearfix">
                    <div style="height: 10px">
                    </div>
                    <span class="creat_tit left fb label_wid">上传图片：</span>
                    <input type="text" id="txtUploadImageFile" runat="server" class="input ty_txt left"
                        size="24" readonly="readonly" />
                    <input type="button" id="btnUploadImageFile" style="height: 25px; line-height: 25px;
                        *line-height: 18px; margin-left: 10px; float: left;" value="上传图片" class="lispan btnGray" />
                    <div id="divUploadImageFileStatus" class="theFont left" style="margin: 5px">
                    </div>
                    <div class="clear">
                    </div>
                    <div class="ty_btnbox">
                        <asp:Button ID="btnNext" runat="server" Text="确定" CssClass="btnBlue" OnClick="btnNext_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btnBlue" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <jianboUI:Footer ID="jianbo_Footer" runat="server" />
    <!--上传的图片文件路径-->
    <asp:HiddenField ID="hfImagePath" runat="server" Value="," />
    </form>
</body>
</html>
