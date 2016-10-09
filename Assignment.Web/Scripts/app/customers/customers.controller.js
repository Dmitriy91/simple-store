(function (app_core) {
    'use strict';

    app_core.controller('CustomersController', CustomersController);

    CustomersController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService', 'pagerService'];

    function CustomersController($scope, $route, $rootScope, $location, dataService, notificationService, pagerService) {
        $scope.customerType = 'naturalPerson';
        $scope.naturalPersons = [];
        $scope.juridicalPersons = [];
        $scope.loadNaturalPersons = loadNaturalPersons;
        $scope.loadJuridicalPersons = loadJuridicalPersons;
        $scope.removeNaturalPerson = removeNaturalPerson;
        $scope.removeJuridicalPerson = removeJuridicalPerson;
        $scope.pageSize = 5;
        $scope.naturalPersonsPager = {};
        $scope.juridicalPersonsPager = {};
        activate();

        function loadJuridicalPersons(pageNumber) {
            var pageSize = $scope.pageSize || 5;

            pageNumber = pageNumber || 1;

            dataService.get('/api/customers/juridical-persons', {
                params: {
                    pageSize: pageSize,
                    pageNumber: pageNumber
                }
            }, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
        }

        function loadNaturalPersons(pageNumber) {
            var pageSize = $scope.pageSize || 5;

            pageNumber = pageNumber || 1;

            dataService.get('/api/customers/natural-persons', {
                params: {
                    pageSize: pageSize,
                    pageNumber: pageNumber
                }
            }, loadNaturalPersonsSucceeded, loadNaturalPersonsFailed);
        }

        function loadJuridicalPersonsSucceeded(response) {
            $scope.juridicalPersons = response.data.juridicalPersons;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.juridicalPersonsPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
        }

        function loadJuridicalPersonsFailed(response) {
            notificationService.displayError("Juridical persons haven't been loaded. Please try again later.");
            $location.path('/');
        }

        function loadNaturalPersonsSucceeded(response) {
            $scope.naturalPersons = response.data.naturalPersons;
            var currentPage = response.data.pagingInfo.currentPage;
            var pageSize = response.data.pagingInfo.pageSize;
            var totalItems = response.data.pagingInfo.totalItems;
            var totalPages = response.data.pagingInfo.totalPages;

            $scope.naturalPersonsPager = pagerService.GetPager(totalItems, totalPages, currentPage, pageSize);
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
