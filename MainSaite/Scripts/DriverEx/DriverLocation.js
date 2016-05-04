$(function () {

	var driverLocationHub = $.connection.driverLocationHub;

	var currentPosition = {
		Latitude: null,
		Longitude: null
	};
	var driverId;

	$.connection.hub.start().done(function () {

		driverId = $('#currentUserId').val();
		driverLocationHub.server.connect("Driver", driverId);

	});

	function checkPosition(position) {

		if (currentPosition.Latitude != position.coords.latitude && currentPosition.Longitude != position.coords.longitude) {

			driverLocationHub.server.updateDriverPosition(position);

		}
	}

	$(document).ready(function () {

		setTimeout(function invoke() {
			navigator.geolocation.getCurrentPosition(checkPosition);
			setTimeout(invoke, 3000);
		}, 3000);
	});


});