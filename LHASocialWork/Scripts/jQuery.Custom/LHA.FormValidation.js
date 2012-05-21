(function ($) {

    $(function () {

        $(".tabbedFormValidation").each(function () {

            function selectTab() {
                var invalidInputs = $(".input-validation-error");
                if (invalidInputs.length == 0) return;
                var errorMsg = $(".input-validation-error").first();
                var input = errorMsg.parent().find(".input-validation-error").first();
                var name = errorMsg.parents(".ui-tabs-panel").first().attr("id");
                var tab = $("#tabs").find("a[href='#" + name + "']");
                var index = tab.parents("ul").first().children("li").index(tab.parents("li").first());
                if (index >= 0) {
                    $("#tabs").tabs("select", index);
                    input.focus();
                }
            }

            $(this).submit(function (evt) { setTimeout(selectTab, 100); });

            selectTab();
        });
    });

})(jQuery);