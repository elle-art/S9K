using Google.Cloud.Firestore;

[FirestoreData]
public class EventInvite
{
    [FirestoreProperty]
    private string message { get; set; }
    [FirestoreProperty]
    private Event preConstructedEvent { get; set; }

    public EventInvite(Event preConstructedEvent, string message = "")
    {
        this.message = message;
        this.preConstructedEvent = preConstructedEvent;
    }
}