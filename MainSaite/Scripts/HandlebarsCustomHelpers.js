Handlebars.registerHelper('ifCond', function (v1, v2, options) {
	if (v1 === v2) {
		return options.fn(this);
	}
	return options.inverse(this);
});
//Handlebars.registerHelper("prettifyDate", function (timestamp) {
//	return moment(newDate);
//	//var newts = timestamp.replace('/Date(', '');
//	//var result = newts.replace(')/', '');
//	//var date = moment(+result);
//	//return date;
//	//return moment(timestamp).format('L').toString();
//	//return moment.locale('en', timestamp);
//	//return moment(timestamp);
//});
//Handlebars.registerHelper("prettifyDate", function (timestamp) {
//	var tryParseThis = moment(timestamp.toString());
//	return tryParseThis;
//});

Handlebars.registerHelper("prettifyDate", function(timestamp) {
	return moment(timestamp).format('MM.DD.YYYY').toString();
});