(function () {

    "use strict";

    function PageController(IdentityService, $location) {
        var vm = this;
        vm.identity = IdentityService;
        vm.isLogin = function() {
            return $location.path() === "/login";
        };
    }

    angular
    .module("geocms")
    .controller("pageController", [
        "IdentityService",
        "$location",
        PageController
    ]);

})();