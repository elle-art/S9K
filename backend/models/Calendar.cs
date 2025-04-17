using Google.Cloud.Firestore;

[FirestoreData]
public class Calendar {
    [FirestoreProperty]

    public List<Event> events {get; set;}

    public Calendar() {
        // constructor
    }
}