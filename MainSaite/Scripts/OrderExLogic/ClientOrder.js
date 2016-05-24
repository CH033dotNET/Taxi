$(document).ready(function () {
	var orderHub = $.connection.MainHub;

	orderHub.client.notifyDriverCoordinate = function (coordinate) {
		setTaxiMarker(coordinate.Latitude, coordinate.Longitude);
	}

	orderHub.client.OrderConfirmed = function (OrderId, WaitingTime) {
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
			$('#orderBtn').prop('disabled', true);
			$('#orderBtn').parent().removeClass('activeBtn');
			var order = {};
			order.AddressFrom = {};
			order.AddressFrom.Address = $('#textField').val();
			$.ajax({
				url: '/Client/AddOrder/',
				contentType: "application/json",
				data: JSON.stringify(order),
				type: "POST",
				success: function (data) {
					orderHub.server.addOrder(data);
					$('#denied-message').hide();
					$('#approved-message').hide();
					$('#waiting-message').slideDown(200);
				}
			});

		});
	});

});
