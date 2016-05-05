var currentDistrict = 0;
var hub;

var districts = [];

function hubInit() {
	hub = $.connection.DistrictsHub;

	hub.client.swap = swap;
}
function mainInit() {
	$.ajax({
		type: "POST",
		url: "./Driver/GetDistricts",
		dataType: "json",
		success: function (data) {
			data.forEach(function (item) {
				var val = item;
				var tr = $('<tr/>', { id: 'DistrictN' + val.DistrictId }).append(
						$('<td/>', { text: val.DriverCount, class: "count" }),
						$('<td/>', { text: val.DistrictName }),
						$('<td><button type="button" class="btn btn-warning">'+(val.ThoseDriver ? currentLocation : joinToLocation)+'</button></td>'));
				if (val.ThoseDriver)
					currentDistrict = val.DistrictId;
				tr.click(function () { changeDistrict(val.DistrictId) });
				var table = $('#Districts').append(
					tr
				);
			});
		},
		error: function (error) {
			alert(error.statusText);
		}
	});
	hubInit();
}
// function that check if a worker has main car available
function checkMainCars() {
	$.ajax({
		method: "GET",
		url: "./Driver/CheckDriverMainCar",
		dataType: "JSON",
	}).done(function (response) {
		if (response.success && response != null) {
			buttonChangeOnClick();
		}
		else {
			getNotFoundMessage();
		}
	});
}
// shows error message if the main car wasn`t found for a specific worker
function getNotFoundMessage() {
	//jQuery.noConflict();
	//(function ($) { $('#get-driver-error-modal').modal('show'); })(jQuery);
	$('#get-driver-error-modal').modal('show');
}

// function that checks driver`s working status after page reloading
function checkstatus() {
	$.ajax({
		method: "GET",
		url: "./Driver/GetCurrentDriverStatus",
		dataType: "JSON",
	}).done(function (response) {
		if (response.success && response != null) {
			$('#inputDriverStatus').val(response.WorkingStatus);
		}
		else {
			alert("Error");
		}
	});
}
// function that changes drivers working status
function workerStatusChange() {
	var newWorkerStatus = $('#inputDriverStatus').val();
	$.ajax({
		method: "POST",
		url: "./Driver/ChangeCurrentDriverStatus",
		data: { status: newWorkerStatus },
		dataType: "JSON"
	}).done(function (response) {
		if (response.success && response != null) {
			return false;
		}
		return false;
	});
}
// event that is used to change status in response on some events.
function setDriverStatus(status) {
	$.ajax({
		method: "POST",
		url: "./Driver/ChangeCurrentDriverStatus",
		data: { status: status },
		dataType: "JSON"
	}).done(function (response) {
		if (response.success && response != null) {
			$('#inputDriverStatus').val(status);
		}
		return false;
	});
}

function changeStatusDisplay(e) {
	if (e == 0) {
		document.getElementById('worker-status-group').style.display = 'none';
		//$('#worker-status-group').hide();
	}
	else if (e == 1) {
		document.getElementById('worker-status-group').style.display = 'inline';
		//$('#worker-status-group').show();
	}
}

$(document).ready(function () {
	mainInit();
	checkstatus();
	ReadDistricts();
});


//read all districts from DB
function ReadDistricts() {
	$.ajax({
		type: "POST",
		url: "./Driver/GetFullDistricts",
		success: function (data) {
			districts = data.districts;
			districts.forEach(function (item) {
				//map coordinates to paths for google map api
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
			
		},
		error: function (error) {
		}
	});
}

function getDistrictIdByCoordinate(coord) {
	var district;
	districts.forEach(function (item) {
		if (google.maps.geometry.poly.containsLocation(coord, item.Polygon)) {
			district = item;
		}
	})
	if (district) {
		return district.Id;
	}
	return null;
}


function changeDistrict(id) {

	if (id && id !== currentDistrict) {
		$.ajax({
			type: "POST",
			url: "./Driver/JoinDriverToLocation",
			dataType: "json",
			data: { "Id": id },
			success: function (data) {
				$('#DistrictN' + id + ">.text").html(currentLocation);
				$('#DistrictN' + currentDistrict + ">.text").html(joinToLocation);
				hub.server.swap(id, currentDistrict);
				currentDistrict = id;
			},
			error: function (error) {
				alert(error.statusText);
			}
		});
	}
	else return;
}
function swap(newDistrct, oldDistrict) {
	if (newDistrct !== 0) {
		newDistrct = $('#DistrictN' + newDistrct + ">.count")[0];
		newDistrct.innerText++;
	}
	if (oldDistrict !== 0) {
		oldDistrict = $('#DistrictN' + oldDistrict + ">.count")[0];
		oldDistrict.innerText--;
	}

}