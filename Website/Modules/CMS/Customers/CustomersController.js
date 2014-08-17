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
        vm.data = {};
        
        CMS.refresh(function (customers) {
            vm.customers = customers;
        });

        vm.query = CMS.query;

        vm.createCustomer = function() {
            Editor.createCustomer(CMS.customers);
        };

        vm.deleteCustomers = function() {
            CMS.customers.deleteWhere({selected: true});
        };

        vm.toggleEditing = function(customer) {
            customer.isEditing = customer.isEditing ? false : true;
        };
        
        vm.search = function () {
            CMS.refresh();
        };

        vm.showDropdown = function (evt) {
            if (!$(evt.currentTarget).find('span.toggle').hasClass('active')) {
                $('.dropdown-slider').slideUp();
                $('span.toggle').removeClass('active');
            }
            $(evt.currentTarget).parent().find('.dropdown-slider').slideToggle('fast');
            $(evt.currentTarget).find('span.toggle').toggleClass('active');

            return false;
        };
    }
})();