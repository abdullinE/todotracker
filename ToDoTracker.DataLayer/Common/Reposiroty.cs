using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ToDoTracker.DataLayer.DataContext;
using ToDoTracker.DataModel.Common;

namespace ToDoTracker.DataLayer.Common
{
    public class Repository<TObject> : IRepository<TObject>
        where TObject : EntityBase
    {
        //protected RaDbContext Context;
        protected ToDoDbContext Context = null;
        private bool _shareContext = false;

        //public Repository()
        //{
        //    Context = new ToDoDbContext();
        //}

        public Repository(ToDoDbContext context)
        {
            Context = context;
            //_shareContext = true;
        }

        protected DbSet<TObject> DbSet
        {
            get { return Context.Set<TObject>(); }
        }

        public void Dispose()
        {
            if (_shareContext && (Context != null))
                Context.Dispose();
        }

        public virtual IQueryable<TObject> All( bool includeDeleted = false)
        {
            return includeDeleted ? DbSet.AsQueryable() : DbSet.Where(u => !u.IsDeleted);
        }

        public virtual IQueryable<TObject>
            Filter(Expression<Func<TObject, bool>> predicate, bool includeDeleted = false)
        {
            IQueryable<TObject> result = null;
            result = includeDeleted ? DbSet.Where(predicate): DbSet.Where(AddGlobalFilters(predicate));
            return result;
        }

        public IQueryable<TKey> Filter<TKey>(Expression<Func<TKey, bool>> predicate, bool includeDeleted = false) where TKey : TObject
        {
            IQueryable<TKey> result = null;
            result = includeDeleted ? DbSet.OfType<TKey>().Where(predicate) : DbSet.OfType<TKey>().Where(AddGlobalFilters(predicate));
            return result;
        }

        public IQueryable<TKey> Filter<TKey>(Expression<Func<TKey, bool>> filter, out int total, bool includeDeleted = false, int index = 0, int size = 50) where TKey : TObject
        {
            int skipCount = index * size;
            IQueryable<TKey> resetSet;
            if (filter != null)
            {
                resetSet = includeDeleted
                    ? DbSet.OfType<TKey>().Where(filter).AsQueryable()
                    : DbSet.OfType<TKey>().Where(AddGlobalFilters(filter)).AsQueryable();
            }
            else
            {
                resetSet = DbSet.OfType<TKey>().AsQueryable();
            }
            resetSet = skipCount == 0
                            ? resetSet.Take(size)
                            : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        public IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter,
                                                  out int total, int index = 0, int size = 50, bool includeDeleted = false)
        {
            int skipCount = index*size;
            IQueryable<TObject> resetSet;
            if(filter != null)
            {
                resetSet = includeDeleted ? DbSet.Where(filter).AsQueryable() : DbSet.Where(AddGlobalFilters(filter)).AsQueryable();
            }
            else
            {
                resetSet =  DbSet.AsQueryable();
            }
            resetSet = skipCount == 0
                            ? resetSet.Take(size)
                            : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate, bool includeDeleted = false)
        {
            if(includeDeleted)
                return DbSet.Any(predicate);
            return DbSet.Any(AddGlobalFilters(predicate));
        }

        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate, bool includeDeleted = false)
        {
            if(includeDeleted)
                return DbSet.SingleOrDefault(predicate);
            return DbSet.SingleOrDefault(AddGlobalFilters(predicate));
        }

        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            if (!_shareContext)
                Context.SaveChanges();
            return newEntry;
        }

        public virtual int Count( bool includeDeleted = false)
        {
             return includeDeleted? DbSet.Count() : DbSet.Count(u=>!u.IsDeleted); 
        }

        public virtual int Delete(TObject TObject)
        {
            TObject.IsDeleted = true;
            if (!_shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public virtual int Update(TObject TObject)
        {
            var entry = Context.Entry(TObject);
            DbSet.Attach(TObject);
            entry.State = EntityState.Modified;
            if (!_shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate, bool includeDeleted = false)
        {
            var objects = includeDeleted ? Filter(predicate):Filter(AddGlobalFilters(predicate));
            foreach (var obj in objects)
                obj.IsDeleted = true;
            if (!_shareContext)
                return Context.SaveChanges();
            return 0;
        }

        private Expression<Func<TObject, bool>> AddGlobalFilters(Expression<Func<TObject, bool>> exp)
        {
            // get the global filter
            
            Expression<Func<TObject, bool>> newExp = c => !c.IsDeleted;
            return Combine(exp, newExp);
        }

        private Expression<Func<TKey, bool>> AddGlobalFilters<TKey>(Expression<Func<TKey, bool>> exp) where TKey : TObject
        {
            // get the global filter

            Expression<Func<TKey, bool>> newExp = c => !c.IsDeleted;
            return Combine(exp, newExp);
        }

        static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> filter1, Expression<Func<T, bool>> filter2)
        {
            // combine two predicates:
            // need to rewrite one of the lambdas, swapping in the parameter from the other
            var rewrittenBody1 = new ParameterUpdateVisitor(filter1.Parameters[0], filter2.Parameters[0]).Visit(filter1.Body);
            var newFilter = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(rewrittenBody1, filter2.Body), filter2.Parameters);
            return newFilter;
        }
    }

    class ParameterUpdateVisitor : ExpressionVisitor
    {
        private ParameterExpression _oldParameter;
        private ParameterExpression _newParameter;

        public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ReferenceEquals(node, _oldParameter))
                return _newParameter;

            return base.VisitParameter(node);
        }
    }
}
