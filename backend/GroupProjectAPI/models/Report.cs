namespace Backend.Models;

using Google.Cloud.Firestore;

[FirestoreData]
public class Report
{
    [FirestoreProperty]
    public DateTime ReportDate { get; set; }
    [FirestoreProperty]
    public UserInfo UserData { get; set; }
    [FirestoreProperty]
    public List<UserTask> CompletedTasks { get; set; }
    [FirestoreProperty]
    public Dictionary<string, int> EventTypeCounts { get; set; }

    //Subject to change
    public List<Event> EventData { get; set; }

    public Report()
    {
        // constructor
    }
}