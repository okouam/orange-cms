(function () {

    "use strict";

    function RoleFactory() {

        function Role(val) {
            this.val = val;
        }

        Role.prototype =  {

            isStandard: function() {
                return this.val.toLowerCase() === "standard";
            },

            isSystem: function() {
                return this.val.toLowerCase() === "system";
            },

            isAdministrator: function() {
                return this.val.toLowerCase() === "administrator";
            }
        };

        return Role;
    }

    angular
     .module("geocms")
     .factory("geocms.security.role", [
         RoleFactory
     ]);

})();