(function ($) {

    if ($.fn.complexDateTime != null) return;

    function getTime(date) {
        var date = new Date();
        var hour = date.getHours();
        var ampm = hour < 12 ? 'am' : 'pm';
        if (hour > 13) hour = hour - 12;
        var minutes = date.getMinutes();
        if (minutes < 10) minutes = "0" + minutes;
        return hour + ":" + minutes + ampm;

    }

    function getDate(strDate) {
        var date = new Date(strDate);
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();

        return month + "/" + day + "/" + year;
    }

    $.fn.complexDateTime = function (options) {

        var now = new Date();
        var container = $(this);
        var startDateEl, endDateEl, startTimeEl, endTimeEl;
        var lastStartDate, lastStartTime, lastEndDate, lastEndTime;

        function init() {
            startDateEl = container.find("input.startDate");
            startTimeEl = container.find("input.startTime");
            endDateEl = container.find("input.endDate");
            endTimeEl = container.find("input.endTime");

            lastStartDate = startDateEl.val();
            lastStartTime = startTimeEl.length > 0 ? startTimeEl.val() : null;
            lastEndDate = endDateEl.val();
            lastEndTime = endTimeEl.length > 0 ? endTimeEl.val() : null;

            setDatePickers();
            if (startTimeEl.length > 0 && endTimeEl.length > 0)
                setTimePickers();
        }

        function validateEndDate(dateText) {
            var selectedEndDate = new Date(dateText);
            if (selectedEndDate < new Date(startDateEl.val())) {
                alert("           End date must be after start date");
                endDateEl.val(lastEndDate == "" ? "" : getDate(lastEndDate));
                return;
            }
            lastEndDate = selectedEndDate;
        }

        function validateStartDate(dateText) {
            var selectedStartDate = new Date(dateText);
            if (selectedStartDate > new Date(endDateEl.val())) {
                alert("           Start date must be before end date");
                startDateEl.val(lastStartDate == "" ? "" : getDate(lastStartDate));
                return;
            }
            lastStartDate = selectedStartDate;
        }

        function setDatePickers() {
            var startDateOptions = { min: new Date(), onClose: validateStartDate };
            startDateEl.datepicker(startDateOptions);

            var endDateOptions = { min: new Date(), onClose: validateEndDate };
            endDateEl.datepicker(endDateOptions);
        }

        function setTimePickers() {
            var startTimeEl = container.find("input.startTime");
            var endTimeEl = container.find("input.endTime");

            function getConvertableTime(time) {
                var safeTime = time.split(new RegExp("am|pm"))[0];
                var values = safeTime.split(":");
                var hour = parseInt(values[0], 10);
                var minute = values[1];
                var split = time.split("pm");

                if (split.length == 2 && hour != 12)
                    hour += 12;

                return "1/1/1970 " + hour + ":" + minute;
            }

            var correctStartTimeEl = $("<input type='hidden' />").attr("name", startTimeEl.attr("name")).val(getConvertableTime(lastStartTime));
            startTimeEl.attr("name", startTimeEl.attr("name") + "_remove");
            startTimeEl.blur(function () { correctStartTimeEl.val(getConvertableTime($(this).val())); });
            startTimeEl.parent().append(correctStartTimeEl);

            var correctEndTimeEl = $("<input type='hidden' />").attr("name", endTimeEl.attr("name")).val(getConvertableTime(lastEndTime));
            endTimeEl.attr("name", endTimeEl.attr("name") + "_remove");
            endTimeEl.blur(function () { correctEndTimeEl.val(getConvertableTime($(this).val())); });
            endTimeEl.parent().append(correctEndTimeEl);
        }

        init();
    };

})(jQuery);