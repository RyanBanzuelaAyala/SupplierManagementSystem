var dnbservices = function ($http) {

    this.getComplain = function (obj) {

        console.log(obj)

        return $http.get('/Complain/ComplainInfoReplies?issueid=' + obj);

    };

    this.GetDataFromUrl = function (url) {

        return $http.get(url);

    };

}

dnbservices.$inject = ['$http'];