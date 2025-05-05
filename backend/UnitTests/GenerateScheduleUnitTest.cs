using Xunit;
using backend.models;
using System;
using System.Collections.Generic;

public class GenerateScheduleUnitTest
{
    [Fact]
    public void TestGenerateSchedule_ReturnsCorrectBusyTime()
    {
        // Arrange
        var calendar = new Calendar();

        // Add events to the calendar
        calendar.events.Add(new Event
        {
            EventDate = DateTime.Today, // Today
            EventTimeBlock = new TimeBlock(new TimeOnly(9, 0), new TimeOnly(10, 0))
        });
        calendar.events.Add(new Event
        {
            EventDate = DateTime.Today.AddDays(1), // Tomorrow
            EventTimeBlock = new TimeBlock(new TimeOnly(14, 0), new TimeOnly(15, 0))
        });
        calendar.events.Add(new Event
        {
            EventDate = DateTime.Today.AddDays(2), // Day after tomorrow
            EventTimeBlock = new TimeBlock(new TimeOnly(16, 0), new TimeOnly(17, 0))
        });

        // Act
        var result = CalendarService.GenerateSchedule(calendar);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(7, result.Count); // Ensure the result contains 7 days

        // Verify today's busy time
        Assert.Single(result[0]); // Today
        Assert.Equal(new TimeOnly(9, 0), result[0][0].StartTime);
        Assert.Equal(new TimeOnly(10, 0), result[0][0].EndTime);

        // Verify tomorrow's busy time
        Assert.Single(result[1]); // Tomorrow
        Assert.Equal(new TimeOnly(14, 0), result[1][0].StartTime);
        Assert.Equal(new TimeOnly(15, 0), result[1][0].EndTime);

        // Verify day after tomorrow's busy time
        Assert.Single(result[2]); // Day after tomorrow
        Assert.Equal(new TimeOnly(16, 0), result[2][0].StartTime);
        Assert.Equal(new TimeOnly(17, 0), result[2][0].EndTime);

        // Verify no busy time for other days
        for (int i = 3; i < 7; i++)
        {
            Assert.Empty(result[i]);
        }
    }

    [Fact]
    public void TestGenerateSchedule_NoEvents_ReturnsEmptyBusyTime()
    {
        // Arrange
        var calendar = new Calendar();

        // Act
        var result = CalendarService.GenerateSchedule(calendar);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(7, result.Count); // Ensure the result contains 7 days

        // Verify all days are empty
        for (int i = 0; i < 7; i++)
        {
            Assert.Empty(result[i]);
        }
    }
}
