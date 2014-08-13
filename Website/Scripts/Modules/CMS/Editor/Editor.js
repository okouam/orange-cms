(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Editor", [
            Editor
        ]);

    function Editor() {
        return {
            onSubmitChanges: null
        };
    }

})();