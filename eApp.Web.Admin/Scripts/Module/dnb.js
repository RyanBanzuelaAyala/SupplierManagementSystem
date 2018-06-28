var dnb = angular.module('dnb', ['ngRoute', 'ui.bootstrap']);

dnb.service('dnbservices', dnbservices);

//dnb.factory('DataFactory', DataFactory);
dnb.factory('ConfirmFactory', ConfirmFactory);
//dnb.factory('GenerateFactory', GenerateFactory);
dnb.factory('ValidateFactory', ValidateFactory);
//dnb.factory('InfoFactory', InfoFactory);
//dnb.factory('UserFactory', UserFactory);

dnb.controller('LandingPageController', LandingPageController);
dnb.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);

var configFunction = function ($routeProvider, $httpProvider, $locationProvider) {

    $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.        
        when('/monitor/activityinfo/:sid/:ixdate', {
            templateUrl: function (params) { return '/Monitor/SupplierActivityInfo?sid=' + params.sid + '&ixdate=' + params.ixdate }
        })
        .when('/monitor/activities/:ixdate', {
            templateUrl: function (params) { return '/Monitor/SupplierActivities?ixdate=' + params.ixdate }
        })
        .when('/monitor/log', {
            templateUrl: 'Monitor/SupplierLog'
        })
        .when('/report/supplier', {
            templateUrl: 'Report/SupplierReport'
        })
        .when('/report/overall', {
            templateUrl: 'Report/OverAllPO'
        })
        .when('/cp/complainlistsolvedinfo/:issueid', {
            templateUrl: function (params) { return '/Complain/ComplainInfoSolved?issueid=' + params.issueid }
        })
        .when('/cp/complainlistsolved', {
            templateUrl: 'Complain/ComplainListofSolved'
        })
        .when('/cp/complainreply/:respnse/:issueid', {
            templateUrl: function (params) { return '/Complain/ComplainInfoReply?respnse=' + params.respnse + "&issueid=' + params.issueid"; }
        })
        .when('/cp/complaininfo/:issueid', {
            templateUrl: function (params) { return '/Complain/ComplainInfo?issueid=' + params.issueid; }
        })
        .when('/cp/complainlist', {
            templateUrl: 'Complain/ComplainList'
        })
        .when('/ag/status/:userid/:stats', {
            templateUrl: function (params) { return '/Agent/AgentStatus?userid=' + params.userid + "&stats=" + params.stats; }
        })
        .when('/ag/agentlist', {
            templateUrl: 'Agent/AgentList'
        })
        .when('/ag/new', {
            templateUrl: 'Agent/NewAgent'
        })
        .when('/report/overall', {
            templateUrl: 'Report/OverAllPO'
        })
        .when('/useraddarinfo/:userid', {
            templateUrl: function (params) { return '/Account/UserArInfo?userid=' + params.userid; }
        })       
        .when('/activation', {
            templateUrl: 'Account/AccountAct'
        })
        .when('/activate/:sid/:reg', {
            templateUrl: function (params) { return '/Account/AccountActivated?sid=' + params.sid + "&reg=" + params.reg; }
        })
        .when('/rk/ranklist/:userid/:stats', {
            templateUrl: function (params) { return '/Rank/RankList?userid=' + params.userid + "&stats=" + params.stats; }
        })
        .when('/rk/rank/processing', {
            templateUrl: 'Rank/ProcessingList'
        })
        .when('/rk', {
            templateUrl: 'Rank/Index'
        })
        .when('/po/polist/:userid/:stats', {
            templateUrl: function (params) { return '/PO/PoList?userid=' + params.userid + "&stats=" + params.stats; }
        })        
        .when('/po', {
            templateUrl: 'PO/Index'
        })
        .when('/sup/newaccount/:username', {
            templateUrl: function (params) { return '/Supplier/NewAccount?username=' + params.username; }
        })          
        .when('/sup/useredit/:userid', {
            templateUrl: function (params) { return '/Supplier/UserEdit?userid=' + params.userid; }
        })
        .when('/sup/userregion/:userid', {
            templateUrl: function (params) { return '/Supplier/UserRegion?userid=' + params.userid; }
        })
        .when('/sup/userlist/:userid', {
            templateUrl: function (params) { return '/Supplier/UserList?userid=' + params.userid; }
        }) 
        .when('/sup/search', {
            templateUrl: 'Supplier/Search'
        })        
        .when('/supplier/new', {
            templateUrl: 'Supplier/NewSupplier'
        })           
        .when('/supplier/list', {
            templateUrl: 'Supplier/SupplierList'
        })   
        .when('/', {
            templateUrl: 'Supplier/SupplierList'
        })
        .otherwise({ redirectTo: '/' });

    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');

}
configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

dnb.config(configFunction);