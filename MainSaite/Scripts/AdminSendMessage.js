$(function () {
	var adminHub = $.connection.MainHub;

	adminHub.client.MessageFromAdministrator2 = function (message) {
		if (window.Notification && Notification.permission !== "denied") {
			Notification.requestPermission(function (status) {  // status is "granted", if accepted by user
				var n = new Notification('Message from Administrator', {
					body: message,
				});
			});
		}
	}


	$.connection.hub.start().done(function () {

		//connect to hub group
		adminHub.server.connect("Driver");

		//take order
		$(document).on('click','#message-btn', function (e) {
			var message = $('#message').val();

			adminHub.server.MessageFromAdministrator(message);
			
			
		});

	});



});