var ComplainController = function ($scope, $location, $http, $routeParams, dnbservices) {
    
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

    $scope.submitSolve = function () {

        console.log("mm");

        $http.post(
            '/Complain/ComplainSolved', {                
                issueid: $scope.in.isid
            }
        ).
        success(function (data) {
            if (data == "True") {

                console.log("success");

                window.location = '/cp/complainlistsolved';

            } else {

                console.log("failed");
            }
        }).
        error(function () {

            console.log(error);
        });
    }

    // -------- Load List of Replies ----------

    loadExp();

    $scope.datalst = {};

    $scope.loadExp = function () {

        loadExp();

    }    

    function loadExp() {

        var getObjList = dnbservices.getComplain($scope.in.isid);

        getObjList.then(function (obj) {

            $scope.datalst = obj.data;

            console.log($scope.datalst);
            
        }, function () {

            alert('Data not found');

        });

    }

}

ComplainController.$inject = ['$scope', '$location', '$http', '$routeParams', 'dnbservices'];