(function() {

    "use strict";

    function GoogleMapsController(CMS, Map) {

        var vm = this;

        vm.map = Map;

        vm.customers = CMS.customers;

        vm.polygons = Map.polygons;

        vm.highlight = function (id) {
            $(".marker-" + id).css({
                backgroundColor: "white", color: "black"
            });
            CMS.highlight(id);
        };
    }

    angular
    .module("geocms")
    .controller("googleMapsController", [
        "GeoCMS.CMS",
        "GeoCMS.CMS.Map",
        GoogleMapsController
    ]);

})();