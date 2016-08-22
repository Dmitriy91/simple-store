(function (app_layout) {
    'use strict';

    app_layout.directive('topBar', topBar);

    function topBar() {
        var directive = {
            link: link,
            restrict: 'E',
            replace: true,
            templateUrl: '/Scripts/app/layout/top-bar.html'
        }

        return directive;

        function link(scope, element) {
            var jq = $.noConflict();

            // highlight a saved theme when the page is loaded
            if (getCookie('theme') !== '') {
                var themeUrl = jq("link[href*='bootstrap']:first").prop('href');
                var themeName = themeUrl.split('/')[5].toLowerCase();
                var normalizedThemeName = '';

                jq('#theme-menu > li').each(function () {
                    normalizedThemeName = jq(this).text().trim().replace(/ /g, '').toLowerCase();

                    if (normalizedThemeName === themeName) {
                        jq(this).addClass('selected-theme');
                        return false;
                    }
                });
            }

            jq('#theme-menu > li').on('click', function (event) {
                // applying a selected theme
                // cross-browser solution
                var target = jq(event.target || event.srcElement || event.originalTarget);
                var uiThemeName = target.text();
                var normalizedThemeName = uiThemeName.trim().replace(/ /g, '').toLowerCase();
                var themeUrl = 'https://maxcdn.bootstrapcdn.com/bootswatch/3.3.7/' + normalizedThemeName + '/bootstrap.min.css';

                jq('link[href*="bootstrap"]:first').prop('href', themeUrl);
                setCookie('theme', themeUrl, 30); // $cookies service is not used because of its encoding '/'
                target.siblings().removeClass('selected-theme');
                target.addClass('selected-theme');
            });

            element.on('$destroy', function () {
                jq('#theme-menu > li').off('click');
            });

            function getCookie(cname) {
                var name = cname + '=';
                var ca = document.cookie.split(';');

                for (var i = 0; i < ca.length; i++) {
                    var c = ca[i];
                    while (c.charAt(0) === ' ') c = c.substring(1);
                    if (c.indexOf(name) === 0) return c.substring(name.length, c.length);
                }

                return '';
            }

            function setCookie(cname, cvalue, exdays) {
                var d = new Date();
                d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
                var expires = 'expires=' + d.toUTCString();
                document.cookie = cname + '=' + cvalue + '; ' + expires + '; path=/';
            }
        }
    }
})(angular.module('app.layout'));
