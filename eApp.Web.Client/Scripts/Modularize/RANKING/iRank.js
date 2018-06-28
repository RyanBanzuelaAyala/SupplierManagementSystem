
var iRank = function (iqtip, idata) {

    var init = function () {

        //idata.iDataBR('bBrn', '/RankRoute/searchCSV/0');

        //brand();

        //dept();

        //sku();
        
    }

    var brand = function () {

        var output = new Array();

        $("#bBrn").select2().on("select2:selecting", function (evt) {

            var brn = evt.params.args.data.id;

            if (output.length != 0) { // not empty

                if (brn == '0') {

                    return false;

                    //var element = evt.params.args.data.element;
                    //var $element = $(element);
                    ////$element.detach();
                    ////$element.hide();

                }
                else {
                    output.push(brn); // add to array                    
                }

            } else {
                if (brn == '0') {
                    output.push(brn); // add to array                    
                    $('.select2').attr('disabled', 'disabled');
                }
                else {
                    output.push(brn); // add to array                    
                }
            }

            //output.push(brn);
            console.log(output);

        }).on('select2:unselecting', function (evt) {

            var brn = evt.params.args.data.id;
            output.pop(brn);

            console.log(output);
        });
        
    }

    var dept = function () {
    
        var iCnt = 0;
        var id1 = '';
        var id2 = '';
        var id3 = '';
        var id4 = '';

        idata.iDataDEPT('dptreq', '/RankRoute/searchDept/' + id1 + "/" + id2 + "/" + id3 + "/" + id4);
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
            
            idata.iDataDEPT('dptreq', '/RankRoute/searchDept/' + id1.split(' - ')[0] + "/" + id2.split(' - ')[0] + "/" + id3.split(' - ')[0] + "/" + id4.split(' - ')[0]);
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

    var sku = function () {
    
        var output = new Array();

        var skuX = $("#skureq").select2({
            tags: true,
            tokenSeparators: [",", " "],
            createTag: function (tag) {
                return {
                    id: tag.term,
                    text: tag.term,
                    isNew: true
                };
            }
        });

        skuX.on("select2:selecting", function (evt) {

            var brn = evt.params.args.data.id;
                        
            output.push(brn); // add to array                    
            
            console.log(output);

        }).on('select2:unselecting', function (evt) {

            var brn = evt.params.args.data.id;
            output.pop(brn);

            console.log(output);
        });

    }

    return {
        init: init
    }

}(iQtip, iData);
