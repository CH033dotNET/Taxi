// This example displays an address form, using the autocomplete feature
// of the Google Places API to help users fill in the information.
//var map;
//var startPoint = new google.maps.Marker;
//var markerTaxi = new google.maps.Marker;

//var geocoder = new google.maps.Geocoder();
//var infowindow = new google.maps.InfoWindow;

var mapInfo = {
	map: null,
	startPoint: new google.maps.Marker,
	markerTaxi: new google.maps.Marker,
	geocoder: new google.maps.Geocoder(),
	infowindow : new google.maps.InfoWindow
};


$(document).ready(function () {
	initialize();
})

function receiver(pos1) {

	var trusteddomain = '*';
	if (pos1.origin != "https://www.google.com") {
		var msgcontent = pos1.data;
		console.log(pos1.data);
		var pos = {
			lat: Number(pos1.data.lat),
			lng: Number(pos1.data.lng)
		};
	}
	window.addEventListener('message', receiver, false);
}


function CenterControl(controlDiv, map) {

	// Setup the click event listeners: simply set the map to Chicago.
	$('#controlOrder').on('click', function () {

		var pos = {
			'LatitudePeekPlace': startPoint.position.lat(),
			'LongitudePeekPlace': startPoint.position.lng()
		}

	})

}


function setTaxiMarker(lt, lg) {

	var latlng = { lat: lt, lng: lg };
	mapInfo.markerTaxi.setPosition(latlng);
	mapInfo.markerTaxi.setIcon(picturePath + 'logo_taxi_yellow.png');
	mapInfo.markerTaxi.setMap(mapInfo.map);
}


function handleLocationError(browserHasGeolocation, infowindow, pos) {
	infowindow.setPosition(pos);
	infowindow.setContent(browserHasGeolocation ?
                          'Error: The Geolocation service failed.' :
                          'Error: Your browser doesn\'t support geolocation.');
}




function initMap() {
	mapInfo.map = new google.maps.Map(document.getElementById('map'), {
		zoom: 13,
		streetViewControl: false,
		panControl: null,
		zoomControl: false,
		center: { lat: 48.290718, lng: 25.934960 }
	});


	var input = document.getElementById('autocomplete');
	var searchBox = new google.maps.places.SearchBox(input);
	mapInfo.map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);


	mapInfo.map.addListener('click', function (e) {
		geocodeLatLng(e.latLng, mapInfo.geocoder, mapInfo.map, mapInfo.infowindow);
	});

	ShowCurCoord();

	//var centerControlDiv = document.createElement('div');
	//var centerControl = new CenterControl(centerControlDiv, mapInfo.map);
	//centerControlDiv.index = 1;

	//mapInfo.map.controls[google.maps.ControlPosition.BOTTOM_CENTER].push(centerControlDiv);

	//hubInit();
}

var ShowCurCoord = function () {

	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(function (position) {
			var pos = {
				lat: position.coords.latitude,
				lng: position.coords.longitude,
			};
			mapInfo.infowindow.setPosition(pos);
			mapInfo.map.setCenter(pos);
			var p = position.coords.accuracy;
			//circle = addCircle(map, pos, position.coords.accuracy);
			test1 = new google.maps.Marker({
				position: pos,
				//draggable: true,
				map: mapInfo.map,
				icon: picturePath + 'logo_client.png'
			});
			mapInfo.startPoint = test1;

			//google.maps.event.addListener(startPoint, 'dragend', function () { setTitle(startPoint); });

			setTitle(mapInfo.startPoint);

		}, function () {

			$("#geoOff").hide().show('medium');

		});
	}
}
function addOnUserPageDriver(myLat, myLng) {
	return marker = new google.maps.Marker({
		position: { lat: myLat, lng: myLng },
		map: mapInfo.map,
		title: 'Hello World!',
		icon: {
			url: picturePath + '/cab.png'
		}
	});
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

				mapInfo.startPoint.setMap(null);

				var test = new google.maps.Marker({
					position: latlng,
					map: mapInfo.map,
					icon: picturePath + 'logo_client.png',

				});

				var pos = {
					lat: latlng.lat,
					lng: latlng.lng,
				};

				mapInfo.startPoint = test;

				setTitle(mapInfo.startPoint);

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
}

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

	var addressLabel = document.getElementById('autocomplete').value;

	if (addressLabel != "") {
		mapInfo.geocoder.geocode({ 'address': addressLabel }, function (results, status) {
			if (status == google.maps.GeocoderStatus.OK) {
				mapInfo.startPoint.setMap(null);
				mapInfo.map.setCenter(results[0].geometry.location);
				mapInfo.startPoint = new google.maps.Marker({
					map: mapInfo.map,
					position: results[0].geometry.location,
					icon: picturePath + 'logo_client.png',
				});

				setTitle(mapInfo.startPoint);
			}
			else {
				alert("Geocode was not successful for the following reason: " + status);
			}
		});
	}
}

$("#autocomplete").keypress(function (e) {
	if (e.which == 13)
		geocode();

});

$(".address").on("click", function () {
	$("#autocomplete").val($(this).text());
	$("#show-map").click();
	$("#OkBtn").click();
	$("#responsive-menu-btn").click();
});



var setTitle = function (mark) {
	var latlng = mark.getPosition();
	mapInfo.geocoder.geocode({
		'latLng': latlng
	}, function (results, status) {
		if (status === google.maps.GeocoderStatus.OK) {
			if (results[1]) {
				mark.setTitle(results[0].formatted_address);
				document.getElementById('autocomplete').value = mapInfo.startPoint.getTitle();

			} else {
				console.log('No results found');
			}
		} else {
			console.log('Geocoder failed due to: ' + status);
		}
	});
}
