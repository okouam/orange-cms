(function () {

    "use strict";

    angular
        .module("geocms")
        .constant("Startup", {

            configureTokenAuthentication: function ($httpProvider) {
                $httpProvider.interceptors.push('OAuthInterceptor');
            },

            configureRouting: function ($stateProvider, $urlRouterProvider) {
                $urlRouterProvider.otherwise("/cms");
                $stateProvider
                    .state('login', {
                        url: "/login",
                        templateUrl: "Modules/Security/Templates/Login.html"
                    })
                    .state('setup', {
                        url: "/setup",
                        templateUrl: "Modules/Security/Templates/Setup.html"
                    })
                    .state('cms', {
                        url: "/cms",
                        templateUrl: "Modules/CMS/Templates/CMS.html",
                        onEnter: function(IdentityService, $state) {
                            if (!IdentityService.token) {
                                $state.go("login");
                            } else {
                                IdentityService.isAuthenticated = true;
                            }
                        }
                    });
                }
    });

})();