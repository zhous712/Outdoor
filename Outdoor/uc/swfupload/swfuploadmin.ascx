<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="swfuploadmin.ascx.cs" Inherits="WebAgent.uc.swfupload.swfuploadmin" %>
<script type="text/javascript" src="<%=WebRootJs %>/swfUpload v2.2.0.1/swfupload.js?<%=base.AppVersion %>"></script>
<script type="text/javascript" src="<%=WebRootJs %>/swfUpload v2.2.0.1/plugins/swfupload.swfobject.js?<%=base.AppVersion %>"></script>
<script type="text/javascript" src="<%=WebRootJs %>/swfUpload v2.2.0.1/plugins/swfupload.queue.js?<%=base.AppVersion %>"></script>
<script type="text/javascript" src="../uc/swfupload/skin/min/handlers.js?<%=base.AppVersion %>"></script>
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
            debug: false,

            // Button Settings
            button_placeholder_id: "spanButtonPlaceholder",
            button_width: 61,
            button_height: 22,
            button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
            button_cursor: SWFUpload.CURSOR.HAND,

            // The event handler functions are defined in handlers.js
            file_queued_handler: fileQueued,
            file_queue_error_handler: fileQueueError,
            file_dialog_complete_handler: fileDialogComplete,
            upload_error_handler: uploadError,
            upload_success_handler: uploadSuccess,
            upload_complete_handler: uploadComplete
        };

        swfu = new SWFUpload(settings);
    }

</script>
        <input type="text" id="txtUploadAdFile" class="input" size="24" readonly="readonly" />
        <p>
            <span id="spanButtonPlaceholder"></span>
            <input id="btnUpload" type="button" value="上传" style="width: 61px; height: 22px;
                font-size: 8pt;" />  
                <div id="divUploadAdFileStatus" class="theFont" style="display:none;">
                <img src="<%=base.MonitorUrl %>uc/swfupload/skin/min/sloading.gif" width="15" height="15" /> 
                            </div>          
        </p>