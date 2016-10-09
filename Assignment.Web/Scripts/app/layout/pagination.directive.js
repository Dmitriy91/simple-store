﻿(function (app_layout) {
    'use strict';

    app_layout.directive('pagination', pagination);

    function pagination() {
        var directive = {
            restrict: 'E',
            transclude: true,
            scope: {
                'loadItems': '&',
                'pager': '=',
                'size': '='
            },
            templateUrl: '/Scripts/app/layout/pagination.html'
        }

        return directive;
    }
})(angular.module('app.layout'));
