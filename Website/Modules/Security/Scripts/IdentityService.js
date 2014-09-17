(function () {

    "use strict";

    function IdentityService(localStorageService) {

        this.clear = function () {
            localStorageService.remove('token');
        };

        Object.defineProperty(this, "token", {
            get: function () {
                return localStorageService.get('token');
            },
            set: function(val) {
                localStorageService.set('token', val);
            }
        });
    }

    angular
    .module("geocms")
    .service("IdentityService", [
        "localStorageService",
        IdentityService
    ]);

})();