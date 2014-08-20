using System;

namespace ToDoTracker.DataLayer
{
    public interface IUnitOfWork:IDisposable
    {
        int SaveChanges();
    }
}
