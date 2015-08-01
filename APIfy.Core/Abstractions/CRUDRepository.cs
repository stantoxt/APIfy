using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace APIfy.Core.Abstractions
{
    public class CRUDRepository<TModel, TKey> where TModel : class 
    {
        public TModel Add(TModel model)
        {
            return _table.Add(model);
        }

        public Task<TModel> AddAsync(TModel model)
        {
            return Task.Run(() => {
                return Add(model);
            });
        }

        public IEnumerable<TModel> Add(IEnumerable<TModel> models)
        {
            return _table.AddRange(models);
        }

        public bool Delete(TKey id)
        {
            var model = Get(id);

            bool found = false;

            if (model != null)
            {
                _table.Remove(model);
                found = true;
            }

            return found;
        }

        public Task<bool> DeleteAsync(TKey id)
        {
            return Task.Run(() => {
                return Delete(id);
            });
        }

        public void DetachAll()
        {

            foreach (DbEntityEntry dbEntityEntry in _db.ChangeTracker.Entries())
            {

                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }

        public IQueryable<TModel> Find(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] includeProperties)
        {
            IQueryable<TModel> items = _table;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public IQueryable<TModel> Get()
        {
            return _table;
        }

        public Task<IQueryable<TModel>> GetAsync()
        {
            return Task.Run(() => {
                return Get();
            });
        }

        public TModel Get(TKey id)
        {
            return _table.Find(id);
        }

        public Task<TModel> GetAsync(TKey id)
        {
            return _table.FindAsync(id);
        }

        public TModel Update(TModel model)
        {
            if (model == null)
                throw new Exception(string.Format("The model cannot be null."));

            _db.Entry(model).State = EntityState.Modified;
            return model;
        }

        public Task<TModel> UpdateAsync(TModel model)
        {
            return Task.Run(() => {
                return Update(model);
            });
        }

        public CRUDRepository(DbContext db)
        {
            _db = db;
            _table = _db.Set<TModel>();
        }

        private DbContext _db;
        private DbSet<TModel> _table;
    }
}
