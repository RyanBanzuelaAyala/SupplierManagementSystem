var RankController = function ($scope, $location, $http) {

    console.log("RankController Password loaded");

    $scope.reprocess = function (item) {

        $http.post(
            '/Rank/UpdateRanking', {
                reqid: item
            }
        ).
        success(function (data) {
            if (data == "True") {

                window.location = '/rk/rank/processing';

            } else {

                console.log(data);
            }
        }).
        error(function () {

        });

    }
}

RankController.$inject = ['$scope', '$location', '$http'];