(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Customers.Customers", [
            "$http",
            Customers
        ]);

   function Customers($http) {
       var self = this;

       var seedData = self.createSeedData();

       this.search = function(strMatch, categories, boundaries) {
           var customers = _.clone(seedData);
           if (strMatch) customers = self.filterOnNameAndPhone(customers, strMatch);
           if (categories && categories.length > 0) customers = self.filterOnCategories(customers, categories);
           if (boundaries && boundaries.length > 0) customers = self.filterOnBoundaries(customers, boundaries);
           return customers;
       };

       this.saveOrUpdate = function(customer, onSuccess) {
           onSuccess();
       };
    }

   Customers.prototype.createSeedData = function () {

       var self = this;

       var seedData = [];

        for (var z = 1000; z < 1050; z++) {
            seedData.push({
                name: faker.Name.findName(),
                phone: faker.PhoneNumber.phoneNumber(),
                id: z,
                boundaries: self.createMockBoundaries(),
                categories: self.createMockTags(),
                longitude: -1 * Math.random() * 0.1 - 4,
                latitude: Math.random() * 0.1 + 5.3,
                image: faker.Image.city()
            });
        }
       
        seedData = _.sortBy(seedData, function (item) {
            return item.name;
        });

       return seedData;
   };

    Customers.prototype.createMockTags = function() {
        var numTags = faker.random.number(5);
        var tags = [];
        _.times(numTags, function() {
            tags.push(faker.random.number([1000, 40]));
        });
        return tags;
    };

    Customers.prototype.createMockBoundaries = function() {
        var numBoundaries = faker.random.number(0, 3);
        var boundaries = [];
        _.times(numBoundaries, function() {
            boundaries.push(faker.random.number([1000, 1020]));
        });
        return boundaries;
    };

    Customers.prototype.filterOnCategories = function (customers, categories) {
        var ids = _.pluck(categories, "id");
        return _.filter(customers, function (customer) {
            return _.intersection(customer.categories, ids).length > 0;
        });
    };

    Customers.prototype.filterOnBoundaries = function (customers, boundaries) {
        var ids = _.pluck(boundaries, "id");
        return _.filter(customers, function(customer) {
            return _.intersection(customer.boundaries, ids).length > 0;
        });
    };

    Customers.prototype.filterOnNameAndPhone = function(customers, q) {
        return _.filter(customers, function (customer) {
            var name = customer.name.toLowerCase();
            var phone = customer.phone;
            return name.indexOf(q.toLowerCase()) > -1 || phone.indexOf(q.toLowerCase()) > -1;
        });
    };

})();