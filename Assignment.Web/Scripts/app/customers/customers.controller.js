(function (app_core) {
    'use strict';

    app_core.controller('CustomersController', CustomersController);

    CustomersController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService'];

    function CustomersController($scope, $route, $rootScope, $location, dataService, notificationService) {
        $scope.customerType = 'naturalPerson';
        $scope.naturalPersons = [];
        $scope.juridicalPersons = [];
        $scope.removeNaturalPerson = removeNaturalPerson;
        $scope.removeJuridicalPerson = removeJuridicalPerson;
        activate();

        function loadCustomers() {
            dataService.get('/api/customers/juridical-persons', null, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
            dataService.get('/api/customers/natural-persons', null, loadNaturalPersonsSucceeded, loadNaturalPersonsFailed);
        }

        function loadJuridicalPersonsSucceeded(response) {
            $scope.juridicalPersons = response.data;
        }

        function loadJuridicalPersonsFailed(response) {
            notificationService.displayError("Juridical persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadNaturalPersonsSucceeded(response) {
            $scope.naturalPersons = response.data;
        }

        function loadNaturalPersonsFailed(response) {
            notificationService.displayError("Natural persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function removeJuridicalPerson(inx) {
            var PersonId = $scope.juridicalPersons[inx].id;

            dataService.post('/api/customers/delete/' + PersonId, null, removePersonSucceeded, removePersonFailed);
        }

        function removeNaturalPerson(inx) {
            var PersonId = $scope.naturalPersons[inx].id;

            dataService.post('/api/customers/delete/' + PersonId, null, removePersonSucceeded, removePersonFailed);
        }

        function removePersonFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removePersonSucceeded(response) {
            $route.reload();
            notificationService.displaySuccess('Customer has been successfuly removed.');
        }

        function activate() {
            loadCustomers();
        }
    }
})(angular.module('app.core'));
