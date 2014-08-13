﻿(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("boundariesController", [
            "GeoCMS.CMS",
            "GeoCMS.CMS.Boundaries.Boundaries",
            "GeoCMS.CMS.CollectionUtils",
            BoundariesController
        ]);

    function BoundariesController(CMS, Boundaries, Utils) {

        var vm = this;

        vm.boundaries = CMS.boundaries;
        
        vm.search = function (query) {
            Utils.replaceContents(Boundaries.search(query), CMS.boundaries);
        };

        vm.display = function (boundary) {
            console.log(boundary);
        };
    }

})();