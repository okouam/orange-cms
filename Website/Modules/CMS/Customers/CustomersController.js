(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("customersController", [
            "GeoCMS.CMS",
            "EditorService",
            CustomersController
        ]);

    function CustomersController(CMS, EditorService) {

        var vm = this;
        vm.data = {};
        
        CMS.refresh(function (customers) {
            vm.customers = customers;
        });

        vm.query = CMS.query;

        vm.createCustomer = function() {
            EditorService.current = {};
        };

        vm.highlight = function(customer) {
            EditorService.highlighted = customer;
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