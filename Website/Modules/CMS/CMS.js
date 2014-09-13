(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS", [
            "GeoCMS.CMS.Customers.Customers",
            "GeoCMS.CMS.Boundaries.Boundaries",
            "GeoCMS.CMS.Map",
            "GeoCMS.CMS.Customers.CustomerService",
            "GeoCMS.CMS.Boundaries.BoundaryService",
            "GeoCMS.CMS.CollectionUtils",
            CMS
        ]);

    function CMS(Customers, Boundaries, Map, CustomerService, BoundaryService, Utils) {

        var self = this;

        this.customers = [];

        this.boundaries = [];
        BoundaryService.extend(this.boundaries);

        Boundaries.findAll(function(results) {
            Utils.replaceContents(results, self.boundaries);
        });

        this.mode = 'map';

        this.query = {
            strMatch: null,
            boundary: null
        };

        this.centerMap = function(latitude, longitude) {
            Map.centerMap(latitude, longitude);
        };

        this.refresh = function (onSuccess) {
            Utils.replaceContents([], self.customers);
            Customers.search(self.query, function (results) {
                Utils.replaceContents(results, self.customers);
                CustomerService.extend(self.customers);
                _.each(self.customers, function (customer) {
                    customer.options = {
                        labelContent: customer.telephone,
                        labelAnchor: "30 -5",
                        labelClass: "marker-label"
                    };
                    customer.labelContent = customer.telephone;
                });
                 if (onSuccess) onSuccess(self.customers);
            });
        };
    }

})();