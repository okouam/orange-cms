(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("pageController", [
            "$scope",
            "IdentityService",
            PageController
        ]);

    function PageController($scope, IdentityService) {
        var vm = this;
        vm.identity = IdentityService;
    }

})();