$(document).ready(function () {
	$(".Edit").on("click", function () {
		$('#myModal').modal('show');
	})

	$("#mdal-submit").on("click", function () {

		if ($(".field-validation-error").length === 0)
			$('#myModal').modal('hide');
	})
})