// This example displays an address form, using the autocomplete feature
// of the Google Places API to help users fill in the information.
var map;
var marker = new google.maps.Marker;
var geocoder = new google.maps.Geocoder();


var addCircle = function (map, coordinates, accuracy) {
    var circleOptions = {
        center: coordinates,
        clickable: false,
        fillColor: "blue",
        fillOpacity: 0.15,
        map: map,
        radius: accuracy,
        strokeColor: "blue",
        strokeOpacity: 0.3,
        strokeWeight: 2
    };

    return new google.maps.Circle(circleOptions);
};


function handleLocationError(browserHasGeolocation, infowindow, pos) {
    infowindow.setPosition(pos);
    infowindow.setContent(browserHasGeolocation ?
                          'Error: The Geolocation service failed.' :
                          'Error: Your browser doesn\'t support geolocation.');
}




function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 13,
        streetViewControl: false,
        panControl: null,
        zoomControl: false,
        center: { lat: 48.290718, lng: 25.934960 }
    });
    var geocoder = new google.maps.Geocoder;
    var infowindow = new google.maps.InfoWindow;

    var input = document.getElementById('autocomplete');
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);


    map.addListener('click', function (e) {
        geocodeLatLng(e.latLng, geocoder, map, infowindow);
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude,
            };
            infowindow.setPosition(pos);
            infowindow.setContent('Location found.');
            map.setCenter(pos);
            addCircle(map, pos, position.coords.accuracy);
            test1 = new google.maps.Marker({
                position: pos,
                map: map
            });
            marker.setMap(test1);
        }, function () {
            // handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        // handleLocationError(false, infoWindow, map.getCenter());
    }

}

var placeSearch, autocomplete;
var componentForm = {
    street_number: 'short_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'short_name',
    country: 'long_name',
    postal_code: 'short_name'
};

function geocodeLatLng(LatLong, geocoder, map, infowindow) {
    var lon = LatLong.lat();
    var lng = LatLong.lng();
    var latlng = { lat: lon, lng: lng };
    geocoder.geocode({ 'location': latlng }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            if (results[1]) {


                marker.setMap(null);

                var test = new google.maps.Marker({
                    position: latlng,
                    map: map
                });


                marker = test;
                infowindow.setContent(results[0].formatted_address);

                //shows adrees text
                infowindow.open(map, marker);
                document.getElementById('autocomplete').value = results[0].formatted_address;
            } else {
                window.alert('No results found');
            }
        } else {
            window.alert('Geocoder failed due to: ' + status);
        }
    });
}

function initialize() {
    // Create the autocomplete object, restricting the search
    // to geographical location types.
    initMap();
    autocomplete = new google.maps.places.Autocomplete(
        /** @type {HTMLInputElement} */(document.getElementById('autocomplete')),
        { types: ['geocode'] });
    // When the user selects an address from the dropdown,
    // populate the address fields in the form.
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        fillInAddress();
    });



}

// [START region_fillform]
function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();

    for (var component in componentForm) {
        document.getElementById(component).value = '';
        document.getElementById(component).disabled = false;
    }

    // Get each component of the address from the place details
    // and fill the corresponding field on the form.
    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        if (componentForm[addressType]) {
            var val = place.address_components[i][componentForm[addressType]];
            document.getElementById(addressType).value = val;
        }
    }
}
// [END region_fillform]

// [START region_geolocation]
// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = new google.maps.LatLng(
                position.coords.latitude, position.coords.longitude);
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}





//Get from input our adress, convert it in coordinate, set point
geocode = function () {
    var address = document.getElementById('autocomplete').value;
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            marker.setMap(null);
            map.setCenter(results[0].geometry.location);
            marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });
        }
        else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
}

$("#autocomplete").keypress(function (e) {
    if (e.which == 13)
        $("#OkBtn").trigger('click');

});

$("a").on("click", function () {
    $("#autocomplete").val($(this).text());
    $("#OkBtn").click();//('click');
    $("#responsive-menu-btn").click();
});

