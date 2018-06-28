var DeptController = function ($scope, $http, $location) {
    
    //console.log("DEPT page loaded");
                     
    $scope.deptForm = {
        currentdate: normalizeDate(addDays(new Date(), 1)),
        howmanydays: '30',
        archivedate: normalizeDate(addDays(new Date(), 30)),        
        dept: '',
        sdept: '',
        iclass: '',                   
        siclass: '',
        ldept: '',
        lsdept: '',
        liclass: '',
        lsiclass: '',        
        isLoading: false,        
        requestFailure: false,        
        xx1: true,
        xx2: true,
        xx3: true,
        xx4: true,
        xx5: true,
        t1: false,
        t2: false,
        t3: false,
        t4: false,
    }
    
    $scope.hasDept = function () {
                
        //console.log("x");

        $scope.deptForm.isLoading = true;        

        $scope.deptForm.xx1 = checkDept("dDept", "Dept");
        
    }
          
    $scope.searchDEPT = function (item) {                    
        
        var array = item.split(' - ');

        if (array[0] == "") {

            $scope.deptForm.xx2 = true;

            return;
        }
        else {

            $scope.deptForm.dept = array[0];

            $scope.deptForm.isLoading = true;        

            $scope.deptForm.xx2 = checkDept("sDept", "Sub-Dept");

            $scope.deptForm.t1 = true;

        }

        //console.log(array[0]);
                                             
    }

    $scope.searchsDEPT = function (item) {

        var array = item.split(' - ');

        if (array[0] == "") {

            $scope.deptForm.xx3 = true;

            return;
        }
        else {

            $scope.deptForm.sdept = array[0];

            $scope.deptForm.isLoading = true;

            $scope.deptForm.xx3 = checkDept("iClass", "Class");

            $scope.deptForm.t2 = true;

        }

        //console.log(array[0]);

    }

    $scope.searchiDEPT = function (item) {

        var array = item.split(' - ');

        if (array[0] == "") {

            $scope.deptForm.xx4 = true;

            return;
        }
        else {

            $scope.deptForm.iclass = array[0];


            $scope.deptForm.isLoading = true;


            $scope.deptForm.xx4 = checkDept("siClass", "Sub-Class");

            $scope.deptForm.t3 = true;

        }
        
        //console.log(array[0]);

    }

    $scope.searchsiDEPT = function (item) {
        
        var array = item.split(' - ');

        if (array[0] == "") {

            $scope.deptForm.xx5 = true;

            return;
        }
        else {

            $scope.deptForm.siclass = array[0];

            $scope.deptForm.isLoading = true;

            $scope.deptForm.xx5 = false;

            $scope.deptForm.t4 = true;

            $scope.deptForm.isLoading = false;

        }
        
        //console.log(array[0]);

    }
    
    $scope.searchReset = function () {

        window.location = "rk/dept";

    }

    function checkDept(obj, nme) {

        var x = document.getElementById(obj);

        while (x.options.length > 0) { x.remove(0); }
               

        $http.get('/RankRoute/searchDept/' + $scope.deptForm.dept + "/" + $scope.deptForm.sdept + "/" + $scope.deptForm.iclass + "/" + $scope.deptForm.siclass)
            .then(function (response) {

                //console.log(response);

                if (response.data.length == 0) {

                    $scope.deptForm.isLoading = false;
                    
                    return;

                } else {

                    //console.log(response);

                    var option = document.createElement("option");

                    option.text = "Select " + nme;

                    x.add(option);
                    
                    $.each(response.data, function (index, element) {

                        var option = document.createElement("option");

                        option.text = element.Code + " - " + element.Name;

                        option.value = element.Code + " - " + element.Name;

                        x.add(option);

                    });

                    $scope.deptForm.isLoading = false;

                    return false;
                }
            });
    }
    
    $scope.requestDEPT = function () {

        $scope.deptForm.t4 = true;

        //console.log($scope.deptForm.dept + "/" + $scope.deptForm.sdept + "/" + $scope.deptForm.iclass + "/" + $scope.deptForm.siclass);
                                                  
        if ($scope.deptForm.dept == "" || $scope.deptForm.sdept == "" || $scope.deptForm.iclass == "" || $scope.deptForm.siclass == "") return;

        $scope.deptForm.isLoading = true;

        $http.post(
            '/RankRoute/SubmitDept', {
                ffrom: $scope.deptForm.currentdate,
                tto: $scope.deptForm.archivedate,
                dept: $scope.deptForm.dept,
                sdept: $scope.deptForm.sdept,
                iclass: $scope.deptForm.iclass,
                siclass: $scope.deptForm.siclass
            }
        ).
        success(function (data) {
            if (data == "True") {

                window.location = '/rk/requestlist';

            } else {

                $scope.deptForm.requestFailure = true;
                $scope.deptForm.isLoading = false;
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

DeptController.$inject = ['$scope', '$http', '$location'];

