using System.ComponentModel.DataAnnotations;

namespace WorkForce360.API.DTOs;

public class EmployeePatchDto
{
    [StringLength(20, MinimumLength = 2)]
    public string? EmployeeCode { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? FirstName { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? LastName { get; set; }

    [EmailAddress]
    [StringLength(254)]
    public string? Email { get; set; }

    [RegularExpression(@"^\+?[1-9]\d{7,14}$", ErrorMessage = "Phone must contain 8 to 15 digits and may start with +.")]
    public string? Phone { get; set; }

    [Range(typeof(DateTime), "1900-01-01", "2100-12-31")]
    public DateTime? JoiningDate { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? Department { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? Designation { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string? EmploymentType { get; set; }

    [RegularExpression("^(?i:Active|Inactive|On Leave|Resigned|Terminated)$", ErrorMessage = "Status must be Active, Inactive, On Leave, Resigned, or Terminated.")]
    public string? Status { get; set; }

    [Range(typeof(decimal), "0", "1000000000")]
    public decimal? Salary { get; set; }

    [StringLength(250, MinimumLength = 1)]
    public string? Address { get; set; }
}
