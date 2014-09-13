(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("boundariesController", [
            "GeoCMS.CMS",
            "GeoCMS.CMS.Boundaries.Boundaries",
            "GeoCMS.CMS.CollectionUtils",
            "GeoCMS.CMS.Map",
            BoundariesController
        ]);

    function BoundariesController(CMS, Boundaries, Utils, Map) {

        var vm = this;

        vm.minimize = function() {
            $("#cms-boundaries").addClass("minimized");
        };

        vm.open = function() {
            $("#cms-boundaries").removeClass("minimized");
        };

        vm.select = function (item) {
            item.selected = !item.selected;
            _.each(vm.boundaries, function(boundary) {
                if (boundary.id != item.id && boundary.selected) {
                    boundary.selected = false;
                }
            });

            if (item.selected) {
                CMS.query.boundary = item.id;
            } else {
                CMS.query.boundary = null;
            }

            CMS.refresh();
        };

        vm.boundaries = CMS.boundaries;
    }

})();