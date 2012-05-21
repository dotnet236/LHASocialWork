(function ($) {

    if ($.fn.ajaxPager != null) return;

    $.fn.ajaxPager = function () {
        var pager = $(this);
        var contentContainer = pager.parents(".pagerContentContainer").first();
        var links = pager.find(".paginationLeft, .paginationRight").find("a");
        links.each(function () {
            var link = $(this);
            var href = link.attr("href").replace(/ /g, "_");
            link.click(function (event) {
                alert("Link Clicked");
                event.preventDefault();
                $(contentContainer).load(href, function() {  setTimeout(function(){ $(".pagination").ajaxPager();}, 500); })
            });
        });
    };

    setTimeout(function(){ $(".pagination").ajaxPager();}, 500);

})(jQuery);