/* globals google: true */

(function () {

    "use strict";

    function Map() {

        var vm = this;
        var counter = 1;

        this.center = {
            latitude: 5,
            longitude: -4.5
        };

        this.zoom = 8;

        /** Resize the Google Maps container as soon as the map is loaded. **/
        this.tilesloaded = function () {
            vm.resizeMapContainer();
        };

        vm.polygons = [];

        /** Resize the Google Maps container every time the window dimensions change. **/
        $(window).resize(Map.resizeMapContainer);
        
        /** Required as Google Maps doesn't resize the DOM container properly. **/
        this.resizeMapContainer = function() {
            var containerHeight = $("#cms-map").height();
            $("#cms-map .angular-google-map-container").height(containerHeight);
        };
        
        this.showBoundary = function (wkt) {
            this.hideBoundaries();
            var shape = wkt.substring(wkt.indexOf(";") + 1);
            var coordinates = _.map($.geo._WKT.parse(shape).coordinates[0], function(coords) {
                return { longitude: coords[0], latitude: coords[1] };
            });
            vm.polygons.push({
                id: counter++,
                path: coordinates,
                stroke: {
                    opacity: 0.1,
                    color: "#6060FB",
                    weight: 3
                },
                fill: {
                    opacity: 0.1,
                    color: "#ff0000"
                } 
            });
            console.log(vm.polygons[0].path);
        };

        this.hideBoundaries = function () {
            vm.polygons.pop();
        };

        this.centerMap = function(longitude, latitude) {
            vm.getGMap().panTo(new google.maps.LatLng(longitude, latitude));
            vm.getGMap().setZoom(19);
        };

        this.centerOnMarkers = function () {

            var markers = vm.getGMarkers();

            console.log(markers);

            if (markers.length > 0) {

                var bounds = new google.maps.LatLngBounds();

                _.each(markers, function (marker) {
                    var position = marker.getPosition();
                    bounds.extend(position);
                });

                vm.getGMap().fitBounds(bounds);
            }
        };
    }

    angular
    .module("geocms")
    .service("GeoCMS.CMS.Map", [
        Map
    ]);

})();