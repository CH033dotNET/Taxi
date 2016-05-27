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

	var map;
	var driverMarker;
	var destinationMarkers = [];


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
				orderblock += order.success.FullAddressFrom;
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
				orderblock += order.success.FullAddressFrom;
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

	mainHub.client.cancelOrder = function (id) {
		currentOrderId = null;

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
					if (prevCoord.Latitude && prevCoord.Longitude) {
						var newDistrict = districts.find(function (item) {
							return google.maps.geometry.poly.containsLocation({
								lat: function () { return prevCoord.Latitude },
								lng: function () { return prevCoord.Longitude }
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

							//load partial view
							$.ajax({
								type: 'POST',
								url: '@Url.Content("~/DriverEx/MyOrder")',
								success: function (data) {
									$('#MyOrder').html(data);
								}
							});

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
			setTimeout(run, 5000);
		}, 5000);

		mapInit();

		function sentCoord(position) {
			var data = {};
			data.Latitude = position.coords.latitude;
			data.Longitude = position.coords.longitude;
			data.Accuracy = position.coords.accuracy;
			data.AddedTime = moment().format('YYYY/MM/DD HH:mm:ss');
			data.OrderId = currentOrderId;

			if (prevCoord.Latitude != position.coords.latitude && prevCoord.Longitude != position.coords.longitude) {

				prevCoord.Latitude = position.coords.latitude;
				prevCoord.Longitude = position.coords.longitude;

				$.ajax({
					url: '/DriverEx/SetCoordinate',
					method: 'POST',
					data: data
				});

				UpdateDriverPosition();

			}
			if (currentOrderId) {
				mainHub.server.notifyDriverCoordinate(data);
			}
		}

		function mapInit() {
			map = new google.maps.Map(document.getElementById("map"), {
				center: { lat: 48.3214409, lng: 25.8638791 },
				zoom: 8
			})
		}

		function UpdateDriverPosition()
		{
			if (driverMarker === undefined) {
				driverMarker = new google.maps.Marker({
					position: { lat: prevCoord.Latitude, lng: prevCoord.Longitude },
					map: map,
					title: 'Driver: ' + name,
					icon: {
						url: imagePath + '/cab.png'
					}
				});
			}
			else {
				driverMarker.setPosition(new google.maps.LatLng(prevCoord.Latitude, prevCoord.Longitude));
			}
		}
	});	
});