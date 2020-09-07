using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Infrastructure
{
    public class MySqlJobDataService : IJobDataService
    {
        private readonly string _connectionString;

        public MySqlJobDataService(IOptions<DbOptions> dbOptions)
        {
            _connectionString = dbOptions.Value.ConnectionString;
        }

        public Task<Job> GetByIdAsync(Guid id)
        {
            using IDbConnection db = new MySqlConnection(_connectionString);
            db.Open();
            return db.QuerySingleOrDefaultAsync<Job>("SELECT id, status, modified FROM job WHERE id = @id;", new { id });
        }

        public Task<IEnumerable<Job>> GetNewJobsAsync()
        {
            using IDbConnection db = new MySqlConnection(_connectionString);
            db.Open();
            return db.QueryAsync<Job>("SELECT id, status, modified FROM job WHERE status = @status;", new { status = JobStatus.Created });
        }

        public Task CreateAsync(Job job)
        {
            using IDbConnection db = new MySqlConnection(_connectionString);
            db.Open();

            return db.ExecuteAsync("INSERT INTO job (id, status, modified) values (@id, @status, @modified);", new { id = job.Id, status = job.Status, modified = job.Modified});
        }

        public Task UpdateAsync(Job job)
        {
            using IDbConnection db = new MySqlConnection(_connectionString);
            db.Open();

            return db.ExecuteAsync("UPDATE job SET status = @status, modified = @modified WHERE id = @id;", new { id = job.Id, status = job.Status, modified = DateTime.UtcNow });
        }
    }
}