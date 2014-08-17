(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Categories.Categories", [
            "$http",
            Categories
        ]);

    function Categories($http) {
        return {
            search: function (q, onSuccess, onError) {
                $http.get("/categories").success(onSuccess).error(onError);
            },
            saveOrUpdate: function (category, onSuccess) {
                onSuccess();
            }
        };
    }

})();