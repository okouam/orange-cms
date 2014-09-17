(function () {

    "use strict";

    function BoundaryService(CollectionUtils) {
        this.extend = function (collection) {
            CollectionUtils.extend(collection);
        };
    }

    angular
    .module("geocms")
    .service("GeoCMS.CMS.Boundaries.BoundaryService", [
        "GeoCMS.CMS.CollectionUtils",
        BoundaryService
    ]);

})();