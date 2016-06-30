(function (app_core) {
    'use strict';

    app_core.factory('dataService', dataService);

    dataService.$inject = ['$http', '$location', '$rootScope', 'notificationService'];

    function dataService($http, $location, $rootScope, notificationService) {
        var service = {
            get: get,
            post: post
        };

        return service;

        function get(url, config, success, error) {
            return $http.get(url, config)
                    .then(success, function (response) {
                            unauthorizedAccessHandler(response)

                            if (error != null) error(response);
                    });
        }

        function post(url, data, success, error, headers) {
            return $http.post(url, data, { headers: headers == undefined ? null : headers })
                    .then(success, function (response) {
                            unauthorizedAccessHandler(response)

                            if (error != null) error(response);
                    });
        }

        function unauthorizedAccessHandler(response) {
            if (response.status == '401') {
                notificationService.displayWarning('Authentication required.');
                $rootScope.previousState = $location.path();
                $location.path('/login');
            }
        };
    }
})(angular.module('app.core'));
