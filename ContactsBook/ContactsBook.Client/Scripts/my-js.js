/// <reference path="angular.js" />

var cbApp = angular.module('cbApp', []);
cbApp.controller('cbCtrl', function ($scope, $http) {
    $http.get('http://localhost:50523/api/contacts/').success(function (response) {
        $scope.result = response;
    });
});