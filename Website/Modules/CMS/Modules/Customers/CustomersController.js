(function () {

    "use strict";

    function CustomersController(CMS, ngDialog, Texts, Language) {

        var vm = this;

        vm.language = Language;
        vm.texts = Texts;
        vm.customers = CMS.customers;
        vm.query = CMS.query;
        vm.CMS = CMS;

        CMS.refresh();

        vm.showDetails = function (customer) {
            customer.selected = !customer.selected;
        };

        vm.deleteCustomer = function (customer) {
            if (confirm("Are you sure you wish to delete this customer?")) {
                CMS.deleteCustomer(customer.id);
            }
        };

        vm.zoomToCustomer = function(customer) {
            CMS.centerMap(customer.latitude, customer.longitude);
        };

        vm.showImageInDialog = function(url) {
            ngDialog.open({
                template: '/Modules/Common/Templates/ImageInDialog.html',
                controller: ['$scope', function ($scope) {
                    $scope.url = url;
                }]
            });
        };

        vm.search = function () {
            CMS.query.boundary = null;
            _.each(CMS.boundaries, function(boundary) {
                boundary.selected = false;
            });
            CMS.refresh();
        };
    }

    angular
     .module("geocms")
     .controller("customersController", [
         "GeoCMS.CMS",
         "ngDialog",
         "Texts",
         "Language",
         CustomersController
     ]);

})();