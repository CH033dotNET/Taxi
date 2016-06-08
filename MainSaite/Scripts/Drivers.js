$(function () {

	getStatuses();
	setInterval(getStatuses, 5000);

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
				$(value).children('.driverStatus').html(statuses[id] != null ? $('[statusId="' + statuses[id] + '"]').html() : $('[statusId="3"]').html());
				if (statuses[id] == null)
					$(value).addClass('not-active');
				else
					$(value).removeClass('not-active');
			});
		}
	});
};