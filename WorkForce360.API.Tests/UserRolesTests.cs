using WorkForce360.API.Models;

namespace WorkForce360.API.Tests;

public class UserRolesTests
{
    [Theory]
    [InlineData("Admin", "Admin")]
    [InlineData("hr", "HR")]
    [InlineData("MANAGER", "Manager")]
    [InlineData("employee", "Employee")]
    [InlineData("Recruiter", "Recruiter")]
    public void IsValidAndNormalize_AcceptsSupportedRolesIgnoringCase(string input, string expected)
    {
        Assert.True(UserRoles.IsValid(input));
        Assert.Equal(expected, UserRoles.Normalize(input));
    }

    [Fact]
    public void IsValid_RejectsUnsupportedRole()
    {
        Assert.False(UserRoles.IsValid("SuperAdmin"));
    }
}
