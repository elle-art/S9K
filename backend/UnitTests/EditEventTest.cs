using Xunit;
using Backend.Models;
using System;
using System.Collections.Generic;

public class EditEventTest
{
    [Fact]
    public void EditEvent_UpdatesEventProperties()
    {
        // Arrange
        var eventService = new EventService();
        var testEvent = new Event(
            "Original Event",
            DateTime.Parse("2024-01-01"),
            new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("10:00")),
            "Original Type",
            new List<UserInfo>()
        );

        // Act
        eventService.EditEvent(
            ref testEvent,
            "Updated Event",
            DateTime.Parse("2024-01-02"),
            new TimeBlock(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")),
            "Updated Type"
        );

        // Assert
        Assert.Equal("Updated Event", testEvent.EventName);
        Assert.Equal(DateTime.Parse("2024-01-02"), testEvent.EventDate);
        Assert.Equal(TimeOnly.Parse("10:00"), testEvent.EventTimeBlock.StartTime);
        Assert.Equal(TimeOnly.Parse("11:00"), testEvent.EventTimeBlock.EndTime);
        Assert.Equal("Updated Type", testEvent.EventType);
    }
}
