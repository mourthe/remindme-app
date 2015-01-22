using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace Remind.Me.Database
{
    public class Reminder
    {
        [PrimaryKey]
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Details { get; private set; }
        public string Local { get; private set; }
        public bool Active { get; set; }

        public Reminder(string title, string details, string local)
        {
            this.Id = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"); 
            this.Title = title;
            this.Details = details;
            this.Local = local;
            this.Active = true;
        }

        /// <summary>
        /// Database use-only!
        /// </summary>
        public Reminder()
        {
        }
    }
}
