using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remind.Me
{
    public class Todo
    {
        public string title { get; private set; }
        public string details { get; private set; }

        public Todo(string title, string details)
        {
            this.title = title;
            this.details = details;
        }
    }
}
