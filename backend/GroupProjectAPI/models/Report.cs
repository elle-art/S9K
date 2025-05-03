namespace backend.models;

using Google.Cloud.Firestore;

[FirestoreData]
public class Report
{
    [FirestoreProperty]
    public DateTime reportDate { get; set; }
    [FirestoreProperty]
    public UserInfo userData { get; set; }
    [FirestoreProperty]
    public List<UserTask> completedTasks { get; set; }

    //Subject to change
    public List<Event> eventData { get; set; }

    public Report()
    {
        // constructor/createTask
    }
}