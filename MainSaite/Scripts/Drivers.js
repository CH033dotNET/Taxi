$(function () {

	var mainHub = $.connection.MainHub;
	$.connection.hub.start();

	$block = $('.block-btn').first();
	$unblock = $('.unblock-btn').first();
	$timer = $('.timer').first();

	getStatuses();
	setInterval(getStatuses, 5000);

	$('#while-time').timepicker({
		showMeridian: false,
		maxHours: 90,
		minuteStep: 1,
		defaultTime: false
	});

	$('.datetimepicker').datetimepicker({
		format: 'DD-MM-YYYY HH:mm',
		minDate: moment().add(1, 'm')
	});

	$('#while-time').val('1:00 ' + $('#hours').html());

	$('#for-a-time').change(function () {
		if ($(this).prop('checked')) {
			$('.radio-inline').removeClass('inactive').find('input').prop('disabled', false);
			$('#block-while').prop('checked', true);
			$('#while-time').prop('disabled', false).parent().removeClass('inactive');
		}
		else {
			$('.radio-inline').addClass('inactive').find('input').prop('disabled', true).prop('checked', false);
			$('#while-time').prop('disabled', true).parent().addClass('inactive');
			$('#until-time').prop('disabled', true).parent().addClass('inactive');
		}
	});

	$('#block-while').change(function () {
		$('#while-time').prop('disabled', false).parent().removeClass('inactive');
		$('#until-time').prop('disabled', true).parent().addClass('inactive');
	});

	$('#block-until').change(function () {
		$('#until-time').prop('disabled', false).parent().removeClass('inactive');
		$('#while-time').prop('disabled', true).parent().addClass('inactive');
	});

	$('#while-time').change(function () {
		$(this).val($(this).val() + ' ' + $('#hours').html());
	});

	$('#block-message').change(function () {
		if ($(this).val().length > 0)
			$(this).removeClass('empty');
	});

	$('#block-btn').click(function () {
		if ($('#block-message').val().length > 0) {
			var driverId = $('#blocking-dialog').attr('driverId');
			var message = $('#block-message').val();
			var whileTime = null;
			var untilTime = null;
			if ($('#block-while').prop('checked'))
				whileTime = $('#while-time').val().split(' ')[0];
			if ($('#block-until').prop('checked'))
				untilTime = $('#until-time').val();
			mainHub.server.blockDriver(driverId, message, whileTime, untilTime);
			$('#blocking-dialog').modal('hide');
		}
		else
			$('#block-message').addClass('empty');
	});

	$(document).on('click', '.block-btn', function () {
		$('#blocking-dialog').attr('driverId', $(this).closest('.driver').attr('id'));
		$('#block-message').val('');
		$('#blocking-dialog').modal('show');
	});

	$(document).on('click', '.unblock-btn', function () {
		var driverId = $(this).closest('.driver').attr('id');
		mainHub.server.unblockDriver(driverId);
	});

	mainHub.client.blockDriver = function (time, driverId) {
		$driver = $('.driver[id="' + driverId + '"]');
		if (time == null)
			$driver.children('.driverStatus').html($('[statusId="2"]').html());
		else {
			$newTimer = $timer.clone();
			$driver.children('.driverStatus').html($('#blockedFor').html() + '\n');
			$driver.children('.driverStatus').append($newTimer);
			initTimer($newTimer, time);
		}
		$driver.removeClass('not-active');
		$driver.children('.driverAction').html($unblock.clone());
		sortDrivers();
	};

	mainHub.client.unblockDriver = unblockDriver;

	mainHub.client.setStatus = function (statusId, driverId) {
		$driver = $('.driver[id="' + driverId + '"]');
		$driver.children('.driverStatus').html($('[statusId="' + statusId + '"]').html());
		$driver.removeClass('not-active');
		$driver.children('.driverAction').html($block.clone());
		sortDrivers();
	}

	function unblockDriver(driverId) {
		$driver = $('.driver[id="' + driverId + '"]');
		$driver.children('.driverStatus').html($('[statusId="3"]').html());
		$driver.addClass('not-active');
		$driver.children('.driverAction').html($block.clone());
		sortDrivers();
	};

	function getStatuses() {
		$.ajax({
			url: '/Administration/GetStatuses',
			type: 'POST',
			success: function (response) {
				var statuses = [];
				$(response).each(function (index, value) {
					statuses[value.WorkerId] = {};
					statuses[value.WorkerId].status = value.WorkingStatus;
					statuses[value.WorkerId].blockTime = value.BlockTime != null ? moment(value.BlockTime).toDate() : null;
				});
				$('.driver').each(function (index, value) {
					var id = $(value).attr('id');
					if ($(value).find('.timer').html() != null)
						return;
					if (statuses[id] != null) {
						if (statuses[id].status == 2) {
							if (statuses[id].blockTime != null) {
								$newTimer = $timer.clone();
								$(value).children('.driverStatus').html($('#blockedFor').html() + '\n');
								$(value).children('.driverStatus').append($newTimer);
								initTimer($newTimer, statuses[id].blockTime);
							}
							else
								$(value).children('.driverStatus').html($('[statusId="2"]').html());
							$(value).children('.driverAction').html($unblock.clone());
							$(value).removeClass('not-active');
							return;
						}
						$(value).children('.driverAction').html($block.clone());
						$(value).children('.driverStatus').html($('[statusId="' + statuses[id].status + '"]').html());
						$(value).removeClass('not-active');
					}
					else {
						$(value).children('.driverAction').html($block.clone());
						$(value).children('.driverStatus').html($('[statusId="3"]').html());
						$(value).addClass('not-active');
					}
				});
				sortDrivers();
			}
		});
	};

	function sortDrivers() {
		$drivers = $('.driver');
		$drivers.sort(function (x, y) {
			if ($(x).hasClass('not-active') && !$(y).hasClass('not-active'))
				return 1;
			return 0;
		});
		$drivers.detach().appendTo($('#drivers'));
	};

	var timerIntervals = [];

	function initTimer($timer, endTime) {
		var id = $timer.closest('.driver').attr('id');

		function tick() {
			var time = Date.parse(endTime) - Date.parse(new Date());
			var seconds = Math.floor((time / 1000) % 60);
			var minutes = Math.floor((time / 1000 / 60) % 60);
			var hours = Math.floor(time / (1000 * 60 * 60));

			$timer.children('.hours').html(('0' + hours).slice(-2));
			$timer.children('.minutes').html(('0' + minutes).slice(-2));
			$timer.children('.seconds').html(('0' + seconds).slice(-2));

			if (time <= 0) {
				clearInterval(timerIntervals[id]);
				unblockDriver(id);
			}
		}

		tick();
		timerIntervals[id] = setInterval(tick, 1000);
	};
});