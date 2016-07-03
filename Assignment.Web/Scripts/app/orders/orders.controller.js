(function (app_core) {
    'use strict';

    app_core.controller('OrdersController', OrdersController);

    OrdersController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService'];

    function OrdersController($scope, $route, $rootScope, $location, dataService, notificationService) {
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
            $scope.orders = response.data;
        }

        function loadOrdersByCustomerIdFailed(response) {
            notificationService.displayError("Orders haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadCustomers() {
            dataService.get('/api/customers/juridical-persons', null, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
            dataService.get('/api/customers/natural-persons', null, loadNaturalPersonsSucceeded, loadNaturalPersonsFailed);
        }

        function loadJuridicalPersonsSucceeded(response) {
            $scope.juridicalPersons = response.data;
        }

        function loadJuridicalPersonsFailed(response) {
            notificationService.displayError("Juridical persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadNaturalPersonsSucceeded(response) {
            $scope.naturalPersons = response.data;
        }

        function loadNaturalPersonsFailed(response) {
            notificationService.displayError("Natural persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function activate() {
            loadCustomers();
        }
    }
})(angular.module('app.core'));
