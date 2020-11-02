var mapboxAccessToken = "pk.eyJ1IjoiaGFiYWhhYmEwNSIsImEiOiJja2d1a3Q5bHcwdXFoMnJtajc2cWF6dThmIn0.CI3ewBXairOZLqVCecvPEg";
var mapboxAttribution = 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>';
var mapboxUrl = 'https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=' + mapboxAccessToken;
var grayscale = L.tileLayer(mapboxUrl, { id: 'mapbox/light-v9', tileSize: 512, zoomOffset: -1, attribution: mapboxAttribution });
var streets = L.tileLayer(mapboxUrl, {id: 'mapbox/streets-v11',tileSize: 512,zoomOffset: -1,attribution: mapboxAttribution});

//var vendorIcon = L.Icon.extend({options: {iconSize: [30, 30],iconAnchor: [15, 30],popupAnchor: [-3, -76]}});
//var foodIcon = new vendorIcon({ iconUrl: '/../Assets/food.png'}),
//    carrepairIcon = new vendorIcon({ iconUrl: '/../Assets/carrepair.png' }),
//    otherIcon = new vendorIcon({ iconUrl: '/../Assets/other.png' });

var markers = L.markerClusterGroup({
    spiderfyShapePositions: function (count, centerPt) {
        var distanceFromCenter = 35,
            markerDistance = 45,
            lineLength = markerDistance * (count - 1),
            lineStart = centerPt.y - lineLength / 2,
            res = [],
            i;

        res.length = count;

        for (i = count - 1; i >= 0; i--) {
            res[i] = new Point(centerPt.x + distanceFromCenter, lineStart + markerDistance * i);
        }

        return res;
    },

    showCoverageOnHover: false,
    zoomToBoundsOnClick: true,
    removeOutsideVisibleBounds: true
});


for (var i = 0; i < arrayOfArrays.length; i++) {
    markers.addLayer(L.marker([arrayOfArrays[i].Latitude, arrayOfArrays[i].Longitude])).addTo(markers);
};


var map = L.map('mapid', {
    maxBounds: [
        //south
        [53.5, 23],
        //west
        [55, 20],
        //north
        [57, 24],
        //east
        [55, 27],
    ],
    maxZoom: 16,
    minZoom: 6,
    layers: [streets,markers]
}).setView([55.303468, 23.9609414], 6.4);

var baseMaps = {
    "<span style='color: gray'>Grayscale</span>": grayscale,
    "Streets": streets
};

L.control.layers(baseMaps).addTo(map);
