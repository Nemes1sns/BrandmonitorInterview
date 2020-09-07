using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace JobExecutionService
{
    internal class Producer
    {
        private readonly BlockingCollection<Job> _cache;
        private readonly IJobDataService _jobDataService;

        public Producer(BlockingCollection<Job> cache, IJobDataService jobDataService)
        {
            _cache = cache;
            _jobDataService = jobDataService;
        }

        public async Task ProduceMessagesAsync()
        {
            try
            {
                await CheckNewJobsAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private async Task CheckNewJobsAsync()
        {
            while (true)
            {
                foreach (var job in (await _jobDataService.GetNewJobsAsync()).OrderBy(job => job.Modified))
                {
                    _cache.Add(job);
                }

                // may be configured
                await Task.Delay(5000);
            }
        }
    }
}