using Xunit;
using Backend.Services;
using Backend.Models;

public class RespondToInviteTests
{
    [Fact]
    public async Task db_CorrectlyHoldsInvites()
    {
        // Arrange
        var testEvent = new Event
        {
            EventName = "Test Event",
            EventDate = DateTime.Now,
            EventTimeBlock = new TimeBlock(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")),
            EventType = "Meeting",
            EventGroup = new List<UserInfo>()
        };

        var invite = new EventInvite(testEvent, "You are invited to a test event!");
        var inbox = new List<EventInvite> { invite };

        // Act
        var user = await UserInfoServices.CreateUserInfo(
                        "Bob", new(),
                        new(), new(), inbox, new Calendar()); // Ensure UserCalendar is initialized

        // Assert
        Assert.NotNull(user);
        Assert.Single(user.InviteInbox);
        Assert.Equal("Test Event", user.InviteInbox[0].PreConstructedEvent.EventName);
    }

    [Fact]
    public async Task RespondToInvite_HandlesAcceptAndDecline()
    {
        // Arrange
        var testEvent1 = new Event
        {
            EventName = "Accepted Event",
            EventDate = DateTime.Now,
            EventTimeBlock = new TimeBlock(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")),
            EventType = "Meeting",
            EventGroup = new List<UserInfo>()
        };

        var testEvent2 = new Event
        {
            EventName = "Declined Event",
            EventDate = DateTime.Now,
            EventTimeBlock = new TimeBlock(TimeOnly.Parse("12:00"), TimeOnly.Parse("13:00")),
            EventType = "Workshop",
            EventGroup = new List<UserInfo>()
        };

        var invite1 = new EventInvite(testEvent1, "You are invited to an accepted event!");
        var invite2 = new EventInvite(testEvent2, "You are invited to a declined event!");
        var inbox = new List<EventInvite> { invite1, invite2 };

        var user = await UserInfoServices.CreateUserInfo(
                        "Bob", new(),
                        new(), new(), inbox, new Calendar()); // Ensure UserCalendar is initialized

        // Act
        var userInfo = await UserInfoServices.GetUserInfo("Bob");
        Assert.NotNull(userInfo);

        var service = new UserInfoServices();
        service.RespondToInvite(invite1, true, "Bob"); // Accept the first invite
        service.RespondToInvite(invite2, false, "Bob"); // Decline the second invite

        var updatedUserInfo = await UserInfoServices.GetUserInfo("Bob");

        // Assert
        Assert.NotNull(updatedUserInfo);
        Assert.Empty(updatedUserInfo.InviteInbox); // Both invites should be removed
        Assert.DoesNotContain(testEvent2, updatedUserInfo.UserCalendar.events); // Declined event should not be in the calendar
    }
}