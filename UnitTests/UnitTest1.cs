using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Tools;
using Model.DTO;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        PriceCounter pc;

        CoordinatesDTO coord1 = new CoordinatesDTO();
        CoordinatesDTO coord2 = new CoordinatesDTO();
        CoordinatesDTO coord3 = new CoordinatesDTO();
        CoordinatesDTO coord4 = new CoordinatesDTO();
        CoordinatesDTO coord5 = new CoordinatesDTO();
        CoordinatesDTO coord6 = new CoordinatesDTO();
        CoordinatesDTO coord7 = new CoordinatesDTO();
        CoordinatesDTO coord8 = new CoordinatesDTO();
        CoordinatesDTO coord9 = new CoordinatesDTO();
        List<CoordinatesDTO> coords = new List<CoordinatesDTO>();
        DateTime time = DateTime.Now;

        TarifDTO tarif1 = new TarifDTO();
        TarifDTO tarif2 = new TarifDTO();
        List<TarifDTO> tarifes = new List<TarifDTO>();

        [TestInitialize]
        public void Setup()
        {
            pc = new PriceCounter(new System.Collections.Generic.List<Model.DTO.CoordinatesDTO>(), 
                                    new System.Collections.Generic.List<Model.DTO.TarifDTO>());

            
            coord1.Latitude = 48.281532;
            coord1.Latitude = 25.923964;
            coord1.AddedTime = time;
            coord1.TarifId = 1;

            coord2.Latitude = 48.279494;
            coord2.Latitude = 25.927353;
            coord2.AddedTime = time.AddMinutes(0.5);
            coord2.TarifId = 1;

            coord3.Latitude = 48.281374;
            coord3.Latitude = 25.929870;
            coord3.AddedTime = time.AddMinutes(1);
            coord3.TarifId = 1;

            coord4.Latitude = 48.282954;
            coord4.Latitude = 25.932398;
            coord4.AddedTime = time.AddMinutes(1.5);
            coord4.TarifId = 1;

            coord5.Latitude = 48.287259;
            coord5.Latitude = 25.933976;
            coord5.AddedTime = time.AddMinutes(2);
            coord5.TarifId = 1;

            coord6.Latitude = 48.286995;
            coord6.Latitude = 25.935931;
            coord6.AddedTime = time.AddMinutes(2.5);
            coord6.TarifId = 1;

            coord7.Latitude = 48.289212;
            coord7.Latitude = 25.934408;
            coord7.AddedTime = time.AddMinutes(3);
            coord7.TarifId = 1;

            coord8.Latitude = 48.292470;
            coord8.Latitude = 25.935543;
            coord8.AddedTime = time.AddMinutes(3.5);
            coord8.TarifId = 1;

            coord9.Latitude = 48.291116;
            coord9.Latitude = 25.940207;
            coord9.AddedTime = time.AddMinutes(4);
            coord9.TarifId = 1;

            tarif1.id = 1;
            tarif1.MinimalPrice = 5;
            tarif1.StartPrice = 10;
            tarif1.WaitingCost = 3;
            tarif1.OneMinuteCost = 5;
            tarif1.Name = "test1";


            tarif2.id = 2;
            tarif2.MinimalPrice = 10;
            tarif2.StartPrice = 15;
            tarif2.WaitingCost = 10;
            tarif2.OneMinuteCost = 15;
            tarif2.Name = "test2";


            coords.Add(coord1);
            coords.Add(coord2);
            coords.Add(coord3);
            coords.Add(coord4);
            coords.Add(coord5);
            coords.Add(coord6);
            coords.Add(coord7);
            coords.Add(coord8);
            coords.Add(coord9);

            tarifes.Add(tarif1);
            tarifes.Add(tarif2);
        }

        [TestMethod]
        public void TestCountOfMinutes_Zero()
        {
            var a = DateTime.Now;
            var zero = pc.CountOfMinutes(a, a);

            Assert.AreEqual(0.0, zero, 0.0001, "sould be zero, but you get " + zero);
        }

                [TestMethod]
        public void TestCountOfMinutes_OneMinute()
        {
            var a = DateTime.Now;
            var actualy = pc.CountOfMinutes(a, a.AddMinutes(1));
            var expected = 1.0;

            Assert.AreEqual(expected, actualy, 0.0000001, "sould be 1, but you get "+ actualy);
        }

        [TestMethod]
        public void TestCountOfMinutes_HalfMinute()
        {
            var a = DateTime.Now;
            var actualy = pc.CountOfMinutes(a, a.AddMinutes(0.5));
            var expected = 0.5;
            
            Assert.AreEqual(expected, actualy, 0.0000001, "sould be 1, but you get " + actualy);
        }


        [TestMethod]
        public void TestGetDistance()
        {
            PrivateObject po = new PrivateObject(typeof(PriceCounter));
            var actualy = Convert.ToDouble(po.Invoke("GetDistance", 48.281533, 25.923964, 48.279494, 25.927352));
            var googleResult = 0.33793;

            Assert.AreEqual(googleResult, actualy, 0.01,"");
        }

        [TestMethod]
        public void TestGetDistanceZero()
        {
            PrivateObject po = new PrivateObject(typeof(PriceCounter));
            var actualy = Convert.ToDouble(po.Invoke("GetDistance", 48.281533, 25.923964, 48.281533, 25.923964));
            var expected = 0;

            Assert.AreEqual(expected, actualy, 0.01, "");
        }

        [TestMethod]
        public void TestPriceCounter1Tarif()
        {
            pc = new PriceCounter(coords, tarifes);
            var actualy = pc.CalcPrice();
            var expected = 4 * tarif1.OneMinuteCost + tarif1.StartPrice;

            Assert.AreEqual(expected, actualy);
        }

        [TestMethod]
        public void TestPriceCounter2Tarif()
        {
            pc = new PriceCounter(coords, tarifes);

            coords[5].TarifId = 2;
            coords[6].TarifId = 2;
            coords[7].TarifId = 2;
            coords[8].TarifId = 2;
            
            var actualy = pc.CalcPrice();
            var expected = 2 * tarif1.OneMinuteCost + tarif1.StartPrice + tarif2.OneMinuteCost*2;
            Assert.AreEqual(expected, actualy);

        }


        [TestMethod]
        public void TestPriceCounter2TarifwhithSlowDriving()
        {
            pc = new PriceCounter(coords, tarifes);

            coords[5].TarifId = 2;
            coords[6].TarifId = 2;
            coords[7].TarifId = 2;
            coords[8].TarifId = 2;

            coords[5].AddedTime = time.AddMinutes(10);
            coords[6].AddedTime = time.AddMinutes(20);
            coords[7].AddedTime = time.AddMinutes(30);
            coords[8].AddedTime = time.AddMinutes(40);

            var actualy = pc.CalcPrice();
            var expected = (2 * tarif1.OneMinuteCost + tarif1.StartPrice) + tarif2.WaitingCost * 8 + tarif2.WaitingCost * 30;
            Assert.AreEqual(expected, actualy);

        }

    }
}
