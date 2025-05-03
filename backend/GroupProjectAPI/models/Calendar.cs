namespace backend.models;

using Google.Cloud.Firestore;

[FirestoreData]
public class Calendar {
    [FirestoreProperty]
    public List<Event> events {get; set;}

    public Calendar() {
        events = new List<Event>();
    }

    public Calendar(List<Event> userEvents) {
        events = userEvents;
    }
}