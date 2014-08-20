using System;
using ToDoTracker.DataModel.Common;

namespace ToDoTracker.DataModel
{
    public class ToDoItem:EntityBase
    {
        public string Text { get; set; }
        public DateTime DeadLine { get; set; }
        public bool IsFinished { get; set; }
    }
}