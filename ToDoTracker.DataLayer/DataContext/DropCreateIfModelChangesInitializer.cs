using System.Data.Entity;

namespace ToDoTracker.DataLayer.DataContext
{
    public class DropCreateIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<ToDoDbContext>
    {
        protected override void Seed(ToDoDbContext context)
        {    
            base.Seed(context);
        }
    }
}
