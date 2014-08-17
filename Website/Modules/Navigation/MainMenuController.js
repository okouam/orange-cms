(function () {

    "use strict";

    angular
        .module("geocms")
        .controller("mainMenuController", [
            MainMenuController
        ]);

    function MainMenuController() {

        var vm = this;

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