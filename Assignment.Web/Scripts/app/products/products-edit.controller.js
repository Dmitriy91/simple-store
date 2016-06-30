(function (app_core) {
    'use strict';

    app_core.controller('ProductsEditController', ProductsEditController);

    ProductsEditController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function ProductsEditController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.product = {};
        $scope.product.id = '';
        $scope.product.productName = '';
        $scope.product.unitPrice = '';
        $scope.product.unitsInStock = '';
        $scope.product.update = updateProduct;
        activate();

        function updateProduct() {
            var productData = {};

            productData.id = $scope.product.id;
            productData.productName = $scope.product.productName;
            productData.unitPrice = $scope.product.unitPrice;
            productData.unitsInStock = $scope.product.unitsInStock;
            dataService.post('/api/products/update/' + productData.id, productData, updateProductSucceeded, updateProductFaild);
        }

        function updateProductSucceeded(response) {
            notificationService.displaySuccess("Product has been successfu updated.");
            $location.path('/products');
        }

        function updateProductFaild(response) {
            notificationService.displayError("Unauthorised actions detected.");
        }

        function loadProduct() {
            dataService.get('/api/products/details/' + $routeParams.id, null, loadProductSucceeded, loadProductFailed);
        }

        function loadProductSucceeded(response) {
            $scope.product.id = response.data.id;
            $scope.product.productName = response.data.productName;
            $scope.product.unitPrice = response.data.unitPrice;
            $scope.product.unitsInStock = response.data.unitsInStock;
        }

        function loadProductFailed(response) {
            notificationService.displayError("Unauthrized actions detected.");
            $location.path('/products');
        }

        function activate() {
            loadProduct();
        }
    }
})(angular.module('app.core'));
