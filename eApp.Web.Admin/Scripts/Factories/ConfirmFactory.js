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

    var UrlContent = function (urlRedirect) {

        $.confirm({            
            title: false,
            boxWidth: '30%',
            closeAnimation: 'bottom',
            useBootstrap: true,
            animateFromElement: true,
            bgOpacity: 0.60,
            content: function () {

                var self = this;
                $('.jconfirm-content-pane').hide();

                self.buttons.ok.disable();
                self.buttons.ok.setText('REDIRECTING..');                

                window.location = urlRedirect;
            },            
            buttons: {
                ok: {
                    isHidden: false,
                    btnClass: 'btn-block'
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
                    $('.jconfirm').hide();
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
    
    return {

        UrlView: UrlView,
        UrlContent: UrlContent,
        UrlConfirm: UrlConfirm,
        UrlPostRedirect: UrlPostRedirect
    };

}

ConfirmFactory.$inject = ['$http', 'dnbservices'];