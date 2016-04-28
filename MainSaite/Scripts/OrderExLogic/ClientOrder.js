$(function(){
	var orderHub = $.connection.OrderHub;

	$.connection.hub.start().done(function () {

		//connect to hub group
		orderHub.server.connect("Client");

		//add order to db
		$('#order').click(function () {
			var order = {};
			order.Address = $('#Address').val();
			$.ajax({
				url: './Client/AddOrder/',
				data: order,
				type: "POST",
				success: function (data) {
					//send order
					orderHub.server.addOrder(order);
					alert("You order id = " + data.Id);
				}
			});
			
		});
	});


})