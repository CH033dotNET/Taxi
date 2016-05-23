$(function () {
	var mainHub = $.connection.MainHub;
	$.connection.hub.logging = true;
	var prevCoord = {
		Latitude: null,
		Longitude: null
	};
	var currentOrderId;

	var currentDistrict = null;
	var districtChecker = null;
	var counts = [];

	Notify = function (header, message) {
		if (window.Notification && Notification.permission !== "denied") {
			Notification.requestPermission(function (status) {  // status is "granted", if accepted by user
				var n = new Notification(header, {
					body: message,
				});
			});
		}
	}

	//initialize districts polygons
	districts.forEach(function (item) {
		var path = item.Coordinates.map(function (item) {
			return {
				lat: item.Latitude,
				lng: item.Longitude,
			}
		});

		item.Polygon = new google.maps.Polygon({
			paths: path
		});
	});

	//initialize counters
	$(".count").each(function (index, item) {
		var count = new CountUp(item.id, 0, 0, 0, 3);
		count.start();
		counts.push(count);
	});

	function fillDriversCount(items) {
		items.forEach(function (item) {
			var find = counts.find(function (count) {
				return count.d.id == (item.Id);
			});
			find.update(item.Count)
		});
	}

	//client hub methods
	mainHub.client.addDriverToDistrict = function (id) {
		var find = counts.find(function (item) {
			return item.d.id == id;
		});
		if (find) {
			find.update(++find.endVal);
		}
	}

	mainHub.client.subtractDriverFromDistrict = function (id) {
		var find = counts.find(function (item) {
			return item.d.id == id;
		});
		if (find) {
			find.update(--find.endVal);
		}
	}

	mainHub.client.OrderApproved = function (id) {
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
				orderblock += "            <input type=\"number\" class=\"waiting-time form-control\" placeholder=\"" + Resources.WaitingTime + "\">";
				orderblock += "            <span class=\"input-group-btn\">";
				orderblock += "                <input itemid=\"" + order.success.Id + "\" type=\"button\" value=\"" + Resources.TakeOrder + "\" class=\"take btn btn-success\" \/>";
				orderblock += "            <\/span>";
				orderblock += "        <\/div>";
				orderblock += "    <\/td>";
				orderblock += "<\/tr>";

				$('#orders').append(orderblock)
			}
		});
	};

	mainHub.client.OrderTaken = function (id) {
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
				orderblock += moment(order.success.OrderTime).format('DD/MM/YY HH:mm');
				orderblock += "    <\/td>";
				orderblock += "    <td>";
				orderblock += "        <div class=\"input-group\">";
				orderblock += "                <button data-feedbackId=\"" + order.success.Id + "\" data-orderId=\"" + order.success.Id + "\" type=\"button\" class=\"btn btn-warning addFeedbackButton\" data-toggle=\"modal\">" + Resources.AddFeedback + "</button>";
				orderblock += "        <\/div>";
				orderblock += "    <\/td>";
				orderblock += "<\/tr>";

				$('#orders_history').append(orderblock)
			}
		});
	};

	mainHub.client.MessageFromAdministrator = function (message) {
		Notify(Resources.AdminMessageHeader, message);
	}

	$.connection.hub.start().done(function () {

		//connect to hub group
		mainHub.server.connect("Driver");

		//check count of drivers in each district
		mainHub.server.getDriversCount().done(function (result) {
			fillDriversCount(result);
		});

		//auto select for districts
		$(document).on('click', '.slider', function (e) {
			if (districtChecker) {
				clearTimeout(districtChecker);
				districtChecker = null;
				$('.joinButton').removeClass('disabled');
			}
			else {
				$('.joinButton').addClass('disabled');
				districtChecker = setTimeout(function checkDistrict() {
					if (currentPosition.Latitude && currentPosition.Longitude) {
						var newDistrict = districts.find(function (item) {
							return google.maps.geometry.poly.containsLocation({
								lat: function () { return currentPosition.Latitude },
								lng: function () { return currentPosition.Longitude }
							}, item.Polygon)
						});
						if (!newDistrict && currentDistrict) {
							mainHub.server.leaveDistrict(currentDistrict.Id);
						}
						currentDistrict = newDistrict;
						setCurrentDistrict(currentDistrict);

					}
					districtChecker = setTimeout(checkDistrict, 5000);
				}, 5000);
			}
			e.stopPropagation();
		});

		//manual district control
		$(document).on('click', '.joinButton', function () {
			if ($(this).hasClass('active')) {
				mainHub.server.leaveDistrict(currentDistrict.Id);
				currentDistrict = null;
				$(this).removeClass('active');
				$(this).children('span').toggleClass('hidenText');

			}
			else {
				var id = $(this).attr('data-id');
				if (currentDistrict) {
					mainHub.server.leaveDistrict(currentDistrict.Id);
				}
				mainHub.server.joinDistrict(id);
				currentDistrict = districts.find(function (item) {
					return item.Id == id;
				});
				$('.joinButton.active').removeClass('active').children('span').toggleClass('hidenText');
				$(this).addClass('active');
				$(this).children('span').toggleClass('hidenText');
			}
		});

		function setCurrentDistrict(district) {
			var current = $('.joinButton.active');
			if (district) {
				if (current.attr('data-id') != district.Id) {
					mainHub.server.joinDistrict(district.Id);
					current.removeClass('active').children('span').toggleClass('hidenText');
					$(".joinButton[data-id='" + district.Id + "']").addClass('active').children('span').toggleClass('hidenText');
				}
			}
			else {
				current.removeClass('active').children('span').toggleClass('hidenText');
			}

		}

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
							mainHub.server.OrderConfirmed(currentOrderId, waiting_time);
							mainHub.client.OrderTaken(currentOrderId)
						}
						else {
							alert("something wrong");
						}
					},
					error: function (result) {
						Notify(result.responseJSON.errorHeader, result.responseJSON.errorMessage);
					}
				});
			}
		});

		setTimeout(function run() {
			navigator.geolocation.getCurrentPosition(sentCoord);
			setTimeout(run, 2000);
		}, 2000);

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
			if (currentOrderId) {
				mainHub.server.notifyDriverCoordinate(data);
			}
		}

	});

	
});