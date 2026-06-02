using System.ComponentModel.DataAnnotations;

namespace SibersDataManager.Models.Employees.Dto;

public record EmployeeToCreate(
    [Required(ErrorMessage = "FirstName is required")]
    string FirstName,
    [Required(ErrorMessage = "LastName is required")]
    string SecondName, 
    [Required(ErrorMessage = "ThirdName is required")]
    string ThirdName,
    [EmailAddress(ErrorMessage = "Email Address is required")]
    string Email
    );