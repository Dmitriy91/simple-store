(function () {
    'use strict';

    angular
        .module('app', ['app.core', 'app.layout'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider'];

    function config($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: '/scripts/app/home/index.html',
                controller: 'IndexController'
            })
            .when('/login', {
                templateUrl: '/scripts/app/account/login.html',
                controller: 'LoginController'
            })
            .when('/register', {
                templateUrl: '/scripts/app/account/register.html',
                controller: 'RegisterController'
            })
            .when('/products', {
                templateUrl: '/scripts/app/products/products.html',
                controller: 'ProductsController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/products/add', {
                templateUrl: '/scripts/app/products/add.html',
                controller: 'ProductsAddController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/products/:id', {
                templateUrl: '/scripts/app/products/details.html',
                controller: 'ProductsDetailsController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/products/edit/:id', {
                templateUrl: '/scripts/app/products/edit.html',
                controller: 'ProductsEditController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers', {
                templateUrl: '/scripts/app/customers/customers.html',
                controller: 'CustomersController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/juridical-person/add', {
                templateUrl: '/scripts/app/customers/add-juridical-person.html',
                controller: 'CustomersAddJuridicalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/natural-person/add', {
                templateUrl: '/scripts/app/customers/add-natural-person.html',
                controller: 'CustomersAddNaturalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/juridical-person/:id', {
                templateUrl: '/scripts/app/customers/details-juridical-person.html',
                controller: 'CustomersDetailsJuridicalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/natural-person/:id', {
                templateUrl: '/scripts/app/customers/details-natural-person.html',
                controller: 'CustomersDetailsNaturalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/juridical-person/edit/:id', {
                templateUrl: '/scripts/app/customers/edit-juridical-person.html',
                controller: 'CustomersEditJuridicalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/natural-person/edit/:id', {
                templateUrl: '/scripts/app/customers/edit-natural-person.html',
                controller: 'CustomersEditNaturalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/orders', {
                templateUrl: '/scripts/app/orders/orders.html',
                controller: 'OrdersController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/orders/add/:id', {
                templateUrl: '/scripts/app/orders/add.html',
                controller: 'OrdersAddController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/orders/edit/:orderId/customer/:customerId', {
                templateUrl: '/scripts/app/orders/edit.html',
                controller: 'OrdersEditController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .otherwise({ redirectTo: '/' });
    }

    run.$inject = ['$rootScope', '$templateCache', '$location', '$http', '$window'];

    function run($rootScope, $templateCache, $location, $http, $window) {
        $rootScope.$on('$viewContentLoaded', function () {
            $templateCache.removeAll();
        });
    }

    isLoggedIn.$inject = ['membershipService', '$rootScope', '$location'];
     
    function isLoggedIn(membershipService, $rootScope, $location) {
        if (!membershipService.isLoggedIn()) {
            $rootScope.prevLocationPath = $location.path();
            $location.path('/login');
        }
    }
})();
