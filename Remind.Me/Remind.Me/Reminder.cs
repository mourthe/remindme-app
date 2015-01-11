using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remind.Me
{
    public class Reminder
    {
        public string Title { get; private set; }
        public string Details { get; private set; }
        public string Local { get; private set; }
        public bool Active { get; set; }

        public Reminder(string title, string details, string local)
        {
            this.Title = title;
            this.Details = details;
            this.Local = local;
            this.Active = true;
        }
    }
}
