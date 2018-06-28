var RtvFactory = function ($http, $q) {
    return function (ipono) {

        //console.log("rtv factory loaded");
                        
        var deferredObject = $q.defer();

        $http.post(
            '/Rtv/SubmitPO', {
                pono: ipono
            }
        ).
        success(function (data) {
            if (data == "True") {

                deferredObject.resolve({ success: true });

            } else {

                deferredObject.resolve({ success: false });
            }
        }).
        error(function () {

            deferredObject.resolve({ success: false });
            
        });

        return deferredObject.promise;
    }
}

RtvFactory.$inject = ['$http', '$q'];