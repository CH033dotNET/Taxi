$(function () {

	var userId = null;
	var tariff = null;

	$('#additional').hide();
	$('#hide').hide();
	$('#load').hide();
	$('#time-group').hide();
	$('#cancel-btn').hide();
	$('[data-toggle="tooltip"]').tooltip();
	$('#pre-order').prop('disabled', true);
	$('#cost').html('+ 0.0');

	if ($('#userId').length) {
		userId = $('#userId').val();
		if (userId) {
			$.ajax({
				url: '/Client/GetPerson',
				type: 'POST',
				data: {id: userId},
				success: function (data) {
					$('#name').val(data.FirstName);
					$('#phone').val(data.Phone);
					$('#remember').prop('checked', true);
					$('#pre-order').prop('disabled', false);
					$('#pre-order').parent().removeAttr('data-original-title');
				}
			});
		}
	}

	var mainHub = $.connection.MainHub;

	$.connection.hub.start().done(function () {
		var orderId = getCookie("orderId");
		if (orderId) {
			$.ajax({
				url: '/Client/GetOrder/',
				type: 'POST',
				data: { Id: orderId },
				success: function (data) {
					if (data.UserId != userId)
						return;
					switch (data.Status) {
						case 2:
						case 4:
						case 5:
							mainHub.server.connect("Client");
							deleteCookie("ordedId");
							break;
						case 0:
							mainHub.server.connect("Client", orderId);
							$('#denied-message').hide();
							$('#approved-message').hide();
							$('#waiting-message').slideDown(200);
							$("#submit-btn").hide();
							$("#cancel-btn").show();
							break;
						case 1:
							mainHub.server.connect("Client", orderId);
							$('#denied-message').hide();
							$('#waiting-message').hide();
							$('#approved-message').slideDown(200);
							$("#submit-btn").hide();
							$("#cancel-btn").show();
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
	});

	$.ajax({
		url: '/Client/GetTariff',
		type: 'POST',
		success: function (data) {
			tariff = data;
		}
	});

	$('#add-address').click(function () {
		var address, building, select;
		if ($('.ActivLang').html() == 'En') {
			address = 'Address:';
			building = 'Building:';
			select = 'Select on the map';
		}
		else {
			address = 'Адреса:';
			building = 'Будинок:';
			select = 'Вибрати на карті';
		}
		var count = $('#form-count').val();
        count++;
		var row = '<div class="address-to-group"><div class="row"><label for="address-to-'
				+ count + '" class="col-sm-2 form-control-label">'
				+ address + '</label><div class="col-sm-8"><input type="text" id="address-to-'
				+ count + '"></div></div><div class="row"><label for="building-to-'
				+ count + '" class="col-sm-2 form-control-label">'
				+ building + '</label><div class="col-sm-4"><input type="text" id="building-to-'
				+ count + '"></div></div><div class="row"><div class="col-sm-2"></div><div class="col-sm-8"><button type="button" class="btn btn-success btn-select-map" id="btn-map-to-'
				+ count + '">'
				+ select + '</button></div></div></div>';
	  	$('#form-count').val(count);
		$(row).hide().appendTo('#form-body').slideDown(200);
		if (count >= 5) $(this).remove();
		$('#btn-map-to-' + count).click(function () {
			$('#address-to-' + count).addClass('Address');
			$('#modal-map').modal('show');
		});
	});

	$('#btn-map-from').click(function () {
		$('#address-from').addClass('Address');
		$('#modal-map').modal('show');
	});

	$('#btn-map-to-1').click(function () {
		$('#address-to-1').addClass('Address');
		$('#modal-map').modal('show');
	});

	$('#btn-select').click(function () {
		$('.Address').val($('#textField').val());
		$('.Address').removeClass('Address');
		$('#modal-map').modal('hide');
	});
	
	$('#show').click(function () {
		$(this).hide();
		$('#hide').show();
		$('#additional').slideDown(200);
	});
	
	$('#hide').click(function () {
		$(this).hide();
		$('#show').show();
		$('#additional').slideUp(200);
	});

	$('input[name="urgency"]').change(function () {
		if ($('#pre-order').prop('checked'))
			$('#time-group').show();
		else
			$('#time-group').hide();
	});

	$('#time-group').datetimepicker({
		format: 'DD-MM HH:mm',
		minDate: moment()
	});

	$('#address-from').change(function () {
		if ($(this).val() != "") {
			$(this).removeClass('input-error');
			$(this).attr('placeholder', '');
		}
	});

	$('#pre-order').change(function () {
		calculateCost(tariff);
	});

	$('#urgently').change(function () {
		calculateCost(tariff);
	});

	$('input[name="car-class"]').change(function () {
		calculateCost(tariff);
	});

	$('.additional').change(function () {
		calculateCost(tariff);
	});

	$('#submit-btn button').click(function () {
		if ($('#address-from').val() == "") {
			if ($('.ActivLang').html() == 'En')
				var inputRequired = 'this field is required';
			else
				var inputRequired = "це поле обов'язкове";
			$('#address-from').addClass('input-error');
			$('#address-from').attr('placeholder', inputRequired);
			return;
		}
		$('#submit-btn').hide();
		$('#cancel-btn').show();
		var order = {
			AddressFrom: getAddressFrom(),
			AddressesTo: getAddressesTo(),
			AdditionallyRequirements: getAdditionallyRequirements(),
			Route: $('#route').prop('checked'),
			UserId: $('#userId').val(),
			Name: $('#name').val(),
			Phone: $('#phone').val(),
			Perquisite: $('#perquisite').val()
		}
		$.connection.hub.start().done(function () {
			mainHub.server.connect("Client");
			$.ajax({
				url: '/Client/AddOrder',
				contentType: "application/json",
				type: "POST",
				data: JSON.stringify(order),
				success: function (data) {
					mainHub.server.addOrder(data);
					setCookie("orderId", data.Id, { expires: 3600 });
					$('#form').trigger("reset");
					$('#denied-message').hide();
					$('#approved-message').hide();
					$('#waiting-message').slideDown(200);
				}
			});
		});
	});

	$('#cancel-btn button').click(function () {
		var orderId = getCookie("orderId");
		$.ajax({
			url: '/Client/CancelOrder/',
			type: "POST",
			data: { Id: orderId },
			success: function () {
				mainHub.server.cancelOrder(orderId);
				$('#cancel-btn').hide();
				$("#submit-btn").show();
				$('#waiting-message').slideUp(200);
				$('#approved-message').slideUp(200);
			}
		});
	});
	
});

function getAddressFrom() {
	return {
		Address: $('#address-from').val(),
		Building: $('#building-from').val(),
		Entrance: $('#entrance-from').val(),
		Note: $('#note-from').val()
	}
}

function getAddressesTo() {
	var addresses = [];
	$('.address-to-group').each(function (index) {
		addresses.push({
			Address: $('#address-to-' + (index + 1)).val(),
			Building: $('#building-to-' + (index + 1)).val()
		});
	});
	return addresses;
}

function getAdditionallyRequirements() {
	return {
		Urgently: $('#urgently').prop('checked'),
		Time: $('#time').val(),
		Passengers: $('#passengers').val(),
		Car: $('input[name="car-class"]:checked').attr('id'),
		Courier: $('#courier').prop('checked'),
		WithPlate: $('#with-plate').prop('checked'),
		MyCar: $('#my-car').prop('checked'),
		Pets: $('#pets').prop('checked'),
		Bag: $('#bag').prop('checked'),
		Conditioner: $('#conditioner').prop('checked'),
		English: $('#english').prop('checked'),
		NoSmoking: $('#nosmoking').prop('checked'),
		Smoking: $('#smoking').prop('checked'),
		Check: $('#check').prop('checked')
	}
}

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

function calculateCost(tariff) {
	var cost = 0;
	if (tariff) {
		if ($('#pre-order').prop('checked'))
			cost += tariff.PricePreOrder;
		if ($('#universal').prop('checked'))
			cost += tariff.PriceRegularCar;
		if ($('#minivan').prop('checked'))
			cost += tariff.PriceMinivanCar;
		if ($('#lux').prop('checked'))
			cost += tariff.PriceLuxCar;
		if ($('#courier').prop('checked'))
			cost += tariff.PriceCourierOption;
		if ($('#with-plate').prop('checked'))
			cost += tariff.PricePlateOption;
		if ($('#my-car').prop('checked'))
			cost += tariff.PriceClientCarOption;
		if ($('#english').prop('checked'))
			cost += tariff.PriceSpeakEnglishOption;
		if ($('#smoking').prop('checked'))
			cost += tariff.PricePassengerSmokerOption;
	}
	$('#cost').html('+ ' + cost.toFixed(1));
}