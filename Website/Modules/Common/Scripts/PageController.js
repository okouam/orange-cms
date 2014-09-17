(function () {

    "use strict";

    function PageController($scope, IdentityService) {
        var vm = this;
        vm.identity = IdentityService;
    }

    angular
    .module("geocms")
    .controller("pageController", [
        "$scope",
        "IdentityService",
        PageController
    ]);

})();