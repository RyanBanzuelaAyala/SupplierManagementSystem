var RtvController = function ($scope, $http, $location, RtvFactory) {
    
    //console.log("RTV page loaded");
                                               
    $scope.Download = function (item) {

        //console.log(item)

        var result = RtvFactory(item);

        result.then(function (result) {

            if (result.success) {

                window.location = '/r/available';

                //console.log("Success");

            } else {

                $scope.skuForm.requestFailure = true;

                $scope.skuForm.isLoading = false;
            }

        });
    }
          
}

RtvController.$inject = ['$scope', '$http', '$location', 'RtvFactory'];

