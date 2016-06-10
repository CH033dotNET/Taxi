$(function () {

	getStatuses();
	setButtons();
	setInterval(getStatuses, 5000);

	$('#while-time').timepicker({
		showMeridian: false,
		maxHours: 90,
		minuteStep: 5,
		defaultTime: false
	});

	$('.datetimepicker').datetimepicker({
		format: 'DD-MM HH:mm',
		minDate: moment(),
		useCurrent: false
	});

	$('.block-btn').click(function () {
		$('#blocking-dialog').attr('driverId', $(this).closest('.driver').attr('id'));
		$('#block-message').val('');
		$('#blocking-dialog').modal('show');
	});

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

	$('#block-message').change(function () {
		if ($(this).val().length > 0)
			$(this).removeClass('empty');
	})

	$('#block-btn').click(function () {
		if ($('#block-message').val().length > 0) {
			var message = $('#block-message').val();
			$('#blocking-dialog').modal('hide');
		}
		else
			$('#block-message').addClass('empty');
	});

});

function getStatuses() {
	$.ajax({
		url: '/Administration/GetStatuses',
		type: 'POST',
		success: function (response) {
			var statuses = [];
			$(response).each(function (index, value) {
				statuses[value.WorkerId] = value.WorkingStatus;
			});
			$('.driver').each(function (index, value) {
				var id = $(value).attr('id');
				if (statuses[id] != null) {
					$(value).children('.driverStatus').html($('[statusId="' + statuses[id] + '"]').html());
					$(value).removeClass('not-active');
					$(value).attr('status', statuses[id]);
				}
				else {
					$(value).children('.driverStatus').html($('[statusId="3"]').html());
					$(value).addClass('not-active');
					$(value).attr('status', 3);
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
}

function setButtons() {
	$block = $('.block-btn').first();
	$unblock = $('.unblock-btn').first();
	$('.driver').each(function (index, value) {
		if ($(value).attr('status') == '2')
			$(value).children('.driverAction').html($unblock.clone());
		else
			$(value).children('.driverAction').html($block.clone());
	});
}