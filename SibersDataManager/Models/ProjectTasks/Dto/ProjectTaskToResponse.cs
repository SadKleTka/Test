namespace SibersDataManager.Models.Tasks.Dto;

public record ProjectTaskToResponse(Guid Id, string Name, Guid AuthorId, Guid? ExecutorId, TaskStatus Status, string Comment, uint Priority, Guid ProjectId);