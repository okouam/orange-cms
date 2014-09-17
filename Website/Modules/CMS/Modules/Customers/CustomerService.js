(function () {

    "use strict";

    function CustomerService(CollectionUtils) {

        this.extend = function (collection) {
            CollectionUtils.extend(collection);
        };
    }

    angular
    .module("geocms")
    .service("GeoCMS.CMS.Customers.CustomerService", [
        "GeoCMS.CMS.CollectionUtils",
        CustomerService
    ]);

})();