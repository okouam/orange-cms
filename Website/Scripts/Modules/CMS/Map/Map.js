(function () {

    "use strict";

    angular
        .module("geocms")
        .service("GeoCMS.CMS.Map", [
            Map
        ]);

    function Map() {

        var self = this;

        this.center = {
            latitude: 45,
            longitude: -73
        };

        this.zoom = 8;

        /** Resize the Google Maps container as soon as the map is loaded. **/
        this.tilesloaded = function () {
            self.resizeMapContainer();
        };

        /** Resize the Google Maps container every time the window dimensions change. **/
        $(window).resize(Map.resizeMapContainer);
        
        /** Required as Google Maps doesn't resize the DOM container properly. **/
        this.resizeMapContainer = function() {
            var containerHeight = $("#cms-map").height();
            $("#cms-map .angular-google-map-container").height(containerHeight);
        };

        this.centerOnMarkers = function () {

            var markers = self.getGMarkers();

            if (markers.length > 0) {

                var bounds = new google.maps.LatLngBounds();

                _.each(self.getGMarkers(), function (marker) {
                    var position = marker.getPosition();
                    bounds.extend(position);
                });

                self.getGMap().fitBounds(bounds);
            }
        };
    }

})();