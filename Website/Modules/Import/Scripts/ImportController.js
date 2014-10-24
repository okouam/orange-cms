(function () {

    "use strict";

    function ImportController($upload, IdentityService, Texts, Language) {
        var vm = this;
        vm.texts = Texts;
        vm.language = Language;

        var file;

        vm.onFileSelect = function ($files) {
            file = $files[0];
        };

        vm.import = function () {
            vm.processing = true;
            vm.upload = $upload.upload({
                url: '/customers/import',
                method: 'POST',
                headers: { 'Authorization': 'Bearer ' + IdentityService.token }, 
                file: file,
            }).success(function () {
                vm.outcome = Texts.import.IMPORT_SUCCESS[Language];
                vm.processing = false;
            })
            .error(function (ex) {
                vm.outcome = Texts.import.IMPORT_FAILURE[Language] + ex.exceptionMessage;
                vm.processing = false;
            });
        };
    }

    angular
    .module("geocms")
    .controller("ImportController", [
        '$upload',
        "IdentityService",
        "Texts",
        "Language",
        ImportController
    ]);

})();