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
           
			return View();
        }

        private void DorinTewst()
        {
            Highcharts chart = new Highcharts("SomehartID");
            chart.SetTitle(new Title() { Text = "My first chart" });
            chart.SetYAxis(new YAxis
            {
                Title = new YAxisTitle() { Text = "Count" },
            });


            List<Series> series = new List<Series>();
            List<object> serieData = new List<object>();

            Series serie = new Series();
            serie.Name = "Opened Cases";
            serie.Type = ChartTypes.Column;
            serieData.Clear();
            serieData.Add(64);
            serie.Data = new Data(serieData.ToArray());
            series.Add(serie);

            serie = new Series();
            serie.Name = "Closed Cases";
            serie.Type = ChartTypes.Column;
            serieData.Clear();
            serieData.Add(50);
            serie.Data = new Data(serieData.ToArray());
            series.Add(serie);

            serie = new Series();
            serie.Name = "Reactivated Cases";
            serie.Type = ChartTypes.Column;
            serieData.Clear();
            serieData.Add(89);
            serie.Data = new Data(serieData.ToArray());
            series.Add(serie);

            serie = new Series();
            serie.Name = "Reopened Cases";
            serie.Type = ChartTypes.Column;
            serieData.Clear();
            serieData.Add(19);
            serie.Data = new Data(serieData.ToArray());
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
			var allOrders = orderManager.GetQueryableOrders().Where(x => x.DistrictId != 0 && x.District != null);
			//var allCompletedOrders = orderManager.GetQueryableOrders().Where(x => x.isFinishedProperty?)
			var allDistricts = districtManager.GetIQueryableDistricts();
			var OrdersPerDistrict = allOrders.Join(allDistricts, x => x.DistrictId, y => y.Id, (x, y) => new { dName = y.Name, ordersSum = 1 }).GroupBy(x => x.dName).ToList();
			return Json(OrdersPerDistrict, JsonRequestBehavior.AllowGet);
		}

        public ActionResult ChartOrders()
        {
            Highcharts orders = new Highcharts("SomehartID");
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


    }
}
