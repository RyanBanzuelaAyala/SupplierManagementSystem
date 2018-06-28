var PaymentController = function ($scope, $http, $location, PaymentFactory) {
    
    //console.log("payment page loaded");
                  
    $scope.searchForm = {
        invoicenum: '',
        invoicelist: '',
        loginFailure: false,        
        isLoading: false,
        requestFailure: false,
        isBtn: true,
        isBtnX: true,
        isDs1: true,
        fail: false
    }

    $scope.addtolist = function (item) {
        
        if ($scope.searchForm.invoicenum == "") return;

        if ($scope.searchForm.invoicelist == "") {

            $scope.searchForm.invoicelist = item;

        } else {

            $scope.searchForm.invoicelist = item + "," + $scope.searchForm.invoicelist;
        }

        $scope.searchForm.invoicenum = "";

        $scope.searchForm.isBtn = false;        
        
    }

    $scope.resetPayment = function () {

        window.location = '/py/paymentrequest';
    }
    
    $scope.requestPayment = function () {                     

        $scope.searchForm.isLoading = true;

        var result = PaymentFactory($scope.searchForm.invoicelist);
        
        result.then(function (result) {

            if (result.success) {

                window.location = '/py/paymentlist';

                //console.log("Success");

            } else {

                $scope.searchForm.requestFailure = true;

                $scope.searchForm.isLoading = false;

                //console.log(result);
            }

        });
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

        $http.post("/Payment/ExUpload", formData, {

            withCredentials: true,

            headers: { 'Content-Type': undefined },

            transformRequest: angular.identity

        }).success(function (response) {

            //console.log(response);

            if (response == "Empty") {

                $scope.searchForm.fail = true;

            }
            else {
                window.location = '/py/paymentlist';

                $scope.searchForm.isBtnX = false;

                $scope.searchForm.isLoading = false;

            }
            

        }).error(function () {

            $scope.searchForm.isBtnX = true;

            $scope.searchForm.isLoading = false;

            $scope.searchForm.fail = true;
            
        });
        
    }             

    $scope.newnew = function () {

        window.location = '/py/paymentrequest';

        //console.log($scope.searchForm.fail);
    }
}

PaymentController.$inject = ['$scope', '$http', '$location', 'PaymentFactory'];

