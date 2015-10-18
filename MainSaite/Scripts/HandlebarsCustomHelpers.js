Handlebars.registerHelper('ifCond', function (v1, v2, options) {
	if (v1 === v2) {
		return options.fn(this);
	}
	return options.inverse(this);
});
//Handlebars.registerHelper("prettifyDate", function(timestamp) {
//	return moment(timestamp).format('MM/DD/YYYY').toString();
//});