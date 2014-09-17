(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("mainMenuController", [
            "AuthenticationService",
            "$state",
            "ngDialog",
            "IdentityService",
            MainMenuController
        ]);

    function MainMenuController(AuthenticationService, $state, ngDialog, IdentityService) {

        var vm = this;

        vm.logout = function () {
            AuthenticationService.logout();
            $state.go("login");
        };

        vm.import = function() {
            ngDialog.open({
                template: "/Modules/Import/Import.html",
                closeByDocument: false
            });
        };

        vm.export = function () {
            window.open("/customers/export?access_token=" + IdentityService.token, '_blank', '');
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

})();