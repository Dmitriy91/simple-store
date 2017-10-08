(function (app_core) {
    'use strict';

    app_core.factory('pagerService', pagerService);

    function pagerService() {
        var service = {};

        service.GetPager = GetPager;

        return service;

        function GetPager(totalItems, totalPages, currentPage, pageSize) {
            currentPage = currentPage || 1; // set to first page by default
            pageSize = pageSize || 5; // default page size is 10
            var startPage = 0;
            var endPage = 0;

            if (totalPages <= 10) {
                // if there are less than 10 pages, show all pages
                startPage = 1;
                endPage = totalPages;
            } else {
                // if there are more than 10 pages, calculate starting and ending pages
                if (currentPage <= 6) {
                    startPage = 1;
                    endPage = 10;
                } else if (currentPage + 4 >= totalPages) {
                    startPage = totalPages - 9;
                    endPage = totalPages;
                } else {
                    startPage = currentPage - 5;
                    endPage = currentPage + 4;
                }
            }

            // calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageSize;
            var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

            // create an array of pages to ng-repeat in the pager control
            var pages = [];

            for (var i = startPage; i <= endPage; i++)
                pages.push(i);

            // return object with all pager properties required by the view
            return {
                totalItems: totalItems,
                currentPage: currentPage,
                pageSize: pageSize,
                totalPages: totalPages,
                startPage: startPage,
                endPage: endPage,
                startIndex: startIndex,
                endIndex: endIndex,
                pages: pages
            };
        }
    }
})(angular.module('app.core'));
