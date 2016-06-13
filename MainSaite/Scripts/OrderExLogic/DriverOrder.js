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
	var currentAddress = 0;
	var selectedMarker;

	var directionsDisplay;
	var directionsService = new google.maps.DirectionsService();

	$('#inputDriverStatus').change(function () {
		$('#currentStatus').val($(this).val());
	})

	//init districts
	var elements = $('#listDistricts> li');
	//intial sorting (folders first)
	elements.sort(function (a, b) {
		if ($(a).hasClass('folder') ^ $(b).hasClass('folder')) {
			if ($(a).hasClass('folder')) {
				return -1;
			}
			else {
				return 1;
			}
		}
		else {
			var aName = $(a).children('.text').first().text().toLowerCase();
			var bName = $(b).children('.text').first().text().toLowerCase();
			return aName.localeCompare(bName);
		}
	}).each(function () {
		$('#listDistricts').append(this);
	});
	//set element according to the hierarchy
	elements.each(function (index, el) {
		var parentId = $(el).attr('parent-id');
		if (parentId) {
			$(el).remove();
			var find = $("li.folder[data-id=" + "'" + parentId + "'] ul").first();
			find.append(el);
		}
	});
	$(document).on('click', '.folder', function (e) {
		$(this).children('ul').first().fadeToggle();
		e.stopPropagation();
	});


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
			url: '/OrderEx/GetDriverNewOrder/',
			data: {
				id: id,
			},
			type: "POST",
			success: function (order) {
				$('#orders tbody').append(order)
			}
		});
	};

	mainHub.client.OrderTaken = function (id) {
		$.ajax({
			url: '/OrderEx/GetDriverHistoryOrder/',
			data: {
				id: id,
			},
			type: "POST",
			success: function (order) {
				$('#orders_history tbody').append(order)
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

	//arrow buttons
	$(document).on('click', '#arrowBtnL', function () {
		changeAddressLabel(currentAddress - 1);

	});
	$(document).on('click', '#arrowBtnR', function () {
		changeAddressLabel(currentAddress + 1);
	});
	$(document).on('click', '#mapReloading', function () {
		//var center = map.getCenter();
		//google.maps.event.trigger(map, 'resize');

		//map.setCenter(center);
	});
	function changeAddressLabel(index)
	{
		driverMarker.setAnimation(null);
		for (var key in destinationMarkers) {
			destinationMarkers[key].setAnimation(null);
		}
		var i = 0;
		var maxLength = Object.keys(destinationMarkers).length + 1;

		if (index >=maxLength) index = 0;

		if (index < 0) index = maxLength - 1;
		for (var key in destinationMarkers) {
			if (i == index) {
				selectedMarker = destinationMarkers[key]; break;
			}
			i++;
		}
		if (i == maxLength-1) {
			selectedMarker = driverMarker;
			i = maxLength;
		}

		currentAddress = i;

		selectedMarker.setAnimation(google.maps.Animation.BOUNCE);

		$('#addressLabel h3').text(selectedMarker.getTitle());

		setDirection();
	}
	function setDirection()
	{
		var x1 = driverMarker.getPosition().lat();
		var y1 = driverMarker.getPosition().lng();

		if (selectedMarker !== undefined) {
			var x2 = selectedMarker.getPosition().lat();
			var y2 = selectedMarker.getPosition().lng();

			var lat = (x1 + x2) / 2;
			var lng = (y1 + y2) / 2;

			map.setCenter(driverMarker.getPosition());


			var request = {
				origin: new google.maps.LatLng(x1, y1),
				destination: new google.maps.LatLng(x2, y2),
				travelMode: google.maps.TravelMode.DRIVING
			};
			directionsService.route(request, function (result, status) {
				if (status == google.maps.DirectionsStatus.OK) {
					directionsDisplay.setDirections(result);

					//$('#addressLabel h3').text($('#addressLabel h3').text()+"("+km.toFixed(2)+" km)");

				} else {
					//alert("couldn't get directions:" + status);
				}
			});
		}
	}

	navigator.geolocation.getCurrentPosition(function (position) {

		UpdateDriverPosition(position.coords.latitude, position.coords.longitude);
	});

	$.connection.hub.start().done(function () {

		//connect to hub group
		mainHub.server.connectDriver("Driver", $('#currentUserId').val());

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
						if (newDistrict != currentDistrict && currentDistrict) {
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
		$(document).on('click', '.joinButton', function (e) {
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
			e.stopPropagation();
		});

		function setCurrentDistrict(district) {
			var current = $('.joinButton.active');
			if (district) {
				if (current.attr('data-id') != district.Id) {
					mainHub.server.joinDistrict(district.Id);
					current.removeClass('active').children('span').toggleClass('hidenText');
					$(".joinButton[data-id='" + district.Id + "']").addClass('active').children('span').toggleClass('hidenText').parents("ul").fadeIn();;
				}
			}
			else {
				current.removeClass('active').children('span').toggleClass('hidenText');
			}

		}

		//take order
		$(document).on('click', '.take', function (e) {
			if ($('#currentStatus').val() == '2') {
				$('#blocking-dialog').modal('show');
				return;
			}
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
		directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true, preserveViewport:true });
		directionsDisplay.setMap(map);
	}

	function UpdateDriverPosition(Latitude, Longitude) {

		var address = getAddressByLocation(Latitude, Longitude);
		if (driverMarker === undefined) {
			driverMarker = new google.maps.Marker({
				position: { lat: Latitude, lng: Longitude },
				map: map,
				title: address,
				icon: {
					url: imagePath + '/cab.png'
				}
			});

			driverMarker.setAnimation(google.maps.Animation.BOUNCE);

		}
		else {
			driverMarker.setPosition(new google.maps.LatLng(prevCoord.Latitude, prevCoord.Longitude));
		}


		setDirection();


		//map.setCenter(driverMarker.getPosition());
	}

	function getAddressByLocation(Latitude, Longitude) {

		$.ajax({
			method: 'GET',
			url: "http://maps.googleapis.com/maps/api/geocode/json?latlng="+Latitude+", "+Longitude +"&sensor=true",
			success: function (data) {

				driverMarker.setTitle(data.results[0].formatted_address);


			}
		});



	}
	window.updateOrderInfo = function (orderID) {
		mainHub.server.orderFinished(orderID);
		GetCurrentOrder();
	}

	function GetCurrentOrder() {

		for (var key in destinationMarkers) {
			destinationMarkers[key].setMap(null);
		}

		directionsDisplay.setDirections({ routes: [] });

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

					$('#addressLabel h3').text(order.FullAddressFrom);

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

					if (order.AddressesTo.length > 0) {
						for (var i = 0; i < order.AddressesTo.length; i++) {
							GetLocationByAddress(order.AddressesTo[i].Address, 'logo_destination.png');
						}
					}
				}
				else {
					$('#currentOrder').hide();
					$('#noCurrentOrder').show();
				}

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
							title:address,
							position: results[0].geometry.location,
							icon: picturePath + path,
						});
						if (path == 'logo_client.png') {
							selectedMarker = destinationMarkers[address];
						}
					}
				} else {
					alert("Geocode was not successful for the following reason: " + status);
				}
			});
		}
	}
});