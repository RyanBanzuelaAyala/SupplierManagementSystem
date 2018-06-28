var rComplainController = function ($scope, $location, $http, $routeParams, dnbservices) {
    
    console.log("Complain Controller loaded");
    
    $scope.in = {
        rsp: '',
        isid: document.getElementById('issueid').value,
        isloading: false,
        isEdit: true,        
        isDisabled: false
    };    

    $scope.submitInfo = function () {

        console.log("mm");

        $http.post(
            '/Complain/ComplainInfoReply', {
                respnse: $scope.in.rsp,
                issueid: $scope.in.isid
            }
        ).
        success(function (data) {
            if (data == "True") {

                console.log("success");

                loadExp();

                $scope.in.rsp = "";

            } else {

                console.log("failed");
            }
        }).
        error(function () {
                        
            console.log(error);
        });
    }


}

rComplainController.$inject = ['$scope', '$location', '$http', '$routeParams', 'dnbservices'];