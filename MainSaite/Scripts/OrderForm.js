$(function () {
	
	$('#additional').hide();
	$('#hide').hide();
	$('#load').hide();
	$('#time-group').hide();

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
		var row = '<div class="row"><label for="address-to-'
				+ count + '" class="col-sm-2 form-control-label">'
				+ address + '</label><div class="col-sm-8"><input type="text" class="form-control" id="address-to-'
				+ count + '"></div></div><div class="row"><label for="building-to-'
				+ count + '" class="col-sm-2 form-control-label">'
				+ building + '</label><div class="col-sm-4"><input type="text" class="form-control" id="building-to-'
				+ count + '"></div></div>';
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
	    locale: 'uk',
	    format: 'DD-MM HH:mm',
	    stepping: 1,
	    minDate: moment(),
	});
	
});