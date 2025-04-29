using Xunit;
using backend.models;

public class EventServiceTests
{
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _eventService = new EventService();
    }

    [Fact]
    public void GenerateEventTime_WithOverlappingAvailability_ReturnsValidTime()
    {
        // Arrange
        var desiredDate = new DateTime(2025, 4, 22); // Tuesday
        int desiredDayOfWeek = (int)desiredDate.DayOfWeek;
        int eventDuration = 60; // 60 minutes

        var user1 = new UserInfo
        {
            userAvailability = new Availability()
        };
        user1.userAvailability.weeklySchedule[desiredDayOfWeek] = new List<TimeBlock>
        {
            new TimeBlock(
                TimeOnly.Parse("09:00"),
                TimeOnly.Parse("17:00")
            )
        };

        var user2 = new UserInfo
        {
            userAvailability = new Availability()
        };
        user2.userAvailability.weeklySchedule[desiredDayOfWeek] = new List<TimeBlock>
        {
            new TimeBlock(
                TimeOnly.Parse("10:00"),
                TimeOnly.Parse("15:00")
            )
        };

        var testEvent = new Event(
            "Test Event",
            desiredDate,
            new TimeBlock(TimeOnly.MinValue, TimeOnly.MaxValue),
            "Test",
            new List<string>()
        );
        testEvent.EventGroup = new List<object> { user1, user2 };

        // Act
        var (eventDateTime, eventTimeBlock) = _eventService.GenerateEventTime(ref testEvent, eventDuration, desiredDate);

        // Assert
        Assert.NotNull(eventDateTime);
        Assert.NotEqual(TimeOnly.MaxValue, eventTimeBlock.StartTime);
        Assert.NotEqual(TimeOnly.MinValue, eventTimeBlock.EndTime);
        
        Assert.True(eventTimeBlock.StartTime >= TimeOnly.Parse("10:00"));
        Assert.True(eventTimeBlock.EndTime <= TimeOnly.Parse("15:00"));
        
        var duration = eventTimeBlock.EndTime.ToTimeSpan() - eventTimeBlock.StartTime.ToTimeSpan();
        Assert.Equal(TimeSpan.FromMinutes(eventDuration), duration);
    }

    [Fact]
    public void GenerateEventTime_NoOverlappingAvailability_ReturnsDefaultTime()
    {
        // Arrange
        var desiredDate = new DateTime(2025, 4, 22);
        int desiredDayOfWeek = (int)desiredDate.DayOfWeek;
        int eventDuration = 60;

        var user1 = new UserInfo
        {
            userAvailability = new Availability()
        };
        user1.userAvailability.weeklySchedule[desiredDayOfWeek] = new List<TimeBlock>
        {
            new TimeBlock(
                TimeOnly.Parse("09:00"),
                TimeOnly.Parse("12:00")
            )
        };

        var user2 = new UserInfo
        {
            userAvailability = new Availability()
        };
        user2.userAvailability.weeklySchedule[desiredDayOfWeek] = new List<TimeBlock>
        {
            new TimeBlock(
                TimeOnly.Parse("13:00"),
                TimeOnly.Parse("17:00")
            )
        };

        var testEvent = new Event(
            "Test Event",
            desiredDate,
            new TimeBlock(TimeOnly.MinValue, TimeOnly.MaxValue),
            "Test",
            new List<string>()
        );
        testEvent.EventGroup = new List<object> { user1, user2 };

        // Act
        var (eventDateTime, eventTimeBlock) = _eventService.GenerateEventTime(ref testEvent, eventDuration, desiredDate);

        // Assert
        Assert.Equal(TimeOnly.MaxValue, eventTimeBlock.StartTime);
        Assert.Equal(TimeOnly.MinValue, eventTimeBlock.EndTime);
    }
}