var RegisterController = function ($scope, $location, $http) {
    
    console.log("Registration loaded");

    $scope.registerForm = {
        username: '',
        password: '',
        confirmPassword: '',
        region: '',
        name: '',
        email: '',
        mobile: '',
        issms: '',
        registrationFailure: false,
        isloading: false
    };
        
    $scope.register = function () {
    
        $scope.registerForm.isloading = true;
        
        $http.post(
            '/Account/Register', {
                Name: $scope.registerForm.name,
                Username: $scope.registerForm.username + $scope.registerForm.region,
                Password: $scope.registerForm.password,
                Region: $scope.registerForm.region,
                ConfirmPassword: $scope.registerForm.confirmPassword,
                Email: $scope.registerForm.email,
                Mobile: $scope.registerForm.mobile,
                IsSms: $scope.registerForm.issms,
                CurrentPassword: $scope.registerForm.password,
                Role: "Supplier"
            }
        ).
        success(function (data) {
            if (data == "True") {
                
                window.location = '/';

            } else {
                
                $scope.registerForm.registrationFailure = true;
                $scope.registerForm.isloading = false;

            }
        }).
        error(function () {
            

        });
        
    }
}

RegisterController.$inject = ['$scope', '$location', '$http'];