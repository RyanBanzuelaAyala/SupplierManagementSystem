var ConfirmFactory = function ($http, dnbservices) {
    
    var UrlView = function (content) {

        $.confirm({
            title: false,
            boxWidth: '45%',
            closeAnimation: 'bottom',
            useBootstrap: false,
            content: content,
            animateFromElement: true,
            bgOpacity: 0.60,
            closeIcon: true,
            closeIconClass: 'fa fa-close',
            buttons: {
                close: {
                    isHidden: true
                }
            }
        });

    };
    
    var UrlConfirm = function (content, urlRedirect) {

        $.confirm({
            title: false,
            boxWidth: '30%',
            closeAnimation: 'bottom',
            useBootstrap: true,
            animateFromElement: true,
            bgOpacity: 0.60,
            content: content,
            autoClose: 'No|8000',
            buttons: {
                Yes: {
                    show: false,
                    action: function () {
                        
                        var self = this;
                        $('.jconfirm-content-pane').hide();

                        this.buttons.No.hide();
                        this.buttons.Yes.disable();                        
                        this.buttons.Yes.setText('Processing..');
                        
                        self.buttons.Yes.setText('Redirecting..');

                        $('.jconfirm').hide();
                        window.location = urlRedirect;
                        

                    }
                },
                No: function () {
                    close
                },
            }
        });


    };
    
    var UrlPostRedirect = function (content, formData, urlRedirect) {
        
        $.confirm({
            title: false,
            boxWidth: '30%',
            closeAnimation: 'bottom',
            useBootstrap: true,
            offsetTop: 0,
            bgOpacity: 0.60,
            content: function () {

                var self = this;
                $('.jconfirm-content-pane').hide();

                self.buttons.ok.disable();
                self.buttons.ok.addClass('btn-default');
                self.buttons.ok.setText('Processing..');

                $http.post(content, formData, {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).success(function (response) {
                    self.buttons.ok.removeClass('btn-default');

                    if (response.trim() == "true" || response.trim() == "True") {
                        self.buttons.ok.addClass('btn-success');
                        self.buttons.ok.setText('Redirecting..');
                        window.location = urlRedirect;
                    }
                    else {
                        self.buttons.ok.addClass('btn-danger');
                        self.buttons.ok.setText('Error encountered..');
                        self.buttons.ok.enable();
                    }
                }).error(function () {
                    self.buttons.ok.addClass('btn-danger');
                    self.buttons.ok.setText('Error encountered..');
                    self.buttons.ok.enable();                    
                });
            },
            buttons: {
                ok: {
                    isHidden: false,
                    btnClass: 'btn-block'
                }
            }
        });
        
    };
    
    var FormInfo = function (link, pono) {

        $.confirm({
            title: ' ',
            draggable: true,
            boxWidth: '75%',
            useBootstrap: false,
            content: '<div class="box text-center"><img class="img-responsive" style="display: block; margin-left: auto; margin-right: auto;" src="http://services.danubeco.com/service.jpg" /></div>',
            buttons: {
                formSubmit: {
                    text: 'Agree',
                    btnClass: 'btn-success',
                    action: function () {

                        //alert("dd");
                        var formData = new FormData();

                        formData.append("pono", pono);

                        dnbservices.PostUpdate('/PORoute/SubmitPO', formData);
                        
                        window.open(
                            'http://services.danubeco.com/supplier/' + link,
                            '_blank' 
                        );
                    
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

    var FormInfoX = function () {

        $.confirm({
            title: ' ',
            draggable: true,
            boxWidth: '75%',
            useBootstrap: false,
            content: 'url:Account/SwitchRegion',
            buttons: {
                formSubmit: {
                    text: 'Submit',
                    btnClass: 'btn-default',
                    action: function () {

                        var result = ValidateFactory.validate();

                        if (result == "success") {

                            Info();

                        }
                        else {
                                                    
                            return false;
                           
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


    function Info() {

        var formDataDoc = new FormData();

        formDataDoc.append("username", u);
        formDataDoc.append("password", document.getElementById(r).value);
        formDataDoc.append("Region", r);

        ConfirmFactory.UrlPostRedirect('/Account/SwitchRegionProcess', formDataDoc, '/');

    }


    return {

        UrlView: UrlView,
        UrlConfirm: UrlConfirm,        
        UrlPostRedirect: UrlPostRedirect,
        FormInfo: FormInfo
    };

}

ConfirmFactory.$inject = ['$http', 'dnbservices'];