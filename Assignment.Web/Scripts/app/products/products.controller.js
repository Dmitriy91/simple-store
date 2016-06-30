(function (app_core) {
    'use strict';

    app_core.controller('ProductsController', ProductsController);

    ProductsController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService'];

    function ProductsController($scope, $route, $rootScope, $location, dataService, notificationService) {
        $scope.products = [];
        $scope.removeProduct = removeProduct;
        activate();

        function loadProducts() {
            dataService.get('/api/products', null, loadProductsSucceeded, loadProductsFailed)
        }

        function loadProductsSucceeded(response) {
            $scope.products = response.data;
        }

        function loadProductsFailed(response) {
            notificationService.displayError("Products haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function removeProduct(inx) {
            var productId = $scope.products[inx].id;

            dataService.post('/api/products/delete/' + productId, null, removeProductSucceeded, removeProductFailed)
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
