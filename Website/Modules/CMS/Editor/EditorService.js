(function () {

    "use strict";

    angular
        .module("geocms")
        .service("EditorService", [
            "GeoCMS.CMS.Map",
            EditorService
        ]);

    function EditorService(Map) {

        this.current = null;

        this.highlighted = null;

        this.edit = function (customer) {
            // weirdness here, maybe set a delay before resizing? and hide content then show..?
            this.current = customer;
            Map.resizeMapContainer();
        };
    }

})();