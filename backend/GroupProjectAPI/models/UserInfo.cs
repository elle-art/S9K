namespace backend.models;

using Google.Cloud.Firestore;

[FirestoreData]
public class UserInfo
{
    [FirestoreProperty]
    public string DisplayName { get; set; }
    [FirestoreProperty]
    public Availability? UserAvailability { get; set; }
    [FirestoreProperty]
    public List<UserTask>? TaskList { get; set; }
    [FirestoreProperty]
    public List<TimeBlock>? PreferredTimes { get; set; }
    [FirestoreProperty]
    public List<EventInvite> InviteInbox { get; set; }
    [FirestoreProperty]
    public Calendar? UserCalendar { get; set; }
    [FirestoreProperty]
    public string? WeeklyGoal { get; set; }

    public UserInfo() { 
        DisplayName = "";
        InviteInbox = new List<EventInvite>();
    }
    public UserInfo(string name, Availability availability, List<UserTask> tasks, List<TimeBlock> preferred, List<EventInvite> invites, Calendar calendar = null, string goal = "schedule more!")
    {
        this.DisplayName = name;
        this.UserAvailability = availability;
        this.TaskList = tasks;
        this.PreferredTimes = preferred;
        this.InviteInbox = invites;
        this.UserCalendar = calendar;
        this.WeeklyGoal = goal;
    }
}

