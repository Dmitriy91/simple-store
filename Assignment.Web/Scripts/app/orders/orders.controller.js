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
        $scope.orderItems = [];

        $scope.selectedCustomerId = 0;
        $scope.selectedOrderId = 0;
        $scope.selectCustomer = selectCustomer;
        $scope.selectOrder = selectOrder;
        $scope.removeOrder = removeOrder;

        $scope.loadNaturalPersons = loadNaturalPersons;
        $scope.loadJuridicalPersons = loadJuridicalPersons;
        $scope.loadJuridicalPersons = loadJuridicalPersons;

        $scope.customersPageSize = 5;
        $scope.orderPageSize = 5;
        $scope.orderDetailsPageSize = 5;

        $scope.naturalPersonsPager = {};
        $scope.juridicalPersonsPager = {};
        activate();

        function clearSelectedCustomerInfo() {
            $scope.selectedCustomerId = 0;
            $scope.selectedOrderId = 0;
            $scope.orders = [];
            $scope.orderItems = [];
        }

        function clearSelectedOrderInfo() {
            $scope.selectedOrderId = 0;
            $scope.orders = [];
            $scope.orderItems = [];
        }

        function selectCustomerType(customerType) {
            clearSelectedCustomerInfo();
            $scope.customerType = customerType;
        }

        function selectCustomer(customerId) {
            clearSelectedCustomerInfo();
            $scope.selectedCustomerId = customerId;
            loadOrdersByCustomerId(customerId);
        }

        function selectOrder(orderId, orderItems) {
            $scope.selectedOrderId = orderId;
            $scope.orderItems = orderItems;
        }

        function removeOrder(inx) {
            var orderId = $scope.orders[inx].id;

            dataService.post('/api/orders/delete/' + orderId, null, removeOrderSucceeded, removeOrderFailed);
        }

        function removeOrderFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeOrderSucceeded(response) {
            var selectedCustomerId = $scope.selectedCustomerId;

            clearSelectedOrderInfo();
            notificationService.displaySuccess('Order has been successfuly removed.');
            loadOrdersByCustomerId(selectedCustomerId);
        }

        function loadOrdersByCustomerId(customerId) {
            dataService.get('/api/orders/' + customerId, null, loadOrdersByCustomerIdSucceeded, loadOrdersByCustomerIdFailed);
        }

        function loadOrdersByCustomerIdSucceeded(response) {
            $scope.orders = response.data.orders;
        }

        function loadOrdersByCustomerIdFailed(response) {
            notificationService.displayError("Orders haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadJuridicalPersons(pageNumber) {
            var pageSize = $scope.pageSize || 5;

            pageNumber = pageNumber || 1;

            dataService.get('/api/customers/juridical-persons', {
                params: {
                    pageSize: pageSize,
                    pageNumber: pageNumber
                }
            }, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
        }

        function loadNaturalPersons(pageNumber) {
            var pageSize = $scope.pageSize || 5;

            pageNumber = pageNumber || 1;

            dataService.get('/api/customers/natural-persons', {
                params: {
                    pageSize: pageSize,
                    pageNumber: pageNumber
                }
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

        function activate() {
            loadJuridicalPersons();
            loadNaturalPersons();
        }
    }
})(angular.module('app.core'));
