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
            
            dataService.post('/api/orders/add', orderData, addOrderSucceeded, addOrderFaild);
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

        function activate() {
            loadProducts();
        }
    }
})(angular.module('app.core'));
