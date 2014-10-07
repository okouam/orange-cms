(function () {

    "use strict";

   function Customers($http) {
       this.search = function (query, onSuccess, onError) {
           var params = {};
           if (query.strMatch) {
               params.strMatch = query.strMatch;
           }
           if (query.boundary) {
               params.boundary = query.boundary;
           }
           $http.get("/customers", { params: params }).success(onSuccess).error(onError);
       };

       this.remove = function (id, onSuccess, onError) {
           $http.delete("/customers/" + id).success(onSuccess).error(onError);
       };
   }

   angular
   .module("geocms")
   .service("GeoCMS.CMS.Customers.Customers", [
       "$http",
       Customers
   ]);

})();