/// <reference path="C:\Users\wesle\Source\Repos\ddd.bitbucket\SgqSystem\Scripts/angular.js" />

var app = angular.module('sgqSystem', ['smart-table', 'timer'])

app.directive('scrollToBottom', function ($timeout, $window) {
    return {
        scope: {
            scrollToBottom: "="
        },
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.$watchCollection('scrollToBottom', function (newVal) {
                $timeout(function () {
                    element[0].scrollTop = element[0].scrollHeight;
                }, 0);
            });
        }
    };
})

app.filter('slice', function () {
    return function (arr, start, end) {
        return arr.slice(start, end);
    };
});
