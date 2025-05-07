using System.Collections.Generic;
using Xunit;
using Backend.Models;

public class ReportServiceTests
{
    [Fact]
    public void GetTaskAndEventStats_ReturnsCorrectCounts()
    {
        // Arrange
        var tasks = new List<UserTask>
            {
                new UserTask { TaskStatus = true },
                new UserTask { TaskStatus = false },
                new UserTask { TaskStatus = true }
            };

        var events = new List<Event>
            {
                new Event { EventType = "Meeting" },
                new Event { EventType = "Workshop" },
                new Event { EventType = "Meeting" }
            };

        var service = new ReportService();

        // Act
        var (completedTaskCount, eventCountsByType) = service.GetTaskAndEventStats(tasks, events);

        // Assert
        Assert.Equal(2, completedTaskCount);
        Assert.Equal(2, eventCountsByType["Meeting"]);
        Assert.Equal(1, eventCountsByType["Workshop"]);
    }
}
