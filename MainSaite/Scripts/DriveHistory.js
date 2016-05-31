$(function () {

	var cancel = $('.cancel').first();
	var finished = $('.finished').first();
	var canceled = $('.canceled').first();

	$('.status').each(function () {
		switch ($(this).attr('status')) {
			case 'Confirmed':
				$(this).html(cancel.clone());
				break;
			case 'Finished':
				$(this).html(finished.clone());
				break;
			case 'Canceled':
				$(this).html(canceled.clone());
				break;
		}
	});

	$('.cancel').click(function () {
		var id = $(this).closest('.order').attr('id');
		$.ajax({
			url: '/Client/CancelOrder',
			type: 'POST',
			data: { id: id },
			success: function () {
				Cancel(id);
			}
		});
	})

	function Cancel(id) {
		$('.order[id=' + id + ']').children('.status').html(canceled.clone());
	}

});