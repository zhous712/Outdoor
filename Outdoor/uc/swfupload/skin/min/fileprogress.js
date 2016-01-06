/*
	A simple class for displaying file information and progress
	Note: This is a demonstration only and not part of SWFUpload.
*/

// Constructor
// file is a SWFUpload file object
// targetID is the HTML element id attribute that the FileProgress HTML structure will be added to.
// Instantiating a new FileProgress object with an existing file will reuse/update the existing DOM elements
function FileProgress(file, targetID) {
    this.fileProgressID = file.id;
	if ($("#" + this.fileProgressID).length == 0) {
	    var loadList = "<div class=\"load_list\" id=\"" + this.fileProgressID + "\">";
	    loadList += "<div class=\"l_left\">";
	    loadList += "<p class=\"l_name\">" + file.name.substr(0, 14) + "</p>";
	    loadList += "<div class=\"l_pro_bar\">";
	    loadList += "<div class=\"bar_box\">";
	    loadList += "<div class=\"bar_pro\">";
	    loadList += "<span class=\"bar_a\"></span>";
	    loadList += "<span class=\"bar_m\" style=\"width:0%;\"></span>";
	    loadList += "<div class=\"clear\"></div>";
	    loadList += "</div>";
	    loadList += "<div class=\"bar_value\">0%</div>";
	    loadList += "</div>";
	    loadList += "<div class=\"bar_end\" style=\"display:none;\">已完成</div>";
	    loadList += "</div>";
	    loadList += "</div>";
	    loadList += "<div class=\"l_right\"><a href=\"javascript:void(0)\" class=\"tab_cancel\">取消</a></div>";
	    loadList += "<div class=\"clear\"></div>";
	    loadList += "</div>";
	    $("#" + targetID).append(loadList);
	}
	this.progressCancel = $("#" + this.fileProgressID).children(".l_right");
	this.progress = $("#" + this.fileProgressID).children(".l_left").children(".l_pro_bar").children(".bar_box");
	this.progressText = this.progress.children(".bar_value");
	this.progressBar = this.progress.children(".bar_pro").children(".bar_m");
	this.progressStatus = $("#" + this.fileProgressID).children(".l_left").children(".l_pro_bar").children(".bar_end");
}

FileProgress.prototype.setProgress = function (percentage) {
    this.progressBar.width(percentage + "%");
    this.progressText.text(percentage + "%");
};
FileProgress.prototype.setComplete = function () {
    this.progress.hide();
    this.progressStatus.show();
};
FileProgress.prototype.setError = function () {
    this.progress.hide();
    this.progressCancel.hide();
    this.progressStatus.text("上传出错");
    this.progressStatus.show();
};
FileProgress.prototype.setCancelled = function () {
    this.progress.hide();
    this.progressCancel.hide();
    this.progressStatus.text("已取消");
    this.progressStatus.show();
};
FileProgress.prototype.setStatus = function (status) {
	
};

// Show/Hide the cancel button
FileProgress.prototype.toggleCancel = function (show, swfUploadInstance) {
    if (show) {
        this.progressCancel.show();
    }
    else {
        this.progressCancel.hide();
    }
    if (swfUploadInstance) {
        var fileID = this.fileProgressID;
        this.progressCancel.children("a").click(function () {
            swfUploadInstance.cancelUpload(fileID);
            return false;
        });
    }
};
