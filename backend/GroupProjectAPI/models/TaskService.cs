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