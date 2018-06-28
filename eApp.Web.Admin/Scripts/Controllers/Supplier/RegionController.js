var RegionController = function ($scope, $location, $http) {
    
    console.log("Region controller loaded");
       
    $scope.ggRegion = {
        region: '',
        username: document.getElementById('iddx').value,
        name: document.getElementById('iddy').value,
        email: '',
        mobile: '',
        password: '12345678',
        confirmPassword: '12345678',
        resetFailure: false,
        isloading: false
    };

    $scope.addnewregion = function () {

        $scope.ggRegion.isloading = true;

        $http.post(
            '/Supplier/AddSupplierRegion', {
                Name: $scope.ggRegion.name,
                Username: $scope.ggRegion.username + $scope.ggRegion.region,
                Password: $scope.ggRegion.password,
                Region: $scope.ggRegion.region,
                ConfirmPassword: $scope.ggRegion.confirmPassword,
                Email: $scope.ggRegion.email,
                Mobile: $scope.ggRegion.mobile,
                CurrentPassword: $scope.ggRegion.password
            }
        ).
        success(function (data) {
            if (data == "True") {
                
                window.location = '/sup/userlist/' + $scope.ggRegion.username;

            } else {
                
                $scope.ggRegion.resetFailure = true;
                $scope.ggRegion.isloading = false;

            }
        }).
        error(function () {
            
        });

    }

    $scope.hasChanged = function (item) {

        if (item == "") return;

        $scope.ggRegion.email = item + $scope.ggRegion.username + "@danubeco.com";
        
    }

}

RegionController.$inject = ['$scope', '$location', '$http'];