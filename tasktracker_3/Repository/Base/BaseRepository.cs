using Microsoft.EntityFrameworkCore;
using System.Collections;
using tasktracker_3.Data;
using tasktracker_3.Models;

namespace tasktracker_3.Repository.Base
{
    public class BaseRepository<TModel>
        where TModel : BaseModel
    {
        protected readonly DataContext _context;
        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        #region Common action

        public bool AddModel(TModel model)
        {
            _context.Add(model);
            return Save(_context);
        }
        public bool DeleteModel(TModel model)
        {
            _context.Remove(model);
            return Save(_context);
        }

        public bool Update(TModel model)
        {
            _context.Update(model);
            return Save(_context);
        }

        public bool Save(DbContext _context)
        {
            return _context.SaveChanges() > 0;
        }

        #endregion

        public IQueryable<TModel> PrepareQueryWithIncludes()
        {
            var strs = typeof(TModel).GetProperties()
                     .Where(p => (typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string)) || p.PropertyType.Namespace == typeof(TModel).Namespace)
                     .Select(p => p.Name)
                     .ToArray();

            var query = _context.Set<TModel>().AsQueryable();

            foreach (var s in strs)
            {
                query = query.Include(s);
            }

            return query;
        }

    public TModel? GetById(long id, bool useIncludes)
    {
        if (useIncludes)
        {
            var query = PrepareQueryWithIncludes();
            return query.SingleOrDefault(a => a.Id == id);
        }
        else
        {
            var query = _context.Set<TModel>();
            return query.SingleOrDefault(a => a.Id == id);
        }
    }

    public bool ModelExists(long id)
    {
        return _context.Set<TModel>().Any(e => e.Id == id);
    }

    public ICollection<TModel> GetAll(bool includes)
    {
        if (includes)
        {
            var query = PrepareQueryWithIncludes();
            return query.OrderBy(t => t.Id).ToList();
        }
        else
        {
            var query = _context.Set<TModel>();
            return query.OrderBy(t => t.Id).ToList();
        }
    }
}
}
