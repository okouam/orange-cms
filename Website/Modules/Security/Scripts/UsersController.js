/* globals alert: true, confirm: true */

(function () {

    "use strict";

    function UsersController(Users) {
        var vm = this;
        var PASSWORD_PLACEHOLDER = "123456789";

        function createUserModel() {
            var model = {
                username: vm.username, 
                role: vm.role, 
                email: vm.email,
            };

            if (vm.password !== PASSWORD_PLACEHOLDER && vm.passwordConfirmation !== PASSWORD_PLACEHOLDER) {
                model.password = vm.password;
            }

            return model;
        }

        function getUsers(callback) {
            Users.all(function (result) {
                vm.users = result;
                if (callback) {
                    callback();
                }
            }, function (error) {
                vm.error = error;
            });
        }

        function onUserChangeSuccess() {
            getUsers(function() {
                vm.mode = 'list';
            });
        }

        function onUserChangeError(error) {
            vm.error = error;
        }

        onUserChangeSuccess();
        
        vm.create = function () {
            vm.mode = 'change';
            vm.error = null;
            vm.id = null;
            vm.username = null;
            vm.password = null;
            vm.passwordConfirmation = null;
            vm.role = null;
            vm.email = null;
        };

        vm.saveChanges = function () {
            vm.error = null;

            if (!vm.username || !vm.role || !vm.email || !vm.passwordConfirmation || !vm.password) {
                vm.error = "One or more required fields have not been provided.";
                alert(vm.error);
                return;
            }

            if (vm.password !== PASSWORD_PLACEHOLDER) {

                if (!vm.password) {
                    vm.error = "No password provided.";
                     alert(vm.error);
                    return;
                }

                if (vm.password !== vm.passwordConfirmation) {
                    vm.error = "Your password confirmation is not correct.";
                    alert(vm.error);
                    return;
                }
            }

            if (vm.id) {
                Users.update(vm.id, createUserModel(), onUserChangeSuccess, onUserChangeError);
            } else {

                Users.create(createUserModel(), onUserChangeSuccess, onUserChangeError);
            }
        };

        vm.cancelChanges = function () {
            vm.error = null;
            vm.id = null;
            vm.mode = 'list';
            vm.username = null;
            vm.password = PASSWORD_PLACEHOLDER;
            vm.passwordConfirmation = PASSWORD_PLACEHOLDER;
            vm.role = null;
            vm.email = null;
        };

        vm.delete = function (user) {
            if (confirm("Are you sure you want to delete this user?")) {
                Users.delete(user.id, function() {
                    getUsers();
                }, onUserChangeError);
            }
        };

        vm.edit = function (user) {
            vm.id = user.id;
            vm.mode = 'change';
            vm.username = user.userName;
            vm.role = vm.role;
            vm.email = vm.email;
            vm.password = PASSWORD_PLACEHOLDER;
            vm.passwordConfirmation = PASSWORD_PLACEHOLDER;
        };
    }

    angular
    .module("geocms")
    .controller("UsersController", [
        "GeoCMS.CMS.Customers.Users",
        UsersController
    ]);

})();