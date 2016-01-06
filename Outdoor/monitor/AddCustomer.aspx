<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCustomer.aspx.cs" Inherits="Outdoor.monitor.AddCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>添加账号-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
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
            if ($("#txtCusName").val() == "") {
                $("#spanCusName").show();
                $("#spanCusName").text("* 账号不能为空");
                return false;
            }
            else if (!checkPwd()) {
             return false;            
            }
            else if (!checkEmail()) {
                return false;
            }
            else if (!checkPhone()) {
                return false;
            }
            else {
                $("#spanCusName").hide();
                return true;
            }
        }

        function checkEmail() {
            var Email = $("#txtEmail").val();
            if (emailCheck(Email) == false && Email != '') {
                $("#spanEmail").show();
                return false;
            }
            $("#spanEmail").hide();
            return true;
        }

        function checkPwd() {
            var objUserPassword = $("#txtPwd");
            var userPassword = $.trim(objUserPassword.val());
            if (userPassword == "") {
                $("#spanPwd").show();
                $("#spanPwd").text("* 请设置密码");
                return false;
            } else if (getStrActualLen(userPassword) < 6 || getStrActualLen(userPassword) > 16) {
                $("#spanPwd").show();
                $("#spanPwd").text("* 请输入6-16位");
                return false;
            } else {
                $("#spanPwd").hide();
                return true;
            }
        }

        function checkPhone() {
            var Phone = $("#txtPhone").val();
            if ($.trim(Phone) == '') {
                $("#spanPhone").hide();
                return true;
            }
            if (phonecheck(Phone) == false) {
                $("#spanPhone").show();
                return false;
            }
            $("#spanPhone").hide();
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
                    <a href="/">首页</a> &gt; 添加账号
                </div>
                <div id="myRadiobuy" class="my_data">
                    <jianboUI:UserMenu ID="jianbo_UserMenu" runat="server" />
                    <div class="creat_jy clearfix" style="padding-top: 30px;">
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">账号：</span>
                            <input type="text" id="txtCusName" runat="server" class="input ty_txt left" /><span style="color: Red; display: none;margin-left:5px" id="spanCusName">* 账号不能为空</span>
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">密码：</span>
                            <input type="text" id="txtPwd" runat="server" class="input ty_txt left" /><span style="color: Red; display: none;margin-left:5px" id="spanPwd">* 请输入6-16位</span>
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">邮箱：</span>
                            <input type="text" id="txtEmail" runat="server" class="input ty_txt left" /><span style="color: Red; display: none;margin-left:5px" id="spanEmail">* 邮箱格式不正确</span>
                            <div class="clear"></div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">手机：</span>
                            <input type="text" id="txtPhone" runat="server" class="input ty_txt left" /><span style="color: Red; display: none;margin-left:5px" id="spanPhone">* 手机号格式不正确</span>
                            <div class="clear">
                            </div>
                        </div>
                        <div style="height: 40px">
                            <span class="creat_tit left fb label_wid">区域：</span>
                            <asp:DropDownList ID="ddlProvince" runat="server" Style="margin-right: 5px; width: 70px; display: inline; float: left;border:1px solid #ccc"
                                OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlCity" runat="server" Style="margin-right: 5px; width: 70px; display: inline; float: left;border:1px solid #ccc"
                                OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlArea" runat="server" Style="margin-right: 5px; width: 70px; display: inline; float: left;border:1px solid #ccc">
                            </asp:DropDownList>
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
    </form>
</body>
</html>
