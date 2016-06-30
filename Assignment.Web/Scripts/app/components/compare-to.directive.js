(function(app_core) {
    'use strict';

    app_core.directive('compareTo', compareTo);
    
    function compareTo () {
        var directive = {
            link: link,
            restrict: 'A',
            require: 'ngModel',
            scope: {
                otherValue: '=compareTo'
            }
        };

        return directive;

        function link(scope, element, attrs, ngModelCtrl) {
            ngModelCtrl.$validators.compareTo = function (modelValue) {
                return modelValue === scope.otherValue;
            };

            scope.$watch('otherValue', function () {
                ngModelCtrl.$validate();
            });
        }
    }
})(angular.module('app.core'));
