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
        public string Title { get; private set; }
        public string Details { get; private set; }
        public bool Checked { get; private set; }

        public Todo(string title, string details)
        {
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
