using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;

namespace ToDoTracker.Web.Helpers
{
    public class ToDoHelper:Hub
    {

        public void Send(dynamic message)
        {
            Clients.Others.AddToDoItem(message);
        }

        public void TodoDone(dynamic message)
        {
            Clients.Others.TodoDone(message);
        }
    }
}