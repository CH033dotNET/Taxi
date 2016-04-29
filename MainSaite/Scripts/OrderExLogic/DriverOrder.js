$(function () {
	var orderHub = $.connection.OrderHub;

	orderHub.client.OrderApproved = function (order) {
		$('#orders').append("<tr>\
			<td>"
			+ order.Address +
			"</td>\
			<td>\
				<input itemid=" + order.Id + " type=\"button\" value=\"Take\" class=\"take btn btn-success\" />\
			</td>\
		</tr>")
	};

	$.connection.hub.start().done(function () {

		//connect to hub group
		orderHub.server.connect("Operator");

		//take order
		$('.take').click(function (e) {
			var row = $(this).closest('tr');
			var id = +$(this).attr('itemId');
			var waiting_time = row.find('.waiting-time').first().val();
			console.log(waiting_time);
			if (id) {
				$.ajax({
					type: "post",
					url: "./DriverEX/TakeOrder/",
					data: {
					    id: id,
					    WaitingTime: waiting_time
					},
					success: function (result) {
						if (result) {
							$(row).fadeOut();
						}
						else {
							alert("something wrong");
						}
					}
				});
			}
		});

	});
})