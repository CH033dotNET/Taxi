$(function () {
	var orderHub = $.connection.OrderHub;
	var prevCoord = {
		Latitude: null,
		Longitude: null
	};
	var currentOrderId;

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
		orderHub.server.connect("Driver");

		//take order
		$('.take').click(function (e) {
			var row = $(this).closest('tr');
			currentOrderId = +$(this).attr('itemId');
			var waiting_time = row.find('.waiting-time').first().val();
			console.log(waiting_time);
			if (currentOrderId) {
				$.ajax({
					type: "post",
					url: "/DriverEX/TakeOrder/",
					data: {
						id: currentOrderId,
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



	function sentCoord(position) {
		var data = {};
		data.Latitude = position.coords.latitude;
		data.Longitude = position.coords.longitude;
		data.Accuracy = position.coords.accuracy;
		data.AddedTime = moment().format('YYYY/MM/DD HH:mm:ss');
		data.OrderId = currentOrderId;

		if (prevCoord.Latitude != data.Latitude && prevCoord.Longitude != data.Longitude) {
			prevCoord = data;
			$.ajax({
				url: '/DriverEx/SetCoordinate',
				method: 'POST',
				data: data
			});
		}
		if(currentOrderId){
			orderHub.server.notifyDriverCoordinate(data);
		}
	}

	$(document).ready(function () {
		setTimeout(function run() {
			navigator.geolocation.getCurrentPosition(sentCoord);
			setTimeout(run, 2000);
		}, 2000);
	});

	
});