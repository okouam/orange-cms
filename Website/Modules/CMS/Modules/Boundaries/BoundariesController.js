(function () {

    "use strict";

    function BoundariesController(CMS, Texts, Language) {

        var vm = this;
        vm.texts = Texts;
        vm.language = Language;
        
        vm.minimize = function() {
            $("#cms-boundaries").addClass("minimized");
        };

        vm.open = function() {
            $("#cms-boundaries").removeClass("minimized");
        };

        vm.select = function (item) {
            item.selected = !item.selected;
            _.each(vm.boundaries, function(boundary) {
                if (boundary.id !== item.id && boundary.selected) {
                    boundary.selected = false;
                }
            });

            CMS.query.strMatch = null;

            if (item.selected) {
                CMS.selectBoundary(item.id);
            } else {
                CMS.deselectBoundary();
            }

            CMS.refresh();
        };

        vm.boundaries = CMS.boundaries;
    }

    angular
    .module("geocms")
    .controller("boundariesController", [
        "GeoCMS.CMS",
        "Texts",
        "Language",
        BoundariesController
    ]);

})();