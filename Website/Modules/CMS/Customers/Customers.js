(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Customers.Customers", [
            "$http",
            Customers
        ]);

   function Customers($http) {

       this.search = function (strMatch, categories, onSuccess, onError) {
           $http.get("/customers").success(onSuccess).error(onError);
       };

       this.saveOrUpdate = function(customer, onSuccess) {
           onSuccess();
       };
    }

})();