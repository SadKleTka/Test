namespace SibersDataManager.Models.Projects.Dto;

public record ProjectToResponse
(
    Guid Id,
    string Name,
    string CustomerCompany,
    string WorkerCompany,
    DateTime StartDate,
    DateTime? EndDate,
    uint Priority,
    Guid? ManagerId
);