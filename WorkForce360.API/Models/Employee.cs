namespace WorkForce360.API.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public string EmploymentType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public string Address { get; set; } = string.Empty;
    }
}
