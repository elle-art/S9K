namespace backend.models;

using System;
using System.Globalization;
using static backend.models.TimeBlock;
using Google.Cloud.Firestore;

public class EventService
{


    //To-Do: Event creation logic implementation
    //To-Do: Determine where event addition to calendar is performed - separate function from create
    //To-Do: Event invite sending (when applicable) on event creation

    public async Task<Event> CreateEvent(string eventName, DateTime eventDate, TimeBlock eventTimeBlock, string eventType, List<UserInfo> group)
    {
        //FirebaseCommunications db = new FirebaseCommunications();
        //DocumentReference docRef = db.Collection("events").Document(eventName); // check collection name in FB
        Event newEvent = new Event(eventName, eventDate, eventTimeBlock, eventType, group);

        //await docRef.SetAsync(newEvent);

        return newEvent;
    }

    //To-do: Event time generation algorithm
    //To-do: consideration of preferred times
    public (DateTime, TimeBlock) GenerateEventTime(ref Event curEvent, int numMin, DateTime desiredDate)
    {
        if (curEvent.EventGroup == null || !curEvent.EventGroup.Any())
        {
            return (desiredDate, new TimeBlock(TimeOnly.MaxValue, TimeOnly.MinValue));
        }

        int desiredDayOfWeek = (int)desiredDate.DayOfWeek;
        var requiredDuration = TimeSpan.FromMinutes(numMin);

        // Get all users' availability blocks for the desired day
        var allUserAvailabilities = curEvent.EventGroup
            .Select(user => user.userAvailability.weeklySchedule[desiredDayOfWeek])
            .ToList();

        // Find all possible overlapping time blocks
        var sharedAvailability = FindOverlappingTimeBlocks(allUserAvailabilities);

        // Find the first available time block that can accommodate the required duration
        foreach (var block in sharedAvailability.OrderBy(b => b.StartTime))
        {
            var potentialEndTime = TimeOnly.FromTimeSpan(block.StartTime.ToTimeSpan().Add(requiredDuration));
            if (potentialEndTime <= block.EndTime)
            {
                var eventTime = new TimeBlock(block.StartTime, potentialEndTime);
                // Combine the date and time properly
                var eventDateTime = desiredDate.Date.Add(eventTime.StartTime.ToTimeSpan());
                return (eventDateTime, eventTime);
            }
        }

        // If no suitable time block is found, return a default value
        return (desiredDate, new TimeBlock(TimeOnly.MaxValue, TimeOnly.MinValue));
    }

    private List<TimeBlock> FindOverlappingTimeBlocks(List<List<TimeBlock>> allUserAvailabilities)
    {
        if (!allUserAvailabilities.Any())
            return new List<TimeBlock>();

        // Start with the first user's availability
        var sharedAvailability = new List<TimeBlock>(allUserAvailabilities[0]);

        // For each additional user, find overlapping time blocks
        foreach (var userAvailability in allUserAvailabilities.Skip(1))
        {
            var newSharedAvailability = new List<TimeBlock>();

            foreach (var sharedBlock in sharedAvailability)
            {
                foreach (var userBlock in userAvailability)
                {
                    if (HasOverlap(sharedBlock, userBlock))
                    {
                        var overlap = new TimeBlock(
                            TimeOnly.FromTimeSpan(TimeSpan.FromTicks(
                                Math.Max(sharedBlock.StartTime.Ticks, userBlock.StartTime.Ticks))),
                            TimeOnly.FromTimeSpan(TimeSpan.FromTicks(
                                Math.Min(sharedBlock.EndTime.Ticks, userBlock.EndTime.Ticks)))
                        );
                        newSharedAvailability.Add(overlap);
                    }
                }
            }

            sharedAvailability = newSharedAvailability;

            if (!sharedAvailability.Any())
                break;
        }

        return sharedAvailability;
    }

    private bool HasOverlap(TimeBlock block1, TimeBlock block2)
    {
        return block1.StartTime < block2.EndTime && block2.StartTime < block1.EndTime;
    }

    //To-do: finish Calendar implementation to test editing of events
    //To-do: ICS event update, although this is on the back-burner
    public void EditEvent(ref Event eventTBE, string? name, DateTime? date, TimeBlock? time, string? type)
    {
        if (name != null)
            eventTBE.EventName = name;

        if (date.HasValue)
            eventTBE.EventDate = date.Value;

        if (time != null)
            eventTBE.EventTimeBlock = time.Value;

        if (type != null)
            eventTBE.EventType = type;


    }

    //To-Do: search for user logic()
    public bool SearchForUser(String userName)
    {
        return false;
    }

    public void AddUserToGroup(ref Event curEvent, string userName)
    {
        // Add user to group bases on their user name
    }

    public void RemoveUserFromGroup(ref Event curEvent, UserInfo user)
    {
        //Assumption made is that we know the user is in the group, so we don't need to check the list first
        curEvent.EventGroup.Remove(user);
    }
}