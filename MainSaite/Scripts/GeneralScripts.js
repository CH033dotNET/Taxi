var intervalId;

function ShowSupportChat() {
	$('#chat').show(500);
	//$('#chat').draggable();
	intervalId = setInterval(DisplayMessages, 2000);
}

function HideSupportChat() {
	$('#chat').hide(500);
	clearInterval(intervalId);
}

var i = 0;
function DisplayMessages() {
	//$('#chat .body').html('');
	//$('#chat .body').append('<div class="question message">Message '+i+'</div>');
	//$('#chat .body').append('<div class="answer message">Message ' + i + '</div>');

	$.ajax({
		type: "POST",
		url: "/Support/GetMessages"//,
		//data: {
		//	id: 0
		//}
	}).done(function (messages) {
		$('#chat .body').html('');

		for (var i in messages) {
			// Append message to end

			if (messages[i].Id % 2 == 0) {
				var message = '<div class="question message">' + messages[i].Message + '</div>';
				$('#chat .body').append(message);
			} else {
				var message = '<div class="answer message">' + messages[i].Message + '</div>';
				$('#chat .body').append(message);
			}
		}
	});
}