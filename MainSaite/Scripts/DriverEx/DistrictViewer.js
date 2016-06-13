$(document).ready(function () {
	var mainHub = $.connection.MainHub;
	checkstatus();
	$('#workshift-button-end').hide();
	$('#workshift-button-start').hide();

	$(document).on('click', '#workshift-button-start', function () {
		checkMainCars();
	});

	$(document).on('click', '#workshift-button-end', function () {
		ChangeBtnProperties(0);
	});

	$blocked = $('#blocked');
	$blockedFor = $('#blockedFor').append('  ').append($('.timer').first().clone());

	mainHub.client.blockDriver = function (time, message) {
		blockDriver(time, message);
		$('#blocking-dialog').modal('show');
	}

	mainHub.client.unblockDriver = function () {
		unblockDriver();
	}

});

function ChangeBtnProperties(status) {
	if (status == 0) //end workshift
	{
		$('#workshift-button-end').hide();
		$('#workshift-button-start').show();
		setEndlocation();
		changeStatusDisplay(0);
	}
	else //start workShift
	{
		$('#workshift-button-start').hide();
		$('#workshift-button-end').show();
		setBeginlocation();
		changeStatusDisplay(1);
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
		url: "/DriverEx/GetCurrentDriverStatus",
		dataType: "JSON",
	}).done(function (response) {
		if (response != null) {
			if (response.WorkingStatus == 2) {
				blockDriver(response.BlockTime, response.BlockMessage);
				return;
			}
			$('#inputDriverStatus').val(response.WorkingStatus);
			ChangeBtnProperties(1);
		}
		else
			ChangeBtnProperties(0);
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
		if (response == true) {
			$('#inputDriverStatus').val(status);
		}
		return false;
	});
}

function changeStatusDisplay(e) {
	if (e == 0)
		document.getElementById('worker-status-group').style.display = 'none';
	else if (e == 1)
		document.getElementById('worker-status-group').style.display = 'block';
}

var timerInterval = null;

function blockDriver(time, message) {
	$('#inputDriverStatus').hide();
	$('#workshift-button-start').hide();
	$('#workshift-button-end').hide();
	$('#currentStatus').val(2);
	$('#adminMessage').html('"' + message + '"');
	if (time != null) {
		$('#inputDriverStatus').parent().append($blockedFor);
		$('#blockTime').html('');
		$('#blockTime').append($('#unblockingAfter').html() + '  ').append($('.timer').first().clone());
		initTimer(time);
	}
	else {
		$('#inputDriverStatus').parent().append($blocked);
		$('#blockTime').html('');
	}
}

function unblockDriver() {
	$blocked.detach();
	$blockedFor.detach();
	clearInterval(timerInterval);
	$('#inputDriverStatus').show();
	$('#workshift-button-start').show();
	$('#inputDriverStatus').val(0);
	var driverId = $('#currentUserId').val();
	$('#currentStatus').val(0);
	$('#blocking-dialog').modal('hide');
}

function initTimer(endTime) {
	$timer = $('.timer');

	function tick() {
		var time = Date.parse(endTime) - Date.parse(new Date());
		if (isNaN(time))
			time = Date.parse(new Date(parseInt(endTime.replace('/Date(', '')))) - Date.parse(new Date());
		var seconds = Math.floor((time / 1000) % 60);
		var minutes = Math.floor((time / 1000 / 60) % 60);
		var hours = Math.floor(time / (1000 * 60 * 60));

		$timer.each(function (index, value) {
			$(value).children('.hours').html(('0' + hours).slice(-2));
			$(value).children('.minutes').html(('0' + minutes).slice(-2));
			$(value).children('.seconds').html(('0' + seconds).slice(-2));
		});

		if (time <= 0) {
			clearInterval(timerInterval);
			unblockDriver();
		}
	}

	tick();
	timerInterval = setInterval(tick, 1000);
};