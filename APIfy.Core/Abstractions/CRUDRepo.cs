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

        public TModel Update(TKey id, TModel model, bool skipNullOrWhitespace = true)
        {
            TModel dbEntry = Get(id);

            if (dbEntry != null)
            {
                Type type = model.GetType();
                PropertyInfo[] properties = type.GetProperties();

                for (int i = 0, length = properties.Length; i < length; i++)
                {
                    PropertyInfo property = properties[i];
                    if (!property.Name.Equals("id", StringComparison.OrdinalIgnoreCase) && !property.Name.Equals(string.Format("{0}id", type.Name), StringComparison.OrdinalIgnoreCase))
                    {
                        bool isNullOrEmpty = false;

                        if (property.PropertyType == typeof(string))
                            isNullOrEmpty = string.IsNullOrWhiteSpace((string)property.GetValue(model)) && skipNullOrWhitespace;

                        if (property.GetValue(dbEntry) != property.GetValue(model) && property.GetValue(model) != null && !isNullOrEmpty)
                            property.SetValue(dbEntry, property.GetValue(model, null));

                    }
                }
            }

            return dbEntry;
        }

        public Task<TModel> UpdateAsync(TKey id, TModel model, bool skipNullOrWhitespace = true)
        {
            return Task.Run(() => {
                return Update(id, model, skipNullOrWhitespace);
            });
        }

        public TModel Update(TModel model, bool skipNullOrWhitespace = true)
        {
            TKey id = default(TKey);

            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();
            bool isValid = false;

            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals("id", StringComparison.OrdinalIgnoreCase) || property.Name.Equals(string.Format("{0}id", type.Name), StringComparison.OrdinalIgnoreCase))
                {
                    id = (TKey)property.GetValue(model);
                    isValid = true;
                }
            }

            if (!isValid)
                throw new Exception(string.Format("Could not determine primary key member. Please use the standard naming 'Id or {0}Id', or use the update method overload with explicit passing of the Id.", type.Name));

            return Update(id, model, skipNullOrWhitespace);
        }

        public Task<TModel> UpdateAsync(TModel model, bool skipNullOrWhitespace = true)
        {
            return Task.Run(() => {
                return Update(model, skipNullOrWhitespace);
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
