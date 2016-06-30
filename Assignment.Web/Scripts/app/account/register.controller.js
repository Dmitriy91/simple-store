(function (app_core) {
    'use strict';

    app_core.controller('RegisterController', RegisterController);

    RegisterController.$inject = ['$scope', '$rootScope', '$location', 'membershipService', 'notificationService'];

    function RegisterController($scope, $rootScope, $location, membershipService, notificationService) {
        $scope.user = {};
        $scope.user.email = 'admin@gmail.com';
        $scope.user.password = 'Admin123!';
        $scope.user.confirmPassword = 'Admin123!';
        $scope.user.register = register;

        function register() {
            membershipService.register($scope.user, registerSucceeded, register)
        }

        function registerSucceeded(response) {
            notificationService.displaySuccess($scope.user.email + ' signed up.');
            $location.path('/login');
        };

        function registerFailed(response) {
            notificationService.displayError('Registration failed. Please try again.');
        };
    }
})(angular.module('app.core'));
