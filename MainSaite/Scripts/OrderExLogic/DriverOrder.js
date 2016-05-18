$(function () {
	var orderHub = $.connection.MainHub;
	var prevCoord = {
		Latitude: null,
		Longitude: null
	};
	var currentOrderId;

	orderHub.client.OrderApproved = function (id) {
		$.ajax({
			url: '/OrderEx/GetOrder/',
			data: {
				id:id,
			},
			type: "POST",
			success: function (order) {

				var orderblock="";
				orderblock += "<tr>";
				orderblock += "    <td>";
				orderblock += order.success.Address;
				orderblock += "    <\/td>";
				orderblock += "    <td>";
				orderblock += "        <div class=\"input-group\">";
				orderblock += "            <input type=\"number\" class=\"waiting-time form-control\" placeholder=\"waitnig time...\">";
				orderblock += "            <span class=\"input-group-btn\">";
				orderblock += "                <input itemid=\"" + order.success.Id + "\" type=\"button\" value=\"Take\" class=\"take btn btn-success\" \/>";
				orderblock += "            <\/span>";
				orderblock += "        <\/div>";
				orderblock += "    <\/td>";
				orderblock += "<\/tr>";

				$('#orders').append(orderblock)
			}
		});
	};

	orderHub.client.OrderTaken = function (id) {
		$.ajax({
			url: '/OrderEx/GetOrder/',
			data: {
				id: id,
			},
			type: "POST",
			success: function (order) {
				
				var orderblock = "";
				orderblock += "<tr>";
				orderblock += "    <td>";
				orderblock += order.success.Address;
				orderblock += "    <\/td>";
				orderblock += "    <td>";
				orderblock += order.success.OrderTime;
				orderblock += "    <\/td>";
				orderblock += "    <td>";
				orderblock += "        <div class=\"input-group\">";
				orderblock += "            <span class=\"input-group-btn\">";
				orderblock += "                <button data-feedbackId=\"" + order.success.Id + "\" data-orderId=\"" + order.success.Id +"\" type=\"button\" class=\"btn btn-warning addFeedbackButton\" data-toggle=\"modal\">Add Feedback</button>";
				orderblock += "            <\/span>";
				orderblock += "        <\/div>";
				orderblock += "    <\/td>";
				orderblock += "<\/tr>";

				$('#orders_history').append(orderblock)
			}
		});
	};

	orderHub.client.MessageFromAdministrator = function (message) {
		if (window.Notification && Notification.permission !== "denied") {
			Notification.requestPermission(function (status) {  // status is "granted", if accepted by user
				var n = new Notification('Message from Administrator', {
					body: message,
				});
			});
		}
	}

	$.connection.hub.start().done(function () {

		//connect to hub group
		orderHub.server.connect("Driver");

		//take order
		$(document).on('click','.take', function (e) {
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
							orderHub.server.OrderConfirmed(currentOrderId, waiting_time);
							orderHub.client.OrderTaken(currentOrderId)
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