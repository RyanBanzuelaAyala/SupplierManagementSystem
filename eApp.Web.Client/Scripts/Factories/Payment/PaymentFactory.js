var PaymentFactory = function ($http, $q) {
    return function (invoices) {

        //console.log("payment factory loaded");
                        
        var deferredObject = $q.defer();

        $http.post(
            '/Payment/SubmitPayment', {                                           
                invoicelist: invoices
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

PaymentFactory.$inject = ['$http', '$q'];