var BrandController = function ($scope, ConfirmFactory) {
    
    $scope.requestRanking = function () {                     

        $scope.searchForm.isLoading = true;

        //$http.post(
        //    '/RankRoute/SubmitBrand', {
        //        ffrom: $scope.searchForm.currentdate,
        //        tto: $scope.searchForm.archivedate,
        //        brandcode: $scope.searchForm.brandcode
        //    }
        //).
        //success(function (data) {
        //    if (data == "True") {

        //        window.location = '/rk/requestlist';

        //    } else {

        //        $scope.searchForm.requestFailure = true;
        //        $scope.searchForm.isLoading = false;
        //    }
        //}).
        //error(function () {

        //});

    }


}

BrandController.$inject = ['$scope', 'ConfirmFactory'];

