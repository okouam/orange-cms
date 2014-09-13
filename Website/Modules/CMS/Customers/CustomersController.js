(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("customersController", [
            "GeoCMS.CMS",
            CustomersController
        ]);

    function CustomersController(CMS) {

        var vm = this;

        vm.customers = CMS.customers;
        vm.query = CMS.query;

        CMS.refresh();

        vm.showDetails = function (customer) {
            customer.selected = !customer.selected;
        };

        vm.zoomToCustomer = function(customer) {
            CMS.centerMap(customer.latitude, customer.longitude);
        };

        vm.search = function () {
            CMS.refresh();
        };
    }
})();