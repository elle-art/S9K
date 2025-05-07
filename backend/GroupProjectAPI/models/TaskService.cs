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
    public TimeBlock GenerateTaskTime(Availability userAvailability, Calendar userCalendar, int taskDurationMinutes)
    {
        int todayDayOfWeek = (int)DateTime.Today.DayOfWeek;
        var todayAvailability = userAvailability.weeklySchedule[todayDayOfWeek];
        var todayEvents = userCalendar.events
            .Where(e => e.EventDate.Date == DateTime.Today.Date)
            .Select(e => e.EventTimeBlock)
            .ToList();

        // Find free time blocks by removing event overlaps from availability
        var freeTimeBlocks = todayAvailability;
        foreach (var eventBlock in todayEvents)
        {
            freeTimeBlocks = TimeBlock.RemoveTimeBlock(freeTimeBlocks, eventBlock);
        }

        // Check for a time block that can accommodate the task duration
        foreach (var block in freeTimeBlocks)
        {
            if (block.getLength() >= taskDurationMinutes)
            {
                var resultBlock = new TimeBlock(block.StartTime, block.StartTime.AddMinutes(taskDurationMinutes));
                return resultBlock;
            }
        }

        // Log default return value (No time block can is available for the day)
        return new TimeBlock(TimeOnly.MinValue, TimeOnly.MinValue);
    }

    //TO-DO: add logic - potentially change to remove task?
    public static async void UpdateTask(string uid, string name, DateTime date, bool status)
    {

    }
}