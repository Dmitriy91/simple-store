(function (app_core) {
    'use strict';

    app_core.controller('LoginController', LoginController);

    LoginController.$inject = ['$scope', '$rootScope', '$location', 'membershipService', 'notificationService'];

    function LoginController($scope, $rootScope, $location, membershipService, notificationService) {
        $scope.user = {};
        $scope.user.logIn = logIn;
        $scope.user.email = 'admin@gmail.com';
        $scope.user.password = 'Admin123!';

        function logIn() {
            var userData = {
                username: $scope.user.email,
                password: $scope.user.password,
                grant_type: 'password'
            };

            membershipService.logIn(userData, loginSucceeded, loginFailed)
        }

        function loginSucceeded(response) {
            notificationService.displaySuccess('Hello ' + $scope.user.email);
            $rootScope.$broadcast('userLoggedIn', $scope.user.email.split('@')[0]);

            var prevLocationPath = $rootScope.prevLocationPath;

            if (prevLocationPath)
                $location.path(prevLocationPath);
            else
                $location.path('/customers');
        }

        function loginFailed(response) {
            notificationService.displayError('Your login or password is incorrect. Please try again.');
        }
    }
})(angular.module('app.core'));
