(function () {

    "use strict";

    angular
        .module("geocms")
        .constant("Startup", {

            configureTokenAuthentication: function ($httpProvider) {
                $httpProvider.interceptors.push('OAuthInterceptor');
            },

            configureRouting: function($stateProvider, $urlRouterProvider) {
                $urlRouterProvider.otherwise("/cms");
                $stateProvider
                    .state('login', {
                        url: "/login",
                        templateUrl: "Modules/Security/Login.html"
                    })
                    .state('users', {
                        url: "/users",
                        templateUrl: "Modules/Security/Users.html"
                    })
                    .state('users-new', {
                        url: "/users",
                        templateUrl: "Modules/Security/Users.html"
                    })
                    .state('users-edit', {
                        url: "/users/{id}",
                        templateUrl: "Modules/Security/Users.html"
                    })
                    .state('import', {
                        url: "/import",
                        templateUrl: "Modules/Import/Import.html"
                    })
                    .state('export', {
                        url: "/export",
                        templateUrl: "Modules/Export/Export.html"
                    })
                    .state('cms', {
                        url: "/cms",
                        templateUrl: "Modules/CMS/CMS.html",
                        onEnter: function(IdentityService, $state) {
                            if (!IdentityService.token) {
                                $state.go("login");
                            }
                        }
                    });
                }
    });

})();