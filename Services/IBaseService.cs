using servicedesk.Common.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;

namespace servicedesk.Common.Services
{
    public interface IBaseService
    {
        Task<IEnumerable<T>> GetAsync<T>() where T: class, IIdentifiable, new();
        Task<IEnumerable<T>> GetAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T: class, IIdentifiable, new();
        Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IIdentifiable, new();
        Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new();
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new();
        Task<T> GetByIdAsync<T>(Guid id) where T : class, IIdentifiable, new();
        Task<T> GetByIdAsync<T>(Guid id, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new();
        Task CreateAsync<T>(T @create) where T : class, IIdentifiable, new();
        Task UpdateAsync<T>(T @create) where T : class, IIdentifiable, new();
        Task DeleteAsync<T>(T @create) where T : class, IIdentifiable, new();
    }
}