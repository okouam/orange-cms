(function () {

    "use strict";

    angular
        .module("geocms")
        .factory("OAuthInterceptor", [
            "$q",
            "$location",
            "IdentityService",
            OAuthInterceptor
        ]);

    function OAuthInterceptor($q, $location, IdentityService) {

        return {

            request: function (config) {
                config.headers = config.headers || {};
                var token = IdentityService.token;
                if (token) {
                    config.headers.Authorization = 'Bearer ' + token;
                }
                return config;
            },

            responseError: function(rejection) {
                if (rejection.status === 401) {
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }
        };
    }

})();