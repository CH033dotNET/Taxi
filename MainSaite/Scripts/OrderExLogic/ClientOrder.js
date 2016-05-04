$(function(){
	var orderHub = $.connection.OrderHub;

	orderHub.client.notifyDriverCoordinate = function (coordinate) {
		setTaxiMarker(coordinate.Latitude, coordinate.Longitude);
	}

	$.connection.hub.start().done(function () {
		//connect to hub group
		orderHub.server.connect("Client");

		

		//add order to db
		$('#orderBtn').click(function () {
			var order = {};
			order.Address = $('#textField').val();
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

})