(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Categories.Categories", [
            "$http",
            Categories
        ]);

    function Categories($http) {

        function createMockData() {
            var items = [];

            for (var j = 1000; j < 1040; j++) {
                var numLocations = faker.random.number(0, 50);
                var locationsIds = [];
                _.times(numLocations, function () {
                    locationsIds.push(faker.random.number([1000, 1050]));
                });
                items.push({
                    name: faker.random.bs_buzz(),
                    locations: locationsIds,
                    id: j
                });
            }

            items = _.uniq(items, function (item) {
                return item.name;
            });

            return _.sortBy(items, function (item) {
                return item.name;
            });
        }

        var seedData = createMockData();
        
        return {
            search: function (q) {
                var categories = _.clone(seedData);
     
                if (q) {
                    return _.filter(categories, function (category) {
                        return category.name.toLowerCase().indexOf(q.toLowerCase()) > -1;
                    });
                } else {
                    return categories;
                }
            },
            saveOrUpdate: function (category, onSuccess) {
                onSuccess();
            }
        };
    }

})();