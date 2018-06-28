var UserFactory = function ($http, ValidateFactory, ConfirmFactory) {

    var FormInfo = function (type, id) {

        $.confirm({
            title: false,
            boxWidth: '75%',
            closeAnimation: 'bottom',
            useBootstrap: false,
            animateFromElement: true,
            content: 'url:User/OtherInfo?empid=' + id,
            buttons: {
                formSubmit: {
                    text: 'Submit',
                    btnClass: 'btn-default',
                    action: function () {
                        var result = ValidateFactory.validate();
                        
                        if (result == "success") {
                            if (type == "useradd") {
                                UserAdd();
                            }                            
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

    function UserAdd() {

        var formDataDoc = new FormData();

        formDataDoc.append("empid", document.getElementById('emmpp').value);
        formDataDoc.append("licensetype", document.getElementById('lcns').value);
        formDataDoc.append("issuedate", document.getElementById('issdt').value);
        formDataDoc.append("expirydate", document.getElementById('expdt').value);
        formDataDoc.append("restriction", document.getElementById('restr').value);
        formDataDoc.append("email", document.getElementById('emdd').value);
        formDataDoc.append("extension", document.getElementById('intt').value);
        formDataDoc.append("mobilepersonal", document.getElementById('pnuu').value);
        formDataDoc.append("mobilecompany", document.getElementById('cnuu').value);
        formDataDoc.append("status", document.getElementById('sts').value);
        formDataDoc.append("emptype", document.getElementById('emtt').value);
        formDataDoc.append("file", document.getElementById('licph').files[0]);
        formDataDoc.append("file", document.getElementById('iqaph').files[0]);
        
        ConfirmFactory.UrlPostRedirect('/User/PostUserInfo', formDataDoc, '/user/list/');
        
    }

    
    return {

        FormInfo: FormInfo
    };

}

UserFactory.$inject = ['$http', 'ValidateFactory', 'ConfirmFactory'];