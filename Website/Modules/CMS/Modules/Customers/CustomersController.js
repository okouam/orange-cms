(function () {

    "use strict";

    function CustomersController(CMS, ngDialog) {

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

        vm.showImageInDialog = function(url) {
            ngDialog.open({
                template: '/Modules/Common/Templates/ImageInDialog.html',
                controller: ['$scope', function ($scope) {
                    $scope.url = url;
                }]
            });
        };

        vm.search = function () {
            CMS.refresh();
        };
    }

    angular
     .module("geocms")
     .controller("customersController", [
         "GeoCMS.CMS",
         "ngDialog",
         CustomersController
     ]);

})();