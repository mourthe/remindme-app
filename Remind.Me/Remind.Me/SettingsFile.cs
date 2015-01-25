using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Remind.Me
{
    [DataContract]
    public class SettingsFile
    {
        [DataMember]
        public double Distance { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        public SettingsFile(double distance, double lat, double lon)
        {
            this.Distance = distance;
            this.Longitude = lon;
            this.Latitude = lat;
        }
    }
}
