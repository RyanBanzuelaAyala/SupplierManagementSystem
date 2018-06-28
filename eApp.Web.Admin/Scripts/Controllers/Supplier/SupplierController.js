var SupplierController = function ($scope, ConfirmFactory, ValidateFactory) {
       
    $scope.Process = function () {
    
        var result = ValidateFactory.validate();

        if (result == "success") {

            var formDataDoc = new FormData();
            
            formDataDoc.append("idd", document.getElementById('idd').value);
            formDataDoc.append("name", document.getElementById('name').value);
            formDataDoc.append("region", document.getElementById('region').value);
            formDataDoc.append("mobile", document.getElementById('mobile').value);            
            formDataDoc.append("password", document.getElementById('password').value);
          
            ConfirmFactory.UrlPostRedirect('/Supplier/AddSupplier', formDataDoc, '/supplier/list/');

        }

    }
}

SupplierController.$inject = ['$scope', 'ConfirmFactory', 'ValidateFactory'];