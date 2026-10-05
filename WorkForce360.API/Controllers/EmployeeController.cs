using Microsoft.AspNetCore.Mvc;
using WorkForce360.API.Data;
using WorkForce360.API.Models;
using Microsoft.EntityFrameworkCore;
using WorkForce360.API.DTOs;

namespace WorkForce360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly WorkForce360DbContext _context;

        public EmployeeController(WorkForce360DbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeRequestDto employeeDto)
        {
           var employee = new Employee
           {
            
                EmployeeCode = employeeDto.EmployeeCode,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                JoiningDate = employeeDto.JoiningDate,
                Department = employeeDto.Department,
                Designation = employeeDto.Designation,
                EmploymentType = employeeDto.EmploymentType,
                Status = employeeDto.Status,
                Salary = employeeDto.Salary

           }; 

            _context.Employees.Add(employee);

             await _context.SaveChangesAsync();

             var response = new EmployeeResponseDto
    {
        Id = employee.Id,
        EmployeeCode = employee.EmployeeCode,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Email = employee.Email,
        Phone = employee.Phone,
        JoiningDate = employee.JoiningDate,
        Department = employee.Department,
        Designation = employee.Designation,
        EmploymentType = employee.EmploymentType,
        Status = employee.Status,
        Salary = employee.Salary
    };
            
            return Ok(response);
        }

    
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();

            return Ok(employees);


        }


    [HttpGet("{id}")]
public async Task<IActionResult> GetEmployeeById(int id)
{
    var employee = await _context.Employees.FindAsync(id);

    if (employee == null)
    {
        return NotFound();
    }

    return Ok(employee);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
{
    var existingEmployee = await _context.Employees.FindAsync(id);

    if (existingEmployee == null)
    {
        return NotFound();
    }

    existingEmployee.EmployeeCode = employee.EmployeeCode;
    existingEmployee.FirstName = employee.FirstName;
    existingEmployee.LastName = employee.LastName;
    existingEmployee.Email = employee.Email;
    existingEmployee.Phone = employee.Phone;
    existingEmployee.JoiningDate = employee.JoiningDate;
    existingEmployee.Department = employee.Department;
    existingEmployee.Designation = employee.Designation;
    existingEmployee.EmploymentType = employee.EmploymentType;
    existingEmployee.Status = employee.Status;
    existingEmployee.Salary = employee.Salary;

    await _context.SaveChangesAsync();

    return Ok(existingEmployee);
}

[HttpPatch("{id}")]
public async Task<IActionResult> PatchEmployee(int id, Employee employee)
{
    var existingEmployee = await _context.Employees.FindAsync(id);

    if (existingEmployee == null)
    {
        return NotFound();
    }

    if (employee.FirstName != null)
    {
        existingEmployee.FirstName = employee.FirstName;
    }

    if (employee.LastName != null)
    {
        existingEmployee.LastName = employee.LastName;
    }

    if (employee.Email != null)
    {
        existingEmployee.Email = employee.Email;
    }

    if (employee.Phone != null)
    {
        existingEmployee.Phone = employee.Phone;
    }

    if (employee.Department != null)
    {
        existingEmployee.Department = employee.Department;
    }

    if (employee.Designation != null)
    {
        existingEmployee.Designation = employee.Designation;
    }

    if (employee.EmploymentType != null)
    {
        existingEmployee.EmploymentType = employee.EmploymentType;
    }

    if (employee.Status != null)
    {
        existingEmployee.Status = employee.Status;
    }

    if (employee.Salary != 0)
    {
        existingEmployee.Salary = employee.Salary;
    }

    await _context.SaveChangesAsync();

    return Ok(existingEmployee);
}
[HttpDelete("{id}") ]
public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    
}

}




