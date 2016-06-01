$(function () {

	$.ajax({
		url: '/Home/GetBestDrivers',
		type: 'POST',
		success: function (data) {
			data.forEach(function (value, index, array) {
				$('#best-drivers').append(
					"<div class='col-lg-6 col-md-12 user'><div class='col-sm-5 avatar'><img src='/Images/" +
					(value.ImageName != null ? value.ImageName : 'item_0_profile.jpg') +
					"'/></div><div class='col-sm-7 description'><div class='userName'><h3>" +
					value.User.UserName + "</h3></div><div class='fullName'>" +
					(value.FullName != null ? value.FullName : '') + "</div><div id='rating'>" +
					getStars(value.User.Rating) + "</div></div><div class='col-sm-12'><hr/></div></div>"
				);
			});
		}
	});

	$.ajax({
		url: '/Home/GetBestClients',
		type: 'POST',
		success: function (data) {
			data.forEach(function (value, index, array) {
				$('#best-clients').append(
					"<div class='col-lg-6 col-md-12 user'><div class='col-sm-5 avatar'><img src='/Images/" +
					(value.ImageName != null ? value.ImageName : 'item_0_profile.jpg') +
					"'/></div><div class='col-sm-7 description'><div class='userName'><h3>" +
					value.User.UserName + "</h3></div><div class='fullName'>" +
					(value.FullName != null ? value.FullName : '') + "</div><div id='rating'>" +
					getStars(value.User.Rating) + "</div></div><div class='col-sm-12'><hr/></div></div>"
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