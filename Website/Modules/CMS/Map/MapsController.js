(function() {

    "use strict";

    angular
        .module("geocms")
        .controller("googleMapsController", [
            "GeoCMS.CMS",
            "GeoCMS.CMS.Map",
            GoogleMapsController
        ]);

    function GoogleMapsController(CMS, Map) {

        var vm = this;

        vm.map = Map;

        vm.customers = CMS.customers;

        vm.highlight = function (id) {
            $(".marker-" + id).css({
                backgroundColor: "white", color: "black"
            });
            CMS.highlight(id);
        };
    }

})();