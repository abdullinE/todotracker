using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ToDoTracker.DataModel;

namespace ToDoTracker.Web.Models
{
    public class ToDoItemViewModel
    {
        public DateTime? DeadLine { get; set; }
        public string Text { get; set; }
        public string AlertClass { get; set; }
        public string RowId { get; set; }
        public string DeadLineTitle { get; set; }

        public string IsDoneStyle { get; set; }
        public static ToDoItemViewModel GetViewModel(ToDoItem model)
        {

            var viewModel = new ToDoItemViewModel()
            {
                Text = model.Text,
                RowId = string.Format("todo_{0}", model.Id),
                IsDoneStyle = "visible;"

            };
            GenerateAlertType(model, viewModel);
            return viewModel;
        }

        public static void GenerateAlertType(ToDoItem model, ToDoItemViewModel viewModel)
        {
            if (model.IsFinished)
            {
                viewModel.AlertClass = "alert-success line-through";
                viewModel.IsDoneStyle = "hidden;";
            }
            else
            {
                
                var days = (model.DeadLine - model.CreatedDate).Days;
                if (days > 1)
                {
                    if ((model.DeadLine - DateTime.Now).Days >= 1)
                    {
                        viewModel.AlertClass = "alert-info";
                    }
                    else
                    {
                        viewModel.AlertClass = "alert-danger";
                    }

                }
                else
                {
                    if (model.DeadLine < DateTime.Now)
                    {
                        viewModel.AlertClass = "alert-danger";
                        viewModel.DeadLineTitle = string.Format("просрочено на {0:dd\\.hh\\:mm}", DateTime.Now - model.DeadLine);
                        return;
                    }
                    else
                    {
                        viewModel.AlertClass = "alert-info";
                    }
                }
                viewModel.DeadLineTitle = string.Format("осталось {0:dd\\.hh\\:mm}", model.DeadLine - DateTime.Now);
            }
        }
    }
}