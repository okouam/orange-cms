(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Customers.CustomerService", [
            "GeoCMS.CMS.CollectionUtils",
            CustomerService
        ]);

    function CustomerService(CollectionUtils) {

        this.extend = function (collection) {
            CollectionUtils.extend(collection);
        };
    }

})();