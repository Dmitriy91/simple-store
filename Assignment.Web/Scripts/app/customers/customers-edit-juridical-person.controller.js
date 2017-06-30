(function (app_core) {
    'use strict';

    app_core.controller('CustomersEditJuridicalPersonController', CustomersEditJuridicalPersonController);

    CustomersEditJuridicalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersEditJuridicalPersonController($scope, $routeParams, $location, dataService, notificationService) {
        $scope.juridicalPerson = {};
        $scope.juridicalPerson.id = '';
        $scope.juridicalPerson.legalName = '';
        $scope.juridicalPerson.tin = '';
        $scope.juridicalPerson.country = '';
        $scope.juridicalPerson.region = '';
        $scope.juridicalPerson.city = '';
        $scope.juridicalPerson.streetAddress = '';
        $scope.juridicalPerson.postalCode = '';
        $scope.juridicalPerson.update = updateJuridicalPerson;
        activate();

        function updateJuridicalPerson() {
            var juridicalPersonData = {};

            juridicalPersonData.id = $scope.juridicalPerson.id;
            juridicalPersonData.legalName = $scope.juridicalPerson.legalName;
            juridicalPersonData.tin = $scope.juridicalPerson.tin;
            juridicalPersonData.country = $scope.juridicalPerson.country;
            juridicalPersonData.region = $scope.juridicalPerson.region;
            juridicalPersonData.city = $scope.juridicalPerson.city;
            juridicalPersonData.streetAddress = $scope.juridicalPerson.streetAddress;
            juridicalPersonData.postalCode = $scope.juridicalPerson.postalCode;
            dataService.post('/api/v1/customers/juridical-person/update/', juridicalPersonData, updateJuridicalPersonSucceeded, updateJuridicalPersonFaild);
        }

        function updateJuridicalPersonSucceeded(response) {
            notificationService.displaySuccess('Customer has been successfu updated.');
            $location.path('/customers');
        }

        function updateJuridicalPersonFaild(response) {
            notificationService.displayError('Unauthorised actions detected.');
        }

        function loadJuridicalPerson() {
            dataService.get('/api/v1/customers/juridical-person/' + $routeParams.id, null, loadJuridicalPersonSucceeded, loadJuridicalPersonFailed);
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
            notificationService.displayError('Unauthrized actions detected.');
            $location.path('/customers');
        }

        function activate() {
            loadJuridicalPerson();
            jQuery('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayBtn: "linked",
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true
            });
        }
    }
})(angular.module('app.core'));
