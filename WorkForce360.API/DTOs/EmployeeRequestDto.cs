namespace WorkForce360.API.DTOs
{
    public class EmployeeRequestDto
    {
        public string EmployeeCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime JoiningDate { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }

        public string EmploymentType { get; set; }

        public string Status { get; set; }

        public decimal Salary { get; set; }
    }
}