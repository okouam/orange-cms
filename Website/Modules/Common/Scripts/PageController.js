(function () {

    "use strict";

    function PageController($scope, IdentityService, $location) {
        var vm = this;
        vm.identity = IdentityService;
        vm.isLogin = function() {
            return $location.path() == "/login";
        }
    }

    angular
    .module("geocms")
    .controller("pageController", [
        "$scope",
        "IdentityService",
        "$location",
        PageController
    ]);

})();