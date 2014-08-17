(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS", [
            "GeoCMS.CMS.Editor",
            "GeoCMS.CMS.Customers.Customers",
            "GeoCMS.CMS.Boundaries.Boundaries",
            "GeoCMS.CMS.Categories.Categories",
            "GeoCMS.CMS.Map",
            "GeoCMS.CMS.Customers.CustomerService",
            "GeoCMS.CMS.Boundaries.BoundaryService",
            "GeoCMS.CMS.Categories.CategoryService",
            "GeoCMS.CMS.CollectionUtils",
            CMS
        ]);

    function CMS(Editor, Customers, Boundaries, Categories, Map, CustomerService, BoundaryService, CategoryService, Utils) {

        var self = this;

        this.customers = [];

        this.boundaries = [];
        BoundaryService.extend(this.boundaries);

        this.categories = [];
        CategoryService.extend(this.categories);

        Boundaries.search(null, function(results) {
            Utils.replaceContents(results, self.boundaries);
        });

        Categories.search(null, function (results) {
            Utils.replaceContents(results, self.categories);
        });

        Editor.onSubmitChanges = function (customer) {
            Customers.saveOrUpdate(customer, function() {
                self.mode = 'map';
            });
        };

        this.mode = 'map';

        this.query = {
            strMatch: null
        };

        this.edit = function(customer) {
            self.mode = 'editor';
            Editor.current = customer;
        };

        this.highlight = function(id) {
            var customer = _.where(self.customers, { id: id });
            customer.selected = true;
            return customer;
        };

        this.refresh = function (onSuccess) {
            Customers.search(self.query.strMatch, self.categories.selected(), function (results) {
                Utils.replaceContents(results, self.customers);
                CustomerService.extend(self.customers);
                 if (onSuccess) onSuccess(self.customers);
            });
        };
    }

})();