(function (app_core) {
    'use strict';

    app_core.factory('membershipService', membershipService);

    membershipService.$inject = ['$http', '$rootScope','$window', 'dataService', 'notificationService'];

    function membershipService($http, $rootScope, $window, dataService, notificationService) {
        var service = {
            register: register,
            logIn: logIn,
            logOut: logOut,
            restoreCachedCredentials: restoreCachedCredentials,
            isLoggedIn: isLoggedIn
        };

        return service;

        function register(user, succeeded, failed) {
            dataService.post('/api/account/register', user, succeeded, failed);
        }

        function logIn(user, succeeded, failed) {
            dataService.post('/Token',
                            jQuery.param(user), //query string data
                            function (response) { //success
                                var loggedUserData = {};

                                loggedUserData.login = response.data.userName.split('@')[0];
                                loggedUserData.authHeader = 'Bearer ' + response.data.access_token;

                                $http.defaults.headers.common['Authorization'] = loggedUserData.authHeader;
                                $window.sessionStorage.setItem('loggedUserData', JSON.stringify(loggedUserData));
                                succeeded(response);
                            },
                            failed, //error
                            { 'Content-Type': 'application/x-www-form-urlencoded' }); //headers    
        }

        function logOut() {
            $http.defaults.headers.common['Authorization'] = '';
            $window.sessionStorage.removeItem('loggedUserData');
        }

        function restoreCachedCredentials()
        {
            var loggedUserDataStr = $window.sessionStorage.getItem('loggedUserData');

            if (loggedUserDataStr !== null) {
                var loggedUserData = JSON.parse(loggedUserDataStr);
                $http.defaults.headers.common['Authorization'] = loggedUserData.authHeader;
                
                return loggedUserData.login;
            }

            return null;
        }

        function isLoggedIn()
        {
            var loggedUserDataStr = $window.sessionStorage.getItem('loggedUserData');

            return loggedUserDataStr !== null;
        }
    }
})(angular.module('app.core'));
