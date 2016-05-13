var currentDistrict = 0;
var hub;

var districts = [];

function hubInit() {

	hub = $.connection.DistrictsHub;
	hub.client.swap = swap;
}
function mainInit() {
	hubInit();
}
function ChangeBtnProperties(status) {
	if (status == 0) {
		$("#workshift-button").attr("change-btn-id", "work_start"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-success'); // changing custom attribute value
		$("#workshift-button").prop('value', 'Start workshift'); // changing buttons text
		document.getElementById('workshift-button').onclick = function () { checkMainCars(); }; // changing onclick property value
		setEndlocation();
		setDriverStatus(0);
		changeStatusDisplay(0);
		$("worker-status-group").css('display', 'none');
	}
	else {
		$("#workshift-button").attr("change-btn-id", "work_end"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-warning'); // changing buttons class
		$("#workshift-button").prop('value', 'End workshift'); // changing buttons text
		document.getElementById('workshift-button').onclick = function () { ChangeBtnProperties(0); }; // changing onclick property value
		setBeginlocation();
		setDriverStatus(3);
		changeStatusDisplay(1);
		$("worker-status-group").css('display', 'inline');
	}

}
// function that check if a worker has main car available
function checkMainCars() {
	$.ajax({
		method: "GET",
		url: "./Driver/CheckDriverMainCar",
		dataType: "JSON",
	}).done(function (response) {
		if (response.success && response != null) {
			ChangeBtnProperties(1);
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
			ChangeBtnProperties(response.WorkingStatus);
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