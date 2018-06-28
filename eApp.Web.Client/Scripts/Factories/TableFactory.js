var TableFactory = function ($http, $q) {

    console.log("d");

    var showInRow = function () {

        //var table = $('#' + tbl).DataTable({
        //    "bDestroy": true,
        //    "bLengthChange": false,
        //    responsive: true
        //});

        //table.rows().eq(0).each(function (idx) {
        //    var row = table.row(idx);
        //    if (row.child.isShown()) {
        //        row.child.hide();
        //    }
        //});

        //tr = obj.closest('tr');
        //row = table.row(tr);

        //if (row.child.isShown()) {

        //    row.child(false).remove();
        //    tr.removeClass('shown');
        //}

        //insertInRow(content, row);

    }

    //function insertInRow(content, row) {

    //    $.get(content, function (viewdata) {

    //        row.child(viewdata, "details").show();
            
    //    });

    //}

    return {

        showInRow: showInRow
    };

}

TableFactory.$inject = ['$http', '$q'];