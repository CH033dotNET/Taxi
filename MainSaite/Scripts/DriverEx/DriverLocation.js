$(function () {

	//var driverLocationHub = $.connection.driverLocationHub;
	var mainHub = $.connection.MainHub;

	var currentPosition = {
		Latitude: null,
		Longitude: null
	};
	var driverId;
	var driverName;
	var currentDistrict = null;
	var districtChecker = null;
	var counts = [];

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
		find.update(++find.endVal);
	}

	mainHub.client.subtractDriverFromDistrict = function (id) {
		var find = counts.find(function (item) {
			return item.d.id == id;
		});
		find.update(--find.endVal);
	}

	$.connection.hub.start().done(function () {

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

		driverId = $('#currentUserId').val();
		//driverLocationHub.server.connectUser("Driver", driverId);
		driverName = $('#currentUserName').val();

		setTimeout(function invoke() {
			navigator.geolocation.getCurrentPosition(checkPosition);
			setTimeout(invoke, 5000);
		}, 5000);

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

	function checkPosition(position) {

		if (currentPosition.Latitude != position.coords.latitude || currentPosition.Longitude != position.coords.longitude) {
			//driverLocationHub.server.updateDriverPosition(driverId, position.coords.latitude, position.coords.longitude, driverName);
			var data = {};
			data.Latitude = position.coords.latitude;
			data.Longitude = position.coords.longitude;
			data.Accuracy = position.coords.accuracy;
			data.AddedTime = moment().format('YYYY/MM/DD HH:mm:ss');
			data.driverId = driverId;
			prevCoord = data;
			$.ajax({
				url: '/Driver/UpdateCoords',
				method: 'POST',
				data: data
			});
			currentPosition.Latitude = position.coords.latitude;
			currentPosition.Longitude = position.coords.longitude;

		}
	}
});