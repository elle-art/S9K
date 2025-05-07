namespace Backend.Models;
using Backend.Services;
public class TaskService
{


    /// <summary>
    /// Creates a task and adds it to the user's profile
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="name"></param>
    /// <param name="dueDate"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static async Task<UserTask> AddTaskToUser(
            string uid,
            string name,
            DateTime? dueDate,
            bool status)
    {
        var task = new UserTask(name, dueDate, status);

        await UserInfoServices.AddToDoTaskAsync(uid, task);

        return task;
    }


    //TO-DO: Task time Generation logic
    public TimeBlock GenerateTaskTime(Availability userAvailability, Calendar userCalendar)
    {
        return new TimeBlock(TimeOnly.MinValue, TimeOnly.MinValue);
    }

    /// <summary>
    /// Modifies a user’s task by matching on name + date + status.
    /// Returns the updated task if the operation succeeded; otherwise <c>null</c>.
    /// </summary>
    public static async Task<UserTask?> UpdateTaskAsync(
            string uid,
            UserTask original,
            UserTask updatedVersion)
    {
        bool replaced = await UserInfoServices.ModifyAToDoTaskAsync(uid, original, updatedVersion);
        return replaced ? updatedVersion : null;
    }

}