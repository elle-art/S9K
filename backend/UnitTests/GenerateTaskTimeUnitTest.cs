using Xunit;
using Backend.Models;
using System;
using System.Collections.Generic;

public class GenerateTaskTimeUnitTest
{
    [Fact]
    public void TestGenerateTaskTime_ReturnsCorrectTimeBlock()
    {
        // Arrange
        var availability = new Availability();
        var calendar = new Calendar();

        // Set up availability for today (Tuesday)
        int todayDayOfWeek = (int)DateTime.Today.DayOfWeek;
        availability.weeklySchedule[todayDayOfWeek] = new List<TimeBlock>
        {
            new TimeBlock(new TimeOnly(9, 0), new TimeOnly(12, 0)),
            new TimeBlock(new TimeOnly(13, 0), new TimeOnly(17, 0))
        };

        // Add an event that overlaps with part of the availability
        calendar.events.Add(new Event
        {
            EventDate = DateTime.Today,
            EventTimeBlock = new TimeBlock(new TimeOnly(10, 0), new TimeOnly(11, 0))
        });

        var taskService = new TaskService();
        int taskDurationMinutes = 60;

        // Act
        var result = taskService.GenerateTaskTime(availability, calendar, taskDurationMinutes);

        // Assert
        Assert.Equal(new TimeOnly(9, 0), result.StartTime);
        Assert.Equal(new TimeOnly(10, 0), result.EndTime);
    }

    [Fact]
    public void TestGenerateTaskTime_NoAvailableTimeBlock_ReturnsDefault()
    {
        // Arrange
        var availability = new Availability();
        var calendar = new Calendar();

        // Set up availability for today (Tuesday)
        int todayDayOfWeek = (int)DateTime.Today.DayOfWeek;
        availability.weeklySchedule[todayDayOfWeek] = new List<TimeBlock>
        {
            new TimeBlock(new TimeOnly(9, 0), new TimeOnly(10, 0))
        };

        // Add an event that fully overlaps with the availability
        calendar.events.Add(new Event
        {
            EventDate = DateTime.Today,
            EventTimeBlock = new TimeBlock(new TimeOnly(9, 0), new TimeOnly(10, 0))
        });

        var taskService = new TaskService();
        int taskDurationMinutes = 60;

        // Act
        var result = taskService.GenerateTaskTime(availability, calendar, taskDurationMinutes);

        // Assert
        Assert.Equal(TimeOnly.MinValue, result.StartTime);
        Assert.Equal(TimeOnly.MinValue, result.EndTime);
    }
}