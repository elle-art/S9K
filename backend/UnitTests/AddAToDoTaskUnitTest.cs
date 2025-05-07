using Xunit;
using Backend.Services;
using Backend.Models;

/// <summary>
/// test case for adding a to-do task
/// </summary>
public class AddAToDoTaskUnitTest
{
    [Fact]
    public async Task AddTaskToUser_PersistsInUserInfo()
    {
        var uid = Guid.NewGuid().ToString();                 // unique key
        await UserInfoServices.CreateUserInfo(
                uid, new Availability(), new(), new(), new());

        await TaskService.AddTaskToUser(uid, "Finish report",
                                        DateTime.Today, false);

        var saved = await UserInfoServices.GetUserInfo(uid);
        Assert.Single(saved!.TaskList);
        Assert.Equal("Finish report", saved.TaskList[0].TaskName);
    }
}
