public class UserProfile {
    public string DisplayName {get; set;}
    public string WeeklyGoal {get; set;}
    public UserInfo Info {get; set;}

    public UserProfile() {
        // constructor/createUserProfile
    }

    public string GetDisplayName() {
        return DisplayName;
    }

    public void string GetDisplayName(string name) {

    }
}