using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using servicedesk.Common.Domain;

namespace servicedesk.Common.Repositories
{
    public interface IBaseRepository
    {
        Task<IEnumerable<T>> AllIncludingAsync<T>(params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new();
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class, IIdentifiable, new();
        int Count<T>() where T : class, IIdentifiable, new();
        Task<T> GetSingleAsync<T>(Guid id) where T : class, IIdentifiable, new();
        Task<T> GetSingleAsync<T>(Guid id, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new();
        Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IIdentifiable, new();
        Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new();
        Task<IEnumerable<T>> FindByAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IIdentifiable, new();
        Task<IEnumerable<T>> FindByAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, new();
        void Add<T>(T entity) where T : class, IIdentifiable, new();
        void Update<T>(T entity) where T : class, IIdentifiable, new();
        void Delete<T>(T entity) where T : class, IIdentifiable, new();
        void DeleteWhere<T>(Expression<Func<T, bool>> predicate) where T : class, IIdentifiable, new();
        Task CommitAsync();
    }
}