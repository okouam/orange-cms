(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Boundaries.BoundaryService", [
            "GeoCMS.CMS.CollectionUtils",
            BoundaryService
        ]);

    function BoundaryService(CollectionUtils) {

        this.extend = function (collection) {
            CollectionUtils.extend(collection);
        };
    }

})();