(function () {

    "use strict";

    function CMS(Customers, Boundaries, Map, CustomerService, BoundaryService, Utils) {

        var self = this;

        self.isRefreshing = false;

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

        this.deleteCustomer = function(id) {
            Customers.remove(id, function() {
                var customer = _.findWhere(self.customers, { id: id });
                Utils.replaceContents(_.without(self.customers, customer), self.customers);
                Boundaries.findAll(function (results) {
                    Utils.replaceContents(results, self.boundaries);
                    if (self.query.boundary) {
                        var boundary = _.findWhere(self.boundaries, { id: self.query.boundary });
                        boundary.selected = true;
                    }
                });
            });
        };

        this.centerMap = function(latitude, longitude) {
            Map.centerMap(latitude, longitude);
        };

        this.centerOnMarkers = function() {
            Map.centerOnMarkers();
        };

        this.refresh = function (onSuccess) {
            self.isRefreshing = true;
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
                self.isRefreshing = false;
                 if (onSuccess) {
                     onSuccess(self.customers);
                 }
            });
        };
    }

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

})();