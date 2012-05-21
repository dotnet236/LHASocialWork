(function ($) {

    if ($.fn.complexDateTime != null) return;
    
    $.fn.confirmableActionLink = function (msg) {
        $(this).click(function () {
            return confirm(msg);
        });
    };

})(jQuery);