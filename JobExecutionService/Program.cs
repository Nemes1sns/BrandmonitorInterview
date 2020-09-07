using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace JobExecutionService
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        
        private static void Main(string[] args)
        {
            ConfigureServices();

            var dbOptions = new DbOptions {ConnectionString = _configuration.GetConnectionString("MySqlConnection")};
            IJobDataService jobDataService = new MySqlJobDataService(new OptionsWrapper<DbOptions>(dbOptions));
            
            var cache = new BlockingCollection<Job>();

            Task.WaitAll(
                new Producer(cache, jobDataService).ProduceMessagesAsync(), 
                new Consumer(cache, jobDataService).ConsumeMessagesAsync(), 
                new Consumer(cache, jobDataService).ConsumeMessagesAsync(), 
                new Consumer(cache, jobDataService).ConsumeMessagesAsync());
        }

        private static void ConfigureServices()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
