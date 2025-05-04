using Xunit;
using backend.models;
using System;
using System.Collections.Generic;
using System.Linq;

public class EventServiceTests
{
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _eventService = new EventService();
    }

    [Fact]
    public void GenerateEventTime_ReturnsCorrectDateTime()
    {
        // Arrange
        var user1Availability = new TimeBlock(
            TimeOnly.Parse("09:00"),
            TimeOnly.Parse("17:00")
        );

        var user2Availability = new TimeBlock(
            TimeOnly.Parse("10:00"),
            TimeOnly.Parse("18:00")
        );

        var user1 = new UserInfo
        {
            displayName = "User1",
            userAvailability = new Availability
            {
                weeklySchedule = new List<TimeBlock>[]
                {
                    new List<TimeBlock>(), // Sunday
                    new List<TimeBlock>(), // Monday
                    new List<TimeBlock> { user1Availability }, // Tuesday
                    new List<TimeBlock>(), // Wednesday
                    new List<TimeBlock>(), // Thursday
                    new List<TimeBlock>(), // Friday
                    new List<TimeBlock>()  // Saturday
                }
            },
            inviteInbox = new List<EventInvite>()
        };

        var user2 = new UserInfo
        {
            displayName = "User2",
            userAvailability = new Availability
            {
                weeklySchedule = new List<TimeBlock>[]
                {
                    new List<TimeBlock>(), // Sunday
                    new List<TimeBlock>(), // Monday
                    new List<TimeBlock> { user2Availability }, // Tuesday
                    new List<TimeBlock>(), // Wednesday
                    new List<TimeBlock>(), // Thursday
                    new List<TimeBlock>(), // Friday
                    new List<TimeBlock>()  // Saturday
                }
            },
            inviteInbox = new List<EventInvite>()
        };

        var testEvent = new Event(
            "Test Event",
            new DateTime(2024, 4, 23), // A Tuesday
            new TimeBlock(TimeOnly.MinValue, TimeOnly.MaxValue),
            "Test Type",
            new List<UserInfo> { user1, user2 }
        );

        // Act
        var (eventDateTime, timeBlock) = _eventService.GenerateEventTime(ref testEvent, 60, testEvent.EventDate);

        // Assert
        // Verify the time block
        Assert.Equal(TimeOnly.Parse("10:00"), timeBlock.StartTime);
        Assert.Equal(TimeOnly.Parse("11:00"), timeBlock.EndTime);

        // Verify the full DateTime
        var expectedDateTime = new DateTime(2024, 4, 23, 10, 0, 0);
        Assert.Equal(expectedDateTime, eventDateTime);
    }

    [Fact]
    public void GenerateEventTime_HandlesDifferentDurations()
    {
        // Arrange
        var availability = new TimeBlock(
            TimeOnly.Parse("09:00"),
            TimeOnly.Parse("17:00")
        );

        var user = new UserInfo
        {
            displayName = "TestUser",
            userAvailability = new Availability
            {
                weeklySchedule = new List<TimeBlock>[]
                {
                    new List<TimeBlock>(), // Sunday
                    new List<TimeBlock>(), // Monday
                    new List<TimeBlock> { availability }, // Tuesday
                    new List<TimeBlock>(), // Wednesday
                    new List<TimeBlock>(), // Thursday
                    new List<TimeBlock>(), // Friday
                    new List<TimeBlock>()  // Saturday
                }
            },
            inviteInbox = new List<EventInvite>()
        };

        var testEvent = new Event(
            "Test Event",
            new DateTime(2024, 4, 23), // A Tuesday
            new TimeBlock(TimeOnly.MinValue, TimeOnly.MaxValue),
            "Test Type",
            new List<UserInfo> { user }
        );

        // Act - Test 30 minute duration
        var (eventDateTime30, timeBlock30) = _eventService.GenerateEventTime(ref testEvent, 30, testEvent.EventDate);

        // Assert
        Assert.Equal(TimeOnly.Parse("09:00"), timeBlock30.StartTime);
        Assert.Equal(TimeOnly.Parse("09:30"), timeBlock30.EndTime);
        Assert.Equal(new DateTime(2024, 4, 23, 9, 0, 0), eventDateTime30);

        // Act - Test 120 minute duration
        var (eventDateTime120, timeBlock120) = _eventService.GenerateEventTime(ref testEvent, 120, testEvent.EventDate);

        // Assert
        Assert.Equal(TimeOnly.Parse("09:00"), timeBlock120.StartTime);
        Assert.Equal(TimeOnly.Parse("11:00"), timeBlock120.EndTime);
        Assert.Equal(new DateTime(2024, 4, 23, 9, 0, 0), eventDateTime120);
    }

    [Fact]
    public void GenerateEventTime_HandlesDifferentDays()
    {
        // Arrange
        var mondayAvailability = new TimeBlock(
            TimeOnly.Parse("09:00"),
            TimeOnly.Parse("17:00")
        );

        var wednesdayAvailability = new TimeBlock(
            TimeOnly.Parse("13:00"),
            TimeOnly.Parse("21:00")
        );

        var user = new UserInfo
        {
            displayName = "TestUser",
            userAvailability = new Availability
            {
                weeklySchedule = new List<TimeBlock>[]
                {
                    new List<TimeBlock>(), // Sunday
                    new List<TimeBlock> { mondayAvailability }, // Monday
                    new List<TimeBlock>(), // Tuesday
                    new List<TimeBlock> { wednesdayAvailability }, // Wednesday
                    new List<TimeBlock>(), // Thursday
                    new List<TimeBlock>(), // Friday
                    new List<TimeBlock>()  // Saturday
                }
            },
            inviteInbox = new List<EventInvite>()
        };

        var testEvent = new Event(
            "Test Event",
            new DateTime(2024, 4, 22), // A Monday
            new TimeBlock(TimeOnly.MinValue, TimeOnly.MaxValue),
            "Test Type",
            new List<UserInfo> { user }
        );

        // Act - Test Monday
        var (mondayDateTime, mondayTimeBlock) = _eventService.GenerateEventTime(ref testEvent, 60, testEvent.EventDate);

        // Assert
        Assert.Equal(TimeOnly.Parse("09:00"), mondayTimeBlock.StartTime);
        Assert.Equal(TimeOnly.Parse("10:00"), mondayTimeBlock.EndTime);
        Assert.Equal(new DateTime(2024, 4, 22, 9, 0, 0), mondayDateTime);

        // Act - Test Wednesday
        testEvent.EventDate = new DateTime(2024, 4, 24); // A Wednesday
        var (wednesdayDateTime, wednesdayTimeBlock) = _eventService.GenerateEventTime(ref testEvent, 60, testEvent.EventDate);

        // Assert
        Assert.Equal(TimeOnly.Parse("13:00"), wednesdayTimeBlock.StartTime);
        Assert.Equal(TimeOnly.Parse("14:00"), wednesdayTimeBlock.EndTime);
        Assert.Equal(new DateTime(2024, 4, 24, 13, 0, 0), wednesdayDateTime);
    }
}