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

            dataService.get('/api/products', {
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
