using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using ToDoTracker.DataModel;
using ToDoTracker.DataModel.Common;

namespace ToDoTracker.DataLayer.DataContext
{
    public class ToDoDbContext:DbContext
    {
        public ToDoDbContext()
            : base("ToDoTrackerConnectionstring")
        {
            //Database.SetInitializer(new DropCreateIfModelChangesInitializer());
            Database.SetInitializer(new CreateDatabaseIfNotExists<ToDoDbContext>());
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        //public DbSet<AccessProfile> AccessProfiles { get; set; }


        public override int SaveChanges()
        {
            ObjectContext context = ((IObjectContextAdapter)this).ObjectContext;

            //Find all Entities that are Added/Modified that inherit from my EntityBase
            IEnumerable<ObjectStateEntry> objectStateEntries =
                context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified).Where(
                    e => e.IsRelationship == false &&
                         e.Entity != null &&
                         e.Entity is EntityBase);

            var currentTime = DateTime.Now;

            foreach (var entry in objectStateEntries)
            {
                var entityBase = entry.Entity as EntityBase;
                if (entityBase != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entityBase.CreatedDate = currentTime;
                    }

                    entityBase.ModifiedDate = currentTime;

                }
            }

            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                //foreach (var validationErrors in dbEx.EntityValidationErrors)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        _logger.ErrorException(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage),dbEx);
                //    }
                //}
                throw;
            }
        }
    }
}