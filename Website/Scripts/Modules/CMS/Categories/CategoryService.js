(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Categories.CategoryService", [
            "GeoCMS.CMS.CollectionUtils",
            CategoryService
        ]);

    function CategoryService(CollectionUtils) {

        this.extend = function (collection) {
            CollectionUtils.extend(collection);
        };
    }

})();