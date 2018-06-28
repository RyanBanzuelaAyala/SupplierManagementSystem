var TransactionController = function ($scope, $location) {
    
    console.log("Transaction Password loaded");
    
    $scope.ggSearch = {

        username: '',
        statt: ''

    }
    
    $scope.searchpo = function () {
        
        window.location = '/po/polist/' + $scope.ggSearch.username + '/' + $scope.ggSearch.statt;

    }

    $scope.searchrank = function () {

        window.location = '/rk/ranklist/' + $scope.ggSearch.username + '/' + $scope.ggSearch.statt;

    }

}

TransactionController.$inject = ['$scope', '$location'];