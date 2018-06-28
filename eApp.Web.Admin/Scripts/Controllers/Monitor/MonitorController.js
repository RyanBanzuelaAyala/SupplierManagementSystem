var MonitorController = function ($scope, $location, $http, $routeParams) {
    
    console.log("Search Password loaded");

    $scope.ggSearch = {

        ixDate: ''

    }

    $scope.search = function () {

        console.log($scope.ggSearch.ixDate);

        window.location = '/monitor/activities/' + $scope.ggSearch.ixDate;

    }
}

MonitorController.$inject = ['$scope', '$location', '$http', '$routeParams'];