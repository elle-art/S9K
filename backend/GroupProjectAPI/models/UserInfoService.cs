using Google.Cloud.Firestore;

namespace Backend.Models
{
    public class UserInfoServices
    {
    //     public async Task<UserInfo> createUserInfo(UserInfo u)
    //     {
    //         FirebaseCommunications db = new FirebaseCommunications();
    //         DocumentReference docRef = db.Collection("users").Document(u.displayName); // needs FBComm class impl, but the collection does exist in FB project console
    //         UserInfo user = new UserInfo
    //         {
    //             displayName = u.displayName,
    //             userAvailability = u.userAvailability,
    //             taskList = u.taskList,
    //             preferredTimes = u.preferredTimes,
    //             inviteInbox = u.inviteInbox,
    //             weeklyGoal = u.weeklyGoal,
    //         };
    //         await docRef.SetAsync(user);

    //         return user;
    //     }

    //     public void respondToInvite(bool response)
    //     {
    //         // TODO: configure messages between user's in FB
    //     }

    //     // Dependent on FBComm impl
    //     public void saveUser(ref UserInfo curUser)
    //     {
    //         FirebaseCommunications dbTrans = new FirebaseCommunications();
    //         dbTrans.save(curUser);
    //     }

    //     public void updateUser(ref UserInfo curUser)
    //     {
    //         FirebaseCommunications dbTrans = new FirebaseCommunications();
    //         dbTrans.update(curUser);
    //     }

    //     public void getUserInfo(ref UserInfo curUser)
    //     {// might change to getUserByID?
    //         FirebaseCommunications dbTrans = new FirebaseCommunications();
    //         dbTrans.get(curUser);
    //     }

    //     public Availability getUserAvailability(ref UserInfo curUser)
    //     {
    //         return curUser.userAvailability;
    //     }

    //     public string getName(ref UserInfo curUser)
    //     {
    //         return curUser.displayName;
    //     }
    }
}