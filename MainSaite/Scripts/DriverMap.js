var map;
var markers = [];

var Redcar;

var pos;

$(document).ready(function () {
	geocoder = new google.maps.Geocoder();
	mainInit();

});
function mapInit() {
	map = new google.maps.Map(document.getElementById("map"), {
		center: { lat: 48.3214409, lng: 25.8638791 },
		zoom: 8
	})
}

function hubInit() {

	var hub = $.connection.driverLocationHub;

	hub.client.locationUpdate = locationUpdate;
	hub.client.driverStart = driverStart;
	hub.client.driverFinish = driverFinish;
	$.connection.hub.start().done(function () {
		//hub.server.connectUser("Operator", $('#currentUserId').val());
	});
}
//test -----
var ShowCurCoord = function () {

	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(function (position) {
			pos = {
				lat: position.coords.latitude,
				lng: position.coords.longitude,
			};
			var k = 0;
		}, function () {

		});
	}
}

//test--------

function mainInit() {
	mapInit();
	$.ajax({
		type: "GET",
		url: '/Administration/GetLoc/',
		dataType: "json",
		success: function (data) {

			for (var i = 0; i < data.length; i++) {
				AddDriverToTheTable(data[i].latitude, data[i].longitude, data[i].updateTime, data[i].startedTime, data[i].id, data[i].name);
			}
		},
		error: function (error) {
			alert(error.statusText);
		}
	});
	hubInit();
}

function AddDriverToTheTable(latitude, longitude, updateTime, startedTime, id, name ) {
	markers['DriverN' + id] = AddDriver(name, latitude, longitude);

	//new Date(parseInt(startedTime.replace(/\/Date\((-?\d+)\)\//, '$1')))
	if (startedTime.indexOf("Date") > -1) {
		startedTime = new Date(+startedTime.match(/\d+/)[0]);
		updateTime = new Date(+updateTime.match(/\d+/)[0]);
	}

	var tableRow = $('<tr/>', { id: 'DriverN' +  id }).append(
			$('<td/>', { text:  name }),
			$('<td/>', { text: new Date( startedTime).toLocaleString(), id: 'DriverN' +  id + 'start' }),
			$('<td/>', { text: new Date( updateTime).toLocaleString(), id: 'DriverN' +  id + 'up' }));
	//$('<td/>', { text: GetAddressByCoordinates(latitude, longitude) }));
	tableRow.click(onClick);
	var table = $('#DrvsCont').append(
		tableRow
	);
}

function locationUpdate(Lat, Lng, Time, ID, name) {

	if (markers['DriverN' + ID] !== undefined) {
		markers['DriverN' + ID].setPosition(new google.maps.LatLng(Lat, Lng));
		//"2016-05-16T13:42:00"
		//var k = new Date(+time.match(/\d+/)[0]).toLocaleString();

		var n = new Date(Time);
		n.setMinutes(n.getMinutes() + n.getTimezoneOffset());

		//var n = new Date(Time).toLocaleString();

		$('#DriverN' + ID + 'up').html(n.toLocaleString());
	}
	else {
		AddDriverToTheTable(Lat, Lng, Time, moment().format('YYYY/MM/DD HH:mm:ss'), ID, name);
	}
}

function driverStart(val) {
	if (markers['DriverN' + val.id] === undefined) {
		AddDriverToTheTable(val.latitude, val.longitude, val.updateTime, val.startedTime, val.id, val.name);
	}
	else {
		locationUpdate(val.latitude, val.longitude, val.updateTime, val.id);
		$('#DriverN' + val.id + 'start').html(new Date(val.startedTime).toLocaleString());
	}
}

function driverFinish(ID) {
	if (markers['DriverN' + ID] !== undefined) {
		$('#DriverN' + ID).remove();
		markers['DriverN' + ID].setMap(null);
		markers['DriverN' + ID] = undefined;
	}
}

function onClick(data) {
	for (var key in markers) {
		markers[key].setAnimation(null);
	}
	if ($(this).hasClass('bold')) {
		markers[(this).id].setIcon(imagePath + '/cab.png');
		$(this).removeClass('bold');
		Redcar = undefined;
	}
	else {
		if (Redcar !== undefined) {
			Redcar.removeClass('bold');
			markers[Redcar.attr('id')].setIcon(imagePath + '/cab.png');
		}
		markers[(this).id].setIcon(imagePath + '/cabRed.png');
		markers[(this).id].setAnimation(google.maps.Animation.BOUNCE);
		$(this).addClass('bold');
		Redcar = $(this);
	}
}

function AddDriver(name, myLat, myLng) {
	return marker = new google.maps.Marker({
		position: { lat: myLat, lng: myLng },
		map: map,
		title: 'Driver: ' + name,
		icon: {
			url: imagePath + '/cab.png'
		}
	});
}
//function GetAddressByCoordinates(lat, lng) {

//	var latlng = new google.maps.LatLng(lat, lng);
//	var latlng = { lat: lat, lng: lng };
//	geocoder.geocode({
//		'latLng': latlng
//	}, function (results, status) {
	//	if (status === google.maps.GeocoderStatus.OK) {
	//		if (results[1]) {
	//			return results[1];
	//		} else {
	//			return "Unknown street";
	//		}
	//	} else {
	//		return "Unknown street";
	//	}
	//});
//}