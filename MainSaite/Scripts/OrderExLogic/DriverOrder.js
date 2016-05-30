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
	var geocoder = new google.maps.Geocoder();


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
				id: id,
			},
			type: "POST",
			success: function (order) {

				var orderblock = "";
				orderblock += "<tr>";
				orderblock += "    <td class=\"col-md-7\">";
				orderblock += order.success.FullAddressFrom;
				orderblock += "    <\/td>";
				orderblock += "    <td class=\"col-md-2\">";
				orderblock += order.success.Perquisite;
				orderblock += "    <\/td>";
				orderblock += "    <td class=\"col-md-2\">";
				orderblock += "<input type=\"number\" class=\"waiting-time form-control\" placeholder=\"" + Resources.WaitingTime + "\">";
				orderblock += "    <\/td>";
				orderblock += "    <td class=\"col-md-3\">";
				orderblock += "        <div class=\"input-group\">";
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
				orderblock += "    <td  class=\"col-md-8\">";
				orderblock += order.success.FullAddressFrom;
				orderblock += "    <\/td>";
				orderblock += "    <td class=\"col-md-2\">";
				orderblock += moment(order.success.OrderTime).format('DD/MM/YY HH:mm');
				orderblock += "    <\/td>";
				orderblock += "    <td class=\"col-md-2\">";
				orderblock += "                <button data-feedbackId=\"" + order.success.Id + "\" data-orderId=\"" + order.success.Id + "\" type=\"button\" class=\"btn btn-success addFeedbackButton\" data-toggle=\"modal\">" + Resources.AddFeedback + "</button>";
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

	//MAP FUNCTIONS
	mapInit();
	GetCurrentOrder();
	navigator.geolocation.getCurrentPosition(function (position) {

		UpdateDriverPosition(position.coords.latitude, position.coords.longitude);
	});

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
		$(document).on('click', '.take', function (e) {
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

							// "1" is "DoingOrder" status
							setDriverStatus(1);

							GetCurrentOrder();
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

				UpdateDriverPosition(position.coords.latitude, position.coords.longitude);
			}
			if (currentOrderId) {
				mainHub.server.notifyDriverCoordinate(data);
			}
		}
	});

	//MAP FUNCTIONALITY
	function mapInit() {
		map = new google.maps.Map(document.getElementById("map"), {
			zoom: 13
		});
	}

	function UpdateDriverPosition(Latitude, Longitude) {
		if (driverMarker === undefined) {
			driverMarker = new google.maps.Marker({
				position: { lat: Latitude, lng: Longitude },
				map: map,
				title: 'Driver: ' + name,
				icon: {
					url: imagePath + '/cab.png'
				}
			});

			driverMarker.setAnimation(google.maps.Animation.BOUNCE);

		}
		else {
			driverMarker.setPosition(new google.maps.LatLng(prevCoord.Latitude, prevCoord.Longitude));
		}

		map.setCenter(driverMarker.getPosition());
	}

	window.updateOrderInfo = function () {
		GetCurrentOrder();
	}

	function GetCurrentOrder() {
		for (var key in destinationMarkers) {
			destinationMarkers[key].setMap(null);
		}

		destinationMarkers = [];

		$.ajax({
			type: "POST",
			url: "/DriverEX/GetCurrentOrder/",

			success: function (order) {
				var k = 0;

				if (order != null && order != "NoOrder") {
					$('#currentOrder').show();
					$('#noCurrentOrder').hide();
					$('#orderId').val(order.Id);
					$('#userId').val(order.UserId);
					$('#fullAddressFrom').text(order.FullAddressFrom);
					$('#cost').text(order.Price);
					GetLocationByAddress(order.FullAddressFrom, 'logo_client.png');
					if (order.UserId != null) {
						$('.bonusTable').show();
					}
					else $('.bonusTable').hide();

					setChecked('#urgently', order.AdditionallyRequirements.Urgently);


					switch (order.AdditionallyRequirements.Car) {
						case 1: setChecked('#normal', true); break;
						case 2: setChecked('#universal', true); break;
						case 3: setChecked('#minivan', true); break;
						case 4: setChecked('#lux', true); break;
					}


					$('#passengers').val(order.AdditionallyRequirements.Passengers);

					setChecked('#courier', order.AdditionallyRequirements.Courier);
					setChecked('#with-plate', order.AdditionallyRequirements.WithPlate);
					setChecked('#my-car', order.AdditionallyRequirements.MyCar);
					setChecked('#pets', order.AdditionallyRequirements.Pets);
					setChecked('#bag', order.AdditionallyRequirements.Bag);
					setChecked('#conditioner', order.AdditionallyRequirements.Conditioner);
					setChecked('#english', order.AdditionallyRequirements.NoSmoking);
					setChecked('#nosmoking', order.AdditionallyRequirements.Smoking);
					setChecked('#smoking', order.AdditionallyRequirements.English);
					setChecked('#check', order.AdditionallyRequirements.Check);

					//if (order.AddressesTo.length > 0) {
					//	for (var i = 0; i < order.AddressesTo.length; i++) {
					//		GetLocationByAddress(order.AddressesTo[i], 'logo_destination.png');
					//	}
					//}
					GetLocationByAddress('вулиця Південно-Кільцева 7, Черновцы, Черновицкая область, Украина', 'logo_destination.png');


				}
				else {
					$('#currentOrder').hide();
					$('#noCurrentOrder').show();
				}
			}
		});
	}
	function UpdateDriverPosition(Latitude, Longitude) {
		if (driverMarker === undefined) {
			driverMarker = new google.maps.Marker({
				position: { lat: Latitude, lng: Longitude },
				map: map,
				title: 'Driver: ' + name,
				icon: {
					url: imagePath + '/cab.png'
				}
			});

			driverMarker.setAnimation(google.maps.Animation.BOUNCE);

		}
		else {
			driverMarker.setPosition(new google.maps.LatLng(prevCoord.Latitude, prevCoord.Longitude));
		}

		map.setCenter(driverMarker.getPosition());
	}

	window.updateOrderInfo = function () {
		GetCurrentOrder();
	}

	function GetCurrentOrder() {


		for (var key in destinationMarkers) {
			destinationMarkers[key].setMap(null);
		}

		destinationMarkers = [];

		$.ajax({
			type: "POST",
			url: "/DriverEX/GetCurrentOrder/",

			success: function (order) {
				var k = 0;

				if (order != null && order != "NoOrder") {
					$('#currentOrder').show();
					$('#noCurrentOrder').hide();
					$('#orderId').val(order.Id);
					$('#userId').val(order.UserId);
					$('#fullAddressFrom').text(order.FullAddressFrom);
					$('#cost').text(order.Price);
					GetLocationByAddress(order.FullAddressFrom, 'logo_client.png');
					if (order.UserId != null) {
						$('.bonusTable').show();
					}
					else $('.bonusTable').hide();

					setChecked('#urgently', order.AdditionallyRequirements.Urgently);


					switch (order.AdditionallyRequirements.Car) {
						case 1: setChecked('#normal', true); break;
						case 2: setChecked('#universal', true); break;
						case 3: setChecked('#minivan', true); break;
						case 4: setChecked('#lux', true); break;
					}


					$('#passengers').val(order.AdditionallyRequirements.Passengers);

					setChecked('#courier', order.AdditionallyRequirements.Courier);
					setChecked('#with-plate', order.AdditionallyRequirements.WithPlate);
					setChecked('#my-car', order.AdditionallyRequirements.MyCar);
					setChecked('#pets', order.AdditionallyRequirements.Pets);
					setChecked('#bag', order.AdditionallyRequirements.Bag);
					setChecked('#conditioner', order.AdditionallyRequirements.Conditioner);
					setChecked('#english', order.AdditionallyRequirements.NoSmoking);
					setChecked('#nosmoking', order.AdditionallyRequirements.Smoking);
					setChecked('#smoking', order.AdditionallyRequirements.English);
					setChecked('#check', order.AdditionallyRequirements.Check);

					//if (order.AddressesTo.length > 0) {
					//	for (var i = 0; i < order.AddressesTo.length; i++) {
					//		GetLocationByAddress(order.AddressesTo[i], 'logo_destination.png');
					//	}
					//}
					GetLocationByAddress('вулиця Південно-Кільцева 7, Черновцы, Черновицкая область, Украина', 'logo_destination.png');


				}
				else {
					$('#currentOrder').hide();
					$('#noCurrentOrder').show();
				}

				//add other addresses



			},
		});

	}

	function setChecked(selector, value) {
		$(selector).attr('checked', value);

	}

	function GetLocationByAddress(address, path) {
		var addressLabel = address;

		if (addressLabel != "") {
			geocoder.geocode({ 'address': addressLabel }, function (results, status) {
				if (status == google.maps.GeocoderStatus.OK) {

					if (destinationMarkers[address] === undefined) {
						destinationMarkers[address] = new google.maps.Marker({
							map: map,
							position: results[0].geometry.location,
							icon: picturePath + path,
						});
					}
				} else {
					alert("Geocode was not successful for the following reason: " + status);
				}
			});
		}
	}
});