$(function () {

	//var driverLocationHub = $.connection.driverLocationHub;

	var currentPosition = {
		Latitude: null,
		Longitude: null
	};
	var driverId;
	var driverName;



	$.connection.hub.start().done(function () {

		

		driverId = $('#currentUserId').val();
		//driverLocationHub.server.connectUser("Driver", driverId);
		driverName = $('#currentUserName').val();

		setTimeout(function invoke() {
			navigator.geolocation.getCurrentPosition(checkPosition);
			setTimeout(invoke, 5000);
		}, 5000);

	});


	function checkPosition(position) {

		if (currentPosition.Latitude != position.coords.latitude || currentPosition.Longitude != position.coords.longitude) {
			//driverLocationHub.server.updateDriverPosition(driverId, position.coords.latitude, position.coords.longitude, driverName);
			var data = {};
			data.Latitude = position.coords.latitude;
			data.Longitude = position.coords.longitude;
			data.Accuracy = position.coords.accuracy;
			data.AddedTime = moment().format('YYYY/MM/DD HH:mm:ss');
			data.driverId = driverId;
			prevCoord = data;
			$.ajax({
				url: './DriverEx/UpdateCoords',
				method: 'POST',
				data: data
			});
			currentPosition.Latitude = position.coords.latitude;
			currentPosition.Longitude = position.coords.longitude;

		}
	}
});