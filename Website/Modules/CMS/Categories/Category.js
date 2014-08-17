(function () {

    "use strict";

    angular
        .module("geocms")
        .service("Categories", [
            "$http",
            Categories
        ]);

    function Categories($http) {

        return {
            saveOrUpdate: function (category, onSuccess) {
                onSuccess();
            }
        };
    }

})();