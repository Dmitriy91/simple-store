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

(function () {
    'use strict';

    angular
        .module('app.core', [
            'ngRoute'
        ]);
})();

(function () {
    'use strict';

    angular
        .module('app.layout', [
            'angular-loading-bar'
        ]);
})();

(function (app_layout) {
    'use strict';

    app_layout.directive('topBar', topBar);

    function topBar() {
        var directive = {
            link: link,
            restrict: 'E',
            replace: true,
            templateUrl: '/views/spa/layout/top-bar.html'
        }

        return directive;

        function link(scope, element) {
            var jq = jQuery.noConflict();

            // highlight a saved theme when the page is loaded
            if (getCookie('theme') !== '') {
                var themeUrl = jq("link[href*='bootstrap']:first").prop('href');
                var themeName = themeUrl.split('/')[5].toLowerCase();
                var normalizedThemeName = '';

                jq('#theme-menu > li').each(function () {
                    normalizedThemeName = jq(this).text().trim().replace(/ /g, '').toLowerCase();

                    if (normalizedThemeName === themeName) {
                        jq(this).addClass('selected-theme');
                        return false;
                    }
                });
            }

            jq('#theme-menu > li').on('click', function (event) {
                // applying a selected theme
                // cross-browser solution
                var target = jq(event.target || event.srcElement || event.originalTarget);
                var uiThemeName = target.text();
                var normalizedThemeName = uiThemeName.trim().replace(/ /g, '').toLowerCase();
                var themeUrl = 'https://maxcdn.bootstrapcdn.com/bootswatch/3.3.7/' + normalizedThemeName + '/bootstrap.min.css';

                jq('link[href*="bootstrap"]:first').prop('href', themeUrl);
                setCookie('theme', themeUrl, 30); // $cookies service is not used because of its encoding '/'
                target.siblings().removeClass('selected-theme');
                target.addClass('selected-theme');
            });

            element.on('$destroy', function () {
                jq('#theme-menu > li').off('click');
            });

            function getCookie(cname) {
                var name = cname + '=';
                var ca = document.cookie.split(';');

                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) === ' ') c = c.substring(1);
                    if (c.indexOf(name) === 0) return c.substring(name.length, c.length);
                }

                return '';
            }

            function setCookie(cname, cvalue, exdays) {
                var d = new Date();
                d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
                var expires = 'expires=' + d.toUTCString();
                document.cookie = cname + '=' + cvalue + '; ' + expires + '; path=/';
            }
        }
    }
})(angular.module('app.layout'));

(function (app_layout) {
    'use strict';

    app_layout.directive('pagination', pagination);

    function pagination() {
        var directive = {
            restrict: 'E',
            transclude: true,
            scope: {
                'loadItems': '&',
                'pager': '=',
                'size': '='
            },
            templateUrl: '/views/spa/layout/pagination.html'
        }

        return directive;
    }
})(angular.module('app.layout'));

