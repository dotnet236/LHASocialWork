(function ($) {

    $.fn.complexAjaxSearch = function (updateTargetId) {

        var form = $(this);
        form.attr("onsubmit", "");
        form.submit(function (evt) {

            evt.preventDefault();
            var gridOptionsModel = { SearchOptions: { Filters: []} };

            $(this).find("fieldset").each(function () {
                var fieldset = $(this);
                var name = fieldset.find("[name='PropertyName']").val();
                var value = fieldset.find("[name='PropertyValue']").val();
                var conditional = fieldset.find("[name='Conditional']").val();
                gridOptionsModel.SearchOptions.Filters.push({ PropertyName: name, PropertyValue: value, Conditional: conditional });
            });

            $.ajax({
                url: form.attr("action"),
                type: form.attr("method"),
                data: JSON.stringify(gridOptionsModel),
                contentType: "application/json; charset=utf-8",
                success: function (data) { $("#" + updateTargetId).html(data); }
            });


            return false;
        });
    }

})(jQuery);