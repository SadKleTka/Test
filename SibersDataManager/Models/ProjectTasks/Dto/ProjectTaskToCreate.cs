using System.ComponentModel.DataAnnotations;
using SibersDataManager.Models.Roles;

namespace SibersDataManager.Models.Tasks.Dto;

public record ProjectTaskToCreate(
    [Required(ErrorMessage = "Task name is required")]
    string Name,
    [Required(ErrorMessage = "Task AuthorId is required")]
    Guid AuthorId,
    Guid? ExecutorId, 
    [Required(ErrorMessage = "Task status is required")]
    TaskStatusEnum Status, 
    [Required(ErrorMessage = "Task comment is required")]
    string Comment, 
    [Required(ErrorMessage = "Task Priority is required")]
    uint Priority, 
    [Required(ErrorMessage = "Task ProjectId is required")]
    Guid ProjectId);