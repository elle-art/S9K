public class UserInfoServices
{
    public UserInfo createUserInfo(u: UserInfo)
    {
        DocumentReference docRef = db.Collection("users").Document(u.displayName);
        UserInfo user = new UserInfo
        {
            displayName = u.displayName,
            userAvailability = u.userAvailability,
            taskList = u.taskList,
            preferredTimes = u.preferredTimes,
            inviteInbox = u.inviteInbox,
            weeklyGoal = u.weeklyGoal,
        };
        await docRef.SetAsync(user);
    }

    public bool respondToInvite(response: bool)
    {
        // TODo: configure messages between user's in FB
    }

    public void saveUser(u: UserInfo)
    {
        FirebaseCommunications dbTrans = new FirebaseCommunications();
        dbTrans.save(u);
    }

    public void updateUser(u:UserInfo)
    {
        FirebaseCommunications dbTrans = new FirebaseCommunications();
        dbTrans.update(u);
    }
    // Assuming we'll eventually need to pass id's or such to retrieve data
    public UserInfoServices getUserInfo()
    {
        FirebaseCommunications dbTrans = new FirebaseCommunications();
        return dbTrans.get(u);
    }

    public Availability getUserAvailability()
    {
         UserInfo u = getUserInfo();
         return u.userAvailability;
    }

    public string getName()
    {
        UserInfo u = getUserInfo();
         return u.displayName;
    }
}