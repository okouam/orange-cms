(function () {

    "use strict";

    function AuthenticationService($q, $http, IdentityService) {

        var self = this;

        this.token = null;

        this.logout = function () {
            IdentityService.clear();
            IdentityService.isAuthenticated = false;
        };

        Object.defineProperty(this, "token", {
            get: function () {
                return IdentityService.token;
            }
        });

        this.login = function (username, password) {

            var deferred = $q.defer();

            var data = "grant_type=password&username=" + username + "&password=" + password;

            var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
            
            $http.post('/tokens', data, { headers: headers }).success(function (response, status, responseHeaders) {
                IdentityService.token = response["access_token"];
                IdentityService.username = responseHeaders()["x-geocms-username"];
                IdentityService.role = responseHeaders()["x-geocms-role"];
                deferred.resolve(response);
            }).error(function (err) {
                self.logout();
                deferred.reject(err);
            });

            return deferred.promise;
        };

    }

    angular
    .module("geocms")
    .service("AuthenticationService", [
        "$q",
        "$http",
        "IdentityService",
        AuthenticationService
    ]);

})();