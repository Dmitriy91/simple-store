(function (app_core) {
    'use strict';

    app_core.controller('CustomersDetailsNaturalPersonController', CustomersDetailsNaturalPersonController);

    CustomersDetailsNaturalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersDetailsNaturalPersonController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.naturalPerson = {};
        $scope.naturalPerson.id = '';
        $scope.naturalPerson.firstName = '';
        $scope.naturalPerson.middleName = '';
        $scope.naturalPerson.lastName = '';
        $scope.naturalPerson.ssn = '';
        $scope.naturalPerson.birthdate = '';
        $scope.naturalPerson.country = '';
        $scope.naturalPerson.region = '';
        $scope.naturalPerson.city = '';
        $scope.naturalPerson.streetAddress = '';
        $scope.naturalPerson.postalCode = '';
        activate();

        function loadNaturalPerson() {
            dataService.get('/api/customers/natural-person/' + $routeParams.id, null, loadNaturalPersonSucceeded, loadNaturalPersonFailed);
        }

        function loadNaturalPersonSucceeded(response) {
            $scope.naturalPerson.id = response.data.id;
            $scope.naturalPerson.firstName = response.data.firstName;
            $scope.naturalPerson.middleName = response.data.middleName;
            $scope.naturalPerson.lastName = response.data.lastName;
            $scope.naturalPerson.ssn = response.data.ssn;
            $scope.naturalPerson.birthdate = response.data.birthdate;
            $scope.naturalPerson.country = response.data.country;
            $scope.naturalPerson.region = response.data.region;
            $scope.naturalPerson.city = response.data.city;
            $scope.naturalPerson.streetAddress = response.data.streetAddress;
            $scope.naturalPerson.postalCode = response.data.postalCode;
        }

        function loadNaturalPersonFailed(response) {
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/customers');
        }

        function activate() {
            loadNaturalPerson();
        }
    }
})(angular.module('app.core'));
