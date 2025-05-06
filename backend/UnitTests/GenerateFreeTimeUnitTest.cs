using Xunit;
using Backend.Models;
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

        for (int i = 2; i < 7; i++)
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
        // Saturday (6) - Today
        availability.weeklySchedule[6] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(9, 0), new TimeOnly(11, 0)),
            new TimeBlock(new TimeOnly(14, 0), new TimeOnly(16, 0))
        };
        // Sunday (0) - Tomorrow
        availability.weeklySchedule[0] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(10, 0), new TimeOnly(12, 0)),
            new TimeBlock(new TimeOnly(15, 0), new TimeOnly(17, 0))
        };
        // Monday (1)
        availability.weeklySchedule[1] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(8, 0), new TimeOnly(10, 0)),
            new TimeBlock(new TimeOnly(13, 0), new TimeOnly(15, 0)),
            new TimeBlock(new TimeOnly(16, 0), new TimeOnly(18, 0))
        };
        // Tuesday (2)
        availability.weeklySchedule[2] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(9, 0), new TimeOnly(11, 0)),
            new TimeBlock(new TimeOnly(14, 0), new TimeOnly(16, 0))
        };
        // Wednesday (3)
        availability.weeklySchedule[3] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(11, 0), new TimeOnly(13, 0)),
            new TimeBlock(new TimeOnly(15, 0), new TimeOnly(17, 0))
        };
        // Thursday (4)
        availability.weeklySchedule[4] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(10, 0), new TimeOnly(12, 0)),
            new TimeBlock(new TimeOnly(15, 0), new TimeOnly(17, 0))
        };
        // Friday (5)
        availability.weeklySchedule[5] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(9, 0), new TimeOnly(11, 0)),
            new TimeBlock(new TimeOnly(16, 0), new TimeOnly(18, 0))
        };

        // Act
        var result = CalendarService.GenerateFreeTime(calendar, availability);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(7, result.Count); // Ensure the result contains 7 days

        // Day 0 (Today - Saturday)
        Assert.Equal(2, result[0].Count);
        Assert.Equal(new TimeOnly(9, 0), result[0][0].StartTime);
        Assert.Equal(new TimeOnly(11, 0), result[0][0].EndTime);
        Assert.Equal(new TimeOnly(14, 0), result[0][1].StartTime);
        Assert.Equal(new TimeOnly(16, 0), result[0][1].EndTime);

        // Day 1 (Tomorrow - Sunday)
        Assert.Equal(2, result[1].Count);
        Assert.Equal(new TimeOnly(10, 0), result[1][0].StartTime);
        Assert.Equal(new TimeOnly(12, 0), result[1][0].EndTime);
        Assert.Equal(new TimeOnly(15, 0), result[1][1].StartTime);
        Assert.Equal(new TimeOnly(17, 0), result[1][1].EndTime);

        // Day 2 (Monday)
        Assert.Equal(3, result[2].Count);
        Assert.Equal(new TimeOnly(8, 0), result[2][0].StartTime);
        Assert.Equal(new TimeOnly(10, 0), result[2][0].EndTime);
        Assert.Equal(new TimeOnly(13, 0), result[2][1].StartTime);
        Assert.Equal(new TimeOnly(15, 0), result[2][1].EndTime);
        Assert.Equal(new TimeOnly(16, 0), result[2][2].StartTime);
        Assert.Equal(new TimeOnly(18, 0), result[2][2].EndTime);

        // Day 3 (Tuesday)
        Assert.Equal(2, result[3].Count);
        Assert.Equal(new TimeOnly(9, 0), result[3][0].StartTime);
        Assert.Equal(new TimeOnly(11, 0), result[3][0].EndTime);
        Assert.Equal(new TimeOnly(14, 0), result[3][1].StartTime);
        Assert.Equal(new TimeOnly(16, 0), result[3][1].EndTime);

        // Day 4 (Wednesday)
        Assert.Equal(2, result[4].Count);
        Assert.Equal(new TimeOnly(11, 0), result[4][0].StartTime);
        Assert.Equal(new TimeOnly(13, 0), result[4][0].EndTime);
        Assert.Equal(new TimeOnly(15, 0), result[4][1].StartTime);
        Assert.Equal(new TimeOnly(17, 0), result[4][1].EndTime);

        // Day 5 (Thursday)
        Assert.Equal(2, result[5].Count);
        Assert.Equal(new TimeOnly(10, 0), result[5][0].StartTime);
        Assert.Equal(new TimeOnly(12, 0), result[5][0].EndTime);
        Assert.Equal(new TimeOnly(15, 0), result[5][1].StartTime);
        Assert.Equal(new TimeOnly(17, 0), result[5][1].EndTime);

        // Day 6 (Friday)
        Assert.Equal(2, result[6].Count);
        Assert.Equal(new TimeOnly(9, 0), result[6][0].StartTime);
        Assert.Equal(new TimeOnly(11, 0), result[6][0].EndTime);
        Assert.Equal(new TimeOnly(16, 0), result[6][1].StartTime);
        Assert.Equal(new TimeOnly(18, 0), result[6][1].EndTime);
    }

    [Fact]
    public void TestGenerateFreeTime_ConsidersCalendarEventsAndAvailability()
    {
        // Arrange
        var calendar = new Calendar();
        var availability = new Availability();

        // Populate availability for a specific day (Monday)
        availability.weeklySchedule[1] = new List<TimeBlock> {
            new TimeBlock(new TimeOnly(9, 0), new TimeOnly(12, 0)),
            new TimeBlock(new TimeOnly(13, 0), new TimeOnly(17, 0))
        };

        // Add an event that overlaps with availability
        calendar.events.Add(new Event
        {
            EventDate = new DateTime(2024, 4, 22), // Monday
            EventTimeBlock = new TimeBlock(new TimeOnly(10, 0), new TimeOnly(11, 0))
        });

        // Act
        var result = CalendarService.GenerateFreeTime(calendar, availability);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(7, result.Count); // Ensure the result contains 7 days

        // Verify Monday's free time
        Assert.Equal(3, result[2].Count); // Monday (index 2 in result)
        Assert.Equal(new TimeOnly(9, 0), result[2][0].StartTime);
        Assert.Equal(new TimeOnly(10, 0), result[2][0].EndTime);
        Assert.Equal(new TimeOnly(11, 0), result[2][1].StartTime);
        Assert.Equal(new TimeOnly(12, 0), result[2][1].EndTime);
        Assert.Equal(new TimeOnly(13, 0), result[2][2].StartTime);
        Assert.Equal(new TimeOnly(17, 0), result[2][2].EndTime);
    }
}