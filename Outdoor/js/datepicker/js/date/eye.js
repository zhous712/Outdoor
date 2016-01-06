(function ($) {
    var eye = window.EYE = function () {
        var registered = {
            init: []
        };
        return {
            init: function () {
                $.each(registered.init, function (nr, fn) {
                    fn.call();
                });
            },
            extend: function (prop) {
                for (var i in prop) {
                    if (prop[i] != undefined) {
                        this[i] = prop[i];
                    }
                }
            },
            register: function (fn, type) {
                if (!registered[type]) {
                    registered[type] = [];
                }
                registered[type].push(fn);
            }
        };
    } ();
    $(eye.init);
})(jQuery);
