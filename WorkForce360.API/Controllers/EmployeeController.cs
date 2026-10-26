using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkForce360.API.Data;
using WorkForce360.API.DTOs;
using WorkForce360.API.Models;

namespace WorkForce360.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController(WorkForce360DbContext context) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.HR}")]
    public async Task<ActionResult<EmployeeResponseDto>> CreateEmployee(EmployeeRequestDto request)
    {
        Normalize(request);
        var emptyFieldProblem = GetEmptyFieldProblem(
            (nameof(request.EmployeeCode), request.EmployeeCode),
            (nameof(request.FirstName), request.FirstName),
            (nameof(request.LastName), request.LastName),
            (nameof(request.Email), request.Email),
            (nameof(request.Phone), request.Phone),
            (nameof(request.Department), request.Department),
            (nameof(request.Designation), request.Designation),
            (nameof(request.EmploymentType), request.EmploymentType),
            (nameof(request.Status), request.Status),
            (nameof(request.Address), request.Address));
        if (emptyFieldProblem is not null)
        {
            return BadRequest(emptyFieldProblem);
        }

        var duplicateProblem = await GetDuplicateProblemAsync(request.EmployeeCode, request.Email);
        if (duplicateProblem is not null)
        {
            return StatusCode(StatusCodes.Status409Conflict, duplicateProblem);
        }

        var employee = new Employee();
        ApplyRequest(employee, request);

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, ToResponse(employee));
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<EmployeeResponseDto>>> GetEmployees(
        [FromQuery] string? department,
        [FromQuery] string? search,
        [FromQuery, Range(1, 100000)] int pageNumber = 1,
        [FromQuery, Range(1, 100)] int pageSize = 10)
    {
        var query = context.Employees.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(department))
        {
            var normalizedDepartment = department.Trim().ToUpper();
            query = query.Where(employee => employee.Department.ToUpper() == normalizedDepartment);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = $"%{search.Trim().ToUpper()}%";
            query = query.Where(employee =>
                EF.Functions.Like(employee.EmployeeCode.ToUpper(), searchTerm) ||
                EF.Functions.Like(employee.FirstName.ToUpper(), searchTerm) ||
                EF.Functions.Like(employee.LastName.ToUpper(), searchTerm) ||
                EF.Functions.Like(employee.Email.ToUpper(), searchTerm) ||
                EF.Functions.Like(employee.Department.ToUpper(), searchTerm) ||
                EF.Functions.Like(employee.Designation.ToUpper(), searchTerm));
        }

        var totalCount = await query.CountAsync();
        var employees = await query
            .OrderBy(employee => employee.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(employee => ToResponse(employee))
            .ToListAsync();

        return Ok(new PagedResponseDto<EmployeeResponseDto>
        {
            Items = employees,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeResponseDto>> GetEmployeeById(int id)
    {
        var employee = await context.Employees.AsNoTracking().FirstOrDefaultAsync(employee => employee.Id == id);
        return employee is null ? NotFound() : Ok(ToResponse(employee));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.HR}")]
    public async Task<ActionResult<EmployeeResponseDto>> UpdateEmployee(int id, EmployeeRequestDto request)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee is null)
        {
            return NotFound();
        }

        Normalize(request);
        var emptyFieldProblem = GetEmptyFieldProblem(
            (nameof(request.EmployeeCode), request.EmployeeCode),
            (nameof(request.FirstName), request.FirstName),
            (nameof(request.LastName), request.LastName),
            (nameof(request.Email), request.Email),
            (nameof(request.Phone), request.Phone),
            (nameof(request.Department), request.Department),
            (nameof(request.Designation), request.Designation),
            (nameof(request.EmploymentType), request.EmploymentType),
            (nameof(request.Status), request.Status),
            (nameof(request.Address), request.Address));
        if (emptyFieldProblem is not null)
        {
            return BadRequest(emptyFieldProblem);
        }

        var duplicateProblem = await GetDuplicateProblemAsync(request.EmployeeCode, request.Email, id);
        if (duplicateProblem is not null)
        {
            return StatusCode(StatusCodes.Status409Conflict, duplicateProblem);
        }

        ApplyRequest(employee, request);
        await context.SaveChangesAsync();
        return Ok(ToResponse(employee));
    }

    [HttpPatch("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.HR}")]
    public async Task<ActionResult<EmployeeResponseDto>> PatchEmployee(int id, EmployeePatchDto request)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee is null)
        {
            return NotFound();
        }

        Normalize(request);
        var emptyFieldProblem = GetEmptyFieldProblem(
            (nameof(request.EmployeeCode), request.EmployeeCode),
            (nameof(request.FirstName), request.FirstName),
            (nameof(request.LastName), request.LastName),
            (nameof(request.Email), request.Email),
            (nameof(request.Phone), request.Phone),
            (nameof(request.Department), request.Department),
            (nameof(request.Designation), request.Designation),
            (nameof(request.EmploymentType), request.EmploymentType),
            (nameof(request.Status), request.Status),
            (nameof(request.Address), request.Address));
        if (emptyFieldProblem is not null)
        {
            return BadRequest(emptyFieldProblem);
        }

        var duplicateProblem = await GetDuplicateProblemAsync(
            request.EmployeeCode ?? employee.EmployeeCode,
            request.Email ?? employee.Email,
            id);
        if (duplicateProblem is not null)
        {
            return StatusCode(StatusCodes.Status409Conflict, duplicateProblem);
        }

        if (request.EmployeeCode is not null) employee.EmployeeCode = request.EmployeeCode;
        if (request.FirstName is not null) employee.FirstName = request.FirstName;
        if (request.LastName is not null) employee.LastName = request.LastName;
        if (request.Email is not null) employee.Email = request.Email;
        if (request.Phone is not null) employee.Phone = request.Phone;
        if (request.JoiningDate.HasValue) employee.JoiningDate = request.JoiningDate.Value;
        if (request.Department is not null) employee.Department = request.Department;
        if (request.Designation is not null) employee.Designation = request.Designation;
        if (request.EmploymentType is not null) employee.EmploymentType = request.EmploymentType;
        if (request.Status is not null) employee.Status = request.Status;
        if (request.Salary.HasValue) employee.Salary = request.Salary.Value;
        if (request.Address is not null) employee.Address = request.Address;

        await context.SaveChangesAsync();
        return Ok(ToResponse(employee));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.HR}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee is null)
        {
            return NotFound();
        }

        context.Employees.Remove(employee);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private static void ApplyRequest(Employee employee, EmployeeRequestDto request)
    {
        employee.EmployeeCode = request.EmployeeCode;
        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.Phone = request.Phone;
        employee.JoiningDate = request.JoiningDate;
        employee.Department = request.Department;
        employee.Designation = request.Designation;
        employee.EmploymentType = request.EmploymentType;
        employee.Status = request.Status;
        employee.Salary = request.Salary;
        employee.Address = request.Address;
    }

    private async Task<ValidationProblemDetails?> GetDuplicateProblemAsync(string employeeCode, string email, int? employeeId = null)
    {
        var normalizedEmployeeCode = employeeCode.Trim().ToUpper();
        var normalizedEmail = email.Trim().ToUpper();
        var query = context.Employees.AsNoTracking();

        if (employeeId.HasValue)
        {
            query = query.Where(employee => employee.Id != employeeId.Value);
        }

        var errors = new Dictionary<string, string[]>();
        if (await query.AnyAsync(employee => employee.EmployeeCode.ToUpper() == normalizedEmployeeCode))
        {
            errors[nameof(EmployeeRequestDto.EmployeeCode)] = ["Employee code already exists."];
        }

        if (await query.AnyAsync(employee => employee.Email.ToUpper() == normalizedEmail))
        {
            errors[nameof(EmployeeRequestDto.Email)] = ["Email already exists."];
        }

        return errors.Count == 0
            ? null
            : new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status409Conflict,
                Title = "An employee with this code or email already exists."
            };
    }

    private static void Normalize(EmployeeRequestDto request)
    {
        request.EmployeeCode = request.EmployeeCode.Trim().ToUpperInvariant();
        request.FirstName = request.FirstName.Trim();
        request.LastName = request.LastName.Trim();
        request.Email = request.Email.Trim().ToLowerInvariant();
        request.Phone = request.Phone.Trim();
        request.Department = request.Department.Trim();
        request.Designation = request.Designation.Trim();
        request.EmploymentType = request.EmploymentType.Trim();
        request.Status = request.Status.Trim();
        request.Address = request.Address.Trim();
    }

    private static void Normalize(EmployeePatchDto request)
    {
        if (request.EmployeeCode is not null) request.EmployeeCode = request.EmployeeCode.Trim().ToUpperInvariant();
        if (request.FirstName is not null) request.FirstName = request.FirstName.Trim();
        if (request.LastName is not null) request.LastName = request.LastName.Trim();
        if (request.Email is not null) request.Email = request.Email.Trim().ToLowerInvariant();
        if (request.Phone is not null) request.Phone = request.Phone.Trim();
        if (request.Department is not null) request.Department = request.Department.Trim();
        if (request.Designation is not null) request.Designation = request.Designation.Trim();
        if (request.EmploymentType is not null) request.EmploymentType = request.EmploymentType.Trim();
        if (request.Status is not null) request.Status = request.Status.Trim();
        if (request.Address is not null) request.Address = request.Address.Trim();
    }

    private static ValidationProblemDetails? GetEmptyFieldProblem(params (string FieldName, string? Value)[] fields)
    {
        var errors = fields
            .Where(field => field.Value is not null && string.IsNullOrWhiteSpace(field.Value))
            .ToDictionary(
                field => field.FieldName,
                _ => new[] { "A value is required when this field is supplied." });

        return errors.Count == 0
            ? null
            : new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more fields are blank."
            };
    }

    private static EmployeeResponseDto ToResponse(Employee employee) => new()
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
        Salary = employee.Salary,
        Address = employee.Address
    };
}
