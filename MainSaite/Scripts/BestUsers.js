$(function () {

	$.ajax({
		url: '/Home/GetBestDrivers',
		type: 'POST',
		success: function (data) {
			data.forEach(function (value, index, array) {
				$('#best-drivers table').append(
					"<tr><td class='avatar'><img src='/Images/" +
					(value.ImageName != null ? value.ImageName : 'item_0_profile.jpg') +
					"'/></td><td class='userName'><h3>" +
					value.User.UserName + "</h3><div class='fullName'>" +
					(value.FullName != null ? value.FullName : '') + "</div><div id='rating'>" +
					getStars(value.User.Rating) + "</div></td></tr>" +
					"><tr><td colspan='2'><hr class='underline'/></td></tr>"
				);
			});
		}
	});

	$.ajax({
		url: '/Home/GetBestClients',
		type: 'POST',
		success: function (data) {
			data.forEach(function (value, index, array) {
				$('#best-clients table').append(
					"<tr><td class='avatar'><img src='/Images/" +
					(value.ImageName != null ? value.ImageName : 'item_0_profile.jpg') +
					"'/></td><td class='userName'><h3>" +
					value.User.UserName + "</h3><div class='fullName'>" +
					(value.FullName != null ? value.FullName : '') + "</div><div id='rating'>" +
					getStars(value.User.Rating) + "</div></td></tr>" +
					"><tr><td colspan='2'><hr class='underline'/></td></tr>"
				);
			});
		}
	});

});

function getStars(rating) {
	if (rating == null)
		rating = 0;
	else
		rating = Math.round(rating);
	var result = '';
	var i = 1;
	for (; i <= rating; i++)
		result += '<span class="glyphicon glyphicon-star star-full"></span>';
	for (; i <= 5; i++)
		result += '<span class="glyphicon glyphicon-star-empty star-empty"></span>';
	return result;
}