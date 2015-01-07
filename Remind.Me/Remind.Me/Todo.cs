using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remind.Me
{
    public class Todo
    {
        public string Title { get; private set; }
        public string Details { get; private set; }

        public Todo(string title, string details)
        {
            this.Title = title;
            this.Details = details;
        }
    }
}
