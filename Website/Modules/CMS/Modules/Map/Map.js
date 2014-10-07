/* globals google: true */

(function () {

    "use strict";

    function Map() {

        var vm = this;

        this.center = {
            latitude: 5,
            longitude: -4.5
        };

        this.zoom = 8;

        /** Resize the Google Maps container as soon as the map is loaded. **/
        this.tilesloaded = function () {
            vm.resizeMapContainer();
        };

        this.polygons = [];

        /** Resize the Google Maps container every time the window dimensions change. **/
        $(window).resize(Map.resizeMapContainer);
        
        /** Required as Google Maps doesn't resize the DOM container properly. **/
        this.resizeMapContainer = function() {
            var containerHeight = $("#cms-map").height();
            $("#cms-map .angular-google-map-container").height(containerHeight);
        };
        
        this.showBoundary = function() {
            var polygon = {
                path: "",
                stroke: "",
                fill: ""
            };
            vm.polygons.push(polygon);
        };

        this.hideBoundary = function() {
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