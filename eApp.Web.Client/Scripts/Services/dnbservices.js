var dnbservices = function ($http) {

    this.getComplain = function (obj) {

        console.log(obj)

        return $http.get('/Complain/ComplainInfoReplies?issueid=' + obj);

    };

    this.GetDataFromUrl = function (url) {

        return $http.get(url);

    };

    this.PostUpdate = function (Url, formData) {
    
        $http.post(Url, formData, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (response) {
            console.log("Updated");
        });


    }

}

dnbservices.$inject = ['$http'];