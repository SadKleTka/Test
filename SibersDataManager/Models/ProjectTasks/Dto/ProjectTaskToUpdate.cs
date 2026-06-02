using System.ComponentModel.DataAnnotations;
using SibersDataManager.Models.Roles;

namespace SibersDataManager.Models.Tasks.Dto;

public record ProjectTaskToUpdate(
    [Required(ErrorMessage = "Task name is required")]
    string Name, 
    Guid? ExecutorId,
    [Required(ErrorMessage = "Task status is required")]
    TaskStatusEnum Status, 
    [Required(ErrorMessage = "Task comment is required")]
    string Comment,
    [Required(ErrorMessage = "Task Priority is required")]
    uint Priority
    );