(function (app_core) {
    'use strict';

    app_core.controller('ProductsController', ProductsController);

    ProductsController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService', 'pagerService'];

    function ProductsController($scope, $route, $rootScope, $location, dataService, notificationService, pagerService) {
        $scope.products = []; 
        $scope.removeProduct = removeProduct;
        $scope.loadProducts = loadProducts;
        $scope.pageSize = 5;
        $scope.pager = {};
        activate();

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

        function removeProduct(inx) {
            var productId = $scope.products[inx].id;

            dataService.post('/api/products/delete/' + productId, null, removeProductSucceeded, removeProductFailed);
        }

        function removeProductFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeProductSucceeded(response) {
            $route.reload();
            notificationService.displaySuccess('Product has been successfuly removed.');
        }

        function activate() {
            loadProducts();
        }
    }
})(angular.module('app.core'));
