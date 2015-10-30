Handlebars.registerHelper("PrettifyDate", function (timestamp) {
    return moment(timestamp).format('L').toString();
})