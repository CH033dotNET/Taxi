var currentDistrict = 0;
var hub;
function hubInit() {
	hub = $.connection.DistrictsHub;//Подключились к хабу

    hub.client.swap = swap;

    $.connection.hub.start().done(function () {
       // hub.server.Hello(); // вызов функции сервера
    });
}
function mainInit() {
	$.ajax({
		type: "POST",
		url: "Driver/GetDistricts",
		dataType: "json",
		success: function (data) {
			for (var i = 0; i < data.length; i++) {
				var val = data[i];
				var tr = $('<tr/>', { id: 'DistrictN' + val.DistrictId }).append(
                        $('<td/>', { text: val.DriverCount , class: "count"}),
                        $('<td/>', { text: val.DistrictName }),
                        $('<td/>', { text: val.ThoseDriver ? currentLocation : joinToLocation, class: "text"}));
				if (val.ThoseDriver)
					currentDistrict = val.DistrictId;
				tr.click(changeDistrict);
				var table = $('#Districts').append(
                    tr
                );
			}
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
		url: "Driver/CheckDriverMainCar",
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
	$('#get-driver-error-modal').modal('show');
}

// function that checks driver`s working status after page reloading
function checkstatus() {
	$.ajax({
		method: "GET",
		url: "Driver/GetCurrentDriverStatus",
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
		url: "Driver/ChangeCurrentDriverStatus",
		data: {status: newWorkerStatus},
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
		url: "Driver/ChangeCurrentDriverStatus",
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

$(document).ready(
	mainInit,
	checkstatus());

function changeDistrict(data)
{
	var newDisrict = +this.id.substr(9);
	if (newDisrict !== currentDistrict) {
		$.ajax({
			type: "POST",
			url: "Driver/JoinToLocation",
			dataType: "json",
			data: { "Id": newDisrict },
			success: function (data) {
				$('#DistrictN' + newDisrict + ">.text").html(currentLocation);
				$('#DistrictN' + currentDistrict + ">.text").html(joinToLocation);
				hub.server.swap(newDisrict, currentDistrict);
				currentDistrict = newDisrict;
			},
			error: function (error) {
				alert(error.statusText);
			}
		});
	}
	else return;
}
function swap(newDistrct, oldDistrict)
{
	if (newDistrct !== 0)
	{
		newDistrct = $('#DistrictN' + newDistrct + ">.count")[0];
		newDistrct.innerText++;
	}
	if (oldDistrict !== 0)
	{
		oldDistrict = $('#DistrictN' + oldDistrict + ">.count")[0];
		oldDistrict.innerText--;
	}
	
}