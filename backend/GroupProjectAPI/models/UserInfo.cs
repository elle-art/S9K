namespace backend.models;

using Google.Cloud.Firestore;

[FirestoreData]
public class UserInfo
{
    [FirestoreProperty]
    public required string displayName { get; set; }
    [FirestoreProperty]
    public Availability? userAvailability { get; set; }
    [FirestoreProperty]
    public List<UserTask>? taskList { get; set; }
    [FirestoreProperty]
    public List<TimeBlock>? preferredTimes { get; set; }
    [FirestoreProperty]
    public required List<EventInvite> inviteInbox { get; set; }
    [FirestoreProperty]
    public Calendar? userCalendar { get; set; }
    [FirestoreProperty]
    public string? weeklyGoal { get; set; }
}