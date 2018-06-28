var ComplainController = function ($scope, $http, $location) {
    
    console.log("complain page loaded");
                                
    $scope.searchForm = {
        itype: document.getElementById('itype').value,
        iname: '',
        icomment: '',
        isLoading: false,        
    }

    $scope.resetIssue = function () {

        window.location = "/cp/issue";

    }
    
    $scope.submitIssue = function () {                     

        $scope.searchForm.isLoading = true;

        $http.post(
            '/Complain/SubmitCompDL', {
                issuetype: $scope.searchForm.itype,
                issuename: $scope.searchForm.iname,
                comment: $scope.searchForm.icomment
            }
        ).
        success(function (data) {
            if (data == "True") {

                window.location = '/cp/list';

            } else {

                $scope.searchForm.requestFailure = true;
                $scope.searchForm.isLoading = false;
            }
        }).
        error(function () {
        
        });
    }
                  
}

ComplainController.$inject = ['$scope', '$http', '$location'];

