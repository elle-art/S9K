namespace Backend.Models;

public class ReportService
{
    public Report generateUserWeeklyReport(Calendar userCal, UserInfo userinfo)
    {
        if (userCal == null)
            throw new ArgumentNullException(nameof(userCal));

        // Count the number of completed tasks
        int completedTaskCount = userinfo.TaskList?.Count(task => task.TaskStatus) ?? 0;

        // Group events by their type and count them
        var eventsByType = userCal.events?
            .GroupBy(evt => evt.EventType ?? "Unspecified")
            .ToDictionary(group => group.Key, group => group.Count())
            ?? new Dictionary<string, int>();

        // Create the report object
        var report = new Report
        {
            ReportDate = DateTime.UtcNow,
            UserData = userinfo,
            CompletedTasks = userinfo.TaskList?.Where(task => task.TaskStatus).ToList() ?? new List<UserTask>(),
            EventData = userCal.events ?? new List<Event>(),
            EventTypeCounts = eventsByType
        };

        return report;
    }

    public (int completedTaskCount, Dictionary<string, int> eventCountsByType) GetTaskAndEventStats(List<UserTask> tasks, List<Event> events)
    {
        int completedTaskCount = tasks.Count(t => t.TaskStatus);

        var eventCountsByType = events
            .GroupBy(e => e.EventType)
            .ToDictionary(g => g.Key, g => g.Count());

        return (completedTaskCount, eventCountsByType);
    }
}
