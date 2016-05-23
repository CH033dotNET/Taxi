$(function () {
	
	$('#additional').hide();
	$('#hide').hide();
	$('#load').hide();
	$('#time-group').hide();
	$('#waiting-message').hide();
	$('#approved-message').hide();
	$('#denied-message').hide();
	$('[data-toggle="tooltip"]').tooltip();

	if ($('#userId').length) {
		var userId = {
			id: $('#userId').val()
		}
		$.ajax({
			url: '/Client/GetPerson',
			type: "post",
			data: userId,
			success: function (data) {
				$('#name').val(data.FirstName);
				$('#phone').val(data.Phone);
				$('#remember').prop('checked', true);
			}
		});
	}

	var orderHub = $.connection.MainHub;

	orderHub.client.OrderApproved = function () {
		$('#waiting-message').hide();
		$('#approved-message').show();
	}

	orderHub.client.OrderDenied = function () {
		$('#waiting-message').hide();
		$('#denied-message').show();
	}

	orderHub.client.OrderConfirmed = function (orderId, waitingTime) {
		$('#waiting-message').hide();
		$('#approved-message').hide();
		$('#denied-message').hide();
		$('#submit button').prop('disabled', false);
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

	$('#add-address').click(function () {
		var address, building;
		if ($('.ActivLang').html() == 'En') {
			address = 'Address:';
			building = 'Building:';
		}
		else {
			address = 'Адреса:';
			building = 'Будинок:';
		}
		var count = $('#form-count').val();
        count++;
		var row = '<div class="address-to-group"><div class="row"><label for="address-to-'
				+ count + '" class="col-sm-2 form-control-label">'
				+ address + '</label><div class="col-sm-8"><input type="text" class="form-control" id="address-to-'
				+ count + '"></div></div><div class="row"><label for="building-to-'
				+ count + '" class="col-sm-2 form-control-label">'
				+ building + '</label><div class="col-sm-4"><input type="text" class="form-control" id="building-to-'
				+ count + '"></div></div></div>';
	  	$('#form-count').val(count);
		$(row).hide().appendTo('#form-body').slideDown(200);
	  	if (count >= 5) $(this).remove();
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
	
	$('#calculate').click(function () {
		$(this).hide();
		$('#load').show();
	});
	
	$('#load').click(function () {
		$(this).hide();
		$('#calculate').show();
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

	$('#submit button').click(function () {
		if ($('#address-from').val() == "") {
			if ($('.ActivLang').html() == 'En')
				var inputRequired = 'this field is required';
			else
				var inputRequired = "це поле обов'язкове";
			$('#address-from').addClass('input-error');
			$('#address-from').attr('placeholder', inputRequired);
			return;
		}
		$(this).prop('disabled', true);
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
			orderHub.server.connect("Client");
			$.ajax({
				url: '/Client/AddOrder',
				contentType: "application/json",
				type: "POST",
				data: JSON.stringify(order),
				success: function (data) {
					alert('success');
					orderHub.server.addOrder(data);
					$('#form').trigger("reset");
					$('#waiting-message').slideDown(200);
				},
				error: function (error) {
					alert('error');
				}
			});
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