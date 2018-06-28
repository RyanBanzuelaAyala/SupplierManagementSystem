var StatementController = function ($scope, $http, $location) {
    
    console.log("statement page loaded");
                  
    $scope.searchForm = {        
        loginFailure: false,        
        isLoading: false,
        requestFailure: false,
        isBtn: true,
        isBtnX: true,
        isDs1: true,
        sccs: true,
        sccx: false

    }

    var formData = new FormData();

    $scope.LoadFileData = function (files) {

        for (var file in files) {

            formData.append("file", files[file]);

        }
    };

    $scope.acBtn = function () {

        $scope.searchForm.isBtnX = false;

        //console.log("dd");
    }

    $scope.uploadsubmit = function () {

        $scope.searchForm.isLoading = true;

        $http.post("/Payment/StatementUpload", formData, {

            withCredentials: true,

            headers: { 'Content-Type': undefined },

            transformRequest: angular.identity

        }).success(function (response) {

            //console.log(response);
                    
            $scope.searchForm.isBtnX = false;

            $scope.searchForm.isLoading = false;
            
            $scope.searchForm.sccs = false;

            $scope.searchForm.sccx = true;

            window.location = '/st/statementlist';

        })
        
    }             
}

StatementController.$inject = ['$scope', '$http', '$location'];

