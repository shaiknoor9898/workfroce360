using System.ComponentModel.DataAnnotations;

namespace WorkForce360.API.DTOs
{
    public class EmployeeRequestDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+?[1-9]\d{7,14}$", ErrorMessage = "Phone must contain 8 to 15 digits and may start with +.")]
        public string Phone { get; set; } = string.Empty;

        [Range(typeof(DateTime), "1900-01-01", "2100-12-31")]
        public DateTime JoiningDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EmploymentType { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(?i:Active|Inactive|On Leave|Resigned|Terminated)$", ErrorMessage = "Status must be Active, Inactive, On Leave, Resigned, or Terminated.")]
        public string Status { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "1000000000")]
        public decimal Salary { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;
    }
}
