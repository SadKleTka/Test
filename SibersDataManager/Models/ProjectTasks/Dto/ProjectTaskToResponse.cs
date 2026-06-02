using SibersDataManager.Models.Roles;

namespace SibersDataManager.Models.Tasks.Dto;

public record ProjectTaskToResponse(
    Guid Id, 
    string Name, 
    Guid AuthorId,
    Guid? ExecutorId, 
    TaskStatusEnum Status, 
    string Comment,
    uint Priority, 
    Guid ProjectId);