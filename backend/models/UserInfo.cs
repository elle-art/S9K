public class UserInfo {
    public Availability userAvailability {get; set;}
    public Task[] taskList {get; set;}
    public DateTime[] preferredTimes {get; set;}
    public eventInvite[] inviteInbox {get; set;}
    public string weeklyGoal {
        get;
        set {

        }
    }

    public UserInfo() {
        // constructor/createUserInfo
    }

    public void EditAvailability() {

    }
}