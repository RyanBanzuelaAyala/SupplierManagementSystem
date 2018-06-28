var LoginController = function ($scope, ConfirmFactory, ValidateFactory) {

    $scope.Process = function (event) {

        if (event.keyCode === 13) {
            _processAuth();
        }

    }

    $scope.Login = function () {

        _processAuth();

    }

    function _processAuth() {

        var result = ValidateFactory.validate();

        console.log(document.getElementById('regg').value);

        if (result == "success") {

            var formDataDoc = new FormData();

            var userid = document.getElementById('username').value + document.getElementById('regg').value;

            //console.log(userid);

            formDataDoc.append("Username", userid);
            formDataDoc.append("Password", document.getElementById('password').value);
            formDataDoc.append("RememberMe", document.getElementById('password').value);
            formDataDoc.append("Region", document.getElementById('regg').value);

            ConfirmFactory.UrlPostRedirect('/Account/Login', formDataDoc, '/Account/Register');

        }

    }

    $scope.Register = function () {

        var result = ValidateFactory.validate();

        if (result == "success") {

            var formDataDoc = new FormData();

            formDataDoc.append("mobile", document.getElementById('mobile').value);
         
            ConfirmFactory.UrlPostRedirect('/Account/Register', formDataDoc, '/');

        }

    }
    
    $scope.Validate = function () {

        var result = ValidateFactory.validate();

        if (result == "success") {

            var formDataDoc = new FormData();

            formDataDoc.append("code", document.getElementById('code').value);

            ConfirmFactory.UrlPostRedirect('/Account/CodeValidate', formDataDoc, '/');

        }

    }
    
    $scope.Resend = function () {

        var formDataDoc = new FormData();

        formDataDoc.append("code", "");

        ConfirmFactory.UrlPostRedirect('/Account/ResendCode', formDataDoc, '/Account/CodeValidate');
       
    }


}

LoginController.$inject = ['$scope', 'ConfirmFactory', 'ValidateFactory'];