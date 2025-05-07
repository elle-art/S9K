using Xunit;
using backend.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GenerateFreeTimeUnitTest
{
    [Fact]
    public void TestEmpty()
    {
        var calendar = new Calendar();
        var availability = new Availability();

        var result = CalendarService.GenerateFreeTime(calendar, availability);

        for (int i = 0; i < 7; i++)
        {
            Assert.Empty(result[i]);
        }
    }

    [Fact]
    public void TestGenerateFreeTime_CopiesAvailabilityToFreeTime()
    {
        // Arrange
        var calendar = new Calendar();
        var availability = new Availability();

        // Populate availability with sample data for all days, including multiple blocks
        for (int i = 0; i < 7; i++)
        {
            availability.weeklySchedule[i] = new List<TimeBlock> {
                new TimeBlock(new TimeOnly(9, 0), new TimeOnly(11, 0)),
                new TimeBlock(new TimeOnly(14, 0), new TimeOnly(16, 0))
            };
        }

        // Act
        var result = CalendarService.GenerateFreeTime(calendar, availability);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(7, result.Count); // Ensure the result contains 7 days

        for (int i = 0; i < 7; i++)
        {
            Assert.Equal(2, result[i].Count);
            Assert.Equal(new TimeOnly(9, 0), result[i][0].StartTime);
            Assert.Equal(new TimeOnly(11, 0), result[i][0].EndTime);
            Assert.Equal(new TimeOnly(14, 0), result[i][1].StartTime);
            Assert.Equal(new TimeOnly(16, 0), result[i][1].EndTime);
        }
    }

    [Fact]
    public void TestGenerateFreeTime_ConsidersCalendarEventsAndAvailability()
    {
        // Arrange
        var calendar = new Calendar();
        var availability = new Availability();

        // Get today's date and calculate the next 6 days
        var today = DateTime.Today;

        // Ensure all days in the weekly schedule are initialized
        for (int i = 0; i < 7; i++)
        {
            availability.weeklySchedule[i] = new List<TimeBlock>();
        }

        // Populate availability for a specific day (e.g., today)
        availability.weeklySchedule[(int)today.DayOfWeek] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(9, 0), new TimeOnly(12, 0)),
            new TimeBlock(new TimeOnly(13, 0), new TimeOnly(17, 0))
        };

        // Add an event that overlaps with availability on today
        calendar.events.Add(new Event
        {
            EventDate = today,
            EventTimeBlock = new TimeBlock(new TimeOnly(10, 0), new TimeOnly(11, 0))
        });

        // Act
        var result = CalendarService.GenerateFreeTime(calendar, availability);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(7, result.Count); // Ensure the result contains 7 days

        // Verify today's free time
        var todayIndex = 0; // Always the first day in the result
        Assert.Equal(3, result[todayIndex].Count); // Today
        Assert.Equal(new TimeOnly(9, 0), result[todayIndex][0].StartTime);
        Assert.Equal(new TimeOnly(10, 0), result[todayIndex][0].EndTime);
        Assert.Equal(new TimeOnly(11, 0), result[todayIndex][1].StartTime);
        Assert.Equal(new TimeOnly(12, 0), result[todayIndex][1].EndTime);
        Assert.Equal(new TimeOnly(13, 0), result[todayIndex][2].StartTime);
        Assert.Equal(new TimeOnly(17, 0), result[todayIndex][2].EndTime);
    }
}