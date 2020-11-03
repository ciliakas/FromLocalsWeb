var marker = new L.marker(5, 5);

map.on('click', function (e) {
    map.removeLayer(marker);
    marker = new L.marker(e.latlng, { draggable: true });
    map.addLayer(marker);
});

var geocoder = new google.maps.Geocoder();

function convertAddressToLatLng() {
    var address = document.getElementById('search_input').value;
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == 'OK') {
            var coord = JSON.stringify(results[0].geometry.location);
            var obj = JSON.parse(coord);
            map.removeLayer(marker);
            marker = new L.marker([obj.lat, obj.lng], { draggable: true });
            map.addLayer(marker);
        } else {
            alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}

function convertLatLngToAddress() {
    geocoder.geocode({ location: marker.getLatLng() }, (results, status) => {
        if (status === "OK") {
            if (results[0]) {
                var address = results[0].formatted_address;
                document.getElementById('search_input').value = address;
            } else {
                alert("No results found");
            }
        } else {
            alert("Geocoder failed due to: " + status);
        }
    });
}

$(document).ready(function () {
    var autocomplete;
    autocomplete = new google.maps.places.Autocomplete((document.getElementById('search_input')), {
        types: ['geocode'],
        componentRestrictions: {
            country: "lt"
        }
    });

    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        var near_place = autocomplete.getPlace();
    });
});