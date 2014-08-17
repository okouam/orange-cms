(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.CollectionUtils", [
            CollectionUtils
        ]);

    function CollectionUtils() {

        this.replaceContents = function (source, dest) {
            while (dest.length > 0) {
                dest.pop();
            }
            for (var i = 0; i < source.length; i++) {
                dest.push(source[i]);
            }
        };

        this.extend = function (collection) {

            collection.deleteWhere = function (criteria) {
                var items = _.where(collection, criteria);
                _.each(items, function (item) {
                    var index = _.indexOf(collection, item);
                    collection.splice(index, 1);
                });
            };

            collection.selected = function () {
                return _.where(collection, { selected: true });
            };

            collection.clearSelection = function () {
                _.each(collection, function (item) {
                    delete item.selected;
                });
            };
        };
    }

})();