(function (app_core) {
    'use strict';

    app_core.factory('dataService', dataService);

    dataService.$inject = ['$http', '$location', '$rootScope', 'notificationService'];

    function dataService($http, $location, $rootScope, notificationService) {
        var service = {
            get: get,
            post: post
        };

        return service;

        function get(url, config, success, error) {
            return $http.get(url, config)
                    .then(success, function (response) {
                            unauthorizedAccessHandler(response)

                            if (error !== null) error(response);
                    });
        }

        function post(url, data, success, error, headers) {
            return $http.post(url, data, { headers: headers === undefined ? null : headers })
                    .then(success, function (response) {
                            unauthorizedAccessHandler(response)

                            if (error !== null) error(response);
                    });
        }

        function unauthorizedAccessHandler(response) {
            if (response.status === '401') {
                notificationService.displayWarning('Authentication required.');
                $rootScope.previousState = $location.path();
                $location.path('/login');
            }
        };
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.factory('notificationService', notificationService);

    function notificationService() {
        toastr.options = {
            "debug": false,
            "positionClass": "toast-bottom-left",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 3000,
            "extendedTimeOut": 1000
        };

        var service = {
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo
        };

        return service;

        function displaySuccess(message) {
            toastr.success(message);
        }

        function displayError(error) {
            if (Array.isArray(error)) {
                error.forEach(function (err) {
                    toastr.error(err);
                });
            } else {
                toastr.error(error);
            }
        }

        function displayWarning(message) {
            toastr.warning(message);
        }

        function displayInfo(message) {
            toastr.info(message);
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.factory('membershipService', membershipService);

    membershipService.$inject = ['$http', '$rootScope','$window', 'dataService', 'notificationService'];

    function membershipService($http, $rootScope, $window, dataService, notificationService) {
        var service = {
            register: register,
            logIn: logIn,
            logOut: logOut,
            restoreCachedCredentials: restoreCachedCredentials,
            isLoggedIn: isLoggedIn
        };

        return service;

        function register(user, succeeded, failed) {
            dataService.post('/api/v1/account/register', user, succeeded, failed);
        }

        function logIn(user, succeeded, failed) {
            dataService.post('/Token',
                            jQuery.param(user), //query string data
                            function (response) { //success
                                var loggedUserData = {};

                                loggedUserData.login = response.data.userName.split('@')[0];
                                loggedUserData.authHeader = 'Bearer ' + response.data.access_token;

                                $http.defaults.headers.common['Authorization'] = loggedUserData.authHeader;
                                $window.sessionStorage.setItem('loggedUserData', JSON.stringify(loggedUserData));
                                succeeded(response);
                            },
                            failed, //error
                            { 'Content-Type': 'application/x-www-form-urlencoded' }); //headers    
        }

        function logOut() {
            $http.defaults.headers.common['Authorization'] = '';
            $window.sessionStorage.removeItem('loggedUserData');
        }

        function restoreCachedCredentials()
        {
            var loggedUserDataStr = $window.sessionStorage.getItem('loggedUserData');

            if (loggedUserDataStr !== null) {
                var loggedUserData = JSON.parse(loggedUserDataStr);
                $http.defaults.headers.common['Authorization'] = loggedUserData.authHeader;
                
                return loggedUserData.login;
            }

            return null;
        }

        function isLoggedIn()
        {
            var loggedUserDataStr = $window.sessionStorage.getItem('loggedUserData');

            return loggedUserDataStr !== null;
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.factory('pagerService', pagerService);

    function pagerService() {
        var service = {};

        service.GetPager = GetPager;

        return service;

        function GetPager(totalItems, totalPages, currentPage, pageSize) {
            currentPage = currentPage || 1; // set to first page by default
            pageSize = pageSize || 5; // default page size is 10
            var startPage = 0;
            var endPage = 0;

            if (totalPages <= 10) {
                // if there are less than 10 pages, show all pages
                startPage = 1;
                endPage = totalPages;
            } else {
                // if there are more than 10 pages, calculate starting and ending pages
                if (currentPage <= 6) {
                    startPage = 1;
                    endPage = 10;
                } else if (currentPage + 4 >= totalPages) {
                    startPage = totalPages - 9;
                    endPage = totalPages;
                } else {
                    startPage = currentPage - 5;
                    endPage = currentPage + 4;
                }
            }

            // calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageSize;
            var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

            // create an array of pages to ng-repeat in the pager control
            var pages = [];

            for (var i = startPage; i <= endPage; i++)
                pages.push(i);

            // return object with all pager properties required by the view
            return {
                totalItems: totalItems,
                currentPage: currentPage,
                pageSize: pageSize,
                totalPages: totalPages,
                startPage: startPage,
                endPage: endPage,
                startIndex: startIndex,
                endIndex: endIndex,
                pages: pages
            };
        }
    }
})(angular.module('app.core'));

(function (app) {
    'use strict';

    app.controller('RootController', RootController);

    RootController.$inject = ['$scope', '$location', '$rootScope', 'membershipService'];

    function RootController($scope, $location, $rootScope, membershipService) {
        $scope.user = {}; 
        $scope.user.logOut = logOut;
        $scope.user.loggedIn = false;
        $scope.user.login = '';

        function logOut() {
            membershipService.logOut();
            $location.path('#/');
            $scope.user.loggedIn = false;
            $scope.user.email = '';
        }

        $scope.$on('userLoggedIn', function (event, login) {
            $scope.user.login = login;
            $scope.user.loggedIn = true;
        });

        $scope.$on('$viewContentLoaded', function () {
            // handle pages' refreshes
            var login = membershipService.restoreCachedCredentials()

            if (login !== null)
                $rootScope.$broadcast('userLoggedIn', login);
        });
    }
})(angular.module('app'));

(function (app_core) {
    'use strict';

    app_core.controller('IndexController', IndexController);

    IndexController.$inject = ['$scope', 'dataService', 'notificationService'];

    function IndexController($scope, dataService, notificationService) {

    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('LoginController', LoginController);

    LoginController.$inject = ['$scope', '$rootScope', '$location', 'membershipService', 'notificationService'];

    function LoginController($scope, $rootScope, $location, membershipService, notificationService) {
        $scope.user = {};
        $scope.user.logIn = logIn;
        $scope.user.email = 'admin@gmail.com';
        $scope.user.password = 'Admin123!';

        function logIn() {
            var userData = {
                username: $scope.user.email,
                password: $scope.user.password,
                grant_type: 'password'
            };

            membershipService.logIn(userData, loginSucceeded, loginFailed);
        }

        function loginSucceeded(response) {
            notificationService.displaySuccess('Hello ' + $scope.user.email);
            $rootScope.$broadcast('userLoggedIn', $scope.user.email.split('@')[0]);

            var prevLocationPath = $rootScope.prevLocationPath;

            if (prevLocationPath)
                $location.path(prevLocationPath);
            else
                $location.path('/customers');
        }

        function loginFailed(response) {
            notificationService.displayError('Your login or password is incorrect. Please try again.');
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('RegisterController', RegisterController);

    RegisterController.$inject = ['$scope', '$rootScope', '$location', 'membershipService', 'notificationService'];

    function RegisterController($scope, $rootScope, $location, membershipService, notificationService) {
        $scope.user = {};
        $scope.user.email = '';
        $scope.user.password = '';
        $scope.user.confirmPassword = '';
        $scope.user.register = register;

        function register() {
            membershipService.register($scope.user, registerSucceeded, registerFailed)
        }

        function registerSucceeded(response) {
            notificationService.displaySuccess($scope.user.email + ' signed up.');
            $location.path('/login');
        };

        function registerFailed(response) {
            notificationService.displayError('Registration failed. Please try again.');
        };
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('ProductsController', ProductsController);

    ProductsController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService', 'pagerService'];

    function ProductsController($scope, $route, $rootScope, $location, dataService, notificationService, pagerService) {
        $scope.products = []; 
        $scope.removeProduct = removeProduct;
        $scope.loadProducts = loadProducts;
        $scope.pageSize = 5;
        $scope.sortBy = '';
        $scope.search = '';
        $scope.pager = {};
        $scope.sortByOptions = {
            'None': '',
            'Price Ascending': 'unitPriceAsc',
            'Price Descending': 'unitPriceDesc',
            'Name Ascending': 'productNameAsc',
            'Name Descending': 'productNameDesc'
        };
        activate();

        function loadProducts(pageNumber) {
            var pageSize = $scope.pageSize || 5;
            var sortBy = $scope.sortBy || '';
            var search = $scope.search || '';

            pageNumber = pageNumber || 1;

            dataService.get('/api/v1/products', {
                params: {
                    pageSize: pageSize,
                    pageNumber: pageNumber,
                    sortBy: sortBy,
                    productName: search
                }
            }, loadProductsSucceeded, loadProductsFailed);
        }

        function loadProductsSucceeded(response) {
            $scope.products = response.data.products;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.pager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadProductsFailed(response) {
            notificationService.displayError("Products haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function removeProduct(inx) {
            var productId = $scope.products[inx].id;

            dataService.post('/api/v1/products/delete/' + productId, null, removeProductSucceeded, removeProductFailed);
        }

        function removeProductFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeProductSucceeded(response) {
            $route.reload();
            notificationService.displaySuccess('Product has been successfully removed.');
        }

        function activate() {
            loadProducts();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('ProductsAddController', ProductsAddController);

    ProductsAddController.$inject = ['$scope', '$rootScope', '$location', 'dataService', 'notificationService'];

    function ProductsAddController($scope, $rootScope, $location, dataService, notificationService) {
        $scope.product = {};
        $scope.product.productName = '';
        $scope.product.unitPrice = '';
        $scope.product.unitsInStock = '';
        $scope.product.add = addProduct;

        function addProduct() {
            var productData = {};

            productData.productName = $scope.product.productName;
            productData.unitPrice = $scope.product.unitPrice;
            productData.unitsInStock = $scope.product.unitsInStock;
            dataService.post('/api/v1/products/add', productData, addProductSucceeded, addProductFaild);
        }

        function addProductSucceeded(response) {
            notificationService.displaySuccess('Product has been successfu created.');
            $location.path('/products');
        }

        function addProductFaild(response) {
            notificationService.displayError('Unauthorised actions detected.');
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('ProductsDetailsController', ProductsDetailsController);

    ProductsDetailsController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function ProductsDetailsController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.product = {};
        $scope.product.id = '';
        $scope.product.productName = '';
        $scope.product.unitPrice = '';
        $scope.product.unitsInStock = '';
        activate();

        function loadProduct() {
            dataService.get('/api/v1/products/details/' + $routeParams.id, null, loadProductSucceeded, loadProductFailed);
        }

        function loadProductSucceeded(response) {
            $scope.product.id = response.data.id;
            $scope.product.productName = response.data.productName;
            $scope.product.unitPrice = response.data.unitPrice;
            $scope.product.unitsInStock = response.data.unitsInStock;
        }

        function loadProductFailed(response) {
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/products');
        }

        function activate() {
            loadProduct();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('ProductsEditController', ProductsEditController);

    ProductsEditController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function ProductsEditController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.product = {};
        $scope.product.id = '';
        $scope.product.productName = '';
        $scope.product.unitPrice = '';
        $scope.product.unitsInStock = '';
        $scope.product.update = updateProduct;
        activate();

        function updateProduct() {
            var productData = {};

            productData.id = $scope.product.id;
            productData.productName = $scope.product.productName;
            productData.unitPrice = $scope.product.unitPrice;
            productData.unitsInStock = $scope.product.unitsInStock;
            dataService.post('/api/v1/products/update/', productData, updateProductSucceeded, updateProductFaild);
        }

        function updateProductSucceeded(response) {
            notificationService.displaySuccess('Product has been successfully updated.');
            $location.path('/products');
        }

        function updateProductFaild(response) {
            notificationService.displayError('Unauthorised actions detected.');
        }

        function loadProduct() {
            dataService.get('/api/v1/products/details/' + $routeParams.id, null, loadProductSucceeded, loadProductFailed);
        }

        function loadProductSucceeded(response) {
            $scope.product.id = response.data.id;
            $scope.product.productName = response.data.productName;
            $scope.product.unitPrice = response.data.unitPrice;
            $scope.product.unitsInStock = response.data.unitsInStock;
        }

        function loadProductFailed(response) {
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/products');
        }

        function activate() {
            loadProduct();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('CustomersController', CustomersController);

    CustomersController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService', 'pagerService'];

    function CustomersController($scope, $route, $rootScope, $location, dataService, notificationService, pagerService) {
        $scope.naturalPersons = [];
        $scope.juridicalPersons = [];

        $scope.loadNaturalPersons = loadNaturalPersons;
        $scope.loadJuridicalPersons = loadJuridicalPersons;

        $scope.removeNaturalPerson = removeNaturalPerson;
        $scope.removeJuridicalPerson = removeJuridicalPerson;

        $scope.naturalPersonsPager = {};
        $scope.juridicalPersonsPager = {};

        $scope.naturalPersonFilters = [
            {
                name:'firstName',
                value: ''
            },
            {
                name: 'middleName',
                value: ''
            },
            {
                name: 'lastName',
                value: ''
            },
            {
                name: 'ssn',
                value: ''
            },
            {
                name: 'birthdate',
                value: ''
            },
            {
                name: 'country',
                value: ''
            },
            {
                name: 'region',
                value: ''
            },
            {
                name: 'city',
                value: ''
            },
            {
                name: 'streetAddress',
                value: ''
            },
            {
                name: 'postalCode',
                value: ''
            }
        ];
        $scope.juridicalPersonFilters = [
            {
                name: 'legalName',
                value: ''
            },
            {
                name: 'tin',
                value: ''
            },
            {
                name: 'country',
                value: ''
            },
            {
                name: 'region',
                value: ''
            },
            {
                name: 'city',
                value: ''
            },
            {
                name: 'streetAddress',
                value: ''
            },
            {
                name: 'postalCode',
                value: ''
            }
        ];
        $scope.filtersDisplayed = false;
        $scope.clearFilters = clearFilters;
        $scope.toggleFilters = toggleFilters;

        $scope.naturalPersonsSortBy = '';
        $scope.juridicalPersonsSortBy = '';

        $scope.pageSize = 5;
        $scope.customerType = 'naturalPerson';

        activate();

        function loadJuridicalPersons(pageNumber) {
            pageNumber = pageNumber || 1;

            var pageSize = $scope.pageSize || 5;
            var queryParams = {
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            if ($scope.juridicalPersonsSortBy)
                queryParams['sortBy'] = $scope.juridicalPersonsSortBy;

            angular.forEach($scope.juridicalPersonFilters, function (filter) {
                if (filter && filter.value)
                    queryParams[filter.name] = filter.value;
            });

            dataService.get('/api/v1/customers/juridical-persons', {
                params: queryParams
            }, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
        }

        function loadNaturalPersons(pageNumber) {
            pageNumber = pageNumber || 1;

            var pageSize = $scope.pageSize || 5;
            var queryParams = {
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            if ($scope.naturalPersonsSortBy)
                queryParams['sortBy'] = $scope.naturalPersonsSortBy;

            angular.forEach($scope.naturalPersonFilters, function (filter) {
                if (filter && filter.value)
                    queryParams[filter.name] = filter.value;
            });

            dataService.get('/api/v1/customers/natural-persons', {
                params: queryParams
            }, loadNaturalPersonsSucceeded, loadNaturalPersonsFailed);
        }

        function loadJuridicalPersonsSucceeded(response) {
            $scope.juridicalPersons = response.data.juridicalPersons;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.juridicalPersonsPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadJuridicalPersonsFailed(response) {
            notificationService.displayError("Juridical persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadNaturalPersonsSucceeded(response) {
            $scope.naturalPersons = response.data.naturalPersons;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.naturalPersonsPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadNaturalPersonsFailed(response) {
            notificationService.displayError("Natural persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function removeJuridicalPerson(inx) {
            var personId = $scope.juridicalPersons[inx].id;

            dataService.post('/api/v1/customers/delete/' + personId, null, removeJuridicalPersonSucceeded, removeJuridicalPersonFailed);
        }

        function removeJuridicalPersonFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeJuridicalPersonSucceeded(response) {
            loadJuridicalPersons();
            notificationService.displaySuccess('Customer has been successfully removed.');
        }

        function removeNaturalPerson(inx) {
            var personId = $scope.naturalPersons[inx].id;

            dataService.post('/api/v1/customers/delete/' + personId, null, removeNaturalPersonSucceeded, removeNaturalPersonFailed);
        }

        function removeNaturalPersonFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeNaturalPersonSucceeded(response) {
            loadNaturalPersons();
            notificationService.displaySuccess('Customer has been successfully removed.');
        }

        function clearFilters() {
            if ($scope.customerType === 'naturalPerson') {
                var updateNaturalPersons = false;

                angular.forEach($scope.naturalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateNaturalPersons = true;
                    }
                });

                if (updateNaturalPersons)
                    loadNaturalPersons();
            }
            else {
                var updateJuridicalPersons = false;

                angular.forEach($scope.juridicalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateJuridicalPersons = true;
                    }
                });

                if (updateJuridicalPersons)
                    loadJuridicalPersons();
            }
        }

        function toggleFilters() {
            if ($scope.filtersDisplayed) {
                var updateNaturalPersons = false;
                var updateJuridicalPersons = false;

                $scope.filtersDisplayed = false;
                angular.forEach($scope.naturalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateNaturalPersons = true;
                    }
                });
                angular.forEach($scope.juridicalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateJuridicalPersons = true;
                    }
                });

                if (updateJuridicalPersons)
                    loadJuridicalPersons();

                if (updateNaturalPersons)
                    loadNaturalPersons();
            }
            else {
                $scope.filtersDisplayed = true;
            }
        }

        function activate() {
            var jq = jQuery.noConflict();

            jq('#naturalPersonsTable').tablesorter({
                headers: {
                    10: {
                        // disable sorting for the 11-th column
                        sorter: false
                    }
                }
            });
            jq('#juridicalPersonsTable').tablesorter({
                headers: {
                    7: {
                        // disable sorting for the 8-th column
                        sorter: false
                    }
                }
            });
            jq('#naturalPersonsTable').bind('sortEnd', function (e) {
                var $columnHeader = jq('#naturalPersonsTable > thead > tr > th[class*="headerSortUp"], #naturalPersonsTable > thead > tr > th[class*="headerSortDown"]').eq(0);
                var sortBy = $columnHeader.attr('data-sort-by');

                if (sortBy) {
                    if ($columnHeader.hasClass('headerSortUp'))
                        $scope.naturalPersonsSortBy = sortBy + 'Asc';
                    else
                        $scope.naturalPersonsSortBy = sortBy + 'Desc';
                }

                loadNaturalPersons();
            });
            jq('#juridicalPersonsTable').bind('sortEnd', function (e) {
                var $columnHeader = jq('#juridicalPersonsTable > thead > tr > th[class*="headerSortUp"], #juridicalPersonsTable > thead > tr > th[class*="headerSortDown"]').eq(0);
                var sortBy = $columnHeader.attr('data-sort-by');

                if (sortBy) {
                    if ($columnHeader.hasClass('headerSortUp'))
                        $scope.juridicalPersonsSortBy = sortBy + 'Asc';
                    else
                        $scope.juridicalPersonsSortBy = sortBy + 'Desc';
                }

                loadJuridicalPersons();
            });
            jq('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true,
            });

            loadJuridicalPersons();
            loadNaturalPersons();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('CustomersAddJuridicalPersonController', CustomersAddJuridicalPersonController);

    CustomersAddJuridicalPersonController.$inject = ['$scope', '$rootScope', '$location', 'dataService', 'notificationService'];

    function CustomersAddJuridicalPersonController($scope, $rootScope, $location, dataService, notificationService) {
        $scope.juridicalPerson = {};
        $scope.juridicalPerson.legalName = '';
        $scope.juridicalPerson.tin = '';
        $scope.juridicalPerson.country = '';
        $scope.juridicalPerson.region = '';
        $scope.juridicalPerson.city = '';
        $scope.juridicalPerson.streetAddress = '';
        $scope.juridicalPerson.postalCode = '';
        $scope.juridicalPerson.add = addJuridicalPerson;
        activate();

        function addJuridicalPerson() {
            var juridicalPersonData = {};

            juridicalPersonData.legalName = $scope.juridicalPerson.legalName;
            juridicalPersonData.tin = $scope.juridicalPerson.tin;
            juridicalPersonData.country = $scope.juridicalPerson.country;
            juridicalPersonData.region = $scope.juridicalPerson.region;
            juridicalPersonData.city = $scope.juridicalPerson.city;
            juridicalPersonData.streetAddress = $scope.juridicalPerson.streetAddress;
            juridicalPersonData.postalCode = $scope.juridicalPerson.postalCode;

            dataService.post('/api/v1/customers/juridical-person/add', juridicalPersonData, addJuridicalPersonSucceeded, addJuridicalPersonFaild);
        }

        function addJuridicalPersonSucceeded(response) {
            notificationService.displaySuccess('Customer has been successfu created.');
            $location.path('/customers');
        }

        function addJuridicalPersonFaild(response) {
            notificationService.displayError('This person has been already added.');
        }

        function activate() {
            jQuery('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayBtn: 'linked',
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true,
            });
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('CustomersAddNaturalPersonController', CustomersAddNaturalPersonController);

    CustomersAddNaturalPersonController.$inject = ['$scope', '$rootScope', '$location', 'dataService', 'notificationService'];

    function CustomersAddNaturalPersonController($scope, $rootScope, $location, dataService, notificationService) {
        $scope.naturalPerson = {};
        $scope.naturalPerson.firstName = '';
        $scope.naturalPerson.middleName = '';
        $scope.naturalPerson.lastName = '';
        $scope.naturalPerson.ssn = '';
        $scope.naturalPerson.birthdate = '';
        $scope.naturalPerson.country = '';
        $scope.naturalPerson.region = '';
        $scope.naturalPerson.city = '';
        $scope.naturalPerson.streetAddress = '';
        $scope.naturalPerson.postalCode = '';
        $scope.naturalPerson.add = addNaturalPerson;
        activate();

        function addNaturalPerson() {
            var naturalPersonData = {};

            naturalPersonData.firstName = $scope.naturalPerson.firstName;
            naturalPersonData.lastName = $scope.naturalPerson.lastName;
            naturalPersonData.middleName = $scope.naturalPerson.middleName;
            naturalPersonData.ssn = $scope.naturalPerson.ssn;
            naturalPersonData.birthdate = $scope.naturalPerson.birthdate;
            naturalPersonData.country = $scope.naturalPerson.country;
            naturalPersonData.region = $scope.naturalPerson.region;
            naturalPersonData.city = $scope.naturalPerson.city;
            naturalPersonData.streetAddress = $scope.naturalPerson.streetAddress;
            naturalPersonData.postalCode = $scope.naturalPerson.postalCode;

            dataService.post('/api/v1/customers/natural-person/add', naturalPersonData, addNaturalPersonSucceeded, addNaturalPersonFaild);
        }

        function addNaturalPersonSucceeded(response) {
            notificationService.displaySuccess('Customer has been successfu created.');
            $location.path('/customers');
        }

        function addNaturalPersonFaild(response) {
            notificationService.displayError('This person has been already added.');
        }

        function activate() {
            jQuery('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true,
            });
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('CustomersDetailsJuridicalPersonController', CustomersDetailsJuridicalPersonController);

    CustomersDetailsJuridicalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersDetailsJuridicalPersonController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.juridicalPerson = {};
        $scope.juridicalPerson.id = '';
        $scope.juridicalPerson.legalName = '';
        $scope.juridicalPerson.tin = '';
        $scope.juridicalPerson.country = '';
        $scope.juridicalPerson.region = '';
        $scope.juridicalPerson.city = '';
        $scope.juridicalPerson.streetAddress = '';
        $scope.juridicalPerson.postalCode = '';
        activate();

        function loadJuridicalPerson() {
            dataService.get('/api/v1/customers/juridical-person/' + $routeParams.id, null, loadJuridicalPersonSucceeded, loadJuridicalPersonFailed);
        }

        function loadJuridicalPersonSucceeded(response) {
            $scope.juridicalPerson.id = response.data.id;
            $scope.juridicalPerson.legalName = response.data.legalName;
            $scope.juridicalPerson.tin = response.data.tin;
            $scope.juridicalPerson.country = response.data.country;
            $scope.juridicalPerson.region = response.data.region;
            $scope.juridicalPerson.city = response.data.city;
            $scope.juridicalPerson.streetAddress = response.data.streetAddress;
            $scope.juridicalPerson.postalCode = response.data.postalCode;
        }

        function loadJuridicalPersonFailed(response) {
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/customers');
        }

        function activate() {
            loadJuridicalPerson();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('CustomersDetailsNaturalPersonController', CustomersDetailsNaturalPersonController);

    CustomersDetailsNaturalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersDetailsNaturalPersonController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.naturalPerson = {};
        $scope.naturalPerson.id = '';
        $scope.naturalPerson.firstName = '';
        $scope.naturalPerson.middleName = '';
        $scope.naturalPerson.lastName = '';
        $scope.naturalPerson.ssn = '';
        $scope.naturalPerson.birthdate = '';
        $scope.naturalPerson.country = '';
        $scope.naturalPerson.region = '';
        $scope.naturalPerson.city = '';
        $scope.naturalPerson.streetAddress = '';
        $scope.naturalPerson.postalCode = '';
        activate();

        function loadNaturalPerson() {
            dataService.get('/api/v1/customers/natural-person/' + $routeParams.id, null, loadNaturalPersonSucceeded, loadNaturalPersonFailed);
        }

        function loadNaturalPersonSucceeded(response) {
            $scope.naturalPerson.id = response.data.id;
            $scope.naturalPerson.firstName = response.data.firstName;
            $scope.naturalPerson.middleName = response.data.middleName;
            $scope.naturalPerson.lastName = response.data.lastName;
            $scope.naturalPerson.ssn = response.data.ssn;
            $scope.naturalPerson.birthdate = response.data.birthdate;
            $scope.naturalPerson.country = response.data.country;
            $scope.naturalPerson.region = response.data.region;
            $scope.naturalPerson.city = response.data.city;
            $scope.naturalPerson.streetAddress = response.data.streetAddress;
            $scope.naturalPerson.postalCode = response.data.postalCode;
        }

        function loadNaturalPersonFailed(response) {
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/customers');
        }

        function activate() {
            loadNaturalPerson();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('CustomersEditJuridicalPersonController', CustomersEditJuridicalPersonController);

    CustomersEditJuridicalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersEditJuridicalPersonController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.juridicalPerson = {};
        $scope.juridicalPerson.id = '';
        $scope.juridicalPerson.legalName = '';
        $scope.juridicalPerson.tin = '';
        $scope.juridicalPerson.country = '';
        $scope.juridicalPerson.region = '';
        $scope.juridicalPerson.city = '';
        $scope.juridicalPerson.streetAddress = '';
        $scope.juridicalPerson.postalCode = '';
        $scope.juridicalPerson.update = updateJuridicalPerson;
        activate();

        function updateJuridicalPerson() {
            var juridicalPersonData = {};

            juridicalPersonData.id = $scope.juridicalPerson.id;
            juridicalPersonData.legalName = $scope.juridicalPerson.legalName;
            juridicalPersonData.tin = $scope.juridicalPerson.tin;
            juridicalPersonData.country = $scope.juridicalPerson.country;
            juridicalPersonData.region = $scope.juridicalPerson.region;
            juridicalPersonData.city = $scope.juridicalPerson.city;
            juridicalPersonData.streetAddress = $scope.juridicalPerson.streetAddress;
            juridicalPersonData.postalCode = $scope.juridicalPerson.postalCode;
            dataService.post('/api/v1/customers/juridical-person/update/', juridicalPersonData, updateJuridicalPersonSucceeded, updateJuridicalPersonFaild);
        }

        function updateJuridicalPersonSucceeded(response) {
            notificationService.displaySuccess('Customer has been successfu updated.');
            $location.path('/customers');
        }

        function updateJuridicalPersonFaild(response) {
            notificationService.displayError('Unauthorised actions detected.');
        }

        function loadJuridicalPerson() {
            dataService.get('/api/v1/customers/juridical-person/' + $routeParams.id, null, loadJuridicalPersonSucceeded, loadJuridicalPersonFailed);
        }

        function loadJuridicalPersonSucceeded(response) {
            $scope.juridicalPerson.id = response.data.id;
            $scope.juridicalPerson.legalName = response.data.legalName;
            $scope.juridicalPerson.tin = response.data.tin;
            $scope.juridicalPerson.country = response.data.country;
            $scope.juridicalPerson.region = response.data.region;
            $scope.juridicalPerson.city = response.data.city;
            $scope.juridicalPerson.streetAddress = response.data.streetAddress;
            $scope.juridicalPerson.postalCode = response.data.postalCode;
        }

        function loadJuridicalPersonFailed(response) {
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/customers');
        }

        function activate() {
            loadJuridicalPerson();
            jQuery('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayBtn: "linked",
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true
            });
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('CustomersEditNaturalPersonController', CustomersEditNaturalPersonController);

    CustomersEditNaturalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersEditNaturalPersonController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.naturalPerson = {};
        $scope.naturalPerson.id = '';
        $scope.naturalPerson.firstName = '';
        $scope.naturalPerson.middleName = '';
        $scope.naturalPerson.lastName = '';
        $scope.naturalPerson.ssn = '';
        $scope.naturalPerson.birthdate = '';
        $scope.naturalPerson.country = '';
        $scope.naturalPerson.region = '';
        $scope.naturalPerson.city = '';
        $scope.naturalPerson.streetAddress = '';
        $scope.naturalPerson.postalCode = '';
        $scope.naturalPerson.update = updateNaturalPerson;
        activate();

        function updateNaturalPerson() {
            var naturalPersonData = {};

            naturalPersonData.id = $scope.naturalPerson.id;
            naturalPersonData.firstName = $scope.naturalPerson.firstName;
            naturalPersonData.lastName = $scope.naturalPerson.lastName;
            naturalPersonData.middleName = $scope.naturalPerson.middleName;
            naturalPersonData.ssn = $scope.naturalPerson.ssn;
            naturalPersonData.birthdate = $scope.naturalPerson.birthdate;
            naturalPersonData.country = $scope.naturalPerson.country;
            naturalPersonData.region = $scope.naturalPerson.region;
            naturalPersonData.city = $scope.naturalPerson.city;
            naturalPersonData.streetAddress = $scope.naturalPerson.streetAddress;
            naturalPersonData.postalCode = $scope.naturalPerson.postalCode;
            dataService.post('/api/v1/customers/natural-person/update/', naturalPersonData, updateNaturalPersonSucceeded, updateNaturalPersonFaild);
        }

        function updateNaturalPersonSucceeded(response) {
            notificationService.displaySuccess('Customer has been successfu updated.');
            $location.path('/customers');
        }

        function updateNaturalPersonFaild(response) {
            notificationService.displayError('Unauthorised actions detected.');
        }

        function loadNaturalPerson() {
            dataService.get('/api/v1/customers/natural-person/' + $routeParams.id, null, loadNaturalPersonSucceeded, loadNaturalPersonFailed);
        }

        function loadNaturalPersonSucceeded(response) {
            $scope.naturalPerson.id = response.data.id;
            $scope.naturalPerson.firstName = response.data.firstName;
            $scope.naturalPerson.middleName = response.data.middleName;
            $scope.naturalPerson.lastName = response.data.lastName;
            $scope.naturalPerson.ssn = response.data.ssn;
            $scope.naturalPerson.birthdate = response.data.birthdate;
            $scope.naturalPerson.country = response.data.country;
            $scope.naturalPerson.region = response.data.region;
            $scope.naturalPerson.city = response.data.city;
            $scope.naturalPerson.streetAddress = response.data.streetAddress;
            $scope.naturalPerson.postalCode = response.data.postalCode;
        }

        function loadNaturalPersonFailed(response) {
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/customers');
        }

        function activate() {
            loadNaturalPerson();
            jQuery('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true
            });
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('OrdersController', OrdersController);

    OrdersController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService', 'pagerService'];

    function OrdersController($scope, $route, $rootScope, $location, dataService, notificationService, pagerService) {
        $scope.customerType = 'naturalPerson';
        $scope.selectCustomerType = selectCustomerType;

        $scope.naturalPersons = [];
        $scope.juridicalPersons = [];
        $scope.orders = [];
        $scope.orderDetails = [];
        $scope.paginatedOrderDetails = [];

        $scope.selectedCustomerId = 0;
        $scope.selectedOrderId = 0;
        $scope.selectCustomer = selectCustomer;
        $scope.selectOrder = selectOrder;
        $scope.removeOrder = removeOrder;

        $scope.loadNaturalPersons = loadNaturalPersons;
        $scope.loadJuridicalPersons = loadJuridicalPersons;
        $scope.naturalPersonsSortBy = '';
        $scope.juridicalPersonsSortBy = '';
        $scope.ordersSortBy = '';
        $scope.naturalPersonFilters = [
            {
                name: 'firstName',
                value: ''
            },
            {
                name: 'middleName',
                value: ''
            },
            {
                name: 'lastName',
                value: ''
            }
        ];
        $scope.juridicalPersonFilters = [
            {
                name: 'legalName',
                value: ''
            },
            {
                name: 'tin',
                value: ''
            }
        ];
        $scope.orderFilters = [
            {
                name: 'id',
                value: ''
            },
            {
                name: 'orderDate',
                value: ''
            }
        ];
        $scope.customerFiltersDisplayed = false;
        $scope.orderFiltersDisplayed = false;
        $scope.toggleCustomerFilters = toggleCustomerFilters;
        $scope.toggleOrderFilters = toggleOrderFilters;
        $scope.clearCustomerFilters = clearCustomerFilters;
        $scope.clearOrderFilters = clearOrderFilters;

        $scope.loadOrdersByCustomer = loadOrdersByCustomer;
        $scope.loadOrderDetails = loadOrderDetails;

        $scope.customersPageSize = 5;
        $scope.ordersPageSize = 5;
        $scope.orderDetailsPageSize = 5;

        $scope.naturalPersonsPager = {};
        $scope.juridicalPersonsPager = {};
        $scope.ordersPager = {};
        $scope.orderDetailsPager = {};
        activate();

        function clearSelectedCustomerInfo() {
            $scope.selectedCustomerId = 0;
            $scope.selectedOrderId = 0;
            $scope.orders = [];
            $scope.orderDetails = [];
            $scope.paginatedOrderDetails = [];
            $scope.ordersPager = {};
            $scope.orderDetailsPager = {};
        }

        function clearSelectedOrderInfo() {
            $scope.selectedOrderId = 0;
            $scope.orders = [];
            $scope.orderDetails = [];
        }

        function selectCustomerType(customerType) {
            clearSelectedCustomerInfo();
            $scope.customerType = customerType;

            if (customerType === 'naturalPerson')
                loadNaturalPersons();
            else
                loadJuridicalPersons();
        }

        function selectCustomer(customerId) {
            clearSelectedCustomerInfo();
            $scope.selectedCustomerId = customerId;
            loadOrdersByCustomer();
        }

        function selectOrder(orderId, orderDetails) {
            $scope.selectedOrderId = orderId;
            $scope.orderDetails = orderDetails;
            loadOrderDetails();
        }

        function removeOrder(inx) {
            var orderId = $scope.orders[inx].id;

            dataService.post('/api/v1/orders/delete/' + orderId, null, removeOrderSucceeded, removeOrderFailed);
        }

        function removeOrderFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeOrderSucceeded(response) {
            var selectedCustomerId = $scope.selectedCustomerId;

            clearSelectedOrderInfo();
            notificationService.displaySuccess('Order has been successfully removed.');
            loadOrdersByCustomer(selectedCustomerId);
        }

        function loadOrderDetails(pageNumber) {
            var pageSize = $scope.orderDetailsPageSize || 5;

            pageNumber = pageNumber || 1;

            var currentPage = pageNumber;
            var totalItems = $scope.orderDetails.length;
            var totalPages = Math.ceil(totalItems / pageSize);

            $scope.orderDetailsPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);

            if ($scope.orderDetails !== 'undefined') {
                $scope.paginatedOrderDetails = $scope.orderDetails.filter(function (currentValue, inx) {
                    return inx >= $scope.orderDetailsPager.startIndex && inx <= $scope.orderDetailsPager.endIndex;
                });
            }
        }

        function loadOrdersByCustomer(pageNumber) {
            pageNumber = pageNumber || 1;

            var pageSize = $scope.ordersPageSize || 5;
            var queryParams = {
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            if ($scope.ordersSortBy)
                queryParams['sortBy'] = $scope.ordersSortBy;

            angular.forEach($scope.orderFilters, function (filter) {
                if (filter && filter.value)
                    queryParams[filter.name] = filter.value;
            });

            if ($scope.selectedCustomerId !== 0) {
                dataService.get('/api/v1/orders/' + $scope.selectedCustomerId, {
                    params: queryParams
                }, loadOrdersByCustomerSucceeded, loadOrdersByCustomerFailed);
            }
        }

        function loadOrdersByCustomerSucceeded(response) {
            $scope.orders = response.data.orders;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.ordersPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadOrdersByCustomerFailed(response) {
            notificationService.displayError("Orders haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadJuridicalPersons(pageNumber) {
            pageNumber = pageNumber || 1;

            var pageSize = $scope.customersPageSize || 5;
            var queryParams = {
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            if ($scope.juridicalPersonsSortBy)
                queryParams['sortBy'] = $scope.juridicalPersonsSortBy;

            angular.forEach($scope.juridicalPersonFilters, function (filter) {
                if (filter && filter.value)
                    queryParams[filter.name] = filter.value;
            });

            dataService.get('/api/v1/customers/juridical-persons', {
                params: queryParams
            }, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
        }

        function loadNaturalPersons(pageNumber) {
            pageNumber = pageNumber || 1;

            var pageSize = $scope.customersPageSize || 5;
            var queryParams = {
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            if ($scope.naturalPersonsSortBy)
                queryParams['sortBy'] = $scope.naturalPersonsSortBy;

            angular.forEach($scope.naturalPersonFilters, function (filter) {
                if (filter && filter.value)
                    queryParams[filter.name] = filter.value;
            });

            dataService.get('/api/v1/customers/natural-persons', {
                params: queryParams
            }, loadNaturalPersonsSucceeded, loadNaturalPersonsFailed);
        }

        function loadJuridicalPersonsSucceeded(response) {
            $scope.juridicalPersons = response.data.juridicalPersons;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.juridicalPersonsPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadJuridicalPersonsFailed(response) {
            notificationService.displayError("Juridical persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadNaturalPersonsSucceeded(response) {
            $scope.naturalPersons = response.data.naturalPersons;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.naturalPersonsPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadNaturalPersonsFailed(response) {
            notificationService.displayError("Natural persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function toggleCustomerFilters() {
            if ($scope.customerFiltersDisplayed) {
                var updateNaturalPersons = false;
                var updateJuridicalPersons = false;

                $scope.customerFiltersDisplayed = false;
                angular.forEach($scope.naturalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateNaturalPersons = true;
                    }
                });
                angular.forEach($scope.juridicalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateJuridicalPersons = true;
                    }
                });

                if (updateJuridicalPersons)
                    loadJuridicalPersons();

                if (updateNaturalPersons)
                    loadNaturalPersons();
            }
            else {
                $scope.customerFiltersDisplayed = true;
            }
        }

        function toggleOrderFilters() {
            if ($scope.orderFiltersDisplayed) {
                var updateOrders = false;

                $scope.orderFiltersDisplayed = false;
                angular.forEach($scope.orderFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateOrders = true;
                    }
                });

                if (updateOrders)
                    loadOrdersByCustomer();
            }
            else {
                $scope.orderFiltersDisplayed = true;
            }
        }

        function clearCustomerFilters() {
            if ($scope.customerType === 'naturalPerson') {
                var updateNaturalPersons = false;

                angular.forEach($scope.naturalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateNaturalPersons = true;
                    }
                });

                if (updateNaturalPersons)
                    loadNaturalPersons();
            }
            else {
                var updateJuridicalPersons = false;

                angular.forEach($scope.juridicalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateJuridicalPersons = true;
                    }
                });

                if (updateJuridicalPersons)
                    loadJuridicalPersons();
            }
        }

        function clearOrderFilters() {
            var updateOrders = false;

            angular.forEach($scope.orderFilters, function (filter) {
                if (filter && filter.value) {
                    filter.value = '';
                    updateOrders = true;
                }
            });

            if (updateOrders)
                loadOrdersByCustomer();
        }

        function activate() {
            var jq = jQuery.noConflict();

            jq('#naturalPersonsTable').tablesorter();
            jq('#juridicalPersonsTable').tablesorter();
            jq('#ordersTable').tablesorter({
                headers: {
                    2: {
                        // disable sorting for the 3-th column
                        sorter: false
                    }
                }
            });
            jq('#naturalPersonsTable').bind('sortEnd', function (e) {
                var $columnHeader = jq('#naturalPersonsTable > thead > tr > th[class*="headerSortUp"], #naturalPersonsTable > thead > tr > th[class*="headerSortDown"]').eq(0);
                var sortBy = $columnHeader.attr('data-sort-by');

                if (sortBy) {
                    if ($columnHeader.hasClass('headerSortUp'))
                        $scope.naturalPersonsSortBy = sortBy + 'Asc';
                    else
                        $scope.naturalPersonsSortBy = sortBy + 'Desc';
                }

                loadNaturalPersons();
            });
            jq('#juridicalPersonsTable').bind('sortEnd', function (e) {
                var $columnHeader = jq('#juridicalPersonsTable > thead > tr > th[class*="headerSortUp"], #juridicalPersonsTable > thead > tr > th[class*="headerSortDown"]').eq(0);
                var sortBy = $columnHeader.attr('data-sort-by');

                if (sortBy) {
                    if ($columnHeader.hasClass('headerSortUp'))
                        $scope.juridicalPersonsSortBy = sortBy + 'Asc';
                    else
                        $scope.juridicalPersonsSortBy = sortBy + 'Desc';
                }

                loadJuridicalPersons();
            });
            jq('#ordersTable').bind('sortEnd', function (e) {
                var $columnHeader = jq('#ordersTable > thead > tr > th[class*="headerSortUp"], #ordersTable > thead > tr > th[class*="headerSortDown"]').eq(0);
                var sortBy = $columnHeader.attr('data-sort-by');

                if (sortBy) {
                    if ($columnHeader.hasClass('headerSortUp'))
                        $scope.ordersSortBy = sortBy + 'Asc';
                    else
                        $scope.ordersSortBy = sortBy + 'Desc';
                }

                loadOrdersByCustomer();
            });
            jq('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true,
            });

            loadJuridicalPersons();
            loadNaturalPersons();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('OrdersAddController', OrdersAddController);

    OrdersAddController.$inject = ['$scope', '$rootScope', '$routeParams', '$location', 'dataService', 'notificationService', 'pagerService'];

    function OrdersAddController($scope, $rootScope, $routeParams, $location, dataService, notificationService, pagerService) {
        $scope.products = [];
        $scope.orderItems = [];
        $scope.totalCost = 0;
        $scope.loadProducts = loadProducts;
        $scope.addOrder = addOrder;
        $scope.addProductToCart = addProductToCart;
        $scope.removeOrderItem = removeOrderItem;
        $scope.calculateTotalCost = calculateTotalCost;
        $scope.pageSize = 5;
        $scope.pager = {};
        activate();

        function calculateTotalCost() {
            var totalCost = 0;

            jQuery.each($scope.orderItems, function (inx, orderItem) {
                if (orderItem.quantity !== undefined)
                    totalCost += orderItem.quantity * orderItem.price;
            });

            $scope.totalCost = totalCost;
        }

        function addProductToCart(inx) {
            var selectedProduct = $scope.products[inx];
            var selectedOrderItem = {
                productId: selectedProduct.id,
                quantity: 1,
                productName: selectedProduct.productName,
                price: selectedProduct.unitPrice
            };

            var ordered = false;

            jQuery.each($scope.orderItems, function (inx, orderItem) {
                if (selectedOrderItem.productId === orderItem.productId) {
                    ordered = true;

                    return false;
                }    
            });

            if (!ordered) {
                $scope.totalCost += selectedOrderItem.price;
                $scope.orderItems.push(selectedOrderItem);
            }
            else {
                notificationService.displayWarning('The product added.');
            }
        }

        function removeOrderItem(inx) {
            $scope.orderItems.splice(inx, 1);
            calculateTotalCost();
        }

        function addOrder() {
            var orderData = {};

            orderData.customerId = $routeParams.id;
            orderData.orderDetails = [];
            
            jQuery.each($scope.orderItems, function (inx, orderItem) {
                orderData.orderDetails.push({
                    productId: orderItem.productId,
                    quantity: orderItem.quantity
                });
            });
            
            dataService.post('/api/v1/orders/add', orderData, addOrderSucceeded, addOrderFaild);
        }

        function addOrderSucceeded(response) {
            notificationService.displaySuccess('Order has been successfu created.');
            $location.path('/orders');
        }

        function addOrderFaild(response) {
            notificationService.displayError('Unauthorised actions detected.');
        }

        function loadProducts(pageNumber) {
            var pageSize = $scope.pageSize || 5;

            pageNumber = pageNumber || 1;

            dataService.get('/api/v1/products', {
                params: {
                    pageSize: pageSize,
                    pageNumber: pageNumber
                }
            }, loadProductsSucceeded, loadProductsFailed);
        }

        function loadProductsSucceeded(response) {
            $scope.products = response.data.products;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.pager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadProductsFailed(response) {
            notificationService.displayError("Products haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function activate() {
            loadProducts();
        }
    }
})(angular.module('app.core'));

(function (app_core) {
    'use strict';

    app_core.controller('OrdersEditController', OrdersEditController);

    OrdersEditController.$inject = ['$scope', '$rootScope', '$routeParams', '$location', 'dataService', 'notificationService', 'pagerService'];

    function OrdersEditController($scope, $rootScope, $routeParams, $location, dataService, notificationService, pagerService) {
        $scope.products = [];
        $scope.orderItems = [];
        $scope.totalCost = 0;
        $scope.loadProducts = loadProducts;
        $scope.editOrder = editOrder;
        $scope.addProductToCart = addProductToCart;
        $scope.removeOrderItem = removeOrderItem;
        $scope.calculateTotalCost = calculateTotalCost;
        $scope.pageSize = 5;
        $scope.pager = {};
        activate();

        function calculateTotalCost() {
            var totalCost = 0;

            jQuery.each($scope.orderItems, function (inx, orderItem) {
                if (orderItem.quantity !== undefined)
                    totalCost += orderItem.quantity * orderItem.price;
            });

            $scope.totalCost = totalCost;
        }

        function addProductToCart(inx) {
            var selectedProduct = $scope.products[inx];
            var selectedOrderItem = {
                productId: selectedProduct.id,
                quantity: 1,
                productName: selectedProduct.productName,
                price: selectedProduct.unitPrice
            };

            var ordered = false;

            jQuery.each($scope.orderItems, function (inx, orderItem) {
                if (selectedOrderItem.productId === orderItem.productId) {
                    ordered = true;

                    return false;
                }
            });

            if (!ordered) {
                $scope.totalCost += selectedOrderItem.price;
                $scope.orderItems.push(selectedOrderItem);
            }
            else {
                notificationService.displayWarning("The product added.");
            }
        }

        function removeOrderItem(inx) {
            $scope.orderItems.splice(inx, 1);
            calculateTotalCost();
        }

        function loadProducts(pageNumber) {
            var pageSize = $scope.pageSize || 5;

            pageNumber = pageNumber || 1;

            dataService.get('/api/v1/products', {
                params: {
                    pageSize: pageSize,
                    pageNumber: pageNumber
                }
            }, loadProductsSucceeded, loadProductsFailed);
        }

        function loadProductsSucceeded(response) {
            $scope.products = response.data.products;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.pager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadProductsFailed(response) {
            notificationService.displayError("Products haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadOrderById(orderId) {
            dataService.get('/api/v1/orders/details/' + orderId, null, loadOrderByIdSucceeded, loadOrderByIdFailed);
        }

        function loadOrderByIdSucceeded(response) {
            var orderDetails = response.data.orderDetails;
            var orderItems = [];

            jQuery.each(orderDetails, function (inx, orderDetail) {
                orderItems.push({
                    productId: orderDetail.productId,
                    productName: orderDetail.productName,
                    quantity: orderDetail.quantity,
                    price: orderDetail.unitPrice
                });
            });

            $scope.orderItems = orderItems;
        }

        function loadOrderByIdFailed(response) {
            notificationService.displayError("Order hasn't been loaded. Please try again later.");
            $location.path('/');
        }

        function editOrder() {
            var orderData = {};

            orderData.id = $routeParams.orderId;
            orderData.customerId = $routeParams.customerId;
            orderData.orderDetails = [];

            jQuery.each($scope.orderItems, function (inx, orderItem) {
                orderData.orderDetails.push({
                    productId: orderItem.productId,
                    quantity: orderItem.quantity
                });
            });

            dataService.post('/api/v1/orders/update', orderData, editOrderSucceeded, editOrderFaild);
        }

        function editOrderSucceeded(response) {
            notificationService.displaySuccess("Order has been successfu created.");
            $location.path('/orders');
        }

        function editOrderFaild(response) {
            notificationService.displayError("Unauthorised actions detected.");
        }

        function activate() {
            loadOrderById($routeParams.orderId);
            loadProducts();
        }
    }
})(angular.module('app.core'));

(function(app_core) {
    'use strict';

    app_core.directive('compareTo', compareTo);
    
    function compareTo () {
        var directive = {
            link: link,
            restrict: 'A',
            require: 'ngModel',
            scope: {
                otherValue: '=compareTo'
            }
        };

        return directive;

        function link(scope, element, attrs, ngModelCtrl) {
            ngModelCtrl.$validators.compareTo = function (modelValue) {
                return modelValue === scope.otherValue;
            };

            scope.$watch('otherValue', function () {
                ngModelCtrl.$validate();
            });
        }
    }
})(angular.module('app.core'));
