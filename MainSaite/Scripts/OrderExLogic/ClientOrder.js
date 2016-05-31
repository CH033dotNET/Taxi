$(document).ready(function () {
	var mainHub = $.connection.MainHub;

	mainHub.client.notifyDriverCoordinate = function (coordinate) {
		setTaxiMarker(coordinate.Latitude, coordinate.Longitude);
	}

	$.connection.hub.start().done(function () {
		//connect to hub group
		var orderId = getCookie("orderId");
		if (orderId) {
			$.ajax({
				url: '/Client/GetOrder/',
				type: "POST",
				data: {Id : orderId},
				success: function (data) {
					switch (data.Status) {
						case 2:
						case 4:
						case 5:
							mainHub.server.connect("Client");
							deleteCookie("ordedId");
							break;
						case 1:
							mainHub.server.connect("Client", orderId);
							$('#denied-message').hide();
							$('#waiting-message').hide();
							$('#approved-message').slideDown(200);
							$("#orderBtn").hide();
							$("#cancelOrder").show();
							break;
						case 0:
							mainHub.server.connect("Client", orderId);
							$('#denied-message').hide();
							$('#approved-message').hide();
							$('#waiting-message').slideDown(200);
							$("#orderBtn").hide();
							$("#cancelOrder").show();
							break;
						case 3:
							mainHub.server.connect("Client", orderId);
							break;
					}
				}
			}).fail(function () {
				mainHub.server.connect("Client");
			});
		}
		else {
			mainHub.server.connect("Client");
		}
		

		//add order to db
		$(document).on('click', '#orderBtn', function () {
			var order = {};
			order.AddressFrom = {};
			order.AddressFrom.Address = $('#textField').val();
			if ($('#userId').length)
				order.UserId = $('#userId').val();
			$.ajax({
				url: '/Client/AddOrder/',
				contentType: "application/json",
				data: JSON.stringify(order),
				type: "POST",
				success: function (data) {
					mainHub.server.addOrder(data);
					setCookie("orderId", data.Id, { expires : 3600});
					$('#waiting-message').slideDown(200);
					$("#orderBtn").hide();
					$("#cancelOrder").show();
				}
			});

		});

		$(document).on('click', '#cancelOrder', function () {
			var orderId = getCookie("orderId");
			$.ajax({
				url: '/Client/CancelOrder/',
				type: "POST",
				data: { Id: orderId },
				success: function () {
					mainHub.server.cancelOrder(orderId);
					$('#waiting-message').slideUp(200);
					$('#approved-message').slideUp(200);
					$("#cancelOrder").hide();
					$("#orderBtn").show();
					removeTaxiMarker();
				}
			});
		})
	});

	//work with cookie

	function getCookie(name) {
		var matches = document.cookie.match(new RegExp(
		  "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
		));
		return matches ? decodeURIComponent(matches[1]) : undefined;
	}

	function setCookie(name, value, options) {
		options = options || {};

		var expires = options.expires;

		if (typeof expires == "number" && expires) {
			var d = new Date();
			d.setTime(d.getTime() + expires * 1000);
			expires = options.expires = d;
		}
		if (expires && expires.toUTCString) {
			options.expires = expires.toUTCString();
		}

		value = encodeURIComponent(value);

		var updatedCookie = name + "=" + value;

		for (var propName in options) {
			updatedCookie += "; " + propName;
			var propValue = options[propName];
			if (propValue !== true) {
				updatedCookie += "=" + propValue;
			}
		}

		document.cookie = updatedCookie;
	}

	function deleteCookie(name) {
		setCookie(name, "", {
			expires: -1
		})
	}

});
