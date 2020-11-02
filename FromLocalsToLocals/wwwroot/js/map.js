var mapboxAccessToken = "pk.eyJ1IjoiaGFiYWhhYmEwNSIsImEiOiJja2d1a3Q5bHcwdXFoMnJtajc2cWF6dThmIn0.CI3ewBXairOZLqVCecvPEg";
var mapboxAttribution = 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>';
var mapboxUrl = 'https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=' + mapboxAccessToken;
var grayscale = L.tileLayer(mapboxUrl, { id: 'mapbox/light-v9', tileSize: 512, zoomOffset: -1, attribution: mapboxAttribution });
var streets = L.tileLayer(mapboxUrl, {id: 'mapbox/streets-v11',tileSize: 512,zoomOffset: -1,attribution: mapboxAttribution});

var vendorIcon = L.Icon.extend({options: {iconSize: [30, 30],iconAnchor: [15, 30],popupAnchor: [-3, -76]}});
var foodIcon = new vendorIcon({ iconUrl: '/../Assets/food.png'}),
    carrepairIcon = new vendorIcon({ iconUrl: '/../Assets/carrepair.png' }),
    otherIcon = new vendorIcon({ iconUrl: '/../Assets/other.png' });

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

//Map

var map = L.map('mapid', {
    maxZoom: 16,
    minZoom: 6,
    layers: [streets, markers]
}).setView([55.303468, 23.9609414], 6.4);


var baseMaps = {
    "<span style='color: gray'>Grayscale</span>": grayscale,
    "Streets": streets
};

L.control.layers(baseMaps).addTo(map);


$(document).ready(function () {
    addMarkers();
});



//Popup stuff

var info = L.control({ position: 'bottomleft'});

info.onAdd = function (map) {
    this._div = L.DomUtil.create('div', 'info'); // create a div with a class "info"
    this.update();
    return this._div;
};

// method that we will use to update the control based on feature properties passed
info.update = function (props) {
    this._div.innerHTML = props ? (`<div ><button type = 'button' id='mygtukas' class='boxclose' onClick='closeInfoTab()'></button><img src='/../Assets/appLogo.png' width='100%'/><div><hr>
                                <h4>${props.title}</h4><br><a href='/Vendors/Details/${props.id}'>Read More</a>`) : '<span style="color:#916814">Select vendor</span>';   
};

info.addTo(map);


function closeInfoTab() {
    info.update();
}


//Filtering

function vendorTypeOnChange(e) {
    if (e.checked) {
        vendorTypes.push(e.id);
    } else {
        vendorTypes = vendorTypes.filter(x => x != e.id);
    }
    map.removeLayer(markers);
    addMarkers();
}

//Updating markers
function addMarkers() {
    markers.clearLayers();

    for (var i = 0; i < allVendors.length; i++) {

        if (vendorTypes.includes(allVendors[i].VendorTypeDb)) {
            var newMarker = L.marker([allVendors[i].Latitude, allVendors[i].Longitude], {
                title: allVendors[i].Title,
                id: allVendors[i].ID,
                icon: new vendorIcon({ iconUrl: `/../Assets/${allVendors[i].VendorTypeDb}.png` })
            }).bindTooltip(allVendors[i].Title).on('click', function onClick(e) {
                info.update(e.target.options);
            });

            markers.addLayer(newMarker);
        }
    };
    markers.addTo(map);
}
