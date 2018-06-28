var UserListController = function ($scope, $http, $location) {
    
    console.log("userlist page loaded");

    $scope.datalst = {};

    $http.get('/Account/UserListArray')
    .then(function (response) {
        $scope.datalst.users = response.data;
    });
       
    
    $scope.editRow = function (username) {
        console.log(username);

        $scope.iform.isViewLoading = true;

        $location.path('/useredit?username='+username);
    };
}

UserListController.$inject = ['$scope', '$http', '$location'];