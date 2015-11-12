// This example displays an address form, using the autocomplete feature
// of the Google Places API to help users fill in the information.
var map;
var marker1 = new google.maps.Marker;
var marker2 = new google.maps.Marker;
var geocoder = new google.maps.Geocoder();
var infowindow = new google.maps.InfoWindow;
var circle;
var address;
var markers = new Array();

var markerTaxi = new google.maps.Marker;
var myOrderId;
var intervalID;

function hubInit() {
    var hub = $.connection.driversLocationHub;//Подключились к хабу

    hub.client.locationUpdate = locationUpdate;//присобачили функцию клиента
    hub.client.driverStartOnUserPage = driverStartOnUserPage;
    hub.client.driverFinishUserPage = driverFinishUserPage;

    $.connection.hub.start().done(function () {
        // hub.server.Hello(); // вызов функции сервера
    });
}


function CenterControl(controlDiv, map) {

    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    controlUI.style.backgroundColor = 'Yellow';
    controlUI.style.border = '2px solid #fff';
    controlUI.style.borderRadius = '3px';
    controlUI.style.boxShadow = '0 2px 6px rgba(0,0,0,.3)';
    controlUI.style.cursor = 'pointer';
    controlUI.style.marginBottom = '22px';
    controlUI.style.textAlign = 'center';
    controlUI.title = 'Click to get a taxi';
    controlDiv.appendChild(controlUI);
    
   
    // Set CSS for the control interior.
    var controlText = document.createElement('div');
    controlText.style.color = 'rgb(25,25,25)';
    controlText.style.fontFamily = 'Roboto,Arial,sans-serif';
    controlText.style.fontSize = '20px';
    controlText.style.lineHeight = '38px';
    controlText.style.paddingLeft = '5px';
    controlText.style.paddingRight = '5px';
    controlText.innerHTML = 'Order a Taxi';
    controlUI.appendChild(controlText);

    // Setup the click event listeners: simply set the map to Chicago.
    controlUI.addEventListener('click', function () {
        map.setCenter(geocode());
      
        if (myOrderId == null) {
            var orderObj = {
                'PeekPlace': marker2.getTitle(),
                'DropPlace': marker1.getTitle(),
                'OrderTime': new Date().toISOString(),
                'LatitudeDropPlace': marker1.position.lat(),
                'LongitudeDropPlace': marker1.position.lng(),
                'Accuracy': circle.getRadius(),
                'LatitudePeekPlace': marker2.position.lat(),
                'LongitudePeekPlace': marker2.position.lng(),
                'IsConfirm': 3
            }
            $.ajax({
                url: '/Order/GetOrder/',
                data: orderObj,
                type: "POST",
                success: function (data) {
                    myOrderId = data;
                    intervalID = setInterval(getMyTaxi, 2000);
                }
            });
        }

        else
        {
            alert('Ви вже замовили таксі, чекайте на відповідь оператора!');
        }
    })

}

function getMyTaxi() {

    $.ajax({
        url: "/Order/GetOrderedTaxi/",
        data: { orderId: myOrderId },
        type: 'POST',

        success: function (data) {
            
            switch (data.IsConfirm) {
               case 4: {
                    clearInterval(intervalID);
                    myOrderId = null;
                    $('#waittime').val(d.WaitingTime);
                    $('#orderinfo').modal('toggle');
                    setTaxiMarker(d.Latitude, d.Longitude);
                    break;
                };
                case 2: {
                    clearInterval(intervalID);
                    myOrderId = null;
                    $('#deniedorderinfo').modal('toggle');
                    break;
                }
                case 5:
                    {
                        clearInterval(intervalID);
                        myOrderId = null;
                        $('#nocarorderinfo').modal('toggle');
                        break;
                    }
            }
        },
        error: function (error) {
            alert("error!" + error);
        }
    });
}


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
    circle = new google.maps.Circle(circleOptions);
    return circle;
};

