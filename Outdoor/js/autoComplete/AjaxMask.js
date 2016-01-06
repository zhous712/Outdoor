var AjaxMask = {
    addMask: function () {
        var maskDiv = '<div id="divmask" style="background: #CCC; display: none;"><img id="loading" src="/images/loading.gif" /></div>';
        $("body").append(maskDiv);
        var offsettop = 0;
        var offsetleft = 0;
        var width = $(document).width() + "px";
        var height = $(document).height() + "px";
        $("#divmask").css({
            position: "absolute",
            'top': offsettop,
            'left': offsetleft,
            'width': width,
            'height': height,
            'z-index': 2,
            'opacity': '0.4'
        });
        $("#loading").center();
    },
    showMask: function () {
        $("#loading").center();
        $("#divmask").show();
    },
    hideMask: function () {
        $("#divmask").hide();
    }
};

var getWH = function () {
    var d = document, doc = d[d.compatMode == "CSS1Compat" ? 'documentElement' : 'body'];
    return function (f) {
        return {
            w: doc[(f ? 'client' : 'scroll') + 'Width'],
            h: f ? doc.clientHeight : Math.max(doc.clientHeight, doc.scrollHeight)
        };
    };
} ();

jQuery.fn.center = function () {


    this.css("position", "absolute");

    this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");

    this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");

    return this;

};