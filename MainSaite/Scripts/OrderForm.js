$(function () {
	
	$('#additional').hide();
	$('#hide').hide();
	$('#load').hide();
	$('#time-group').hide();
	$('[data-toggle="tooltip"]').tooltip();

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

	function getAddressFrom() {
		return {
			Address: $('#address-from').val(),
			Building: $('#building-from').val(),
			Entrance: $('#entrance-from').val(),
			Note: $('#note-from').val()
		}
	}

	function getAddressTo() {
		var address = [];
		$('.address-to-group').each(function (index) {
			address.push({
				Address: $('#address-to-' + (index + 1)).val(),
				Building: $('#building-to-' + (index + 1)).val()
			});
		});
		return {
			Address: address,
			Route: $('#route').prop('checked')
		}
	}

	function getAdditionally() {
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
			noSmoking: $('#nosmoking').prop('checked'),
			Smoking: $('#smoking').prop('checked'),
			Check: $('#check').prop('checked')
		}
	}

	$('#submit').click(function () {
		if ($('#address-from').val() == "") {
			if ($('.ActivLang').html() == 'En')
				var inputRequired = 'this field is required';
			else
				var inputRequired = "це поле обов'язкове";
			$('#address-from').addClass('input-error');
			$('#address-from').attr('placeholder', inputRequired);
			return;
		}
		var order = {
			AddressFrom: getAddressFrom(),
			AddressTo: getAddressTo(),
			Additionally: getAdditionally(),
			Name: $('#name').val(),
			Phone: $('#phone').val(),
			Perquisite: $('#perquisite').val(),
			Remember: $('#remember').prop('checked')
		}
		var orderHub = $.connection.OrderHub;
		$.connection.hub.start().done(function () {
			orderHub.server.connect("Client");
			$.post(
				'/Client/AddOrder',
				order,
				function (data) {
					orderHub.server.addOrder(data);
					alert("You order id = " + data.Id);
				}
			);
		});
	});
	
});