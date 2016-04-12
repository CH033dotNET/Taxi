
var data,
	   container = $("#content"),
	   sourceEdit = $("#TarifesEdit").html(),
	   templateEdit = Handlebars.compile(sourceEdit),
	   sourceDelete = $("#TarifesDelete").html(),
	   templateDelete = Handlebars.compile(sourceDelete);

var EditMethod = function (d) {
	data = d;

	html = templateEdit(d);
	container.html(html);
}

var DeleteMethod = function (d) {
	data = d;
	html = templateDelete(d);
	container.html(html);
}


$(".Edit, .Delete").on("click", function () {
	$("#myModal").modal("show");
})

var Close = function () {
	$("#myModal").modal("hide");
}

var HideMethod = function () {


	$('.IsStandart').css("display", "none");

	if ($('#Tarif_IsIntercity, #Tarif_IsStandart').is(':checked')) {
		$("#DistrictList").attr("hidden", true)
	} else {
		$("#DistrictList").removeAttr("hidden");
	}

	$('#Tarif_IsIntercity, #Tarif_IsStandart').change(function () {
		if ($('#Tarif_IsIntercity, #Tarif_IsStandart').is(':checked')) {
			$("#DistrictList").attr("hidden", true)
		} else {
			$("#DistrictList").removeAttr("hidden");
		}
	});


}

