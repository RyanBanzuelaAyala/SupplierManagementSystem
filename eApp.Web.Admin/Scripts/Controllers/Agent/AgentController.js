var AgentController = function ($scope, $routeParams, $location, $http) {

    console.log("login controller loaded");

    $scope.in = {
        empid: '',
        pss: '',
        rpss: '',
        rle: '',
        rge: '',
        isLoading: false
    };

    $scope.createAgent = function () {

        $scope.in.isLoading = true;

        if ($scope.in.pss != $scope.in.rpss) return;

        $http.post(
            '/Agent/AgentSubmit', {
                name: $scope.in.empid,
                password:  $scope.in.pss,
                region: $scope.in.rge,
                role: $scope.in.rle
            }
        ).
        success(function (data) {
            if (data == "True") {

                window.location = '/ag/new';

            } else {

                $scope.in.isLoading = false;
            }
        }).
        error(function () {

            
        });

    }

    $scope.resetAgent = function () { 
    
        window.location = '/agent/new';

    }
}

AgentController.$inject = ['$scope', '$routeParams', '$location', '$http'];