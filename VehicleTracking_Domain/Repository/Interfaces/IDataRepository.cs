using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking_Domain.Entities;

namespace VehicleTracking_Domain.Repository.Interfaces
{
    public interface IDataRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T newEntity);
        Task<T> GetAsync(string entityId);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(string entityId);
        Task<IReadOnlyList<T>> GetAllAsync();
    }
}
