(function () {

    "use strict";

    function Users($http) {

        this.all = function (onSuccess, onError) {
            $http.get("/users").success(onSuccess).error(onError);
        };

        this.update = function (user, onSuccess, onError) {
            $http.post("/users/" + user.id, user).success(onSuccess).error(onError);
        };

        this.create = function (user, onSuccess, onError) {
            $http.post("/users", user).success(onSuccess).error(onError);
        };

        this.delete = function (id, onSuccess, onError) {
            $http.delete("/users/" + id).success(onSuccess).error(onError);
        };
    }

    angular
    .module("geocms")
    .service("GeoCMS.CMS.Customers.Users", [
        "$http",
        Users
    ]);

})();