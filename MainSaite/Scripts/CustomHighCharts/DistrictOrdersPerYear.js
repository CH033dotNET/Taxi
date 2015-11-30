
var ChartController = {
	// gets necessary charts data from ajax call and sends it to function via parameter
	getChartData: function () {
		var breaky = "lolololo";
		var getEverything =
			$.ajax({
				url: "./ReportCharts/GetDistrictReportsPerYear", // <----------------------------------------------------!!
				method: "GET",
				dataType: "JSON",
			}).done(function (result) {
				if (result != null) {
					ChartController.showOrderCharts(result);
				}
				else { alert("!!!!") }
			});
		//getEverything.then(ChartController.showOrderCharts(result), function () { alert("!!!!") });
	},

	// main function for charts calling
	showOrderCharts: function (e) {
		// get data
		var getData = new Array();
		for (var i = 0; i < e.length; i++) {
			getData[i] = {
				name: ChartsLocStrings.district + ": " + e[i][0].dName, // district name is created here.
				y: e[i].length, // percent are evaluated here. 
				Year: e[i][0].Year // curent orders year is evaluated here.
			}
		};
		// summon highcharts!!
		ChartController.summonChart(getData);
	},

	// function that containing charts code
	summonChart: function (receivedData) {
		// Build the chart
		$('#container').highcharts({
			chart: {
				plotBackgroundColor: null,
				plotBorderWidth: null,
				plotShadow: false,
				type: 'pie'
			},
			title: {
				text: ChartsLocStrings.districtHeader + " " + receivedData[0].Year
			},
			tooltip: {
				pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
			},
			plotOptions: {
				pie: {
					allowPointSelect: true,
					cursor: 'pointer',
					dataLabels: {
						enabled: true
					},
					showInLegend: true
				}
			},
			series: [{
				name: ChartsLocStrings.completedOrdersN,
				colorByPoint: true,
				data: receivedData
			}]
		});
	},
}