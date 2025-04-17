namespace backend.models;

public class Calendar {

    public List<Event> events {get; set;}

    public Calendar() {
        events = new List<Event>();
    }
}