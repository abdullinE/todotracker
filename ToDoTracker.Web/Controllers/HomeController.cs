using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using ToDoTracker.DataLayer;
using ToDoTracker.DataModel;
using ToDoTracker.Web.Helpers;
using ToDoTracker.Web.Models;

namespace ToDoTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        private IToDoRepo _toDoRepository;

        public HomeController(IToDoRepo toDoRepo)
        {
            _toDoRepository = toDoRepo;
        }
        public ActionResult Index()
        {
            var list = _toDoRepository.All().OrderByDescending(i => i.Id).ToList();
            var model = list.Select(ToDoItemViewModel.GetViewModel).ToList();
            return View(model);
        }

        public ActionResult IsFinished(string idString)
        {
            var id = int.Parse(idString.Replace("#todo_",""));
            var item = _toDoRepository.Find(i => i.Id == id);
            item.IsFinished = true;
            _toDoRepository.Update(item);
            return Json(new {success = true});
        }
        public ActionResult Add(ToDoItemViewModel model)
        {
            var toDoItem = new ToDoItem()
            {
                Text = model.Text,
                DeadLine = model.DeadLine ?? DateTime.Now + new TimeSpan(7, 0, 0, 0),
                IsFinished = false
            };
            var item = _toDoRepository.Create(toDoItem);
            var viewModel = ToDoItemViewModel.GetViewModel(item);
            return PartialView("ToDoItemBlock", viewModel);
        }
    }
}
