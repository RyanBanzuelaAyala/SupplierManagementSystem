var NewAccountController = function ($scope, ConfirmFactory, ValidateFactory) {
    
   
    $scope.New = function () {
    
        var result = ValidateFactory.validate();

        if (result == "success") {

            var formDataDoc = new FormData();

            var sid = document.getElementById('sid').value;

            formDataDoc.append("sid", sid);
            formDataDoc.append("name", document.getElementById('name').value);
          
            ConfirmFactory.UrlPostRedirect('/Supplier/RegisterNewAccount', formDataDoc, '/sup/userlist/' + sid);

        }

    }
}

NewAccountController.$inject = ['$scope', 'ConfirmFactory', 'ValidateFactory'];