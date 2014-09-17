(function () {

    "use strict";
    
    function Boundaries($http) {
        return {
            findAll: function (onSuccess, onError) {
                $http.get("/boundaries").success(onSuccess).error(onError);
            }
        };
    }

    angular
    .module("geocms")
    .service("GeoCMS.CMS.Boundaries.Boundaries", [
        "$http",
        Boundaries
    ]);

})();