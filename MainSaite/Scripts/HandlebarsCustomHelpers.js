Handlebars.registerHelper("PrettifyDate", function (timestamp) {
	return moment(timestamp).format('L').toString();
});

//DO NOT DELETE!
Handlebars.registerHelper('ifCond', function (v1, v2, options) {
	if (v1 === v2) {
		return options.fn(this);
	}
	return options.inverse(this);
});