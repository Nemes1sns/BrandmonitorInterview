using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace JobExecutionService
{
    public class Consumer
    {
        private readonly BlockingCollection<Job> _cache;
        private readonly IJobDataService _jobDataService;

        public Consumer(BlockingCollection<Job> cache, IJobDataService jobDataService)
        {
            _cache = cache;
            _jobDataService = jobDataService;
        }

        public async Task ConsumeMessagesAsync()
        {
            try
            {
                await ProcessJobAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private async Task ProcessJobAsync()
        {
            foreach (var job in _cache.GetConsumingEnumerable())
            {
                try
                {
                    job.Status = JobStatus.Running;
                    await _jobDataService.UpdateAsync(job);

                    await Task.Delay(1000 * 60 * 2);

                    job.Status = JobStatus.Finished;
                    await _jobDataService.UpdateAsync(job);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }
            }
        }
    }
}