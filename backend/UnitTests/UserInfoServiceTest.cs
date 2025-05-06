public class UserInfoServiceTests 
{
    [Fact]
    public async Task CreateUserInfo_WritesToFakeDb_AndReturnsObject()
    {
        // Arrange
        var availability = new Availability();                  // dummy data
        var user = await UserInfoServices.CreateUserInfo(
                        "Bob", availability,
                        new(), new(), new());                   // other params empty

        // Act  – read it back via the real public API
        var fetched = await UserInfoServices.GetUserInfo("Bob");

        // Assert
        Assert.NotNull(fetched);
        Assert.Equal("Bob", fetched!.DisplayName);              // path correctness
    }
}