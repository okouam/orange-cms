(function () {

    "use strict";

    function MainMenuController(AuthenticationService, $state, ngDialog, IdentityService) {

        var vm = this;

        vm.logout = function () {
            AuthenticationService.logout();
            $state.go("login");
        };

        vm.import = function() {
            ngDialog.open({
                template: "/Modules/Import/Templates/Import.html",
                closeByDocument: false
            });
        };

        vm.export = function () {
            window.open("/customers/export?access_token=" + IdentityService.token, '_blank', '');
        };

        vm.showUsers = function() {
            ngDialog.open({
                template: "/Modules/Security/Templates/Users.html",
                closeByDocument: false
            });
        };

        vm.showDropdown = function (evt) {
            if (!$(evt.currentTarget).find('span.toggle').hasClass('active')) {
                $('.dropdown-slider').slideUp();
                $('span.toggle').removeClass('active');
            }
            $(evt.currentTarget).parent().find('.dropdown-slider').slideToggle('fast');
            $(evt.currentTarget).find('span.toggle').toggleClass('active');

            return false;
        };
    }

    angular
    .module("geocms")
    .controller("mainMenuController", [
        "AuthenticationService",
        "$state",
        "ngDialog",
        "IdentityService",
        MainMenuController
    ]);

})();