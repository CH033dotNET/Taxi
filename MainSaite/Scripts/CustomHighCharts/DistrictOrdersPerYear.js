
var ChartController = {
	showOrderCharts: function () {
		$(function () {

			var colors = Highcharts.getOptions().colors,
				categories = ['District1', 'District2', 'District3', 'District4', 'District5'],
				data = [{
					y: 20.00,
					color: colors[0],
					drilldown: {
						name: 'Orders',
						categories: ['Rejected orders', 'Completed orders'],
						data: [10.00, 10.00],
						color: colors[0]
					}
				}, {
					y: 20.00,
					color: colors[1],
					drilldown: {
						name: 'Orders',
						categories: ['Rejected orders', 'Completed orders'],
						data: [10.00, 10.00],
						color: colors[1]
					}
				}, {
					y: 20.00,
					color: colors[2],
					drilldown: {
						name: 'Orders',
						categories: ['Rejected orders', 'Completed orders'],
						data: [10.00, 10.00],
						color: colors[2]
					}
				}, {
					y: 20.00,
					color: colors[3],
					drilldown: {
						name: 'Orders',
						categories: ['Rejected orders', 'Completed orders'],
						data: [10.00, 10.00],
						color: colors[3]
					}
				}, {
					y: 20.00,
					color: colors[4],
					drilldown: {
						name: 'Orders',
						categories: ['Rejected orders', 'Completed orders'],
						data: [10.00, 10.00],
						color: colors[4]
					}
				}],
				browserData = [],
				versionsData = [],
				i,
				j,
				dataLen = data.length,
				drillDataLen,
				brightness;


			// Build the data arrays
			for (i = 0; i < dataLen; i += 1) {

				// add browser data
				browserData.push({
					name: categories[i],
					y: data[i].y,
					color: data[i].color
				});

				// add version data
				drillDataLen = data[i].drilldown.data.length;
				for (j = 0; j < drillDataLen; j += 1) {
					brightness = 0.2 - (j / drillDataLen) / 5;
					versionsData.push({
						name: data[i].drilldown.categories[j],
						y: data[i].drilldown.data[j],
						color: Highcharts.Color(data[i].color).brighten(brightness).get()
					});
				}
			}

			// Create the chart
			$('#container').highcharts({
				chart: {
					type: 'pie'
				},
				title: {
					text: 'Orders per district statistic, January, 2015 to May, 2015'
				},
				subtitle: {
					text: 'Source: <a href="http://netmarketshare.com/">somesource.com</a>'
				},
				yAxis: {
					title: {
						text: 'Total percent of orders processed'
					}
				},
				plotOptions: {
					pie: {
						shadow: false,
						center: ['50%', '50%']
					}
				},
				tooltip: {
					valueSuffix: '%'
				},
				series: [{
					name: 'Districts',
					data: browserData,
					size: '60%',
					dataLabels: {
						formatter: function () {
							return this.y > 5 ? this.point.name : null;
						},
						color: '#ffffff',
						distance: -30
					}
				}, {
					name: 'Orders',
					data: versionsData,
					size: '80%',
					innerSize: '60%',
					dataLabels: {
						formatter: function () {
							// display only if larger than 1
							return this.y > 1 ? '<b>' + this.point.name + ':</b> ' + this.y + '%' : null;
						}
					}
				}]
			});
		});
	}
}