using System;
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobDataService _jobDataService;

        public JobController(IJobDataService jobDataService)
        {
            _jobDataService = jobDataService;
        }

        [HttpPut]
        public async Task<ActionResult> Put()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Status = JobStatus.Created,
                Modified = DateTime.UtcNow
            };

            await _jobDataService.CreateAsync(job);

            HttpContext.Response.StatusCode = (int)HttpStatusCode.Accepted;
            return Content(job.Id.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] Guid id)
        {
            var job = await _jobDataService.GetByIdAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return new JsonResult(new
            {
                status = job.Status.DisplayName(),
                timestamp = job.Modified.ToString("O")
            });
        }
    }
}
