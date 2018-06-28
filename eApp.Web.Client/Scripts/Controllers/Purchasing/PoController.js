var PoController = function ($scope, ConfirmFactory, dnbservices) {

    var output = new Array();

    generateTable("Available");

    function generateTable(status) {
    
        var dataObject = {
            columns: [
                {
                    "data": "pono",
                    render: function (data, type, full, row) {
                        var iStats;

                        if (status != "Expired") {
                            if (full.filestatus == "Available") {
                                iStats = '<small class="label pull-right bg-green">Available</small>';
                            }
                            else if (full.filestatus == "Downloaded") {
                                iStats = '<small class="label pull-right bg-red">Downloaded</small>';
                            }
                        }
                        else {
                            iStats = '<small class="label pull-right bg-gray">Archived</small>';

                        }
                        return data + iStats;

                    }, "title": "Purchasing No."
                },
                { "data": "location", "title": "Branch" },               
                {
                    "data": "released",
                    render: function (data, type, full, row) {

                        return full.released + " <small class='label bg-orange pull-right'>" + full.expiration + "</small>";
                        
                    }, "title": "File Details", "class": "tablemid"
                },
                {
                    "data": "link",
                    render: function (data, type, full, row) {
                        var iStats;

                        if (status != "Expired") {
                            iStats = '<button class="btn btn-default btn-xs margin-r-5" style="width: 30px;" onclick="angular.element(document.getElementById(\'poc\')).scope().DownloadPDF(\'' + full.link + '\', \'' + full.pono + '\');"><i class="fa fa-cloud-download"></i></button></div>';
                        }
                        else {
                            iStats = '';

                        }

                        return iStats;

                    }, "title": "Option", "class": "tablemid"
                },
                { "data": "link", "title": "Option" }

            ]
        };

        var table = $('#example').DataTable({
            "iDisplayLength": 6,
            "bDestroy": true,
            "autoWidth": true,
            "bLengthChange": false,
            "processing": true,
            "bServerSide": false,
            "sAjaxSource": "/PORoute/GetAllPO?status=" + status,
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
            'order': [[1, 'asc']],
            "aoColumnDefs": [{ "bVisible": false, "aTargets": [4] }],
            initComplete: function () {

                //for (var i = 1; i <= 2; i++) {
                //    searchTB('x' + i, i);
                //}

            }
        });

    }

    //function searchTB(obj, index) {

    //    console.log(obj);

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
    
    function UpdateDB(value) {

        var formData = new FormData();

        formData.append("pono", value);

        dnbservices.PostUpdate('/PORoute/SubmitPO', formData);
        
    }
    
    $scope.LoadTable = function (status) {

        generateTable(status);

    }
    
    $scope.DownloadPDF = function (link, pono) {

        UpdateDB(pono);
        
        window.open(
            'http://services.danubeco.com/supplier/' + link,
            '_blank'
        );

        //ConfirmFactory.FormInfo(value, xy);        

    }

}

PoController.$inject = ['$scope', 'ConfirmFactory', 'dnbservices'];

