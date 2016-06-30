(function (app) {
    'use strict';

    app.controller('RootController', RootController);

    RootController.$inject = ['$scope', '$location', '$rootScope', 'membershipService'];

    function RootController($scope, $location, $rootScope, membershipService) {
        $scope.user = {}; 
        $scope.user.logOut = logOut;
        $scope.user.loggedIn = false;
        $scope.user.login = '';

        function logOut() {
            membershipService.logOut();
            $location.path('#/');
            $scope.user.loggedIn = false;
            $scope.user.email = '';
        }

        $scope.$on('userLoggedIn', function (event, login) {
            $scope.user.login = login;
            $scope.user.loggedIn = true;
        });

        $scope.$on('$viewContentLoaded', function () {
            // handle pages' refreshes
            var login = membershipService.restoreCachedCredentials()

            if (login !== null)
                $rootScope.$broadcast('userLoggedIn', login);
        });
    }
})(angular.module('app'));
