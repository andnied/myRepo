/// <reference path="jquery-2.2.3.js" />
/// <reference path="angular.js" />

var cbApp = angular.module('cbApp', []);
cbApp.controller('cbCtrl', function ($scope, $http) {
    function getSelectedIndex(id) {
        for (var i = 0; i < $scope.result.length; i++) {
            if ($scope.result[i].Id == id)
                return i;
        }

        return -1;
    }

    function addContact(contact) {
        $.ajax({
            type: 'POST',
            url: 'http://localhost:50523/api/contacts/',
            data: contact,
            success: function (response) {
                
            },
            function(jqXHR, textStatus, errorThrown) {
                alert("Error, status = " + textStatus + ", " +
                      "error thrown: " + errorThrown
                );
            }
        });
    }

    function updateContact(id, contact) {
        $.ajax({
            type: 'POST',
            url: 'http://localhost:50523/api/contacts/update/' + id,
            data: contact,
            success: function (response) {

            },
            function(jqXHR, textStatus, errorThrown) {
                alert("Error, status = " + textStatus + ", " +
                      "error thrown: " + errorThrown
                );
            }
        });

        //$http.post('http://localhost:50523/api/contacts/update/' + id, contact).success(function (response) {
        //    var index = getSelectedIndex(id);

        //    $scope.result[index].FirstName = contact.FirstName;
        //    $scope.result[index].LastName = contact.LastName;
        //    $scope.result[index].Email = contact.Email;
        //    $scope.result[index].Address = contact.Address;
        //    $scope.result[index].Phone = contact.Phone;
        //})
    }

    $http.get('http://localhost:50523/api/contacts/').success(function (response) {
        $scope.result = response;
    });

    $scope.delete = function (id) {
        $http.post('http://localhost:50523/api/contacts/' + id).success(function (response) {
            var index = getSelectedIndex(id);
            $scope.result.splice(index, 1);
        });
    }

    $scope.edit = function (id) {
        var index = getSelectedIndex(id);
        var contact = $scope.result[index];

        $scope.Id = contact.Id;
        $scope.FirstName = contact.FirstName;
        $scope.LastName = contact.LastName;
        $scope.Address = contact.Address;
        $scope.Email = contact.Email;
        $scope.Phone = contact.Phone;
    }

    $scope.add = function () {
        var contact = {
            Id: 0,
            FirstName: $scope.FirstName,
            LastName: $scope.LastName,
            Address: $scope.Address,
            Email: $scope.Email,
            Phone: $scope.Phone
        };

        addContact(contact);

        $scope.result.push(contact);
    }

    $scope.update = function () {
        var contact = {
            Id: $scope.Id,
            FirstName: $scope.FirstName,
            LastName: $scope.LastName,
            Address: $scope.Address,
            Email: $scope.Email,
            Phone: $scope.Phone
        };

        updateContact($scope.Id, contact);

        var index = getSelectedIndex($scope.Id);

        $scope.result[index].FirstName = contact.FirstName;
        $scope.result[index].LastName = contact.LastName;
        $scope.result[index].Email = contact.Email;
        $scope.result[index].Address = contact.Address;
        $scope.result[index].Phone = contact.Phone;
    }
});