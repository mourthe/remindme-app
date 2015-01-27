using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

using Remind.Me.Database;

namespace Remind.Me
{
    public class Container
    {
        public Reminder R { get; set; }
        public Geopoint Point { get; set; }

        public Container(Reminder r, Geopoint g)
        {
            R = r; Point = g;
        }
    }

    public class TwoPoints
    {
        public Geopoint start { get; set; }
        public Geopoint finish { get; set; }

        public TwoPoints(Geopoint start, Geopoint finish)
        {
            this.start = start; this.finish = finish;
        }
    }
}
