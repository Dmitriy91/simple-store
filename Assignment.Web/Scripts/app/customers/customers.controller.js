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

        function loadJuridicalPersons() {
            dataService.get('/api/customers/juridical-persons', null, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
        }

        function loadNaturalPersons() {
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
            var personId = $scope.juridicalPersons[inx].id;

            dataService.post('/api/customers/delete/' + personId, null, removeJuridicalPersonSucceeded, removeJuridicalPersonFailed);
        }

        function removeJuridicalPersonFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeJuridicalPersonSucceeded(response) {
            loadJuridicalPersons();
            notificationService.displaySuccess('Customer has been successfuly removed.');
        }

        function removeNaturalPerson(inx) {
            var personId = $scope.naturalPersons[inx].id;

            dataService.post('/api/customers/delete/' + personId, null, removeNaturalPersonSucceeded, removeNaturalPersonFailed);
        }

        function removeNaturalPersonFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeNaturalPersonSucceeded(response) {
            loadNaturalPersons();
            notificationService.displaySuccess('Customer has been successfuly removed.');
        }

        function activate() {
            loadJuridicalPersons();
            loadNaturalPersons();
        }
    }
})(angular.module('app.core'));
