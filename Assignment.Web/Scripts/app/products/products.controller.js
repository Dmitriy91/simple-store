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

            dataService.get('/api/products', {
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

            dataService.post('/api/products/delete/' + productId, null, removeProductSucceeded, removeProductFailed);
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
