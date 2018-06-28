var RankController = function ($scope, ConfirmFactory, DataFactory, ValidateFactory) {


    var outputB = new Array();
    var outputS = new Array();

    var id1 = '';
    var id2 = '';
    var id3 = '';
    var id4 = '';
    
    brand();
    dept();
    sku();

    function brand() {

        DataFactory.iDataBR('bBrn', '/RankRoute/searchCSV/0');
        
        $("#bBrn").select2().on("select2:selecting", function (evt) {

            var brn = evt.params.args.data.id;

            if (outputB.length != 0) { // not empty

                if (brn == '0') {
                    return false;                    
                }
                else {
                    outputB.push(brn); // add to array                    
                }

            } else {
                if (brn == '0') {
                    outputB.push(brn); // add to array   
                    $('.select2').attr('disabled', 'disabled');
                }
                else {
                    outputB.push(brn); // add to array                    
                }
            }

            //output.push(brn);
            console.log(outputB);

        }).on('select2:unselecting', function (evt) {

            var brn = evt.params.args.data.id;

            var i = outputB.indexOf(brn);

            if (i != -1) {
                outputB.splice(i, 1);
            }

            console.log(outputB);
        });

    }

    $scope.pBrand = function () {

        console.log(outputB);

        if (outputB.length != 0) {

            var formDataDoc = new FormData;

            for (var i = 0; i < outputB.length; i++) {
                formDataDoc.append('arr', outputB[i]);
            }

            ConfirmFactory.UrlPostRedirect('/RankRoute/SubmitBrand', formDataDoc, '/rk/requestlist');

        }
        else {

            ValidateFactory.notify("Please check brand selection.", "warning");
        }
    }

    function dept() {

        var iCnt = 0;        

        DataFactory.iDataDEPT('dptreq', '/RankRoute/searchDept/' + id1 + "/" + id2 + "/" + id3 + "/" + id4);
        iCnt++;

        var deptX = $("#dptreq").select2({ allowClear: false });

        deptX.on("select2:selecting", function (evt) {

            var iRdata = evt.params.args.data.id;

            if (iCnt == 1) {
                id1 = iRdata;
            } else if (iCnt == 2) {
                id2 = iRdata;
            } else if (iCnt == 3) {
                id3 = iRdata;
            } else if (iCnt == 4) {
                id4 = iRdata;
            }

            DataFactory.iDataDEPT('dptreq', '/RankRoute/searchDept/' + id1.split(' - ')[0] + "/" + id2.split(' - ')[0] + "/" + id3.split(' - ')[0] + "/" + id4.split(' - ')[0]);
            iCnt++;

            if (id1 != "") {
                deptX.append(new Option(id1, id1, true, true));
            }
            if (id2 != "") {
                deptX.append(new Option(id2, id2, true, true));
            }
            if (id3 != "") {
                deptX.append(new Option(id3, id3, true, true));
            }
            if (id4 != "") {
                deptX.append(new Option(id4, id4, true, true));
            }

            $('#dptreq > .select2-selection__choice__remove').css('background', '#000');

        }).on('select2:unselecting', function (evt) {

            return false;

        });

    }

    $scope.pDept = function () {
    
        var formData = new FormData();

        if (id1 != '' && id2 != '' && id3 != '' && id4 != '') {
        
            formData.append("dept", id1.split(' - ')[0]);
            formData.append("sdept", id2.split(' - ')[0]);
            formData.append("iclass", id3.split(' - ')[0]);
            formData.append("siclass", id4.split(' - ')[0]);

            ConfirmFactory.UrlPostRedirect('/RankRoute/SubmitDept', formData, '/rk/requestlist');

        }
        else {

            ValidateFactory.notify("Please check department and class selection.", "warning");
        }
        
    }

    function sku() {
            
        var skuX = $("#skureq").select2({
            tags: true,
            tokenSeparators: [""],
            createTag: function (tag) {
                return {
                    id: tag.term,
                    text: tag.term,
                    isNew: true
                };
            }
        });

        //$('#skureq').append('<div><input type="text" style="width: 86%; padding: 9px;"name="lname"><input class="PrimaryBtn" type="submit" value="Add"></div>').trigger('change');

        //var data = {
        //    id: 1,
        //    text: 'Barn owl'
        //};

        //var newOption = new Option(data.text, data.id, false, false);
        //$('#skureq').append(newOption).trigger('change');

        skuX.on("select2:selecting", function (evt) {

            var brn = evt.params.args.data.id;

            console.log(brn);

            if (isNaN(brn) || outputS.length == 20 || brn.length > 9) {

                $.notify({
                    message: 'Only NUMERIC characters are allowed. Please check!',
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

                return false;
            }

            if (brn.trim() == "") {

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

                return false;
            }

            outputS.push(brn); // add to array                    

            $('#cntDD').empty().append(outputS.length);

            console.log(outputS);
            console.log(outputS.length);

        }).on('select2:unselecting', function (evt) {

            var brn = evt.params.args.data.id;
            
            var i = outputS.indexOf(brn);

            if (i != -1) {
                outputS.splice(i, 1);
            }

            $('#cntDD').empty().append(outputS.length);

            console.log(outputS);
        });



    }

    $scope.sSku = function () {

        console.log(outputS);
        
        if (outputS.length != 0) {

            var formDataDoc = new FormData;

            for (var i = 0; i < outputS.length; i++) {
                formDataDoc.append('arr', outputS[i]);
            }

            ConfirmFactory.UrlPostRedirect('/RankRoute/SubmitSKU', formDataDoc, '/rk/requestlist');

        }    
        else {

            ValidateFactory.notify("Please check SKU input(s).", "warning");
        }

    }
}

RankController.$inject = ['$scope', 'ConfirmFactory', 'DataFactory', 'ValidateFactory'];

