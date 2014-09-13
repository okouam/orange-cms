(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("LoginController", [
            "$state",
            "AuthenticationService",
            "IdentityService",
            LoginController
        ]);

    function LoginController($state, AuthenticationService, IdentityService) {
        var vm = this;

        this.login = function() {
            var username = vm.username;
            var password = vm.password;
            var promise = AuthenticationService.login(username, password);
            promise.then(function () {
                IdentityService.isAuthenticated = true;
                $state.go("cms");
                
            }, function () {
                vm.error = "Your username or password is incorrect";
                vm.problem = "Authentication failed.";
            });
        };
    }

})();