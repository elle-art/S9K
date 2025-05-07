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

    /// <summary>
    /// Append a new <see cref="UserTask"/> to the user’s TaskList and persist the update.
    /// Noop if the user record doesn’t exist.
    /// </summary>
    public static async Task AddToDoTaskAsync(string uid, UserTask newTask)
    {
        // Pull the latest copy
        var user = await GetUserInfo(uid);
        if (user == null) return;                       // or throw, if you prefer

        user.TaskList ??= new List<UserTask>();         // lazy‑init
        user.TaskList.Add(newTask);

        // Re‑save the whole UserInfo document
        await DBCommunications.SaveObjectAsync(uid, user);
    }

    /// <summary>
    /// Replace the first task that matches <paramref name="taskToFind"/> with
    /// <paramref name="replacement"/> inside the user’s TaskList and persist.
    /// Returns <c>true</c> if a match was changed; otherwise <c>false</c>.
    /// </summary>
    public static async Task<bool> ModifyAToDoTaskAsync(
            string uid,
            UserTask taskToFind,
            UserTask replacement)
    {
        var user = await GetUserInfo(uid);
        if (user == null || user.TaskList == null) return false;

        // Find the index of the first matching task (strict equality on all 3 fields)
        int idx = user.TaskList.FindIndex(t =>
               t.TaskName == taskToFind.TaskName &&
               t.TaskDate == taskToFind.TaskDate &&
               t.TaskStatus == taskToFind.TaskStatus);

        if (idx == -1) return false;                  // no match: nothing to change

        user.TaskList[idx] = replacement;             // in‑place overwrite
        await DBCommunications.SaveObjectAsync(uid, user);
        return true;
    }

}