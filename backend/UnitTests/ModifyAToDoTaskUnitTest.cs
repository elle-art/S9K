using System;
using System.Threading.Tasks;
using Xunit;
using Backend.Models;      // adjust namespace as needed
using Backend.Services;

/// <summary>
/// Unit test for modifying an existing To Do task
/// </summary>
public class ModifyAToDoTaskUnitTest
{
[Fact]
    public async Task UpdateTaskAsync_ReplacesMatchingTask()
    {
        var uid = Guid.NewGuid().ToString();                 // unique key
        await UserInfoServices.CreateUserInfo(
                uid, new Availability(), new(), new(), new());

        var original = new UserTask("Write intro", DateTime.Today, false);
        await TaskService.AddTaskToUser(uid, original.TaskName,
                                        original.TaskDate, original.TaskStatus);

        var updated = new UserTask("Write intro", DateTime.Today, true);
        await TaskService.UpdateTaskAsync(uid, original, updated);

        var saved = await UserInfoServices.GetUserInfo(uid);

        Assert.Single(saved!.TaskList);                      // now 1 item
        Assert.True(saved.TaskList[0].TaskStatus);               // status flipped
    }
}
