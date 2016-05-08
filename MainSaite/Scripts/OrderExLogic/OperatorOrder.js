$(document).ready(function () {
	var orderHub = $.connection.OrderHub;

	orderHub.client.addOrder = function (order) {
		$('#orders').append("<tr>\
			<td>"
			+ order.Address +
			"</td>\
			<td>\
				<input itemid=" + order.Id + " type=\"button\" value=\"Approve\" class=\"approve btn btn-success\" /> |\
				<input itemid=" + order.Id + " type=\"button\" value=\"Deny\" class=\"deny btn btn-danger\" />\
			</td>\
		</tr>")
	};

	$.connection.hub.start().done(function () {

		//connect to hub group
		orderHub.server.connect("Operator");

		//approve order
		$(document).on('click', '.approve', function (e) {
			var row = $(this).closest('tr');
			var id = +$(this).attr('itemId');
			if (id) {
				$.ajax({
					type: "post",
					url: "/OrderEx/ApproveOrder/",
					data: { id: id },
					success: function (result) {
						if (result) {
							$(row).fadeOut();
							orderHub.server.OrderApproved(id);
						}
						else {
							alert("something wrong");
						}
					}
				});
			}
		});

		//deny order
		$(document).on('click', '.deny', function (e) {
			var row = $(this).closest('tr');
			var id = +$(this).attr('itemId');
			if (id) {
				$.ajax({
					type: "post",
					url: "/OrderEx/DenyOrder/",
					data: { id: id },
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
});