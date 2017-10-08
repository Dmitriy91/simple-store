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
