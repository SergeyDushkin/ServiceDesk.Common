using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using servicedesk.Common.Domain;
using servicedesk.Common.Repositories;

namespace servicedesk.Common.Services
{
    public class BaseDependentlyService : BaseService, IBaseDependentlyService
    {
        public BaseDependentlyService(IBaseRepository repository) : base(repository)
        {
        }

        public Task<IEnumerable<T>> GetByReferenceIdAsync<T>(Guid id) where T : class, IIdentifiable, IDependently, new() => repository.FindByAsync<T>(r => r.ReferenceId == id);
        public Task<IEnumerable<T>> GetByReferenceIdAsync<T>(Guid id, params Expression<Func<T, object>>[] includeProperties) where T : class, IIdentifiable, IDependently, new() => repository.FindByAsync<T>(r => r.ReferenceId == id, includeProperties);
    }
}