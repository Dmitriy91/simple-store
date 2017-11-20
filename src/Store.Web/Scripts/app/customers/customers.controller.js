(function (app_core) {
    'use strict';

    app_core.controller('CustomersController', CustomersController);

    CustomersController.$inject = ['$scope', '$route', '$rootScope', '$location', 'dataService', 'notificationService', 'pagerService'];

    function CustomersController($scope, $route, $rootScope, $location, dataService, notificationService, pagerService) {
        $scope.naturalPersons = [];
        $scope.juridicalPersons = [];

        $scope.loadNaturalPersons = loadNaturalPersons;
        $scope.loadJuridicalPersons = loadJuridicalPersons;

        $scope.removeNaturalPerson = removeNaturalPerson;
        $scope.removeJuridicalPerson = removeJuridicalPerson;

        $scope.naturalPersonsPager = {};
        $scope.juridicalPersonsPager = {};

        $scope.naturalPersonFilters = [
            {
                name:'firstName',
                value: ''
            },
            {
                name: 'middleName',
                value: ''
            },
            {
                name: 'lastName',
                value: ''
            },
            {
                name: 'ssn',
                value: ''
            },
            {
                name: 'birthdate',
                value: ''
            },
            {
                name: 'country',
                value: ''
            },
            {
                name: 'region',
                value: ''
            },
            {
                name: 'city',
                value: ''
            },
            {
                name: 'streetAddress',
                value: ''
            },
            {
                name: 'postalCode',
                value: ''
            }
        ];
        $scope.juridicalPersonFilters = [
            {
                name: 'legalName',
                value: ''
            },
            {
                name: 'tin',
                value: ''
            },
            {
                name: 'country',
                value: ''
            },
            {
                name: 'region',
                value: ''
            },
            {
                name: 'city',
                value: ''
            },
            {
                name: 'streetAddress',
                value: ''
            },
            {
                name: 'postalCode',
                value: ''
            }
        ];
        $scope.filtersDisplayed = false;
        $scope.clearFilters = clearFilters;
        $scope.toggleFilters = toggleFilters;

        $scope.naturalPersonsSortBy = '';
        $scope.juridicalPersonsSortBy = '';

        $scope.pageSize = 5;
        $scope.customerType = 'naturalPerson';

        activate();

        function loadJuridicalPersons(pageNumber) {
            pageNumber = pageNumber || 1;

            var pageSize = $scope.pageSize || 5;
            var queryParams = {
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            if ($scope.juridicalPersonsSortBy)
                queryParams['sortBy'] = $scope.juridicalPersonsSortBy;

            angular.forEach($scope.juridicalPersonFilters, function (filter) {
                if (filter && filter.value)
                    queryParams[filter.name] = filter.value;
            });

            dataService.get('/api/v1/customers/juridical-persons', {
                params: queryParams
            }, loadJuridicalPersonsSucceeded, loadJuridicalPersonsFailed);
        }

        function loadNaturalPersons(pageNumber) {
            pageNumber = pageNumber || 1;

            var pageSize = $scope.pageSize || 5;
            var queryParams = {
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            if ($scope.naturalPersonsSortBy)
                queryParams['sortBy'] = $scope.naturalPersonsSortBy;

            angular.forEach($scope.naturalPersonFilters, function (filter) {
                if (filter && filter.value)
                    queryParams[filter.name] = filter.value;
            });

            dataService.get('/api/v1/customers/natural-persons', {
                params: queryParams
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
            if (response.data.naturalPersons !== null) {
                response.data.naturalPersons.forEach(function (person) {
                    person.birthdate = person.birthdate !== null ? person.birthdate.split('T')[0] : '';
                });
            }

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

            dataService.post('/api/v1/customers/delete/' + personId, null, removeJuridicalPersonSucceeded, removeJuridicalPersonFailed);
        }

        function removeJuridicalPersonFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeJuridicalPersonSucceeded(response) {
            loadJuridicalPersons();
            notificationService.displaySuccess('Customer has been successfully removed.');
        }

        function removeNaturalPerson(inx) {
            var personId = $scope.naturalPersons[inx].id;

            dataService.post('/api/v1/customers/delete/' + personId, null, removeNaturalPersonSucceeded, removeNaturalPersonFailed);
        }

        function removeNaturalPersonFailed(response) {
            notificationService.displayError('Unauthorised action detected.');
        }

        function removeNaturalPersonSucceeded(response) {
            loadNaturalPersons();
            notificationService.displaySuccess('Customer has been successfully removed.');
        }

        function clearFilters() {
            if ($scope.customerType === 'naturalPerson') {
                var updateNaturalPersons = false;

                angular.forEach($scope.naturalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateNaturalPersons = true;
                    }
                });

                if (updateNaturalPersons)
                    loadNaturalPersons();
            }
            else {
                var updateJuridicalPersons = false;

                angular.forEach($scope.juridicalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateJuridicalPersons = true;
                    }
                });

                if (updateJuridicalPersons)
                    loadJuridicalPersons();
            }
        }

        function toggleFilters() {
            if ($scope.filtersDisplayed) {
                var updateNaturalPersons = false;
                var updateJuridicalPersons = false;

                $scope.filtersDisplayed = false;
                angular.forEach($scope.naturalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateNaturalPersons = true;
                    }
                });
                angular.forEach($scope.juridicalPersonFilters, function (filter) {
                    if (filter && filter.value) {
                        filter.value = '';
                        updateJuridicalPersons = true;
                    }
                });

                if (updateJuridicalPersons)
                    loadJuridicalPersons();

                if (updateNaturalPersons)
                    loadNaturalPersons();
            }
            else {
                $scope.filtersDisplayed = true;
            }
        }

        function activate() {
            var jq = jQuery.noConflict();

            jq('#naturalPersonsTable').tablesorter({
                headers: {
                    10: {
                        // disable sorting for the 11-th column
                        sorter: false
                    }
                }
            });
            jq('#juridicalPersonsTable').tablesorter({
                headers: {
                    7: {
                        // disable sorting for the 8-th column
                        sorter: false
                    }
                }
            });
            jq('#naturalPersonsTable').bind('sortEnd', function (e) {
                var $columnHeader = jq('#naturalPersonsTable > thead > tr > th[class*="headerSortUp"], #naturalPersonsTable > thead > tr > th[class*="headerSortDown"]').eq(0);
                var sortBy = $columnHeader.attr('data-sort-by');

                if (sortBy) {
                    if ($columnHeader.hasClass('headerSortUp'))
                        $scope.naturalPersonsSortBy = sortBy + 'Asc';
                    else
                        $scope.naturalPersonsSortBy = sortBy + 'Desc';
                }

                loadNaturalPersons();
            });
            jq('#juridicalPersonsTable').bind('sortEnd', function (e) {
                var $columnHeader = jq('#juridicalPersonsTable > thead > tr > th[class*="headerSortUp"], #juridicalPersonsTable > thead > tr > th[class*="headerSortDown"]').eq(0);
                var sortBy = $columnHeader.attr('data-sort-by');

                if (sortBy) {
                    if ($columnHeader.hasClass('headerSortUp'))
                        $scope.juridicalPersonsSortBy = sortBy + 'Asc';
                    else
                        $scope.juridicalPersonsSortBy = sortBy + 'Desc';
                }

                loadJuridicalPersons();
            });
            jq('.datepicker').datepicker({
                forceParse: true,
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                daysOfWeekHighlighted: '0,6',
                calendarWeeks: true,
                weekStart: 1,
                autoclose: true,
                clearBtn: true,
            });

            loadJuridicalPersons();
            loadNaturalPersons();
        }
    }
})(angular.module('app.core'));
