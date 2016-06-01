$(function () {

	$('#waiting-message').hide();
	$('#approved-message').hide();
	$('#denied-message').hide();

	var orderHub = $.connection.MainHub;

	orderHub.client.OrderApproved = function () {
		$('#waiting-message').hide();
		$('#denied-message').hide();
		$('#approved-message').slideDown(200);
	}

	orderHub.client.OrderDenied = function () {
		$('#waiting-message').hide();
		$('#approved-message').hide();
		$('#denied-message').slideDown(200);
		$('#cancel-btn').hide();
		$("#submit-btn").show();
		$('#submit button').prop('disabled', false);
		$('#orderBtn').prop('disabled', false);
		$('#orderBtn').parent().addClass('activeBtn');
	}

	orderHub.client.OrderConfirmed = function (orderId, waitingTime) {
		$('#waiting-message').hide();
		$('#approved-message').hide();
		$('#denied-message').hide();
		$('#submit button').prop('disabled', false);
		$('#orderBtn').prop('disabled', false);
		$('#orderBtn').parent().addClass('activeBtn');
		$('#waiting-time').html(waitingTime);
		$.ajax({
			url: '/Client/GetOrder',
			type: "POST",
			data: {
				id: orderId
			},
			success: function (data) {
				$('#car-number').html(data.Car.CarNumber);
			}
		});
		$('#modal-message').modal('show');
	}

});