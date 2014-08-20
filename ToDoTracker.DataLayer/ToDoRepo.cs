using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTracker.DataLayer.Common;
using ToDoTracker.DataLayer.DataContext;
using ToDoTracker.DataModel;

namespace ToDoTracker.DataLayer
{
    public interface IToDoRepo : IRepository<ToDoItem>
    {
    }
    public class ToDoRepo :Repository<ToDoItem>, IToDoRepo
    {
        public ToDoRepo(ToDoDbContext context) : base(context)
        {
        }
    }
}
