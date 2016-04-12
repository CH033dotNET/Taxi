$(function () {
	var container = $('#content');
	$.ajax({
		url: GetHistory,
		type: 'GET',
		dateType: 'json',
		success: function (data) {
			var source = $("#driveHistotyList").html();
			var template = Handlebars.compile(source);
			var wrapper = { objects: data };
			var html = template(wrapper);
			container.append(html);
		}
	});
})