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
                templateUrl: '/views/spa/home/index.html',
                controller: 'IndexController'
            })
            .when('/login', {
                templateUrl: '/views/spa/account/login.html',
                controller: 'LoginController'
            })
            .when('/register', {
                templateUrl: '/views/spa/account/register.html',
                controller: 'RegisterController'
            })
            .when('/products', {
                templateUrl: '/views/spa/products/products.html',
                controller: 'ProductsController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/products/add', {
                templateUrl: '/views/spa/products/add.html',
                controller: 'ProductsAddController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/products/:id', {
                templateUrl: '/views/spa/products/details.html',
                controller: 'ProductsDetailsController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/products/edit/:id', {
                templateUrl: '/views/spa/products/edit.html',
                controller: 'ProductsEditController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers', {
                templateUrl: '/views/spa/customers/customers.html',
                controller: 'CustomersController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/juridical-person/add', {
                templateUrl: '/views/spa/customers/add-juridical-person.html',
                controller: 'CustomersAddJuridicalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/natural-person/add', {
                templateUrl: '/views/spa/customers/add-natural-person.html',
                controller: 'CustomersAddNaturalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/juridical-person/:id', {
                templateUrl: '/views/spa/customers/details-juridical-person.html',
                controller: 'CustomersDetailsJuridicalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/natural-person/:id', {
                templateUrl: '/views/spa/customers/details-natural-person.html',
                controller: 'CustomersDetailsNaturalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/juridical-person/edit/:id', {
                templateUrl: '/views/spa/customers/edit-juridical-person.html',
                controller: 'CustomersEditJuridicalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/customers/natural-person/edit/:id', {
                templateUrl: '/views/spa/customers/edit-natural-person.html',
                controller: 'CustomersEditNaturalPersonController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/orders', {
                templateUrl: '/views/spa/orders/orders.html',
                controller: 'OrdersController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/orders/add/:id', {
                templateUrl: '/views/spa/orders/add.html',
                controller: 'OrdersAddController',
                resolve: { isLoggedIn: isLoggedIn }
            })
            .when('/orders/edit/:orderId/customer/:customerId', {
                templateUrl: '/views/spa/orders/edit.html',
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
