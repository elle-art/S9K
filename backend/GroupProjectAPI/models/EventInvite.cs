namespace Backend.Models;

using Google.Cloud.Firestore;

[FirestoreData]
public class EventInvite
{
    [FirestoreProperty]
    public string Message { get; set; }
    [FirestoreProperty]
    public Event PreConstructedEvent { get; set; }

    public EventInvite () {
        Message = "";
        PreConstructedEvent = new Event();
    }
    public EventInvite(Event preConstructedEvent, string message = "")
    {
        this.Message = message;
        this.PreConstructedEvent = preConstructedEvent;
    }
}