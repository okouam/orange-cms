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
            vm.upload = $upload.upload({
                url: '/customers/import',
                method: 'POST',
                headers: { 'Authorization': 'Bearer ' + IdentityService.token }, 
                file: file,
            }).progress(function () {

            }).success(function () {
                vm.outcome = Texts.imports.IMPORT_SUCCESS[Language];
            })
            .error(function (ex) {
                vm.outcome = Texts.imports.IMPORT_FAILURE[Language] + ex.exceptionMessage;
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