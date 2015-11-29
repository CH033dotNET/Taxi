using BAL.Manager;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Collections;
using Model.DTO;

namespace MainSaite.Controllers
{
    public class ReportChartsController : BaseController
    {
		IDistrictManager districtManager;
        IOrderManager orderManager;
        IUserManager userManager;
		public ReportChartsController(IOrderManager orderManager, IUserManager userManager, IDistrictManager districtManager)
        {
            this.orderManager = orderManager;
            this.userManager = userManager;
			this.districtManager = districtManager;
        }
        
        //
        // GET: /ReportCharts/
		public ActionResult Index()
        { 
            ChartOrders();
            DorinTewst();
			DriversIncome();
			return View(GrafickOf10TopClients());
        }

        private void DorinTewst()
        {
			Queue<string> categoriesQueue = new Queue<string>(new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" });
			for (int i = 0; i < DateTime.Now.Month; i++)
			{
				categoriesQueue.Enqueue(categoriesQueue.Dequeue());
			}
			var categories = categoriesQueue.ToArray();
			Highcharts chart = new Highcharts("FuelConsumptionID");
			chart.SetTitle(new Title() { Text = Resources.Resource.FuelConsumDorin });
			var list = orderManager.AnnualFuelConsumption();

			List<object> obList = new List<object>();
			foreach (var item in list)
			{
				var iut = (int)item;
				obList.Add((object)iut);
			}

			chart.SetYAxis(new YAxis
			{
				Title = new YAxisTitle() { Text = Resources.Resource.FuleLDorin },

			});

			chart.SetXAxis(new XAxis
			{
				Title = new XAxisTitle() { Text = Resources.Resource.FuleMonthDorin },
				Categories = categories
			});



			List<Series> series = new List<Series>();

			Series serie = new Series();
			serie.Name = Resources.Resource.LetresDorin;
			serie.Type = ChartTypes.Column;
			//serie.Data = new Data(serieData.ToArray());
			serie.Data = new Data(obList.ToArray());
			series.Add(serie);


			chart.SetSeries(series.ToArray());
			chart.SetLegend(new Legend()
			{
				Align = HorizontalAligns.Right,
				Layout = Layouts.Vertical,
				VerticalAlign = VerticalAligns.Top
			});

			chart.SetPlotOptions(new PlotOptions()
			{
				Area = new PlotOptionsArea() { Stacking = Stackings.Normal }
			});

			chart.SetCredits(new Credits() { Enabled = false });


			ViewBag.Chart = chart;
        }

		public ActionResult DistrictReportsPerYear()
		{
			return PartialView();
		}

		public JsonResult GetDistrictReportsPerYear()
		{
			var allOrders = orderManager.GetQueryableOrders().Where(x => x.District != null && x.DistrictId != 0 && x.DriverId != 0 && x.OrderTime.Year == DateTime.Now.Year);
			//var allCompletedOrders = orderManager.GetQueryableOrders().Where(x => x.isFinishedProperty?)
			var allDistricts = districtManager.GetIQueryableDistricts();
			var OrdersPerDistrict = allOrders.Join(allDistricts, x => x.DistrictId, y => y.Id, (x, y) => new { dName = y.Name, ordersSum = 1, Year = x.OrderTime.Year }).GroupBy(x => x.dName).ToList();
			return Json(OrdersPerDistrict, JsonRequestBehavior.AllowGet);
		}

        public ActionResult ChartOrders()
        {
            Highcharts orders = new Highcharts("OrderID");
            orders.SetTitle(new Title() { Text = "Orders" });
            orders.SetYAxis(new YAxis
            {
                Title = new YAxisTitle() { Text = "Count" },
            });

            var ord = orderManager.GetQueryableOrders();
            var drivers = userManager.GetQueryableDrivers();

            var res = ord.Join(drivers, x => x.DriverId, y => y.Id, (x, y) => new { Name = y.UserName, Orders = 1 }).GroupBy(x=>x.Name).ToList();


			

            List<Series> series = new List<Series>();
            List<object> serieData = new List<object>();

            foreach (var i in res)
            {

                Series serie = new Series();
                serie.Name = i.Key;
                serie.Type = ChartTypes.Column;
                serieData.Clear();
                serieData.Add(i.Count());
                serie.Data = new Data(serieData.ToArray());
                series.Add(serie);

            }


            orders.SetSeries(series.ToArray());
            orders.SetLegend(new Legend()
            {
                Align = HorizontalAligns.Right,
                Layout = Layouts.Vertical,
                VerticalAlign = VerticalAligns.Top
            });

            orders.SetPlotOptions(new PlotOptions()
            {
                Area = new PlotOptionsArea() { Stacking = Stackings.Normal }
            });

            orders.SetCredits(new Credits() { Enabled = false });


            ViewBag.Order = orders;

            return View();
        }

		public Highcharts GrafickOf10TopClients()
		{
			Highcharts chart = new Highcharts("OrdersReport");
			chart.SetTitle(new Title() { Text = Resources.Resource.OrderStatisticMax });
			chart.SetSubtitle(new Subtitle() { Text = Resources.Resource.To10ClientsMax });
			chart.SetYAxis(new YAxis() { Title = new YAxisTitle() { Text = Resources.Resource.OrderIncomeMax } });
			chart.SetXAxis(new XAxis() { Title = new XAxisTitle() { Text = Resources.Resource.AmountOfOrdersMax } });
			List<Series> series = new List<Series>();
			List<object[]> data = new List<object[]>();
			foreach (var client in orderManager.GetTop10())
			{
				Series serie = serie = new Series();
				serie.Type = ChartTypes.Column;
				serie.Name = client.Select(x => x.Person.FirstName).First(); ;
				data.Clear();
				data.Add(new object[] { client.Count(), client.Sum(x => x.TotalPrice) });
				serie.Data = new Data(data.ToArray());
				series.Add(serie);
			}
			chart.SetSeries(series.ToArray());
			chart.SetLegend(new Legend()
			{
				Align = HorizontalAligns.Right,
				Layout = Layouts.Vertical,
				VerticalAlign = VerticalAligns.Top
			});

			return chart;
		}
		public ActionResult YearIncome()
		{
			Queue<string> categoriesQueue = new Queue<string>(new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" });
			for(int i = 0; i < DateTime.Now.Month; i++)
			{
				categoriesQueue.Enqueue(categoriesQueue.Dequeue());
			}
			var categories = categoriesQueue.ToArray();
			Highcharts chart = new Highcharts("IncomeByMonthes");
			chart.SetTitle(new Title() { Text = Resources.Resource.AllDriversIncomeHeader });
			var list = orderManager.YearIncome().ToList();
			var data = list.Select(x => (object)x).ToArray();
			chart.SetSeries(new Series()
			{
				Name = Resources.Resource.TotalDriversIncomeA,
				Type = ChartTypes.Column,
				Data = new Data(data)
			});
			chart.SetXAxis(new XAxis()
			{
				Categories = categories
			});
			chart.SetYAxis(new YAxis() { Title = new YAxisTitle() { Text = Resources.Resource.ValuesAsix } });
			return PartialView(chart);
		}

		public void DriversIncome()
		{
			var DriversInc = orderManager.GetDriversIncome();

			Highcharts driversIncomeChart = new Highcharts("driversChartId");

			driversIncomeChart.SetTitle(new Title() { Text = Resources.Resource.DriversIncome });

			driversIncomeChart.SetXAxis(new XAxis() { 
				Title = new XAxisTitle() { Text = @Resources.Resource.Drivers },
				Categories = new string[] {Resources.Resource.Info}
			});
			driversIncomeChart.SetYAxis(new YAxis() { 
				Title = new YAxisTitle() { Text = @Resources.Resource.IncomeUAH }});

			List<Series> series = new List<Series>();
			List<object> serieData = new List<object>();

			Series serie = new Series();

			foreach (ChartsColumnDTO item in DriversInc)
			{
				serie = new Series();
				serie.Name = item.ColumnName;
				serie.Type = ChartTypes.Column;
				serieData.Clear();
				serieData.Add(new object[] { item.Value });
				serie.Data = new Data(serieData.ToArray());
				series.Add(serie);

			};

			driversIncomeChart.SetSeries(series.ToArray());


			driversIncomeChart.SetLegend(new Legend()
			{
				Align = HorizontalAligns.Right,
				Layout = Layouts.Vertical,
				VerticalAlign = VerticalAligns.Top
			});

			driversIncomeChart.SetPlotOptions(new PlotOptions()
			{
				Area = new PlotOptionsArea() { Stacking = Stackings.Normal }
			});

			driversIncomeChart.SetCredits(new Credits() { Enabled = false });
			ViewBag.DriversIncomeChart = driversIncomeChart;

		}
    }
}
