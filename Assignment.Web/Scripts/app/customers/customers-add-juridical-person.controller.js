(function (app_core) {
    'use strict';

    app_core.controller('CustomersAddJuridicalPersonController', CustomersAddJuridicalPersonController);

    CustomersAddJuridicalPersonController.$inject = ['$scope', '$rootScope', '$location', 'dataService', 'notificationService'];

    function CustomersAddJuridicalPersonController($scope, $rootScope, $location, dataService, notificationService) {
        $scope.juridicalPerson = {};
        $scope.juridicalPerson.legalName = '';
        $scope.juridicalPerson.tin = '';
        $scope.juridicalPerson.country = '';
        $scope.juridicalPerson.region = '';
        $scope.juridicalPerson.city = '';
        $scope.juridicalPerson.streetAddress = '';
        $scope.juridicalPerson.postalCode = '';
        $scope.juridicalPerson.add = addJuridicalPerson;
        activate();

        function addJuridicalPerson() {
            var juridicalPersonData = {};

            juridicalPersonData.legalName = $scope.juridicalPerson.legalName;
            juridicalPersonData.tin = $scope.juridicalPerson.tin;
            juridicalPersonData.country = $scope.juridicalPerson.country;
            juridicalPersonData.region = $scope.juridicalPerson.region;
            juridicalPersonData.city = $scope.juridicalPerson.city;
            juridicalPersonData.streetAddress = $scope.juridicalPerson.streetAddress;
            juridicalPersonData.postalCode = $scope.juridicalPerson.postalCode;

            dataService.post('/api/customers/juridical-person/add', juridicalPersonData, addJuridicalPersonSucceeded, addJuridicalPersonFaild);
        }

        function addJuridicalPersonSucceeded(response) {
            notificationService.displaySuccess('Customer has been successfu created.');
            $location.path('/customers');
        }

        function addJuridicalPersonFaild(response) {
            notificationService.displayError('This person has been already added.');
        }

        function activate() {
            jQuery('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayBtn: 'linked',
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true,
            });
        }
    }
})(angular.module('app.core'));
