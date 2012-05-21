(function ($) {

    if ($.fn.complexDateTime != null) return;

    $.fn.multiAutoComplete = function (url, options) {

        var opt = {
            uniqueList: true,
            name: $(this).attr("name")
        };

        $.extend(opt, options);

        return this.each(function () {
            var list = $("<div/>").addClass('multiAutoCompleteSelectList');
            var input = $(this);
            input.parent().append(list);
            var autoComplete = input.autocomplete({
                source: url,
                minLength: 2,
                select: function (event, ui) {
                    var itm = ui.item;
                    if (opt.uniqueList && alreadySelected(itm.id))
                        return alert("No duplicates allowed.");
                    else {
                        var linkContainer = $("<div />");
                        var valueLabel = $("<label />").text(itm.value);
                        var removeLink = $("<a href='javascript:' />").text("Remove").click(function () { linkContainer.remove() });
                        var idInput = $("<input type='hidden' name='" + opt.name + "' />").attr({ value: itm.id });
                        linkContainer.append(valueLabel);
                        linkContainer.append(removeLink);
                        linkContainer.append(idInput);
                        list.append(linkContainer);
                    }
                    setTimeout(function () { input.val("") }, 100);
                },
                focus: function () { return false; }
            }).attr("name", input.attr("name") + "_Autocomplete"); //Needed prevent model binder from using the value of this input.

            function alreadySelected(id) {
                return list.find("input:hidden[value='" + id + "']").length != 0;
            }
        });

    };
})(jQuery);