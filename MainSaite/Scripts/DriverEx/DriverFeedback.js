$(document).ready(function () {

	var orderId;
	var feedback = {
		Id: null,
		Comment: "",
		Rating: 0
	};

	$(document).on('click', '.addFeedbackButton', function () {
		orderId = +$(this).attr('data-orderId');
		feedback.Id = +$(this).attr('data-feedbackId');
		if (feedback.Id) {
			$.ajax({
				url: "DriverEx/GetFeedback",
				method: 'post',
				data: {
					id: feedback.Id
				},
				success: function (result) {
					if (result) {
						feedback = result;
					}
					$('#comment').val(feedback.Comment);
					fillStars(feedback.Rating - 1);
				}
			});
		}
		$('#driverFeedbackModal').modal('show');
		
		
	});

	$(document).on('click', '#feedbackConfirm', function () {
		var comment = $('#comment').val();
		if (feedback.Rating > 0 || comment.length > 0) {
			feedback.Comment = comment;
			if (feedback.Id) {
				$.ajax({
					url: "DriverEx/UpdateFeedback",
					method: 'post',
					data: feedback,
					success: function (result) {
						$('#driverFeedbackModal').modal('hide');
					}
				});
			}
			else {
				$.ajax({
					url: "DriverEx/AddFeedback",
					method: 'post',
					data: feedback,
					success: function (result) {
						feedback = result;
						$.ajax({
							url: "DriverEx/SetDriverFeedback",
							method: 'post',
							data: {
								orderId: orderId,
								feedbackId: result.Id
							},
							success: function () {
								$('#addFeedbackButton[data-orderId="' + orderId + '"]').attr('data-feedbackId', feedback.Id);
								$('#driverFeedbackModal').modal('hide');
							}
						});
					}
				});
			}
		}
	});


	//functionw for drawing stars
	$('#rating .glyphicon')
		.hover(function () {
			var stars = $('#rating .glyphicon');
			var rate = stars.index(this);
			fillStars(rate);
		})
		.click(function () {
			var stars = $('#rating .glyphicon');
			feedback.Rating = stars.index(this)+1;
			fillStars(feedback.Rating-1);
		})
		.mouseout(function () {
			fillStars(feedback.Rating-1);
		});

	function fillStars(count) {
		var stars = $('#rating .glyphicon');
		stars.addClass('glyphicon-star-empty').removeClass('glyphicon-star');
		stars.each(function (index, elem) {
			if (index <= count) {
				$(elem).removeClass('glyphicon-star-empty').addClass('glyphicon-star');
			}
		});
	}
	

});