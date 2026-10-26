namespace WorkForce360.API.Tests;

public class PasswordHashingServiceTests
{
    [Fact]
    public void Hash_CreatesDifferentHashesForTheSamePassword()
    {
        var service = new WorkForce360.API.Services.PasswordHashingService();
        var firstHash = service.Hash("Password12345");
        var secondHash = service.Hash("Password12345");

        Assert.NotEqual(firstHash, secondHash);
        Assert.True(service.Verify("Password12345", firstHash));
        Assert.True(service.Verify("Password12345", secondHash));
    }
}
