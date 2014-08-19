(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("UsersController", [
            "GeoCMS.CMS.Customers.Users",
            UsersController
        ]);

    function UsersController(Users) {
        var vm = this;
        Users.all(function (result) {
            vm.users = result;
        });
    }

})();