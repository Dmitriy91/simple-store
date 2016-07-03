(function (app_core) {
    'use strict';

    app_core.controller('OrdersAddController', OrdersAddController);

    OrdersAddController.$inject = ['$scope', '$rootScope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function OrdersAddController($scope, $rootScope, $routeParams, $location, dataService, notificationService) {
        $scope.products = [];
        $scope.orderItems = [];
        $scope.totalCost = 0;
        $scope.addOrder = addOrder;
        $scope.addProductToCart = addProductToCart;
        $scope.removeOrderItem = removeOrderItem;
        $scope.calculateTotalCost = calculateTotalCost;
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
            
            dataService.post('/api/orders/add', orderData, addOrderSucceeded, addOrderFaild);
        }

        function addOrderSucceeded(response) {
            notificationService.displaySuccess("Order has been successfu created.");
            $location.path('/orders');
        }

        function addOrderFaild(response) {
            notificationService.displayError("Unauthorised actions detected.");
        }

        function loadProducts() {
            dataService.get('/api/products', null, loadProductsSucceeded, loadProductsFailed);
        }

        function loadProductsSucceeded(response) {
            $scope.products = response.data;
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
