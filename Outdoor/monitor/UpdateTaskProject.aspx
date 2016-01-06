<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateTaskProject.aspx.cs" Inherits="Outdoor.monitor.UpdateTaskProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>编辑点位-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
    <jianboUI:Keyword ID="jianbo_Keyword" runat="server" />
    <link href="../css/base.css" rel="stylesheet" type="text/css" />
    <link href="../css/my_radio.css" rel="stylesheet" type="text/css" />
    <link href="../css/paiqi.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/bgiframe-2.1.2-1/jquery.bgiframe.js" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.artDialog.source.js?skin=blue" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/jquery.alert.js" type="text/javascript"></script>
    <script src="../js/artDialog4.1.6/plugins/iframeTools.js" type="text/javascript"></script>
    <link href="../js/autoComplete/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="../js/autoComplete/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validConfirm() {
            if ($("#txtCity").val() == "") {
                $("#spanCity").show();
                return false;
            }
            else if ($("#txtDate").val() == "") {
                $("#spanDate").show();
                return false;
            }
            else {
                $("#spanCity").hide();
                $("#spanDate").hide();
                return true;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top_bg">
            <jianboUI:Header ID="jianbo_Header" runat="server" />
            <div id="bodyRadio" class="border">
                <div class="position">
                    <a href="/">首页</a> &gt; 编辑点位
                </div>
                <div id="myRadiobuy" class="my_data">
                    <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                    <div class="creat_jy clearfix" style="padding-top: 30px;">
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">城市：</span>
                            <input type="text" id="txtCity" runat="server" class="input ty_txt left" style="width: 300px" /><span style="color: Red; display: none; margin-left: 5px" id="spanCity">* 城市不能为空</span>
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">区域：</span>
                            <input type="text" id="txtAreaName" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear"></div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">街道地址：</span>
                            <input type="text" id="txtStreetAddress" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">楼宇：</span>
                            <input type="text" id="txtBlockName" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">点位：</span>
                            <input type="text" id="txtPointName" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">媒体类型：</span>
                            <input type="text" id="txtMediaType" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear"></div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">广告产品名：</span>
                            <input type="text" id="txtAdProductName" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">任务周期：</span>
                            <input type="text" id="inputDate20" runat="server" readonly="readonly" style="width: 300px" class="sear_txt" />
                            <b class="img_date" id="imgDate20"></b>
                            <span style="color: Red; display: none; margin-left: 5px" id="spanDate">* 周期不能为空</span>
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">拍照要求：</span>
                            <input type="text" id="txtPhotoRequire" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear"></div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">任务金额：</span>
                            <input type="text" id="txtPrice" runat="server" class="input ty_txt left" style="width: 300px" />
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">异常类型：</span>
                            <asp:RadioButtonList ID="radioAbnormalType" RepeatDirection="Horizontal" runat="server">
                                <asp:ListItem Value="0" Text="正常" />
                                <asp:ListItem Value="1" Text="黑屏" />
                                <asp:ListItem Value="2" Text="花屏" />
                                <asp:ListItem Value="3" Text="未上画" />
                                <asp:ListItem Value="4" Text="破损" />
                                <asp:ListItem Value="5" Text="电梯维修" />
                                <asp:ListItem Value="6" Text="遮挡" />
                                <asp:ListItem Value="7" Text="异常" />
                            </asp:RadioButtonList>
                            <div class="clear">
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
        <asp:HiddenField ID="hfTid" runat="server" />
        <asp:HiddenField ID="hfStatus" runat="server" />
        <!--多选日期控件-->
        <link href="../js/datepicker/style/date/datepicker.css" rel="stylesheet" />
        <script type="text/javascript" src="../js/datepicker/js/date/datepicker.js"></script>
        <script type="text/javascript" src="../js/datepicker/js/date/eye.js"></script>
        <script type="text/javascript" src="../js/datepicker/js/date/layout.js"></script>
    </form>
</body>
</html>
