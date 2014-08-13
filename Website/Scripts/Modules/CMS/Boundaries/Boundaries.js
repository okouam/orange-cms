(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Boundaries.Boundaries", [
            "$http",
            Boundaries
        ]);

    function Boundaries($http) {

        var seedData = [];

        for (var i = 1000; i < 1020; i++) {
            seedData.push({
                name: faker.random.uk_county(), id: i
            });
        }

        seedData = _.uniq(seedData, function(item) {
            return item.name;
        });

        seedData = _.sortBy(seedData, function(item) {
            return item.name;
        });

        return {
            search: function (q) {
                var boundaries = _.clone(seedData); 
                if (q) {
                    return _.filter(boundaries, function (boundary) {
                        return boundary.name.toLowerCase().indexOf(q.toLowerCase()) > -1;
                    });
                } else {
                    return boundaries;
                }
            },
            saveOrUpdate: function (boundary, onSuccess) {
                onSuccess();
            }
        };
    }

})();