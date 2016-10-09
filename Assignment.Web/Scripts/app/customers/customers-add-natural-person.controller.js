(function (app_core) {
    'use strict';

    app_core.controller('CustomersAddNaturalPersonController', CustomersAddNaturalPersonController);

    CustomersAddNaturalPersonController.$inject = ['$scope', '$rootScope', '$location', 'dataService', 'notificationService'];

    function CustomersAddNaturalPersonController($scope, $rootScope, $location, dataService, notificationService) {
        $scope.naturalPerson = {};
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
        $scope.naturalPerson.add = addNaturalPerson;
        activate();

        function addNaturalPerson() {
            var naturalPersonData = {};

            naturalPersonData.firstName = $scope.naturalPerson.firstName;
            naturalPersonData.lastName = $scope.naturalPerson.lastName;
            naturalPersonData.middleName = $scope.naturalPerson.middleName;
            naturalPersonData.ssn = $scope.naturalPerson.ssn;
            naturalPersonData.birthdate = $scope.naturalPerson.birthdate;
            naturalPersonData.country = $scope.naturalPerson.country;
            naturalPersonData.region = $scope.naturalPerson.region;
            naturalPersonData.city = $scope.naturalPerson.city;
            naturalPersonData.streetAddress = $scope.naturalPerson.streetAddress;
            naturalPersonData.postalCode = $scope.naturalPerson.postalCode;

            dataService.post('/api/customers/natural-person/add', naturalPersonData, addNaturalPersonSucceeded, addNaturalPersonFaild);
        }

        function addNaturalPersonSucceeded(response) {
            notificationService.displaySuccess("Customer has been successfu created.");
            $location.path('/customers');
        }

        function addNaturalPersonFaild(response) {
            notificationService.displayError("This person has been already added.");
        }

        function activate() {
            jQuery(".datepicker").datepicker({
                forceParse: true,
                format: "yyyy-mm-dd",
                todayHighlight: true,
                daysOfWeekHighlighted: "0,6",
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true,
            });
        }
    }
})(angular.module('app.core'));
