(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("LoginController", [
            "$state",
            "AuthenticationService",
            LoginController
        ]);

    function LoginController($state, AuthenticationService) {
        var vm = this;

        this.login = function() {
            var username = vm.username;
            var password = vm.password;
            var promise = AuthenticationService.login(username, password);
            promise.then(function() {
                $state.go("cms");
            }, function (response) {
                vm.error = response.error;
            });
        };
    }

})();