namespace SibersDataManager.Models.Tasks.Dto;

public record ProjectTaskToCreate(string Name, Guid AuthorId, Guid? ExecutorId, TaskStatus Status, string Comment, uint Priority, Guid ProjectId);