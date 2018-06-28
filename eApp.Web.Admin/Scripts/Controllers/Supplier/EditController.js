var EditController = function ($scope, $location, $http) {
    
    console.log("Reset Password loaded");
    
    $scope.ggform = {
        idd: document.getElementById('iddy').value,
        password: '',
        confirmPassword: '',
        resetFailure: false
    };
        
    $scope.updatePassword = function () {
    
        console.log($scope.ggform.idd + $scope.ggform.password);
               
        if ($scope.ggform.password != $scope.ggform.confirmPassword) return;

        $http.post(
            '/Supplier/ResetPasswordSupplier', {
                UserId: $scope.ggform.idd,
                Password: $scope.ggform.password
            }
        ).
        success(function (data) {
            if (data == "True") {
                
                window.location = '/';

            } else {
                
                $scope.ggform.resetFailure = true;

            }
        }).
        error(function () {
            
        });

       
    }

    $scope.ggInfo = {
        idd: document.getElementById('iddx').value,
        Email: document.getElementById('email').value,
        Mobile: document.getElementById('mobile').value,
        resetFailure: false
    };

    $scope.updateInfo = function () {

        console.log($scope.ggInfo.Email + $scope.ggInfo.Mobile);
        
        $http.post(
            '/Supplier/UpdateInfo', {
                UserId: $scope.ggInfo.idd,
                Email: $scope.ggInfo.Email,
                Mobile: $scope.ggInfo.Mobile
            }
        ).
        success(function (data) {
            if (data == "True") {
                
                window.location = '/';

            } else {
                
                $scope.ggInfo.resetFailure = true;

            }
        }).
        error(function () {
            
        });

    }
}

EditController.$inject = ['$scope', '$location', '$http'];