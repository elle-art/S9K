using Google.Cloud.Firestore;

[FirestoreData]
public class UserInfo
{
    [FirestoreProperty]
    public string displayName { get; set; }
    [FirestoreProperty]
    public Availability userAvailability { get; set; }
    [FirestoreProperty]
    public List<Task> taskList { get; set; }
    [FirestoreProperty]
    public List<TimeBlock> preferredTimes { get; set; }
    [FirestoreProperty]
    public List<EventInvite> inviteInbox { get; set; }
    [FirestoreProperty]
    public string weeklyGoal { get; set; };
}