using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoTracker.Web.Models
{
    public class ToDoViewModel
    {
        public string Text { get; set; }
        public DateTime DeadLine { get; set; }
    }
}