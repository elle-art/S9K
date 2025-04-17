namespace backend.models;

public class Event
{
    public string EventName { get; set; }
    public DateTime EventDate { get; set; }
    public TimeBlock EventTimeBlock { get; set; }
    public string EventType { get; set; }
    public List<string> EventGroup { get; set; }

    public Event(string eventName, DateTime eventDate, TimeBlock eventTimeBlock, string eventType)
    {
        EventName = eventName;
        EventDate = eventDate;
        EventTimeBlock = eventTimeBlock;
        EventType = eventType;

        //Subject to change depending on if we decide to store user profiles, or just userName strings
        EventGroup = new List<string>();
    }
}