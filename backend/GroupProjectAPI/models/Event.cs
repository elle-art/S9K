namespace Backend.Models;

using Google.Cloud.Firestore;

[FirestoreData]
public class Event
{
    [FirestoreProperty]
    public string EventName { get; set; }
    [FirestoreProperty]
    public DateTime EventDate { get; set; }
    [FirestoreProperty]
    public TimeBlock EventTimeBlock { get; set; }
    [FirestoreProperty]
    public string? EventType { get; set; }
    [FirestoreProperty]
    public List<UserInfo> EventGroup { get; set; }

    public Event() {
        EventName = "";
        EventGroup = [];
    }

    public Event(string eventName, DateTime eventDate, TimeBlock eventTimeBlock, string eventType, List<UserInfo> eventGroup)
    {
        this.EventName = eventName;
        this.EventDate = eventDate;
        this.EventTimeBlock = eventTimeBlock;
        this.EventType = eventType;

        //Subject to change depending on if we decide to store user profiles, or just userName strings
        this.EventGroup = eventGroup;
    }
}