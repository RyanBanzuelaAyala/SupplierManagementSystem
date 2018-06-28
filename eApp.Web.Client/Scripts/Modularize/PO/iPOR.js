
var iPOR = function () {

    var init = function (stats) {

        var table = $('#example').DataTable({
            "bLengthChange": false,
            "bSort": true,
            "pageLength": 5
        });

    }


    return {
        init: init
    }

}();
