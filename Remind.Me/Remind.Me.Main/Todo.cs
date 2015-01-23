using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace Remind.Me.Database
{
    public class Todo
    {
        [PrimaryKey]
        public string Id { get; private set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool Checked { get; set; }

        public Todo(string title, string details)
        {
            this.Id = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"); 
            this.Title = title;
            this.Details = details;
            this.Checked = false;
        }

        /// <summary>
        /// Database use-only!
        /// </summary>
        public Todo()
        {
        }
    }
}
