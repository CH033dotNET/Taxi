// This example displays an address form, using the autocomplete feature
// of the Google Places API to help users fill in the information.
var map;
//var finishPoint = new google.maps.Marker;
var startPoint = new google.maps.Marker;
//var markerTaxi = new google.maps.Marker;

var geocoder = new google.maps.Geocoder();
var infowindow = new google.maps.InfoWindow;
var circle;
//var counter = 0;
//var markers = [];

var myOrderId;
//var intervalID;
var newOrder;
var operatorHub;
//function hubInit() {

//	var hub = $.connection.driversLocationHub;//Подключились к хабу

//	hub.client.locationUpdate = locationUpdate;//присобачили функцию клиента
//	hub.client.driverStartOnUserPage = driverStartOnUserPage;
//	hub.client.driverFinishUserPage = driverFinishUserPage;
//}


$(document).ready(function () {

	$("#geoOff").hide();

	initialize();

	//operatorHub = $.connection.OperatorHub;

	//operatorHub.client.deniedClientOrder = function () {
	//	debugger;
	//	myOrderId = null;
	//	$('#deniedorderinfo').modal('toggle');
	//}

	//operatorHub.client.noFreeCar = function () {
	//	myOrderId = null;
	//	$('#nocarorderinfo').modal('toggle');
	//}

	//operatorHub.client.waitYourCar = function (waitingTime, lat, lng) {

	//	whereMyDriver(myOrderId);
	//	$('#waittime').val(waitingTime);
	//	$('#orderinfo').modal('toggle');
	//	setTaxiMarker(lat, lng);
	//	myOrderId = null;
	//}

	//$.connection.hub.start().done(function () {
	//	// LogIn

	//	var clientRoleId = 3;
	//	var clientUserId = $('#currentUserId').val();

	//	operatorHub.server.connectUser(clientRoleId, clientUserId);


	//});
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
		//ShowFakeCoord(pos);
	}
	window.addEventListener('message', receiver, false);
}


function CenterControl(controlDiv, map) {

	// Set CSS for the control border.

	//controlUI = document.createElement('div');
	//controlUI.id = "controlOrder";

	//controlUI.style.backgroundColor = '#5cb85c';
	//controlUI.style.border = '2px solid #4cae4c';
	//controlUI.style.borderRadius = '3px';
	//controlUI.style.boxShadow = '0 2px 6px rgba(0,0,0,.3)';
	//controlUI.style.cursor = 'pointer';
	//controlUI.style.marginBottom = '22px';
	//controlUI.style.textAlign = 'center';
	//controlUI.title = 'Click to get a taxi';
	//controlDiv.appendChild(controlUI);


	// Set CSS for the control interior.
	//var controlText = document.createElement('div');
	//controlText.style.color = 'white';
	//controlText.style.fontFamily = 'Roboto,Arial,sans-serif';
	//controlText.style.fontSize = '20px';
	//controlText.style.lineHeight = '38px';
	//controlText.style.paddingLeft = '5px';
	//controlText.style.paddingRight = '5px';
	//controlText.innerHTML = 'Order Taxi';

	//controlUI.appendChild(controlText);

	// Setup the click event listeners: simply set the map to Chicago.
	$('#controlOrder').on('click', function () {

		//alert("Order");

		map.setCenter(geocode());

		//if (finishPoint.getTitle() == null) {
		//	$('#noDestPoint').modal('toggle');
		//	return false;
		//}



		if (myOrderId == null) {
			//debugger;
			var orderObj = {
				'PeekPlace': startPoint.getTitle(),
				'DropPlace': finishPoint.getTitle(),
				'OrderTime': new Date().toISOString(),
				'LatitudeDropPlace': finishPoint.position.lat(),
				'LongitudeDropPlace': finishPoint.position.lng(),
				'Accuracy': circle.getRadius(),
				'LatitudePeekPlace': startPoint.position.lat(),
				'LongitudePeekPlace': startPoint.position.lng(),
				'IsConfirm': 3
			}

			$.ajax({
				url: './Order/NewOrder/',
				data: orderObj,
				type: "POST",
				success: function (data) {
					myOrderId = data.Id;
					//debugger;
					operatorHub.server.sendNewOrderToOperators(data);
				}
			});
		}

		else {
			//debugger;
			$('#hasorder').modal('toggle');
		}
	})

}



//var addCircle = function (map, coordinates, accuracy) {

//	var circleOptions = {
//		center: coordinates,
//		clickable: false,
//		fillColor: "blue",
//		fillOpacity: 0.15,
//		map: map,
//		radius: accuracy,
//		strokeColor: "blue",
//		strokeOpacity: 0.3,
//		strokeWeight: 2
//	};
//	circle = new google.maps.Circle(circleOptions);
//	return circle;
//};

var setTaxiMarker = function (lt, lg) {

	var latlng = { lat: lt, lng: lg };
	markerTaxi.setPosition(latlng);
	markerTaxi.setIcon(picturePath + 'logo_taxi_yellow.png');
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

	//hubInit();
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
			var p = position.coords.accuracy;
			//circle = addCircle(map, pos, position.coords.accuracy);
			test1 = new google.maps.Marker({
				position: pos,
				//draggable: true,
				map: map,
				icon: picturePath + 'logo_client.png'
			});
			startPoint = test1;

			//google.maps.event.addListener(startPoint, 'dragend', function () { setTitle(startPoint); });

			setTitle(startPoint);

		}, function () {

			$("#geoOff").hide().show('medium');

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

//var ShowFakeCoord = function (pos) {
//	infowindow.setPosition(pos);
//	map.setCenter(pos);
//	startPoint.setMap(null);
//	circle.setMap(null);
//	circle = addCircle(map, pos, 200);
//	test1 = new google.maps.Marker({
//		position: pos,
//		map: map,
//		icon: picturePath + 'logo_client.png'
//	});
	//startPoint = test1;
//	setTitle(startPoint);
//}



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

				startPoint.setMap(null);

				var test = new google.maps.Marker({
					position: latlng,
					map: map,
					icon: picturePath + 'logo_client.png',

				});

				var pos = {
					lat: latlng.lat,
					lng: latlng.lng,
				};

				//circle = addCircle(map, pos, 20);


				startPoint = test;

				setTitle(startPoint);

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

	var addressLabel = document.getElementById('autocomplete').value;

	if (addressLabel != "") {
		geocoder.geocode({ 'address': addressLabel }, function (results, status) {
			if (status == google.maps.GeocoderStatus.OK) {
				startPoint.setMap(null);
				map.setCenter(results[0].geometry.location);
				startPoint = new google.maps.Marker({
					map: map,
					position: results[0].geometry.location,
					icon: picturePath + 'logo_client.png',
				});

				setTitle(startPoint);
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
	geocoder.geocode({
		'latLng': latlng
	}, function (results, status) {
		if (status === google.maps.GeocoderStatus.OK) {
			if (results[1]) {
				mark.setTitle(results[0].formatted_address);
				document.getElementById('autocomplete').value = startPoint.getTitle();

			} else {
				console.log('No results found');
			}
		} else {
			console.log('Geocoder failed due to: ' + status);
		}
	});
}
