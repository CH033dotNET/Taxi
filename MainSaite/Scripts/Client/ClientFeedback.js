$(function () {

	var orderId;
	var feedback = {
		Id: null,
		Comment: "",
		Rating: 0
	};

	$('.addFeedback').click(function () {
		$('#comment').val('');
		fillStars(0);
		orderId = $(this).attr('data-orderId');
		feedback.Id = $(this).attr('data-feedbackId');
		if (feedback.Id) {
			$.ajax({
				url: "GetFeedback",
				method: 'post',
				data: {
					id: feedback.Id
				},
				success: function (result) {
					if (result) feedback = result;
					$('#comment').val(feedback.Comment);
					fillStars(feedback.Rating);
				}
			});
		}
		else {
			feedback.Id = 0;
			feedback.Comment = "";
			feedback.Rating = 0;
		}
		$('#clientFeedbackModal').modal('show');
	});

	$('#feedbackConfirm').click(function () {
		var comment = $('#comment').val();
		if (feedback.Rating > 0 || comment.length > 0) {
			feedback.Comment = comment;
			if (feedback.Id) {
				$.ajax({
					url: "UpdateFeedback",
					method: 'post',
					data: feedback,
					success: function (result) { }
				});
			}
			else {
				$.ajax({
					url: "AddFeedback",
					method: 'post',
					data: feedback,
					success: function (result) {
						feedback = result;
						$.ajax({
							url: "SetClientFeedback",
							method: 'post',
							data: {
								orderId: orderId,
								feedbackId: result.Id
							},
							success: function () {
								$('button[data-orderId="' + orderId + '"]').attr('data-feedbackId', feedback.Id);
							}
						});
					}
				});
			}
		}
		$('#clientFeedbackModal').modal('hide');
	});

	//functionw for drawing stars
	$('#rating .glyphicon')
		.hover(function () {
			var stars = $('#rating .glyphicon');
			var rate = stars.index(this);
			fillStars(rate + 1);
		})
		.click(function () {
			var stars = $('#rating .glyphicon');
			feedback.Rating = stars.index(this) + 1;
			fillStars(feedback.Rating);
		})
		.mouseout(function () {
			fillStars(feedback.Rating);
		});

	function fillStars(count) {
		var stars = $('#rating .glyphicon');
		stars.addClass('glyphicon-star-empty').removeClass('glyphicon-star');
		stars.each(function (index, elem) {
			if (index <= count - 1) {
				$(elem).removeClass('glyphicon-star-empty').addClass('glyphicon-star');
			}
		});
	}

});