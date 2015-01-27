using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

// using Microsoft.Phone.Tasks;

namespace TaskBackground
{
    public sealed class BingService
    {
        public IDictionary<string, Geopoint> ListNearbyPlaces { get; private set; }
        private string[] _places = { "Casa", "Supermercado", "Cafeteria" };
        private string[] _points = { "-22.9362,-43.1895", "-22.9322,-43.1828", "-22.9386,-43.1914" };

        public BingService(/**/)
        {
            /*
             // Get the Current location Coordinates

            Geolocator geolocator = new Geolocator();

            geolocator.DesiredAccuracyInMeters = 50;

            Geoposition position = await geolocator.GetGeopositionAsync(

            maximumAge: TimeSpan.FromMinutes(1), timeout: TimeSpan.FromSeconds(30));
             
            BingMapsTask bingMapsTask = new BingMapsTask();

            // Pass the current location coordinates

            bingMapsTask.Center = new GeoCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude);

            bingMapsTask.ZoomLevel = 2;

            // Search Term; To search coffee shops set ‘coffee’

            bingMapsTask.SearchTerm = “hospital“;
            
            
             */


            this.ListNearbyPlaces = new Dictionary<string,Geopoint>();

            for (int i = 0; i < _places.Length; i++)
			{
                var coords = _points[i].Split(',');
                var position = new BasicGeoposition();
                position.Latitude = Double.Parse(coords[0]);
                position.Longitude = Double.Parse(coords[1]);

                this.ListNearbyPlaces.Add(_places[i], new Geopoint(position));	
			}            
        }
    }
}
