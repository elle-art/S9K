namespace backend.models;

using System;
using System.Globalization;
using static backend.models.TimeBlock;
using Google.Cloud.Firestore;
using Backend.Services;

public class EventService
{


    //To-Do: Event creation logic implementation
    //To-Do: Determine where event addition to calendar is performed - separate function from create
    //To-Do: Event invite sending (when applicable) on event creation

    /// <summary>
    /// Creates a new event and saves it to firebase
    /// </summary>
    /// <param name="uid">user id</param>
    /// <param name="eventName">name of the event</param>
    /// <param name="eventDate">date of the event</param>
    /// <param name="eventTimeBlock">time block of the event</param>
    /// <param name="eventType">the type of event</param>
    /// <param name="group">the group of people in the event</param>
    /// <returns></returns>
    public static async Task<Event> CreateEventAsync(
        string uid, 
        string eventName, 
        DateTime eventDate, 
        TimeBlock eventTimeBlock, 
        string eventType, 
        List<UserInfo> group) 
    {

        Event newEvent = new Event(eventName, eventDate, eventTimeBlock, eventType, group);

        await CalendarService.AddEventToCalendar(uid, newEvent);

        return newEvent;
    }

    //To-do: Event time generation algorithm algorithm
    //To-do: consideration of preferred times
    public (DateTime, TimeBlock) GenerateEventTime(ref Event curEvent, int numMin, DateTime desiredDate)
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        int desiredDayOfTheWeek = (int) desiredDate.DayOfWeek;

        List<TimeBlock> sharedAvailability = new List<TimeBlock>();

        foreach (UserInfo user in curEvent.EventGroup.Cast<UserInfo>())
        {
            foreach (TimeBlock tb in user.userAvailability.weeklySchedule[desiredDayOfTheWeek])
            {

            }
        }



        return (DateTime.Now, new TimeBlock(TimeOnly.MaxValue, TimeOnly.MinValue));
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
        curEvent.EventGroup.Add(userName);
    }

    public void RemoveUserFromGroup(ref Event curEvent, string user)
    {
        //Assumption made is that we know the user is in the group, so we don't need to check the list first
        curEvent.EventGroup.Remove(user);
    }
}