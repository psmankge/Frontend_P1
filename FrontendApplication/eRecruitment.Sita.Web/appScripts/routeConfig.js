angular.module('MyApp', [
    'ngRoute',
    'MyApp.ctrl.crud',
])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider.when('/Index', {
            templateUrl: '/Home/Index',
            controller: 'myController'
        });
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }]);