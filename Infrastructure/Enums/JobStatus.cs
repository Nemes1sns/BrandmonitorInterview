using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Enums
{
    public enum JobStatus
    {
        [Display(ResourceType = typeof(Resources), Name = "JobStatus_Created")]
        Created = 10,
        
        [Display(ResourceType = typeof(Resources), Name = "JobStatus_Running")]
        Running = 20,
        
        [Display(ResourceType = typeof(Resources), Name = "JobStatus_Finished")]
        Finished = 100
    }
}