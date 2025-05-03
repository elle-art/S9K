using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using backend.models;
using System.Text.Json; // if FireBaseCommunications or DBCommunications is here

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("save")]
        public async Task<IActionResult> SaveUser([FromQuery] string uid, [FromBody] JsonElement rawUserData)
        {
            if (string.IsNullOrEmpty(uid))
                return BadRequest("UID is missing.");

            try
            {
                var userInfo = await ConvertToUserInfo(rawUserData);

                Console.WriteLine("got user obj");
                Console.WriteLine(userInfo);
                await DBCommunications.SaveObjectAsync(uid, "UserInfo", userInfo);
                Console.WriteLine("Saved to FB");

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to convert or save user data: {ex.Message}");
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUser([FromQuery] string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return BadRequest("UID is required.");

            UserInfo User = await DBCommunications.GetObjectAsync<UserInfo>(uid, "UserInfo");

            if (User == null)
                return NotFound("User not found.");

            return Ok(User);
        }

        private async Task<UserInfo> ConvertToUserInfo(JsonElement raw)
        {
            Console.Write("in create from json function!");
            var displayName = raw.GetProperty("displayName").GetString();
            Console.WriteLine("got namer...");

            var weeklyGoal = raw.TryGetProperty("weeklyGoal", out var wg) ? wg.GetString() : null;
            Console.WriteLine("got goal, startin availa...");

            var availability = AvailabilityService.CreateObjFromJson(raw.GetProperty("userAvailability").EnumerateArray());

            Console.WriteLine("got avail, stating cal...");

            var eventTasks = raw.GetProperty("userCalendar")
            .EnumerateArray()
            .Select(async c =>
            {
                var groupTasks = c.GetProperty("group")
                    .EnumerateArray()
                    .Select(async u =>
                    {
                        var userId = u.GetString();
                        if (string.IsNullOrEmpty(userId))
                            return null;

                        return await DBCommunications.GetObjectAsync<UserInfo>(userId, "UserInfo");
                    })
                    .ToList();

                return new Event
                {
                    EventName = c.GetProperty("name").GetString(),
                    EventDate = DateTime.Parse(c.GetProperty("date").GetString()),
                    EventTimeBlock = new TimeBlock
                    {
                        StartTime = TimeOnly.Parse(c.GetProperty("time").GetProperty("startTime").GetString()),
                        EndTime = TimeOnly.Parse(c.GetProperty("time").GetProperty("endTime").GetString())
                    },
                    EventGroup = (await Task.WhenAll(groupTasks)).ToList(),
                    EventType = c.TryGetProperty("type", out var typeProp) ? typeProp.GetString() : null
                };
            }).ToList();

            var userCalendar = new Calendar((await Task.WhenAll(eventTasks)).ToList());
            Console.WriteLine("got cal, starting tasks...");

            var rawTasks = raw.GetProperty("taskList").EnumerateArray();
            var tasks = new List<UserTask>();
            foreach (var entry in rawTasks)
            {

                var task = new UserTask
                {
                    TaskName = entry.GetProperty("name").GetString(),
                    TaskDate = DateTime.Parse(entry.GetProperty("date").GetString()),
                    TaskStatus = entry.GetProperty("status").GetBoolean()
                };

                tasks.Add(task);
            }

            var taskList = tasks;
            Console.WriteLine("got tasks, starting preferred...");

            var rawPreferred = raw.GetProperty("preferredTimes").EnumerateArray();
            var preferred = new List<TimeBlock>();
            foreach (var entry in rawPreferred)
            {

                var timeBlock = new TimeBlock
                {
                    StartTime = TimeOnly.Parse(entry.GetProperty("startTime").GetString()),
                    EndTime = TimeOnly.Parse(entry.GetProperty("endTime").GetString())
                };


                preferred.Add(timeBlock);
            }

            var preferredTimes = preferred;
            Console.WriteLine("got pref, starting invites...");

            var inviteTasks = raw.GetProperty("inviteInbox")
            .EnumerateArray()
            .Select(async c =>
            {
                var eventJson = c.GetProperty("event");

                var groupTasks = eventJson.GetProperty("group")
                    .EnumerateArray()
                    .Select(u => DBCommunications.GetObjectAsync<UserInfo>(u.ToString(), "UserInfo"))
                    .ToList();

                var constructedEvent = new Event
                {
                    EventName = eventJson.GetProperty("name").GetString(),
                    EventDate = DateTime.Parse(eventJson.GetProperty("date").GetString()),
                    EventTimeBlock = new TimeBlock
                    {
                        StartTime = TimeOnly.Parse(eventJson.GetProperty("time").GetProperty("startTime").GetString()),
                        EndTime = TimeOnly.Parse(eventJson.GetProperty("time").GetProperty("endTime").GetString())
                    },
                    EventGroup = (await Task.WhenAll(groupTasks)).ToList(),
                    EventType = eventJson.TryGetProperty("type", out var typeProp) ? typeProp.GetString() : null
                };

                return new EventInvite(constructedEvent, c.GetProperty("message").GetString());
            }).ToList();

            var invites = await Task.WhenAll(inviteTasks); // returns EventInvite[]
            var inviteList = invites.ToList();


            Console.WriteLine($"at bottom: name={displayName}, goal={weeklyGoal}, availability={availability != null}, calendar={userCalendar != null}, tasks={tasks.Count}, preferred={preferredTimes.Count}, invites={inviteList.Count}");

            return new UserInfo
            {
                DisplayName = displayName,
                WeeklyGoal = weeklyGoal,
                UserAvailability = availability,
                UserCalendar = userCalendar,
                TaskList = taskList,
                PreferredTimes = preferredTimes,
                InviteInbox = inviteList
            };
        }
    }
}