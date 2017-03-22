using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using servicedesk.Common.Domain;

namespace servicedesk.Common.Repositories
{
    public class BaseRepository<DbContextType> : IBaseRepository where DbContextType : DbContext
    {
        private DbContextType _context;
        
        public BaseRepository(DbContextType context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>() where T : class, IIdentifiable, new()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual int Count<T>() where T : class, IIdentifiable, new()
        {
            return _context.Set<T>().Count();
        }
        public virtual async Task<IEnumerable<T>> AllIncludingAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new()
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public Task<T> GetSingleAsync<T>(Guid id) where T : class, IIdentifiable, new()
        {
            return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IIdentifiable, new()
        {
            return _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public Task<T> GetSingleAsync<T>(Guid id, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new()
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new()
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> FindByAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IIdentifiable, new()
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindByAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new()
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).ToListAsync();
        }
        
        public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new()
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate);
        }

        public virtual void Add<T>(T entity) where T : class, IIdentifiable, new()
        {
            var dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual void Update<T>(T entity) where T : class, IIdentifiable, new()
        {
            var dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete<T>(T entity) where T : class, IIdentifiable, new()
        {
            var dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere<T>(Expression<Func<T, bool>> predicate) where T : class, IIdentifiable, new()
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public virtual Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}