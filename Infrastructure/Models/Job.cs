using System;
using Infrastructure.Enums;

namespace Infrastructure.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public JobStatus Status { get; set; }
        public DateTime Modified { get; set; }
    }
}
