var intervalId;

function ShowSupportChat() {
	$.ajax({
		type: "POST",
		url: "/Support/GetSupporter"
	}).done(function (support) {
		$('#chat .name').html(support.Name);
		$('#chat .avatar').attr('src', '/Images/' + support.Photo);
		$('#chat').data('supportId', support.Id);

		$('#chat').show(500);
		$('#chat').draggable();
		intervalId = setInterval(DisplayMessages, 2000);
	});
	
}

function HideSupportChat() {
	$('#chat').hide(500);
	clearInterval(intervalId);
}

function DisplayMessages() {
	$.ajax({
		type: "POST",
		url: "/Support/GetMessages"
	}).done(function (messages) {
		$('#chat .body').html('');

		for (var i in messages) {
			// Append message to end
			if (messages[i].Sender.Id != $('#chat').data('supportId')) {
				var message = '<div class="question message">' + messages[i].Message + '</div>';
				$('#chat .body').append(message);
			} else {
				var message = '<div class="answer message">' + messages[i].Message + '</div>';
				$('#chat .body').append(message);
			}
		}
	});
}

function SendMessage() {
	$.ajax({
		type: "POST",
		url: "/Support/SendMessage",
		data: {
			message: $('#chat .bottom .message').val(),
			toUserID: $('#chat').data('supportId')
		}
	}).done(function () {
		$('#chat .bottom .message').val('');
	});
}