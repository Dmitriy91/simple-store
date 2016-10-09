(function (app_core) {
    'use strict';

    app_core.controller('OrdersEditController', OrdersEditController);

    OrdersEditController.$inject = ['$scope', '$rootScope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function OrdersEditController($scope, $rootScope, $routeParams, $location, dataService, notificationService) {
        $scope.products = [];
        $scope.orderItems = [];
        $scope.totalCost = 0;
        $scope.editOrder = editOrder;
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

        function loadProducts() {
            dataService.get('/api/products', null, loadProductsSucceeded, loadProductsFailed);
        }

        function loadProductsSucceeded(response) {
            $scope.products = response.data.products;
        }

        function loadProductsFailed(response) {
            notificationService.displayError("Products haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadOrderById(orderId) {
            dataService.get('/api/orders/details/' + orderId, null, loadOrderByIdSucceeded, loadOrderByIdFailed);
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

            dataService.post('/api/orders/update', orderData, editOrderSucceeded, editOrderFaild);
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
