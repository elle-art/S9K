namespace Backend.Models;
using Backend.Services;
public class TaskService
{

    /// <summary>
    /// Create a new task object and save it to Firebase
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="name"></param>
    /// <param name="date"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static async Task<UserTask> AddTaskToUser(string uid, string name, DateTime? date, bool status)
    {
        UserTask task = new UserTask(name, date, status);

        await DBCommunications.SaveObjectAsync(uid, task);

        return task;
    }

    //TO-DO: Task time Generation logic
    public TimeBlock GenerateTaskTime(Availability userAvailability, Calendar userCalendar)
    {
        return new TimeBlock(TimeOnly.MinValue, TimeOnly.MinValue);
    }

    //TO-DO: add logic - potentially change to remove task?
    public static async void UpdateTask(string uid, string name, DateTime date, bool status)
    {

    }
}