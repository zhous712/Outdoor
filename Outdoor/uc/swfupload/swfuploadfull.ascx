<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="swfuploadfull.ascx.cs" Inherits="WebAgent.uc.swfupload.swfuploadfull" %>
<link href="<%=base.MonitorUrl %>uc/swfupload/skin/full/swfupload.css?<%=base.AppVersion %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="<%=WebRootJs %>/swfUpload v2.2.0.1/swfupload.js?<%=base.AppVersion %>"></script>
<script type="text/javascript" src="<%=WebRootJs %>/swfUpload v2.2.0.1/plugins/swfupload.swfobject.js?<%=base.AppVersion %>"></script>
<script type="text/javascript" src="<%=WebRootJs %>/swfUpload v2.2.0.1/plugins/swfupload.queue.js?<%=base.AppVersion %>"></script>
<script type="text/javascript" src="../uc/swfupload/skin/full/fileprogress.js?<%=base.AppVersion %>"></script>
<script type="text/javascript" src="../uc/swfupload/skin/full/handlers.js?<%=base.AppVersion %>"></script>
<script type="text/javascript">
    var swfu;
    
    SWFUpload.onload = function () {
        var settings = {
            // Flash Settings
            flash_url: "../uc/swfupload/swfupload.swf",
            upload_url: "../ashx/UploadTest.ashx",
            post_params: {
                "ASPSESSID": "<%=Session.SessionID %>"
            },

            // File Upload Settings
            file_size_limit: "2 MB",
            file_types: "*.jpg",
            file_types_description: "JPG Images",
            file_upload_limit: "0",    // Zero means unlimited
            file_queue_limit: 0,
            custom_settings: {
                progressTarget: "fsUploadProgress",
                cancelButtonId: "btnCancel"
            },
            debug: false,

            // Button Settings
            button_placeholder_id: "spanButtonPlaceholder",
            button_width: 61,
            button_height: 22,
            button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
            button_cursor: SWFUpload.CURSOR.HAND,

            // The event handler functions are defined in handlers.js
            swfupload_loaded_handler: swfUploadLoaded,
            file_queued_handler: fileQueued,
            file_queue_error_handler: fileQueueError,
            file_dialog_complete_handler: fileDialogComplete,
            upload_start_handler: uploadStart,
            upload_progress_handler: uploadProgress,
            upload_error_handler: uploadError,
            upload_success_handler: uploadSuccess,
            upload_complete_handler: uploadComplete,

            // SWFObject settings
            minimum_flash_version: "9.0.28",
            swfupload_pre_load_handler: swfUploadPreLoad,
            swfupload_load_failed_handler: swfUploadLoadFailed
        };

        swfu = new SWFUpload(settings);
    }

</script>

<div id="content">
    <div id="divSWFUploadUI">
        <div class="fieldset flash" id="fsUploadProgress">
            <span class="legend">上传队列</span>
        </div>
        <p>
            <span id="spanButtonPlaceholder"></span>
            <input id="btnUpload" type="button" value="选择文件" style="width: 61px; height: 22px;
                font-size: 8pt;" />
            <input id="btnCancel" type="button" value="取消上传" disabled="disabled" style="margin-left: 2px;
                height: 22px; font-size: 8pt;" />
        </p>
        <br style="clear: both;" />
    </div>
    <noscript style="background-color: #FFFF66; border-top: solid 4px #FF9966; border-bottom: solid 4px #FF9966;
        margin: 10px 25px; padding: 10px 15px;">
        We're sorry. SWFUpload could not load. You must have JavaScript enabled to enjoy
        SWFUpload.
    </noscript>
    <div id="divLoadingContent" class="content" style="background-color: #FFFF66; border-top: solid 4px #FF9966;
        border-bottom: solid 4px #FF9966; margin: 10px 25px; padding: 10px 15px; display: none;">
        SWFUpload is loading. Please wait a moment...
    </div>
    <div id="divLongLoading" class="content" style="background-color: #FFFF66; border-top: solid 4px #FF9966;
        border-bottom: solid 4px #FF9966; margin: 10px 25px; padding: 10px 15px; display: none;">
        SWFUpload is taking a long time to load or the load has failed. Please make sure
        that the Flash Plugin is enabled and that a working version of the Adobe Flash Player
        is installed.
    </div>
    <div id="divAlternateContent" class="content" style="background-color: #FFFF66; border-top: solid 4px #FF9966;
        border-bottom: solid 4px #FF9966; margin: 10px 25px; padding: 10px 15px; display: none;">
        We're sorry. SWFUpload could not load. You may need to install or upgrade Flash
        Player. Visit the <a href="http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash">
            Adobe website</a> to get the Flash Player.
    </div>
</div>