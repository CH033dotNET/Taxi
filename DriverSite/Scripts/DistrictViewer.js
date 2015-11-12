var currentDistrict = 0;
var hub;
function hubInit() {
	hub = $.connection.DistrictsHub;//Подключились к хабу

    hub.client.swap = swap;

    $.connection.hub.start().done(function () {
       // hub.server.Hello(); // вызов функции сервера
    });
}
function mainInit() {
	$.ajax({
		type: "POST",
		url: "Driver/GetDistricts",
		dataType: "json",
		success: function (data) {
			for (var i = 0; i < data.length; i++) {
				var val = data[i];
				var tr = $('<tr/>', { id: 'DistrictN' + val.DistrictId }).append(
                        $('<td/>', { text: val.DriverCount , class: "count"}),
                        $('<td/>', { text: val.DistrictName }),
                        $('<td/>', { text: val.ThoseDriver ? currentLocation : joinToLocation, class: "text"}));
				if (val.ThoseDriver)
					currentDistrict = val.DistrictId;
				tr.click(changeDistrict);
				var table = $('#Districts').append(
                    tr
                );
			}
		},
		error: function (error) {
			alert(error.statusText);
		}
	});
	hubInit();
}
$(document).ready(mainInit);

function changeDistrict(data)
{
	var newDisrict = +this.id.substr(9);
	if (newDisrict !== currentDistrict) {
		$.ajax({
			type: "POST",
			url: "Driver/JoinToLocation",
			dataType: "json",
			data: { "Id": newDisrict },
			success: function (data) {
				$('#DistrictN' + newDisrict + ">.text").html(currentLocation);
				$('#DistrictN' + currentDistrict + ">.text").html(joinToLocation);
				hub.server.swap(newDisrict, currentDistrict);
				currentDistrict = newDisrict;
			},
			error: function (error) {
				alert(error.statusText);
			}
		});
	}
	else return;
}
function swap(newDistrct, oldDistrict)
{
	if (newDistrct !== 0)
	{
		newDistrct = $('#DistrictN' + newDistrct + ">.count")[0];
		newDistrct.innerText++;
	}
	if (oldDistrict !== 0)
	{
		oldDistrict = $('#DistrictN' + oldDistrict + ">.count")[0];
		oldDistrict.innerText--;
	}
	
}