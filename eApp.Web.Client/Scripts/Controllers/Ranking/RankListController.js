var RankListController = function ($scope, ConfirmFactory) {

    var output = new Array();
    
    generateTable("Requested");

    function generateTable(status) {
    
        var dataObject = {
            columns: [
                {
                    "data": "reqid",
                    render: function (data, type, full, row) {
                        var iStats;

                        if (status != "Expired") {
                            if (full.sts == "processed") {
                                iStats = '<small class="label pull-right bg-green">' + full.sts + '</small>';
                            }
                            else if (full.sts == "downloaded") {
                                iStats = '<small class="label pull-right bg-red">' + full.sts + '</small>';
                            }
                            else {
                                iStats = '<small class="label pull-right bg-orange">' + full.sts + '</small>';
                            }
                        } else {
                            iStats = '<small class="label pull-right bg-gray">Archived</small>';

                        }
                        return data + iStats;

                    }, "title": "Request Id."
                },
                { "data": "rtype", "title": "Ranking Type" },                
                {
                    "data": "dreq",
                    render: function (data, type, full, row) {

                        return full.dreq + " <small class='label bg-orange pull-right'>" + full.exprtn + "</small>";

                    }, "title": "File Details", "class": "tablemid"
                },
                {
                    "data": "lnk",
                    render: function (data, type, full, row) {
                        var iStats;

                        console.log(full.sts);

                        if (status != "Expired") {
                            if (full.sts != "processing") {
                                iStats = '<button class="btn btn-default btn-xs margin-r-5" style="width: 30px;" onclick="angular.element(document.getElementById(\'rlc\')).scope().DownloadPDF(\'' + full.lnk + '\', \'' + full.reqid + '\');"><i class="fa fa-cloud-download"></i></button></div>';
                            }
                            else {
                                iStats = '';
                            }
                        } else {
                            iStats = '';

                        }

                        return iStats;

                    }, "title": "Option", "class": "tablemid"
                },
                { "data": "lnk", "title": "Option" }

            ]
        };

        var table = $('#example').DataTable({
            "iDisplayLength": 5,
            "bDestroy": true,
            "autoWidth": true,
            "bLengthChange": false,
            "processing": true,
            "bServerSide": false,
            "sAjaxSource": "/RankRoute/GetAllRanking?status=" + status,
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
            //'order': [[1, 'asc']],
            "aoColumnDefs": [{ "bVisible": false, "aTargets": [4] }],
            initComplete: function () {

                //for (var i = 1; i <= 2; i++) {
                //    searchTB('x' + i, i);
                //}

            }
        });
        
    }

    //function searchTB(obj, index) {

    //    var x = document.getElementById(obj);
    //    var option = document.createElement("option");
    //    while (x.options.length > 0) {
    //        x.remove(0);
    //    }
    //    option.text = " ";
    //    x.add(option);
    //    table.column(index).data().unique().sort().each(function (d, j) {
    //        var option = document.createElement("option");
    //        option.text = d;
    //        x.add(option);
    //    });
    //    $('#' + obj).on('change', function () {
    //        var elem = document.getElementById(obj);
    //        table.columns(index).search(elem.options[elem.selectedIndex].value).draw();
    //    });

    //}
    
    $scope.DownloadPDF = function (value, xy) {
            
        window.open(
            'http://services.danubeco.com/ranking/' + value,
            '_blank'
        );

    }
    
    $scope.LoadTable = function (status) {

        generateTable(status);

    }

}

RankListController.$inject = ['$scope', 'ConfirmFactory'];

