var SKUController = function ($scope, $http, $location) {
    
    //console.log("SKU page loaded");
                     
    $scope.skuForm = {
        currentdate: normalizeDate(addDays(new Date(), 1)),
        howmanydays: '30',
        archivedate: normalizeDate(addDays(new Date(), 30)),        
        invoicenum: '',
        invoicelist: '',        
        isLoading: false,
        requestFailure: false,
        isBtn: true,
        isDs1: true,
        isBtn: true,
        cntXX: 0,
        sku1: '0',
        sku2: '0',
        sku3: '0',
        sku4: '0',
        sku5: '0',
    }

    $scope.resetSku = function () {

        $scope.skuForm.invoicenum = "";
        $scope.skuForm.invoicelist = "";
        $scope.skuForm.cntXX = 0;
        $scope.skuForm.isBtn = true;
        
    }

    $scope.addtolist = function (item) {
        
        if ($scope.skuForm.invoicenum == "" || $scope.skuForm.invoicenum == " " || $scope.skuForm.invoicenum == "0") return;

        if ($scope.skuForm.cntXX == 20) return;

        if (isNaN($scope.skuForm.invoicenum)) return;


        if ($scope.skuForm.invoicelist == "") {

            $scope.skuForm.invoicelist = item;

        } else {

            $scope.skuForm.invoicelist = item + "/" + $scope.skuForm.invoicelist;
        }

        $scope.skuForm.cntXX = $scope.skuForm.cntXX + 1;

        $scope.skuForm.invoicenum = "";
                
        $scope.skuForm.isBtn = false;

        //console.log($scope.skuForm.cntXX);

    }

    $scope.requestSKU = function () {

        if ($scope.skuForm.sku1 == " " && $scope.skuForm.sku2 == " " && $scope.skuForm.sku3 == " " && $scope.skuForm.sku4 == " " && $scope.skuForm.sku5 == " ") return;

        $scope.skuForm.sku1 = $scope.skuForm.invoicelist;
        
        //console.log($scope.skuForm.sku1);
                
        $scope.skuForm.isLoading = true;
        
        $http.post(
            '/RankRoute/SubmitSKU', {
                ffrom: $scope.skuForm.currentdate,
                tto: $scope.skuForm.archivedate,
                sku1: $scope.skuForm.sku1,
                sku2: $scope.skuForm.sku2,
                sku3: $scope.skuForm.sku3,
                sku4: $scope.skuForm.sku4,
                sku5: $scope.skuForm.sku5
            }
        ).
        success(function (data) {

            //console.log(data);

            if (data == "True") {

                window.location = '/rk/requestlist';

            } else {

                $scope.skuForm.requestFailure = true;
                $scope.skuForm.isLoading = false;
            }
        }).
        error(function () {
                        
        });

    }

    $scope.filterValue = function ($event) {

        if (isNaN(String.fromCharCode($event.keyCode))) {
            $event.preventDefault();
        }
    };
      
    var _curr = new Date();

    function addDays(date, days) {

        var result = new Date(date);

        result.setDate(result.getDate() - days);

        return result;

    }

    function normalizeDate(dateString) {

        // If it's not at least 6 characters long (8/8/88), give up.
        if (dateString.length && dateString.length < 6) {
            return '';
        }

        var date = new Date(dateString),
            month, day;

        // If input format was in UTC time, adjust it to local.
        if (date.getHours() || date.getMinutes()) {
            date.setMinutes(date.getTimezoneOffset());
        }

        month = date.getMonth() + 1;
        day = date.getDate();

        // Return empty string for invalid dates
        if (!day) {
            return '';
        }

        // Return the normalized string.
        return date.getFullYear() + '-' + (month > 9 ? '' : '0') + month + '-' + (day > 9 ? '' : '0') + day;
    }


}

SKUController.$inject = ['$scope', '$http', '$location'];

