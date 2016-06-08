$(document).ready(function () {
	checkstatus();
	$('#workshift-button-end').hide();
	$('#workshift-button-start').hide();

	$(document).on('click', '#workshift-button-start', function () {
		checkMainCars();
	});

	$(document).on('click', '#workshift-button-end', function () {
		ChangeBtnProperties(0);
	});

});
function ChangeBtnProperties(status) {
	if (status == 0) //end workshift
	{
		$('#workshift-button-end').hide();
		$('#workshift-button-start').show();
		setEndlocation();
		//setDriverStatus(0);
		changeStatusDisplay(0);
		$("#inputDriverStatus").css('display', 'none');
	}
	else //start workShift
	{
		$('#workshift-button-start').hide();
		$('#workshift-button-end').show();

		setBeginlocation();
		//setDriverStatus(1);
		changeStatusDisplay(1);
		$("#inputDriverStatus").css('display', 'block');
		setDriverStatus($('#inputDriverStatus').val());
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
			//alert("Error");
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
			return true;
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
		console.log(response.success);
		console.log(response);
		console.log(status);
		if (response == true) {
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
		document.getElementById('worker-status-group').style.display = 'block';
		//$('#worker-status-group').show();
	}
}
