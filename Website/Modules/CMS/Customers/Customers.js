(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Customers.Customers", [
            "$http",
            Customers
        ]);

   function Customers($http) {
       this.search = function (query, onSuccess, onError) {
           var params = {};
           if (query.strMatch) params.strMatch = query.strMatch;
           if (query.boundary) params.boundary = query.boundary;
           $http.get("/customers", { params: params }).success(onSuccess).error(onError);
       };
    }

})();