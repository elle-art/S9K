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

    public async void RespondToInvite(EventInvite curInvite, bool response, string uid)
    {
        if (response == true)
        {
            var eventCopy = new Event
            {
                EventName = curInvite.PreConstructedEvent.EventName,
                EventDate = curInvite.PreConstructedEvent.EventDate,
                EventTimeBlock = curInvite.PreConstructedEvent.EventTimeBlock,
                EventType = curInvite.PreConstructedEvent.EventType,
                EventGroup = new List<UserInfo>(curInvite.PreConstructedEvent.EventGroup)
            };
            await CalendarService.AddEventToCalendar(uid, eventCopy);
        }

        var userInfo = await GetUserInfo(uid);
        if (userInfo != null)
        {
            userInfo.InviteInbox.Remove(curInvite);
            UpdateUserInfo(userInfo);
        }
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