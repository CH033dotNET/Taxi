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

Handlebars.registerHelper("prettifyDate", function (timestamp) {
	var TimeString = timestamp.toString();
	var date = moment(TimeString, ["MM/DD/YYYY", "DD/MM/YYYY", "DD.MM.YYYY", "MM.DD.YYYY"]);

	//return new Date(timestamp).toString('MM/dd/yyyy');

	var noMili = new Date(date).toLocaleDateString("en");

	//var formatThis = moment(noMili).format('DD/MM/YYYY');

	//var formatThis = moment(noMili, "L");
	return noMili;
});

Handlebars.registerHelper("prettifyDate2", function (timestamp) {
	return moment(timestamp);
});

Handlebars.registerHelper("prettifyDate3", function (timestamp) {
	var curr_date = timestamp.getDate();
	var curr_month = timestamp.getMonth();
	curr_month++;
	var curr_year = timestamp.getFullYear();
	var result = curr_date + "/" + curr_month + "/" + curr_year;
	return result;
});
