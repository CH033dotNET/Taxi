$(function () {

	//var driverLocationHub = $.connection.driverLocationHub;
	var driverDistrictHub = $.connection.DriverDistrictHub;

	var currentPosition = {
		Latitude: null,
		Longitude: null
	};
	var driverId;
	var driverName;
	var currentDistrict = null;
	var districtChecker = null;

	$(window).on("beforeunload", function () {
		if (currentDistrict) {
			driverDistrictHub.server.leaveDistrict(currentDistrict.Id);
		}
	})

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

	//check count of drivers in each district
	setTimeout(function checkDistricts() {
		{
			driverDistrictHub.server.getDriversCount().done(function (result) {
				fillDriversCount(result);
			});
		}
		setTimeout(checkDistricts, 5000);
	}, 5000);

	function fillDriversCount(items) {
		items.forEach(function (item) {
			var t = $('tr').has(".joinButton[data-id='" + item.Id + "']");
			//t = t.parentsUntil('tbody');
			t = t.find('.count');
			t= t.html(item.Count);
		});
	}

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
					if (!newDistrict) {
						driverDistrictHub.server.leaveDistrict(currentDistrict.Id);
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
			driverDistrictHub.server.leaveDistrict(currentDistrict.Id);
			currentDistrict = null;
			$(this).removeClass('active');
			$(this).children('span').toggleClass('hidenText');

		}
		else {
			var id = $(this).attr('data-id');
			if (currentDistrict) {
				driverDistrictHub.server.leaveDistrict(currentDistrict.Id);
			}
			driverDistrictHub.server.joinDistrict(id);
			currentDistrict = districts.find(function (item) {
				return item.Id == id;
			});
			$('.joinButton.active').removeClass('active').children('span').toggleClass('hidenText');
			$(this).addClass('active');
			$(this).children('span').toggleClass('hidenText');
		}
	});


	$.connection.hub.start().done(function () {

		driverId = $('#currentUserId').val();
		//driverLocationHub.server.connectUser("Driver", driverId);
		driverName = $('#currentUserName').val();

		setTimeout(function invoke() {
			navigator.geolocation.getCurrentPosition(checkPosition);
			setTimeout(invoke, 15000);
		}, 15000);

	});

	function setCurrentDistrict(district) {
		var current = $('.joinButton.active');
		if (district) {
			if (current.attr('data-id') != district.Id) {
				driverDistrictHub.server.joinDistrict(district.Id);
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