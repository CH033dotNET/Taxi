$(function () {

	var driverLocationHub = $.connection.driverLocationHub;

	var currentPosition = {
		Latitude: null,
		Longitude: null
	};
	var driverId;
	var driverName;
	$.connection.hub.start().done(function () {

		driverId = $('#currentUserId').val();
		driverLocationHub.server.connectUser("Driver");
	    driverName = $('#currentUserName').val();
	});

	function checkPosition(position) {

		if (currentPosition.Latitude != position.coords.latitude || currentPosition.Longitude != position.coords.longitude) {


			driverLocationHub.server.updateDriverPosition(driverId, position.coords.latitude, position.coords.longitude, timeNow, driverName);

			currentPosition.Latitude = position.coords.latitude;
			currentPosition.Longitude = position.coords.longitude;

		}
	}

	$(document).ready(function () {

		setTimeout(function invoke() {
			navigator.geolocation.getCurrentPosition(checkPosition);
			setTimeout(invoke, 3000);
		}, 3000);
	});


});