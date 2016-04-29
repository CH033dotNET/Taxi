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
	    $('#time-group').toggle();
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

		var address = [];
		$('.address-to-group').each(function (index) {
			address.push({
				address: $('#address-to-' + (index + 1)).val(),
				building: $('#building-to-' + (index + 1)).val()
			});
		});

		var addressFrom = {
			address: $('#address-from').val(),
			building: $('#building-from').val(),
			entrance: $('#entrance-from').val(),
			note: $('#note-from').val()
		}

		var addressTo = {
			address: address,
			route: $('#route').prop('checked')
		}
	});
	
});