using SibersDataManager.Models.Roles;

namespace SibersDataManager.Models.Tasks.Dto;

public record ProjectTaskToResponse(
    Guid Id, 
    string Name, 
    Guid AuthorId,
    Guid? WorkerId, 
    TaskStatusEnum Status, 
    string Comment,
    uint Priority, 
    Guid ProjectId);