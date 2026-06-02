namespace SibersDataManager.Models.Tasks.Dto;

public record ProjectTaskToUpdate(string Name, Guid? ExecutorId, TaskStatus Status, string Comment, uint Priority);