using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remind.Me
{
    public class SettingsFile
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        public SettingsFile(double distance, double lat, double lon)
        {
            this.Distance = distance;
            this.Longitude = lon;
            this.Latitude = lat;
        }
    }
}
