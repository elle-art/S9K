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
    public string EventType { get; set; }
    [FirestoreProperty]
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