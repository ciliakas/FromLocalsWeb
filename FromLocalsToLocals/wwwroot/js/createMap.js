var mapboxAccessToken = "pk.eyJ1IjoiaGFiYWhhYmEwNSIsImEiOiJja2d1a3Q5bHcwdXFoMnJtajc2cWF6dThmIn0.CI3ewBXairOZLqVCecvPEg";
var mapboxAttribution =
    'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>';
var mapboxUrl = "https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=" + mapboxAccessToken;
var grayscale = L.tileLayer(mapboxUrl,
    { id: "mapbox/light-v9", tileSize: 512, zoomOffset: -1, attribution: mapboxAttribution });
var streets = L.tileLayer(mapboxUrl,
    { id: "mapbox/streets-v11", tileSize: 512, zoomOffset: -1, attribution: mapboxAttribution });

var map = L.map("mapid",
    {
        maxZoom: 16,
        minZoom: 6,
        layers: [streets]
    }).setView(center, zoom);