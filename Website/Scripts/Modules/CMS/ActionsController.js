(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("actionsController", [
            "GeoCMS.CMS",
            "GeoCMS.CMS.Map",
            ActionsController
        ]);

    function ActionsController(CMS, Map) {

        this.centerMap = function () {
            Map.centerOnMarkers();
        };

        this.clearFilters = function () {
            CMS.customers.clearSelection();
            CMS.boundaries.clearSelection();
            CMS.categories.clearSelection();
        };
    }
})();