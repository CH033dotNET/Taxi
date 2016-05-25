$(function () {

	var currentDriverId = $('#currentDriverId').val();

	$.ajax({
		url: 'GetDriversWithsOrdersLastMonth',
		type: 'POST',
		data: {
			id: currentDriverId
		},
		success: function (data) {
			var i = 1;
			data.forEach(function (value, index, array) {
				$('#lastMonth').append(
					"<tr" + (value.Driver.Id == currentDriverId ? " class='active'" : '') +
					"><td rowspan='2' class='index'><h3>" + i + "</h3></td><td rowspan='2' class='avatar'>" +
					"<img src='~/Images/" + value.Image + "'/>" + "</td><td class='userName'><h3>" +
					value.Driver.UserName + "</h3></td><td rowspan='2' class='ordersCount'><h3>" +
					value.OrdersCount + "</h3></td></tr>" +
					"<tr" + (value.Driver.Id == currentDriverId ? " class='active'" : '') +
					"><td class='name'>" + (value.Name != null ? value.Name : '') +
					"</td></tr><tr><td colspan='4'><hr class='underline'/></td></tr>"
				);
				i++;
			});
		}
	});

	$.ajax({
		url: 'GetDriversWithsOrders',
		type: 'POST',
		data: {
			id: currentDriverId
		},
		success: function (data) {
			var i = 1;
			data.forEach(function (value, index, array) {
				$('#allTheTime').append(
					"<tr" + (value.Driver.Id == currentDriverId ? " class='active'" : '') +
					"><td rowspan='2' class='index'><h3>" + i + "</h3></td><td rowspan='2' class='avatar'>" +
					"<img src='~/Images/" + value.Image + "'/>" + "</td><td class='userName'><h3>" +
					value.Driver.UserName + "</h3></td><td rowspan='2' class='ordersCount'><h3>" +
					value.OrdersCount + "</h3></td></tr>" +
					"<tr" + (value.Driver.Id == currentDriverId ? " class='active'" : '') +
					"><td class='name'>" + (value.Name != null ? value.Name : '') +
					"</td></tr><tr><td colspan='4'><hr class='underline'/></td></tr>"
				);
				i++;
			});
		}
	});

});