(function () {

    "use strict";

    function ImportController($upload, IdentityService) {
        var vm = this;
        var file;

        vm.onFileSelect = function ($files) {
            file = $files[0];
        };

        vm.import = function () {
            vm.upload = $upload.upload({
                url: '/customers/import',
                method: 'POST',
                headers: { 'Authorization': 'Bearer ' + IdentityService.token }, 
                file: file,
            }).progress(function () {

            }).success(function () {
                vm.outcome = "The file has been successfully imported.";
            })
            .error(function (ex) {
                vm.outcome = "Failed when trying to import the file. " + ex.exceptionMessage;
            });
        };
    }

    angular
    .module("geocms")
    .controller("ImportController", [
        '$upload',
        "IdentityService",
        ImportController
    ]);

})();