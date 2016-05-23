
function ChangeBtnProperties(status) {
	if (status == 0) {
		$("#workshift-button").attr("change-btn-id", "work_start"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-success'); // changing custom attribute value
		$("#workshift-button").prop('value', 'Start workshift'); // changing buttons text
		document.getElementById('workshift-button').onclick = function () { checkMainCars(); }; // changing onclick property value
		setEndlocation();
		//setDriverStatus(0);
		changeStatusDisplay(0);
		$("worker-status-group").css('display', 'none');
	}
	else {
		$("#workshift-button").attr("change-btn-id", "work_end"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-warning'); // changing buttons class
		$("#workshift-button").prop('value', 'End workshift'); // changing buttons text
		document.getElementById('workshift-button').onclick = function () { ChangeBtnProperties(0); }; // changing onclick property value
		setBeginlocation();
		//setDriverStatus(3);
		changeStatusDisplay(1);
		$("worker-status-group").css('display', 'inline');
	}

}
// function that check if a worker has main car available
function checkMainCars() {
	$.ajax({
		method: "GET",
		url: "./DriverEx/CheckDriverMainCar",
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

function getNotFoundMessage() {
	$('#get-driver-error-modal').modal('show');
}

function checkstatus() {
	$.ajax({
		method: "GET",
		url: "./DriverEx/GetCurrentDriverStatus",
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
function workerStatusChange() {
	var newWorkerStatus = $('#inputDriverStatus').val();
	$.ajax({
		method: "POST",
		url: "./DriverEx/ChangeCurrentDriverStatus",
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
		url: "./DriverEx/ChangeCurrentDriverStatus",
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
	checkstatus();
});