var setTaxiMarker = function(lt, lg)
{

    var latlng = { lat: lt, lng: lg };
    markerTaxi.setPosition(latlng);
    markerTaxi.setIcon(picturePath + 'logo_taxi_yellow.png');
    markerTaxi.setAnimation(google.maps.Animation.BOUNCE);
    markerTaxi.setMap(map);
}


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
   

    var input = document.getElementById('autocomplete');
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);


    map.addListener('click', function (e) {
        geocodeLatLng(e.latLng, geocoder, map, infowindow);
    });

    ShowCurCoord();
    var centerControlDiv = document.createElement('div');
    var centerControl = new CenterControl(centerControlDiv, map);
    centerControlDiv.index = 1;
    map.controls[google.maps.ControlPosition.BOTTOM_CENTER].push(centerControlDiv);

    $.ajax({
        type: "POST",
        url: "Administration/GetLoc",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var val = data[i];
                markers['DriverN' + val.id] = addOnUserPageDriver(val.latitude, val.longitude);
                var tr = $('<tr/>', { id: 'DriverN' + val.id }).append(
                        $('<td/>', { text: val.name }),
                        $('<td/>', { text: new Date(+val.startedTime.match(/\d+/)[0]).toLocaleString(), id: 'DriverN' + val.id + 'start' }),
                        $('<td/>', { text: new Date(+val.updateTime.match(/\d+/)[0]).toLocaleString(), id: 'DriverN' + val.id + 'up' }));
                //tr.marker = adddriver(val.latitude, val.longitude);
                tr.click(onclic);
                var table = $('#DrvsCont').append(
                    tr
                );
            }
        },
        error: function (error) {
            alert(error);
        }
    });
    hubInit();
}

function locationUpdate(Lat, Lng, Time, ID) {
    if (markers['DriverN' + ID] !== undefined) {
        markers['DriverN' + ID].setPosition(new google.maps.LatLng(Lat, Lng));
        $('#DriverN' + ID + 'up').html(new Date(Time).toLocaleString());
    }
}

function driverStartOnUserPage(val) {
    if (markers['DriverN' + val.id] === undefined) {
        markers['DriverN' + val.id] = addOnUserPageDriver(val.latitude, val.longitude);
        var tr = $('<tr/>', { id: 'DriverN' + val.id }).append(
                $('<td/>', { text: val.name }),
                $('<td/>', { text: new Date(val.startedTime).toLocaleString(), id: 'DriverN' + val.id + 'start' }),
                $('<td/>', { text: new Date(val.updateTime).toLocaleString(), id: 'DriverN' + val.id + 'up' }));
        tr.click(onclic);
        var table = $('#DrvsCont').append(tr);
    }
    else {
        locationUpdate(val.latitude, val.longitude, val.updateTime, val.id);
        $('#DriverN' + val.id + 'start').html(new Date(val.startedTime).toLocaleString());
    }
}
function driverFinishUserPage(ID) {
    $('#DriverN' + ID).remove();
    markers['DriverN' + ID].setMap(null);
    markers['DriverN' + ID] = undefined;
}

var ShowCurCoord = function () {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude,
            };
            infowindow.setPosition(pos);
            map.setCenter(pos);
            circle = addCircle(map, pos, position.coords.accuracy);
            test1 = new google.maps.Marker({
                position: pos,
                draggable:true,
                map: map,
                icon: picturePath + 'logo_client.png'
            });
            marker2 = test1;

            google.maps.event.addListener(marker2, 'dragend', function () { setTitle(marker2); });
        


            setTitle(marker2);
        }, function () {
            // handleLocationError(true, infoWindow, map.getCenter());
        });
    }
}
function addOnUserPageDriver(myLat, myLng) {
    return marker = new google.maps.Marker({
        position: { lat: myLat, lng: myLng },
        map: map,
        title: 'Hello World!',
        icon: {
            url: picturePath + '/cab.png'
        }
    });
}

var ShowFakeCoord = function (pos) {
            infowindow.setPosition(pos);
            map.setCenter(pos);
            marker2.setMap(null);
            circle.setMap(null);
            circle = addCircle(map, pos, 200);
            test1 = new google.maps.Marker({
                position: pos,
                map: map,
                icon: picturePath + 'logo_client.png'
            });
            marker2 = test1;
            setTitle(marker2);
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


                marker1.setMap(null);

                var test = new google.maps.Marker({
                    position: latlng,
                    map: map,
                    icon: picturePath + 'logo_destination.png',
                   
                });


                marker1 = test;
                setTitle(marker1);
                infowindow.setContent(results[0].formatted_address);
                
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
var curentPossitionLTD;
var curentPossitionLaGD;
function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = new google.maps.LatLng(
             curentPossitionLTD = position.coords.latitude, curentPossitionLoGD = position.coords.longitude);
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
            marker1.setMap(null);
            map.setCenter(results[0].geometry.location);
            marker1 = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location,
                icon: picturePath + 'logo_destination.png',
            });

            setTitle(marker1);
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

$(".address").on("click", function () {
    $("#autocomplete").val($(this).text());
    $("#show-map").click();
    $("#OkBtn").click();
    $("#responsive-menu-btn").click();
});



var setTitle = function (mark) {
    var latlng = mark.getPosition();
    geocoder.geocode({
        'latLng': latlng
    }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                mark.setTitle(results[0].formatted_address);
            } else {
                alert('No results found');
            }
        } else {
            alert('Geocoder failed due to: ' + status);
        }
    });
}


    


