(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("categoriesController", [
            "GeoCMS.CMS.Categories.Categories",
            "GeoCMS.CMS",
            "GeoCMS.CMS.CollectionUtils",
            CategoriesController
        ]);

    function CategoriesController(Categories, CMS, Utils) {

        var vm = this;

        vm.categories = CMS.categories;

        vm.minimize = function () {
            $("#cms-categories").addClass("minimized");
        };

        vm.open = function () {
            $("#cms-categories").removeClass("minimized");
        };

        vm.select = function (item) {
            item.selected = !item.selected;
        };

        vm.search = function (query) {
            Utils.replaceContents(Categories.search(query), CMS.categories);
        };

        vm.filter = function () {
            CMS.refresh();
        };
    }

})();