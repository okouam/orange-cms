(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Boundaries.Boundaries", [
            "$http",
            Boundaries
        ]);

    function Boundaries($http) {
        return {
            findAll: function(onSuccess, onError) {
                $http.get("/boundaries").success(onSuccess).error(onError);
            }
        };
    }

})();