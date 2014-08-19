(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("CustomerActionsController", [
            "EditorService",
            CustomerActionsController
        ]);

    function CustomerActionsController(EditorService) {

        var vm = this;

        vm.editCustomer = function () {
            EditorService.edit(EditorService.highlighted);
        };

        vm.deleteCustomer = function () {
            var customer = EditorService.highlighted;
            if (confirm("Are you sure you want to delete " + customer.name + "?")) {
                alert("not yet implemented");
            };
        };
    }

})();