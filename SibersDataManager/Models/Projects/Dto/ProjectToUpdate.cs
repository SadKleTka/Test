using System.ComponentModel.DataAnnotations;

namespace SibersDataManager.Models.Projects.Dto;

public record ProjectToUpdate(
    [Required(ErrorMessage = "Project name is required")]
    string Name,
    [Required(ErrorMessage = "Customer Company Name is required")]
    string CustomerCompany,
    [Required(ErrorMessage = "Worker Company Name is required")]
    string WorkerCompany,
    [Range(1,10,ErrorMessage = "Priority must be between 1 and 10")]
    uint Priority,
    DateTime? EndDate,
    Guid? ManagerId
    );