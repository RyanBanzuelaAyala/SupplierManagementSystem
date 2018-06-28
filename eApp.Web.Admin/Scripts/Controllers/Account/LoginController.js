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

            ConfirmFactory.UrlPostRedirect('/Account/Login', formDataDoc, '/');

        }

    }
}

LoginController.$inject = ['$scope', 'ConfirmFactory', 'ValidateFactory'];