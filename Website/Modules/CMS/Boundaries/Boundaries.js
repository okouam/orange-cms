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
            search: function (q, onSuccess, onError) {
                $http.get("/boundaries").success(onSuccess).error(onError);
            },
            get: function(id, onSuccess, onError) {
                $http.get("/boundaries/" + id).success(onSuccess).error(onError);
            }
        };
    }

})();