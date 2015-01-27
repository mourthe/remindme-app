namespace TaskBackground
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;

    using Windows.Devices.Geolocation;
    using Windows.Devices.Geolocation.Geofencing;

    public static class GeofencesHelper
    {
        public static IList<string> RemindersPlaces { get; set; }

        private static SettingsFile _settings = null;

        public static IList<Geofence> GetGeofences()
        {
            return GeofenceMonitor.Current.Geofences;
        }

        public static void SetSettings (string settings)
        {
            _settings = SettingsFile.Deseralize(settings);
        }

        public static void CreateGeofence(string id, double lat, double lon, double radius)
        {
            if (GeofenceMonitor.Current.Geofences.SingleOrDefault(g => g.Id == id) != null) return;

            var position = new BasicGeoposition();
            position.Latitude = lat;
            position.Longitude = lon;

            var geocircle = new Geocircle(position, radius);

            MonitoredGeofenceStates mask = 0;
            mask |= MonitoredGeofenceStates.Entered;

            // Create Geofence with the supplied id, geocircle and mask, not for single use
            // and with a dwell time of 5 seconds
            var geofence = new Geofence(id, geocircle, mask, false, new TimeSpan(0, 0, 5));
            GeofenceMonitor.Current.Geofences.Add(geofence);
        }

        public static void RemoveGeofence(string id)
        {
            var geofence = GeofenceMonitor.Current.Geofences.SingleOrDefault(g => g.Id == id);

            if (geofence != null)
                GeofenceMonitor.Current.Geofences.Remove(geofence);
        }
    }

    [DataContract]
    public sealed class SettingsFile
    {
        [DataMember]
        public double Distance { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        public SettingsFile(string settings)
        {
            var s = Deseralize(settings);

            this.Distance = s.Distance;
            this.Longitude = s.Longitude;
            this.Latitude = s.Longitude;
        }

        public static SettingsFile Deseralize(string st)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(st));
            var ser = new DataContractJsonSerializer(typeof(SettingsFile));

            return (SettingsFile)ser.ReadObject(stream);
        }
    }
}
