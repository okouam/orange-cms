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

        Editor.onSubmitChanges = function (customer) {
            Customers.saveOrUpdate(customer, function() {
                self.mode = 'map';
            });
        };

        this.boundaries = Boundaries.search();
        BoundaryService.extend(this.boundaries);

        this.categories = Categories.search();
        CategoryService.extend(this.categories);

        this.customers = Customers.search();
        CustomerService.extend(this.customers);

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

        this.refresh = function () {
            var results = Customers.search(self.query.strMatch, self.categories.selected(), self.boundaries.selected());
            Utils.replaceContents(results, self.customers);
            return self.customers;
        };
    }

})();