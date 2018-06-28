var InfoFactory = function ($http, ValidateFactory, ConfirmFactory) {

    var FormInfo = function (type, vin) {

        $.confirm({
            title: ' ',
            draggable: true,
            boxWidth: '75%',
            useBootstrap: false,
            content: 'url:Vehicles/_Template?link=' + type + '&vin=' + vin,
            buttons: {
                formSubmit: {
                    text: 'Submit',
                    btnClass: 'btn-default',
                    action: function () {

                        var result = ValidateFactory.validate();
                        
                        if (result == "success") {
                            
                            ProcessEdit(type);

                        }
                        else {

                            var rResult = ValidateFactory.hasValue();

                            if (rResult == "success") {

                                ProcessEdit(type);

                            }
                            else {

                                return false;
                            }
                        }
                    }
                },
                cancel: function () {

                },
            },
            onContentReady: function () {
                // bind to events
                var jc = this;
                this.$content.find('form').on('submit', function (e) {
                    // if the user submits the form by pressing enter in the field.
                    e.preventDefault();
                    jc.$$formSubmit.trigger('click'); // reference the button and click it
                });
            }
        });

    }

    function ProcessEdit(type) {

        if (type == "info") {
            Info();
        }
        else if (type == "pic") {
            Pic();
        }
        else if (type == "specs") {
            Spec();
        }
        else if (type == "purc") {
            Purc();
        }
        else if (type == "reg") {
            Reg();
        }
        else if (type == "insu") {
            Insu();
        }
        else if (type == "ins") {
            Ins();
        }
        else if (type == "capa") {
            Capa();
        }
        else if (type == "cont") {
            Cont();
        }
        else if (type == "conti") {
            Conti();
        }
        else if (type == "cool") {
            Cool();
        }
        else if (type == "cooli") {
            Cooli();
        }

    }

    function Info() {

        var formDataDoc = new FormData();

        var ar = $('#arn1').val() + " " + $('#arn2').val() + " " + $('#arn3').val();
        var en = $('#enn1').val() + " " + $('#enn2').val() + " " + $('#enn3').val();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", vn);
        formDataDoc.append("pn", document.getElementById('pn').value);
        formDataDoc.append("ar", ar);
        formDataDoc.append("en", en);
        formDataDoc.append("bptype", document.getElementById('bptype').value);
        formDataDoc.append("mtype", document.getElementById('mtype').value);
        formDataDoc.append("vmanu", document.getElementById('vmanu').value);
        formDataDoc.append("vmodel", document.getElementById('vmodel').value);
        formDataDoc.append("vcolor", document.getElementById('vcolor').value);
        formDataDoc.append("vgroup", document.getElementById('vgroup').value);
        formDataDoc.append("vcondition", document.getElementById('vcondition').value);
        formDataDoc.append("vregion", document.getElementById('vregion').value);
        formDataDoc.append("vstatus", document.getElementById('vstatus').value);
        formDataDoc.append("vtype", document.getElementById('vtype').value);
        formDataDoc.append("note", document.getElementById('note').value);

        ConfirmFactory.UrlPostRedirect('/Vehicles/PostVehicleUpdate', formDataDoc, '/vehicle/info/' + vn);
        
    }

    function Pic() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", vn);

        var totalFiles = document.getElementById("imgInpX").files.length;

        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("imgInpX").files[i];

            formDataDoc.append("file", file);
        }
        
        ConfirmFactory.UrlPostRedirect('/Image/UploadPic', formDataDoc, '/vehicle/info/' + vn);
        
    }

    function Spec() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", document.getElementById('vvn').value);
        formDataDoc.append("fueltype", document.getElementById('fueltype').value);
        formDataDoc.append("enginesize", document.getElementById('enginesize').value);
        formDataDoc.append("transmission", document.getElementById('transmission').value);
        formDataDoc.append("cyclinder", document.getElementById('cyclinder').value);
        formDataDoc.append("windows", document.getElementById('windows').value);
        formDataDoc.append("oilpansize", document.getElementById('oilpansize').value);
        formDataDoc.append("polviscosity", document.getElementById('polviscosity').value);
        formDataDoc.append("tiresize", document.getElementById('tiresize').value);
        formDataDoc.append("batterysize", document.getElementById('batterysize').value);        

        formDataDoc.append("serialnumber", document.getElementById('serialnumber').value);
        formDataDoc.append("chasisnum", document.getElementById('chasisnum').value);
        formDataDoc.append("passenger", document.getElementById('passenger').value);
        formDataDoc.append("weight", document.getElementById('weight').value);

        formDataDoc.append("isNew", "false");

        ConfirmFactory.UrlPostRedirect('/Specs/PostVehicleSpecs', formDataDoc, '/vehicle/info/' + vn);

    }

    function Purc() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", document.getElementById('vvn').value);
        formDataDoc.append('warranty', document.getElementById('warranty').value);
        formDataDoc.append('supplierid', document.getElementById('supplierid').value);
        formDataDoc.append("approvalid", document.getElementById('approvalid').value);
        formDataDoc.append('vcondition', document.getElementById('vcondition').value);
        formDataDoc.append('approvaldate', document.getElementById('approvaldate').value);
        formDataDoc.append("approvedby", document.getElementById('approvedby').value);
        formDataDoc.append('approvalnote', document.getElementById('approvalnote').value);
        formDataDoc.append('price', document.getElementById('price').value);
        formDataDoc.append("file", document.getElementById('purcXX').files[0]);

        ConfirmFactory.UrlPostRedirect('/Purchase/PostVehiclePurchase', formDataDoc, '/vehicle/info/' + vn);

    }
    
    function Reg() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", document.getElementById('vvn').value);
        formDataDoc.append('ownership', document.getElementById('ownerid').value);
        formDataDoc.append('vrplace', document.getElementById('vrplace').value);
        formDataDoc.append('vrexpiry', document.getElementById('vrexpiry').value);
        formDataDoc.append('vrstatus', document.getElementById('vrstatus').value);
        formDataDoc.append('ownername', document.getElementById('ownername').value);
        formDataDoc.append('costenter', document.getElementById('costenter').value);
        formDataDoc.append("file", document.getElementById('reggXX').files[0]);

        ConfirmFactory.UrlPostRedirect('/Detail/PostVehicleDetail', formDataDoc, '/vehicle/info/' + vn);

    }
    
    function Insu() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", document.getElementById('vvn').value);
        formDataDoc.append('insuranceplace', document.getElementById('insuranceplace').value);
        formDataDoc.append('insuranceexpiry', document.getElementById('insuranceexpiry').value);
        formDataDoc.append("insurancestatus", document.getElementById('insurancestatus').value);        
        formDataDoc.append("file", document.getElementById('insuXX').files[0]);

        ConfirmFactory.UrlPostRedirect('/Insurance/PostVehicleInsurance', formDataDoc, '/vehicle/info/' + vn);

    }
    
    function Ins() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", document.getElementById('vvn').value);
        formDataDoc.append('inspectionid', document.getElementById('inspectionid').value);
        formDataDoc.append('inspectionexpiry', document.getElementById('inspectionexpiry').value);
        formDataDoc.append("inspectionestatus", document.getElementById('inspectionestatus').value);
        formDataDoc.append("file", document.getElementById('insXX').files[0]);


        ConfirmFactory.UrlPostRedirect('/Inspection/PostVehicleInspect', formDataDoc, '/vehicle/info/' + vn);

    }
    
    function Capa() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append("vin", document.getElementById('vvn').value);
        formDataDoc.append('totalcapacity', document.getElementById('totalcapacity').value);
        formDataDoc.append('netcapacity', document.getElementById('netcapacity').value);
      
        ConfirmFactory.UrlPostRedirect('/Capacity/PostVehicleCapacity', formDataDoc, '/vehicle/info/' + vn);

    }

    function Cont() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append('vin', document.getElementById('vvn').value);
        formDataDoc.append('containertype', document.getElementById('containertype').value);
        formDataDoc.append('containersupp', document.getElementById('containersupp').value);
        formDataDoc.append('containeragency', document.getElementById('containeragency').value);
        formDataDoc.append('containerstatus', document.getElementById('containerstatus').value);
        formDataDoc.append('approvaldate', document.getElementById('approvaldate').value);
        formDataDoc.append('approvalby', document.getElementById('approvalby').value);
        formDataDoc.append('approvalnote', document.getElementById('approvalnote').value);
        formDataDoc.append('price', document.getElementById('price').value);
        formDataDoc.append("file", document.getElementById('contXX').files[0]);

        ConfirmFactory.UrlPostRedirect('/ContainerPurchase/PostVehicleContainerPurc', formDataDoc, '/vehicle/info/' + vn);

    }
    
    function Conti() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;
                
        formDataDoc.append('vin', document.getElementById('vvn').value);
        formDataDoc.append('capacity', document.getElementById('capacity').value);
        formDataDoc.append('clength', document.getElementById('clength').value);
        formDataDoc.append('cwidth', document.getElementById('cwidth').value);
        formDataDoc.append('cheight', document.getElementById('cheight').value);

        ConfirmFactory.UrlPostRedirect('/ContainerInfo/PostVehicleContainerInfo', formDataDoc, '/vehicle/info/' + vn);

    }
    
    function Cool() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append('vin', document.getElementById('vvn').value);
        formDataDoc.append('cusupp', document.getElementById('cusupp').value);
        formDataDoc.append('cuagency', document.getElementById('cuagency').value);
        formDataDoc.append('custatus', document.getElementById('custatus').value);
        formDataDoc.append('approvaldate', document.getElementById('approvaldate').value);
        formDataDoc.append('approvalby', document.getElementById('approvalby').value);
        formDataDoc.append('approvalnote', document.getElementById('approvalnote').value);
        formDataDoc.append('price', document.getElementById('price').value);
        formDataDoc.append("file", document.getElementById('coollXX').files[0]);

        ConfirmFactory.UrlPostRedirect('/CoolingPurc/PostVehicleCoolPurc', formDataDoc, '/vehicle/info/' + vn);

    }

    function Cooli() {

        var formDataDoc = new FormData();

        var vn = document.getElementById('vvn').value;

        formDataDoc.append('vin', document.getElementById('vvn').value);
        formDataDoc.append('cutype', document.getElementById('cutype').value);
        formDataDoc.append('maxtemp', document.getElementById('maxtemp').value);
        formDataDoc.append('cunote', document.getElementById('cunote').value);


        ConfirmFactory.UrlPostRedirect('/CoolingInfo/PostVehicleCoolingInfo', formDataDoc, '/vehicle/info/' + vn);

    }
    
    return {

        FormInfo: FormInfo
    };

}

InfoFactory.$inject = ['$http', 'ValidateFactory', 'ConfirmFactory'];