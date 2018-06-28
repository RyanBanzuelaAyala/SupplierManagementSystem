
var iCDate = function () {

    var init = function (from, to) {        

        from.val(normalizeDate(addDays(new Date(), 1)));

        to.val(normalizeDate(addDays(new Date(), 30)));

    }

    function addDays(date, days) {

        var result = new Date(date);

        result.setDate(result.getDate() - days);

        return result;

    }

    function normalizeDate(dateString) {

        // If it's not at least 6 characters long (8/8/88), give up.
        if (dateString.length && dateString.length < 6) {
            return '';
        }

        var date = new Date(dateString),
            month, day;

        // If input format was in UTC time, adjust it to local.
        if (date.getHours() || date.getMinutes()) {
            date.setMinutes(date.getTimezoneOffset());
        }

        month = date.getMonth() + 1;
        day = date.getDate();

        // Return empty string for invalid dates
        if (!day) {
            return '';
        }

        // Return the normalized string.
        return date.getFullYear() + '-' + (month > 9 ? '' : '0') + month + '-' + (day > 9 ? '' : '0') + day;
    }

    return {
        init: init
    }

}();
