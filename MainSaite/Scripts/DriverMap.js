var map;
var markers = [];

var Redcar;

var pos;


$(document).ready(function () {

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
		hub.server.connectUser("Operator", $('#currentUserId').val());
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

			//test -- 

			//var testObj = {
			//	name: "Jack",
			//	id: 1,
			//	startedTime: data,
			//	updateTime: data,
			//	latitude: 48.2760595,
			//	longitude: 25.9490
			//};
			//AddDriverToTheTable(testObj);


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
	markers['DriverN' +  id] = AddDriver( name,  latitude,  longitude);

	var tableRow = $('<tr/>', { id: 'DriverN' +  id }).append(
			$('<td/>', { text:  name }),
			$('<td/>', { text: new Date( startedTime).toLocaleString(), id: 'DriverN' +  id + 'start' }),
			$('<td/>', { text: new Date( updateTime).toLocaleString(), id: 'DriverN' +  id + 'up' }));
	tableRow.click(onClick);
	var table = $('#DrvsCont').append(
		tableRow
	);
}

function locationUpdate(Lat, Lng, Time, ID, name) {

	if (markers['DriverN' + ID] !== undefined) {
		markers['DriverN' + ID].setPosition(new google.maps.LatLng(Lat, Lng));
		$('#DriverN' + ID + 'up').html(new Date(Time).toLocaleString());
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