namespace backend.models;

public class EventInvite
{
    private string message { get; set; }
    private Event preConstructedEvent { get; set; }

    public EventInvite(Event preConstructedEvent, string message = "")
    {
        this.message = message;
        this.preConstructedEvent = preConstructedEvent;
    }
}