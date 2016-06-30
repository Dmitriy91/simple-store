(function (app_core) {
    'use strict';

    app_core.controller('ProductsAddController', ProductsAddController);

    ProductsAddController.$inject = ['$scope', '$rootScope', '$location', 'dataService', 'notificationService'];

    function ProductsAddController($scope, $rootScope, $location, dataService, notificationService) {
        $scope.product = {};
        $scope.product.productName = '';
        $scope.product.unitPrice = '';
        $scope.product.unitsInStock = '';
        $scope.product.add = addProduct;

        function addProduct() {
            var productData = {};

            productData.productName = $scope.product.productName;
            productData.unitPrice = $scope.product.unitPrice;
            productData.unitsInStock = $scope.product.unitsInStock;
            dataService.post('/api/products/add', productData, addProductSucceeded, addProductFaild);
        }

        function addProductSucceeded(response) {
            notificationService.displaySuccess("Product has been successfu created.");
            $location.path('/products');
        }

        function addProductFaild(response) {
            notificationService.displayError("Unauthorised actions detected.");
        }
    }
})(angular.module('app.core'));
