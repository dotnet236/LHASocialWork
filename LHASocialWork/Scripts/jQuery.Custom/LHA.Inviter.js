(function ($) {

    if ($.fn.inviter != null) return;

    $.fn.inviter = function () {

        if (typeof $.fn.livequery == "undefined") {
            alert("Livequery is required to use ajax pager");
            return;
        }

        var invitationContainer = $(this);
        var pendingInvitesContainer = $(".unsentInvites");
        var invitationListContainer = invitationContainer.find(".invitationListContainer").first();
        invitationContainer.find(".inviteElement").livequery(function () {

            if (invitationListContainer.find(this).length > 0) $(this).hide;

            $(this).click(function () {

                var inviteElement = $(this);
                function addInvite(id, name) {
                    var listElement = $("<div/>").addClass("invitationListItem");
                    var listElementContent = $("<div />");
                    var clearElement = $("<div />").addClass("clear");
                    var display = $("<div />").addClass("name").text(name);
                    var removeButton = $("<div />").addClass("removeButton").text("X").click(function () {
                        if (invitationContainer.find(".invitationListItem").length == 0)
                            pendingInvitesContainer.hide();
                        listElement.remove();
                        inviteElement.show();
                    });
                    var idInput = $("<input type='hidden' />").val(id).attr("name", "invites");

                    listElementContent.append(idInput);
                    listElementContent.append(display);
                    listElementContent.append(removeButton);
                    listElement.append(listElementContent);
                    listElement.append(clearElement);
                    invitationListContainer.append(listElement);
                }

                if (inviteElement.hasClass("emailInviter")) {
                    var textArea = inviteElement.parents(".inviteByEmailContainer").first().find("textarea");
                    var emailAddresses = textArea.val();
                    var parsedEmailAddresses = parseEmailAddresses(emailAddresses);

                    for (var i = 0; i < parsedEmailAddresses.length; i++)
                        addInvite(parsedEmailAddresses[i], parsedEmailAddresses[i]);
                    textArea.val("");

                } else {
                    var id = inviteElement.find("input").val();
                    var name = inviteElement.find(".name").text();
                    addInvite(id, name);
                    inviteElement.hide();
                }

                if (invitationContainer.find(".invitationListItem").length > 0)
                    pendingInvitesContainer.show();
            });
        });
    };

    function parseEmailAddresses(emailAddresses) {
        var addressArray = [];
        if (typeof (emailAddresses) == "string") {
            var splitAddresses = emailAddresses.split(/\r|\n|\f|\t|\s|,|;/);
            for (var i = 0; i < splitAddresses.length; i++) {
                if (validEmailAddress(splitAddresses[i]))
                    addressArray.push(splitAddresses[i]);
            }
        }
        return addressArray;
    }

    function validEmailAddress(emailAddress) {
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        return emailPattern.test(emailAddress);
    }

    $(".inviter").inviter();
})(jQuery);