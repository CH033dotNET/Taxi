$(document).ready(function () {
	var orderHub = $.connection.OrderHub;

	orderHub.client.notifyDriverCoordinate = function (coordinate) {
		setTaxiMarker(coordinate.Latitude, coordinate.Longitude);
	}

	orderHub.client.OrderConfirmed = function (WaitingTime) {
		if (window.Notification && Notification.permission !== "denied") {
			Notification.requestPermission(function (status) {  // status is "granted", if accepted by user
				var n = new Notification('Waiting Time', {
					body: 'Wait for ' + WaitingTime + ' minutes, please.',
				});
			});
		}
	}

	$.connection.hub.start().done(function () {
		//connect to hub group
		orderHub.server.connect("Client");


		//add order to db
		$(document).on('click', '#orderBtn', function () {
			var order = {};
			order.Address = $('#autocomplete').val();
			$.ajax({
				url: '/Client/AddOrder/',
				data: order,
				type: "POST",
				success: function (data) {
					//send order
					orderHub.server.addOrder(data);
					alert("You order id = " + data.Id);
				}
			});

		});
	});

});