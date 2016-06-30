(function (app_core) {
    'use strict';

    app_core.controller('IndexController', IndexController);

    IndexController.$inject = ['$scope', 'dataService', 'notificationService'];

    function IndexController($scope, dataService, notificationService) {

    }
})(angular.module('app.core'));
