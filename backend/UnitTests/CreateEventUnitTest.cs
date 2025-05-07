using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Backend.Models;      // Event, TimeBlock, Calendar, UserInfo
using Backend.Services;    // DBCommunications

public class CreateEventUnitTest
{
    [Fact]
    public async Task CreateEventAsync_PersistsEventInCalendar()
    {
        // ─── Arrange ─────────────────────────────────────────────────────────
        var uid    = Guid.NewGuid().ToString();              // unique key per test
        var date   = DateTime.Today;
        var block  = new TimeBlock(new TimeOnly(10, 0),
                                   new TimeOnly(11, 0));

        // Act ─ create the event via the public use‑case method
        var created = await EventService.CreateEventAsync(
                          uid,
                          "Stand‑up Meeting",
                          date,
                          block,
                          "Meeting",
                          new List<UserInfo>());             // empty group

        // ─── Assert ──────────────────────────────────────────────────────────
        // 1) The call returns the expected object
        Assert.Equal("Stand‑up Meeting", created.EventName);
        Assert.Equal(date,  created.EventDate);
        Assert.Equal(block, created.EventTimeBlock);

        // 2) The calendar was saved to the fake DB and contains exactly that event
        var savedCal = await DBCommunications.GetObjectAsync<Calendar>(uid, "calendar");

        Assert.NotNull(savedCal);
        Assert.Single(savedCal!.events);

        var savedEvent = savedCal.events[0];

        // same data => persistence worked
        Assert.Equal(created.EventName,   savedEvent.EventName);
        Assert.Equal(created.EventDate,   savedEvent.EventDate);
        Assert.Equal(created.EventType,   savedEvent.EventType);
        Assert.Equal(created.EventTimeBlock, savedEvent.EventTimeBlock);

        // Optional: prove it’s the very same reference (because AddEventToCalendar
        // stores the object that CreateEventAsync returns)
        Assert.True(ReferenceEquals(created, savedEvent));
    }
}
