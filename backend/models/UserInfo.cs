public class UserInfo {
    public string displayName {get; set;}
    public Availability userAvailability {get; set;}
    public List<Task> taskList {get; set;}
    public List<TimeBlock> preferredTimes {get; set;}
    public List<EventInvite> inviteInbox {get; set;}
    public string WeeklyGoal;

    public UserInfo() {
        // constructor/createUserInfo
    }
}