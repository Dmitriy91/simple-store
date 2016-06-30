(function (app_core) {
    'use strict';

    app_core.controller('CustomersEditNaturalPersonController', CustomersEditNaturalPersonController);

    CustomersEditNaturalPersonController.$inject = ['$scope', '$routeParams', '$location', 'dataService', 'notificationService'];

    function CustomersEditNaturalPersonController($scope, $routeParams, $location, dataService, notificationService) {
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
        $scope.naturalPerson.update = updateNaturalPerson;
        activate();

        function updateNaturalPerson() {
            var naturalPersonData = {};

            naturalPersonData.id = $scope.naturalPerson.id;
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
            dataService.post('/api/customers/natural-person/update/' + naturalPersonData.id, naturalPersonData, updateNaturalPersonSucceeded, updateNaturalPersonFaild);
        }

        function updateNaturalPersonSucceeded(response) {
            notificationService.displaySuccess("Customer has been successfu updated.");
            $location.path('/customers');
        }

        function updateNaturalPersonFaild(response) {
            notificationService.displayError("Unauthorised actions detected.");
        }

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
            notificationService.displayError("Unauthrized actions detected.");
            $location.path('/customers');
        }

        function activate() {
            loadNaturalPerson();
            jQuery(".datepicker").datepicker({
                forceParse: true,
                format: "yyyy-mm-dd",
                todayBtn: "linked",
                todayHighlight: true,
                daysOfWeekHighlighted: "0,6",
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true
            });
        }
    }
})(angular.module('app.core'));
