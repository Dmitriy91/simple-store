(function (app_core) {
    'use strict';

    app_core.controller('ProductsDetailsController', ProductsDetailsController);

    ProductsDetailsController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function ProductsDetailsController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.product = {};
        $scope.product.id = '';
        $scope.product.productName = '';
        $scope.product.unitPrice = '';
        $scope.product.unitsInStock = '';
        activate();

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
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/products');
        }

        function activate() {
            loadProduct();
        }
    }
})(angular.module('app.core'));
