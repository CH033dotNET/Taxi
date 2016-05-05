using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;

namespace DriverSite.Helpers
{
    internal class CoordinateMapper
    {
        public static CoordinatesDTO InitializeCoordinates(string Longitude, string Latitude, string Accuracy, int UserId)
        {
            CoordinatesDTO coordinates = new CoordinatesDTO();
            try
            {
                coordinates.Latitude = double.Parse(Latitude);
                coordinates.Longitude = double.Parse(Longitude);
                coordinates.Accuracy = double.Parse(Accuracy);
            }
            catch (FormatException)
            {
                try
                {
                    Latitude = Latitude.Replace('.', ',');
                    Longitude = Longitude.Replace('.', ',');
                    Accuracy = Accuracy.Replace('.', ',');
                    coordinates.Latitude = double.Parse(Latitude);
                    coordinates.Longitude = double.Parse(Longitude);
                    coordinates.Accuracy = double.Parse(Accuracy);
                }
                catch (FormatException)
                {
                    Latitude = Latitude.Replace(',', '.');
                    Longitude = Longitude.Replace(',', '.');
                    Accuracy = Accuracy.Replace(',', '.');
                    coordinates.Latitude = double.Parse(Latitude);
                    coordinates.Longitude = double.Parse(Longitude);
                    coordinates.Accuracy = double.Parse(Accuracy);
                }
            }
            coordinates.UserId = UserId;
            coordinates.AddedTime = DateTime.Now;
            return coordinates;
        }
    }
}
