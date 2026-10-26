using System.ComponentModel.DataAnnotations;
using WorkForce360.API.DTOs;

namespace WorkForce360.API.Tests;

public class RequestValidationTests
{
    [Fact]
    public void EmployeeRequest_IsValid_WhenAllRulesAreSatisfied()
    {
        Assert.Empty(Validate(CreateValidEmployeeRequest()));
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("+012345678")]
    [InlineData("phone-number")]
    public void EmployeeRequest_RejectsInvalidPhone(string phone)
    {
        var request = CreateValidEmployeeRequest();
        request.Phone = phone;

        Assert.Contains(Validate(request), result => result.MemberNames.Contains(nameof(EmployeeRequestDto.Phone)));
    }

    [Fact]
    public void InitializeAdminRequest_RequiresAStrongPassword()
    {
        var request = new InitializeAdminRequestDto { Email = "admin@example.test", Password = "weakpassword" };

        Assert.Contains(Validate(request), result => result.MemberNames.Contains(nameof(InitializeAdminRequestDto.Password)));
    }

    private static EmployeeRequestDto CreateValidEmployeeRequest() => new()
    {
        EmployeeCode = "EMP1001", FirstName = "Test", LastName = "Employee", Email = "employee@example.test",
        Phone = "+919876543210", JoiningDate = new DateTime(2026, 1, 1), Department = "Engineering",
        Designation = "Developer", EmploymentType = "FullTime", Status = "Active", Salary = 50000, Address = "Test address"
    };

    private static IReadOnlyCollection<ValidationResult> Validate(object value)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(value, new ValidationContext(value), results, validateAllProperties: true);
        return results;
    }
}
