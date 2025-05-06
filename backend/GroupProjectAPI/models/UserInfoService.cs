namespace Backend.Models;

using System.Threading.Tasks;
using Backend.Services;


public class UserInfoServices
{
    /// <summary>
    /// Create a new user object and save it to Firebase
    /// </summary>
    /// <param name="u"></param>
    /// <returns></returns>
    public static async Task<UserInfo> CreateUserInfo(string name, Availability availability, List<UserTask> tasks, List<TimeBlock> preferred, List<EventInvite> invites, Calendar calendar = null, string goal = "schedule more!")
    {
        UserInfo user = new UserInfo(name, availability, tasks, preferred, invites, calendar, goal);

        await DBCommunications.SaveObjectAsync(name, user);

        return user;
    }

    public void RespondToInvite(bool response)
    {
        // TODO: configure messages between user's in FB
    }

    /// <summary>
    /// Create a new user object and save it to firebase
    /// </summary>
    /// <param name="curUser"></param>
    /// <returns></returns>
    public static async void UpdateUserInfo(UserInfo curUser)
    {
        await DBCommunications.SaveObjectAsync(curUser.DisplayName, curUser);
    }

    /// <summary>
    /// Create a new user object and save it to firebase
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public async Task<Availability> GetUserAvailability(string uid)
    {
        return await AvailabilityService.GetAvailabilityAsync(uid);
    }

    /// <summary>
    /// Create a new user object and save it to firebase
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static async Task<UserInfo> GetUserInfo(string uid)
    {
        return await DBCommunications.GetObjectAsync<UserInfo>(uid, "UserInfo");
    }
}