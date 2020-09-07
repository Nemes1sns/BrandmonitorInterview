using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IJobDataService
    {
        Task<Job> GetByIdAsync(Guid id);
        Task<IEnumerable<Job>> GetNewJobsAsync();
        Task CreateAsync(Job job);
        Task UpdateAsync(Job job);
    }
}