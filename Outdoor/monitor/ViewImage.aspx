<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewImage.aspx.cs" Inherits="Outdoor.monitor.ViewImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看图片-RadioBuy广告监播 广告监测 电台刊例 广告刊例</title>
    <jianboUI:Keyword ID="jianbo_Keyword" runat="server" />
    <link href="../css/base.css" rel="stylesheet" type="text/css" />
    <link href="../css/my_radio.css" rel="stylesheet" type="text/css" />
    <link href="../css/login.css" rel="stylesheet" type="text/css" />
    <link href="../css/paiqi.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            overflow-y: scroll;
            font-family: "HelveticaNeue-Light", "Helvetica Neue Light", "Helvetica Neue", Helvetica, sans-serif;
            background: #f4f4f4;
            padding: 0;
            margin: 0;
        }

        h1 {
            font-size: 31px;
            line-height: 32px;
            font-weight: normal;
            margin-bottom: 25px;
        }

        a, a:hover {
            border: none;
            text-decoration: none;
        }

            img, a img {
                border: none;
            }

        pre {
            overflow-x: scroll;
            background: #ffffff;
            border: 1px solid #cecece;
            padding: 10px;
        }

        .clear {
            clear: both;
        }

        .zoomed > .container {
            -webkit-filter: blur(3px);
            filter: blur(3px);
        }

        .container {
            width: 950px;
            margin: 0 auto;
        }

        .gallery {
            float: left;
            background: #ffffff;
            padding: 20px 20px 10px 20px;
            margin: 0;
            -webkit-box-shadow: 0 1px 3px rgba(0,0,0,0.25);
            -moz-box-shadow: 0 1px 3px rgba(0,0,0,0.25);
            box-shadow: 0 1px 3px rgba(0,0,0,0.25);
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
        }

            .gallery div {
                float: left;
                padding: 0 10px 10px 0;
            }

                .gallery div:nth-child(6n) {
                    padding-right: 0;
                }

            .gallery a, .gallery img {
                float: left;
            }
    </style>
    <script type="text/javascript">
        function validConfirm() {
            var inputs = document.getElementsByTagName('input');
            var pathandsort = '';
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "text") {
                    pathandsort += inputs[i].getAttribute("url") + ',' + inputs[i].value + '|';
                }
            }
            document.getElementById('HiddenField1').value = pathandsort;
            return true;
        }
    </script>
    <link href="../js/zoom/css/zoom.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="top_bg">
            <jianboUI:Header ID="jianbo_Header" runat="server" />
            <div class="border">
                <div class="container">
                    <div style="height: 20px">
                    </div>
                    <div class="clear">
                    </div>
                    <div class="gallery" style="margin: 0 auto;">
                    </div>
                    <div class="clear">
                    </div>
                    <div style="height: 20px">
                    </div>
                    <div class="ty_btnbox">
                        <asp:Button ID="btnNext" runat="server" Text="确定" CssClass="btnBlue" OnClick="btnNext_Click"
                            OnClientClick="return validConfirm();" />
                    </div>
                </div>
            </div>
        </div>
        <jianboUI:Footer ID="jianbo_Footer" runat="server" />
        <asp:HiddenField ID="hfDirectory" runat="server" Value="" />
        <asp:HiddenField ID="hfImagePath" runat="server" Value="," />
        <asp:HiddenField ID="HiddenField1" runat="server" Value="," />
    </form>
    <script src="../js/zoom/js/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script src="../js/zoom/js/zoom.js" type="text/javascript"></script>
    <script type="text/javascript">
        function addImg(path) {
            window.location.href = 'UploadImage.aspx?tpuid=<%=TPUId %>&path=' + path;
        }
    </script>
</body>
</html>
