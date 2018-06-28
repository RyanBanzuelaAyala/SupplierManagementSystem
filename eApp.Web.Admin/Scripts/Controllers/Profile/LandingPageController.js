var LandingPageController = function ($scope, $templateCache, $rootScope, ConfirmFactory) {

    $scope.Switch = function () {
        ConfirmFactory.UrlView('url:Account/SwitchRegion');
    };

    $scope.Process = function (r,u) {

        var formDataDoc = new FormData();
        
        formDataDoc.append("username", u);
        formDataDoc.append("password", document.getElementById(r).value);
        formDataDoc.append("Region", r);

        ConfirmFactory.UrlPostRedirect('/Account/SwitchRegionProcess', formDataDoc, '/');
    };

    $scope.Logout = function () {
        console.log("log loaded")
        window.location = '/Account/LogOff';
    };

    $scope.models = {
        helloAngular: 'DanubeCo : SMP - Supplier Management Portal'
    };

    $scope.iform = {
        isViewLoading: false
    };

    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        //$templateCache.removeAll();
        if (typeof (current) !== "undefined") {
            console.log(current);
            $templateCache.remove(current.templateUrl);
        }
        //if (current.$$route && current.$$route.resolve) {
        //    // Show a loading message until promises aren't resolved
        //    $scope.iform.isViewLoading = true;
        //}
        console.log("loading");
        $scope.iform.isViewLoading = true;
    });
    $rootScope.$on('$routeChangeSuccess', function () {
        console.log("unloading");
        $scope.iform.isViewLoading = false;
    });
    $rootScope.$on('$routeChangeError', function () {
        console.log("error loading");
        $scope.iform.isViewLoading = false;
    });
}

LandingPageController.$inject = ['$scope', '$templateCache', '$rootScope', 'ConfirmFactory'];