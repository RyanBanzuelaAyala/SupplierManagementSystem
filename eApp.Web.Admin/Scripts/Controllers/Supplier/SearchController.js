var SearchController = function ($scope, ConfirmFactory, ValidateFactory) {
        
    $scope.Search = function () {

        var result = ValidateFactory.validate();

        if (result == "success") {
        
            ConfirmFactory.UrlContent('/sup/userlist/' + document.getElementById('sid').value);

        }
        
    }

}

SearchController.$inject = ['$scope', 'ConfirmFactory', 'ValidateFactory'];