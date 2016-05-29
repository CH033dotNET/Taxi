$(function () {

	var currentDriverId = $('#currentDriverId').val();

	$.ajax({
		url: 'GetDriversWithsOrdersLastMonth',
		type: 'POST',
		data: {
			id: currentDriverId
		},
		success: function (data) {
			data.forEach(function (value, index, array) {
				$('#lastMonth').append(
					"<tr" + (value.Driver.Id == currentDriverId ? " class='active'" : '') +
					"><td class='index'><h3>" + value.Index + "</h3></td><td class='avatar'>" +
					"<img src='/Images/" + value.Image + "'/>" + "</td><td class='userName'><h3>" +
					value.Driver.UserName + "</h3><div class='name'>" +
					(value.Name != null ? value.Name : '') + "</div></td><td class='ordersCount'><h3>" +
					value.OrdersCount + "</h3></td></tr>" + "><tr><td colspan='4'><hr class='underline'/></td></tr>"
				);
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
			data.forEach(function (value, index, array) {
				$('#allTheTime').append(
					"<tr" + (value.Driver.Id == currentDriverId ? " class='active'" : '') +
					"><td class='index'><h3>" + value.Index + "</h3></td><td class='avatar'>" +
					"<img src='/Images/" + value.Image + "'/>" + "</td><td class='userName'><h3>" +
					value.Driver.UserName + "</h3><div class='name'>" +
					(value.Name != null ? value.Name : '') + "</div></td><td class='ordersCount'><h3>" +
					value.OrdersCount + "</h3></td></tr>" + "><tr><td colspan='4'><hr class='underline'/></td></tr>"
				);
			});
		}
	});

});