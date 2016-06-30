(function (app_core) {
    'use strict';

    app_core.controller('CustomersDetailsJuridicalPersonController', CustomersDetailsJuridicalPersonController);

    CustomersDetailsJuridicalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersDetailsJuridicalPersonController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.juridicalPerson = {};
        $scope.juridicalPerson.id = '';
        $scope.juridicalPerson.legalName = '';
        $scope.juridicalPerson.tin = '';
        $scope.juridicalPerson.country = '';
        $scope.juridicalPerson.region = '';
        $scope.juridicalPerson.city = '';
        $scope.juridicalPerson.streetAddress = '';
        $scope.juridicalPerson.postalCode = '';
        activate();

        function loadJuridicalPerson() {
            dataService.get('/api/customers/juridical-person/' + $routeParams.id, null, loadJuridicalPersonSucceeded, loadJuridicalPersonFailed);
        }

        function loadJuridicalPersonSucceeded(response) {
            $scope.juridicalPerson.id = response.data.id;
            $scope.juridicalPerson.legalName = response.data.legalName;
            $scope.juridicalPerson.tin = response.data.tin;
            $scope.juridicalPerson.country = response.data.country;
            $scope.juridicalPerson.region = response.data.region;
            $scope.juridicalPerson.city = response.data.city;
            $scope.juridicalPerson.streetAddress = response.data.streetAddress;
            $scope.juridicalPerson.postalCode = response.data.postalCode;
        }

        function loadJuridicalPersonFailed(response) {
            notificationService.displayError("Unauthrized actions detected.");
            $location.path('/customers');
        }

        function activate() {
            loadJuridicalPerson();
        }
    }
})(angular.module('app.core'));
