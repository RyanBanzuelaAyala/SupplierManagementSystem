var PerformanceController = function ($scope, ConfirmFactory, dnbservices) {

    var output = new Array();
    
    $scope.filter = function () {

        var chckk = $('#ixDate').val();

        if (chckk == "") {
            $.notify({
                message: 'Blank is not allowed. Please check!',
            }, {
                    type: 'warning',
                    placement: {
                        from: 'top',
                        align: 'right'
                    },
                    offset: {
                        x: 10,
                        y: 10
                    },
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    template: '<div data-notify="container" style="z-index: 9999999999" class="col-xs-4 col-sm-4 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>'
                });
        }
        else {
            $('#cdate').val(chckk);
            initTable($('#ixDate').val());
        }
    }

    function initTable(ddate) {
    
        var dataObject = {
            columns: [
                { "data": "region", "title": "REGION" },
                { "data": "totalpocost", "title": "OVERAL TOTAL <br> PO COST" },
                { "data": "deliveredpocost", "title": "DELIVERED <br> PO COST" },
                { "data": "PercentPODelivered", "title": "% OF PO <br> DELIVERED" },
                { "data": "PercentValuePODelivered", "title": "% Value OF PO <br> DELIVERED" },
                { "data": "PercentPODeliveredLate", "title": "% OF PO <br> DELIVERED LATE" },
                { "data": "OnAverageEaPoLate", "title": "AVERAGE EA <br> PO WAS <br> LATE (DAYS)" },
                { "data": "POExpiredNotDelivered", "title": "EXPIRED PO <br> NOT DELIVERED" }
            ]
        };

        var table = $('#example').DataTable({
            dom: 'Bfrtip',
            buttons: [
                'copy', 'csv', 'excel', 'pdf', 'print'
            ],
            "iDisplayLength": 6,
            "bDestroy": true,
            "autoWidth": true,
            "bLengthChange": false,
            "processing": true,
            "bServerSide": false,
            "sAjaxSource": "/Performance/GetAllPOSummary?daterange=" + ddate,
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
                        'selectRow': false
                    }
                }
            ],
            'select': {
                'style': 'single'
            },
            'order': [[1, 'asc']]
        });

        table
            .on('click', 'tbody td', function (e) {

                var data = table.row(this).data();

                var tr = $(this).closest('tr');
                var row = table.row(tr);

                row.child(forTable()).show();                
                tr.addClass('shown');

                forChildTable($('#cdate').val());
                

            });        

    }
    
    function forTable() {
    
        var htmldatah = '<div class="col-md-12 bg-white">' +
                    '<hr />' +
                    '<table id="exampleX" class="table table-bordered table-hover table-striped" width= "100%"></table> <hr /></div>';       

        return htmldatah;

    }

    function forChildTable(date) {
    
        var dataObjectX = {
            columns: [
                { "data": "store", "title": "STORE" },
                { "data": "totalposcost", "title": "TOTAL PO <BR> COST" },
                { "data": "totalpostdeliveredcost", "title": "TOTAL PO <BR> DELIVERED COST" },
                { "data": "totalnoofpos", "title": "TOTAL NO <BR> OF PO" },
                { "data": "totalnoofposdelivered", "title": "DELIVERED <BR> ON TIME" },
                { "data": "totalnoofexpiredpo", "title": "EXPIRED NOT <BR> DELIVERED" },
                { "data": "totalnooflatedelivered", "title": "WAS LATE <BR> DELIVERED PO" },
                { "data": "totallatedays", "title": "NO OF <BR> DAYS LATE " }
            ]
        };

        var tableX = $('#exampleX').DataTable({
            //"iDisplayLength": 6,
            dom: 'Bfrtip',
            buttons: [
                'copy', 'csv', 'excel', 'pdf', 'print'
            ],
            "bDestroy": true,
            "bFilter": false,
            "autoWidth": true,
            "bLengthChange": false,
            "processing": true,
            "bServerSide": false,
            "sAjaxSource": "/Performance/GetAllPOSummaryByBranchRegion?daterange=" + date,
            "sAjaxDataProp": "",
            "columns": dataObjectX.columns,
            "rowReorder": {
                selector: 'td:nth-child(0)'
            },
            "autoWidth": true,
            'bLengthChange': false,
            'columnDefs': [
                {
                    'targets': 0,
                    'checkboxes': {
                        'selectRow': false
                    }
                }
            ],
            'select': {
                'style': 'single'
            },
            'order': [[1, 'asc']]
        });

        tableX
            .on('click', 'tbody td', function (e) {

                var data = tableX.row(this).data();
                
                // hide all open row child
                tableX.rows().eq(0).each(function (idx) {
                    var row = tableX.row(idx);

                    if (row.child.isShown()) {
                        row.child.hide();
                    }
                });

                var tr = $(this).closest('tr');
                var row = tableX.row(tr);

                if (row.child.isShown()) {
                    
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                                
                    $('#xtt').remove();
                    row.child(forChildTableElement()).show();
                    tr.addClass('shown');

                    forElementChildTable(data.store, $('#cdate').val());
                    
                }
            });
    }
    
    function forChildTableElement() {
    

        htmldatah = '<div id="xtt" class="col-md-12 bg-white text-black">' +
            '<hr />' +
            '<table id="exampleXX" class="table table-bordered table-hover table-striped" width= "100%"></table><hr /></div>';

        return htmldatah;

    }

    function forElementChildTable(store, date) {
    
        var dataObjectXX = {
            columns: [
                { "data": "store", "title": "STORE" },
                { "data": "totalposcost", "title": "TOTAL PO <BR> COST" },
                { "data": "totalpostdeliveredcost", "title": "TOTAL PO <BR> DELIVERED COST" },
                { "data": "totalnoofpos", "title": "TOTAL NO <BR> OF PO" },
                { "data": "totalnoofposdelivered", "title": "DELIVERED <BR> ON TIME" },
                { "data": "totalnoofexpiredpo", "title": "EXPIRED NOT <BR> DELIVERED" },
                { "data": "totalnooflatedelivered", "title": "WAS LATE <BR> DELIVERED PO" },
                { "data": "totallatedays", "title": "NO OF <BR> DAYS LATE " }
            ]
        };

        var tableXX = $('#exampleXX').DataTable({
            "iDisplayLength": 6,
            dom: 'Bfrtip',
            buttons: [
                'copy', 'csv', 'excel', 'pdf', 'print'
            ],
            "bDestroy": true,
            "bFilter": false,
            "autoWidth": true,
            "bLengthChange": false,
            "processing": true,
            "bServerSide": false,
            "sAjaxSource": "/Performance/GetAllPOSummaryByPOPerBranchRegion?store=" + store + "&daterange=" + date,
            "sAjaxDataProp": "",
            "columns": dataObjectXX.columns,
            "rowReorder": {
                selector: 'td:nth-child(0)'
            },
            "autoWidth": true,
            'bLengthChange': false,
            'columnDefs': [
                {
                    'targets': 0,
                    'checkboxes': {
                        'selectRow': false
                    }
                }
            ],
            'select': {
                'style': 'single'
            },
            'order': [[1, 'asc']]
        });
        
    }


    $scope.removeRow = function (irow, tbl) {

        alert(irow);

        var row = tbl.row(irow);

        if (row.child.isShown()) {

            row.child.hide();

        }        
    }
    
}

PerformanceController.$inject = ['$scope', 'ConfirmFactory', 'dnbservices'];

