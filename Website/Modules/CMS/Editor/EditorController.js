(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("EditorController", [
            "EditorService",
            EditorController
        ]);

    function EditorController(EditorService) {

        var vm = this;

        vm.editor = EditorService;
    }

})();