

var iSupplierController = function () {

    var table;

    var init = function () {

        var dataObject = {
            columns: [
                { "data": "sid", "title": "Supplier Id", "class": "tablemid" },
                { "data": "region", "title": "Region", "class": "tablemid" },
                { "data": "password", "title": "Password", "class": "tablemid" },
                { "data": "mobile", "title": "Mobile" },
                { "data": "status", "title": "Status" }
            ]
        };

        var table = $('#example').DataTable({
            "iDisplayLength": 10,
            "bDestroy": true,
            "autoWidth": true,
            "bLengthChange": false,
            "processing": true,
            "bServerSide": false,
            "sAjaxSource": "Supplier/GetSupplierCombiAll",
            "sAjaxDataProp": "",
            "columns": dataObject.columns,
            "rowReorder": {
                selector: 'td:nth-child(0)'
            },
            "autoWidth": true,
            'bLengthChange': false,
            'columnDefs': [
                {
                    'targets': 0,
                    'checkboxes': {
                        'selectRow': true
                    }                    
                }
            ],
            'select': {
                'style': 'single'
            },
            initComplete: function () {

            }
        });


        $('#example tbody').on('click', 'td', function () {
            var tr = $(this).closest('tr');
            var row = table.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                row.child(format(row.data())).show();
                tr.addClass('shown');
            }
        });

        function format(d) {


            return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
                '<tr>' +
                '<button onclick="angular.element(document.getElementById(\'ucc\')).scope().AddRole(\'' + d.userid + '\');" class="btn btn-default btn-xs margin-r-5"> UPDATE </button>' +
                '<button onclick="angular.element(document.getElementById(\'ucc\')).scope().AddRole(\'' + d.userid + '\');" class="btn btn-default btn-xs margin-r-5"> DELETE </button>' +
                '</td>' +
                '</tr>' +
                '</table>';


        }

    };

    return {
        init: init
    }

}();

