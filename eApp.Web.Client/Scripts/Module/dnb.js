var dnb = angular.module('dnb', ['ngRoute', 'ui.bootstrap']);

dnb.service('dnbservices', dnbservices);

dnb.factory('DataFactory', DataFactory);
dnb.factory('ConfirmFactory', ConfirmFactory);
dnb.factory('GenerateFactory', GenerateFactory);
dnb.factory('ValidateFactory', ValidateFactory);
dnb.factory('InfoFactory', InfoFactory);
dnb.factory('UserFactory', UserFactory);

dnb.controller('LandingPageController', LandingPageController);
dnb.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);

dnb.controller('PoController', PoController);

var configFunction = function ($routeProvider, $httpProvider, $locationProvider) {

    $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/cp/listinfo/:issueid', {
            templateUrl: function (params) { return '/Complain/IssueListInfo?issueid=' + params.issueid }
        })
        .when('/cp/list', {
            templateUrl: 'Complain/IssueList'
        })
        .when('/cp/issue', {
            templateUrl: 'Complain/Issue'
        })        
        .when('/r/downloaded', {
            templateUrl: 'Rtv/Downloaded'
        })
        .when('/r/submitpo', {
            templateUrl: 'Rtv/SubmitPO'
        })
        .when('/r/available', {
            templateUrl: 'Rtv/Available'
        })
        .when('/a/password', {
            templateUrl: 'Account/ChangePassword'
        })        
        .when('/p/po', {
            templateUrl: 'PORoute/PO'
        })        
        .when('/rk/create', {
            templateUrl: 'RankRoute/Brand'
        })
        .when('/rk/searchCSV/:brandcode', {
            templateUrl: function (params) { return '/RankRoute/searchCSV?brandcode=' + params.brandcode; }
        })
        .when('/rk/searchCSVB/:brandcode', {
            templateUrl: function (params) { return '/RankRoute/searchCSVB?brandcode=' + params.brandcode; }
        })
        .when('/rk/brandrequest', {
            templateUrl: 'RankRoute/SubmitBrand'
        })
        .when('/rk/requestarlist', {
            templateUrl: 'RankRoute/RequestARList'
        })
        .when('/rk/requestdllist', {
            templateUrl: 'RankRoute/RequestDLList'
        })
        .when('/rk/requestlist', {
            templateUrl: 'RankRoute/RequestList'
        })
        .when('/rk/sku', {
            templateUrl: 'RankRoute/SKU'
        })
        .when('/rk/skurequest', {
            templateUrl: 'RankRoute/SubmitSKU'
        })
        .when('/rk/dept', {
            templateUrl: 'RankRoute/Department'
        })
        .when('/rk/searchCSV/:brandcode', {
            templateUrl: function (params) { return '/RankRoute/searchCSV?brandcode=' + params.brandcode }
        })
        .when('/rk/deptrequest', {
            templateUrl: 'RankRoute/SubmitDept'
        })
        .when('/rk/searchDept/:dept/:sdept/:iclass/:siclass', {
            templateUrl: function (params) {
                return '/RankRoute/searchDept?dept='
                    + params.dept
                    + '&sdept=' + params.sdept
                    + '&iclass=' + params.iclass
                    + '&siclass=' + params.siclass
            }
        })
        .when('/rk/submitrankdl/:reqid', {
            templateUrl: function (params) { return '/RankRoute/SubmitRankDL?reqid=' + params.reqid }
        })        
        .when('/dashboard', {
            templateUrl: '/Home/Startup'
        })     
        .when('/stats/summary', {
            templateUrl: '/Performance/Summary'
        })    
        .when('/', {
            templateUrl: '/Home/Startup'
        })
        .otherwise({ redirectTo: '/' });

    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');

}
configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

dnb.config(configFunction);
