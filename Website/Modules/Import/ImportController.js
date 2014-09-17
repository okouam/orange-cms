(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("ImportController", [
            '$upload',
            "IdentityService",
            ImportController
        ]);

    function ImportController($upload, IdentityService) {
        var vm = this;
        var file;

        vm.onFileSelect = function ($files) {
            file = $files[0];
            console.log(file);
        };

        vm.import = function () {
            vm.upload = $upload.upload({
                url: '/customers/import',
                method: 'POST',
                headers: { 'Authorization': 'Bearer ' + IdentityService.token }, 
                file: file,
            }).progress(function (evt) {
                console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
            }).success(function () {
                vm.outcome = "The file has been successfully imported.";
            })
            .error(function (ex) {
                vm.outcome = "Failed when trying to import the file. " + ex.exceptionMessage;
            });
        };
    }

})();