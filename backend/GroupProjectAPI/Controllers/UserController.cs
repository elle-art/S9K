using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using backend.models;
using System.Text.Json;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging; // Import the necessary namespace for ILogger

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly FirestoreDb db = FirestoreDb.Create("the-scheduler-9000");

        private readonly ILogger<UserController> _logger; // Declare the logger

        // Constructor to inject the ILogger
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger; // Assign the logger
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUser([FromQuery] string uid, [FromBody] JsonElement rawUserData)
        {
            if (string.IsNullOrEmpty(uid))
                return BadRequest("UID is missing.");

            try
            {
                var converted = ConvertJsonElement(rawUserData);
                await DBCommunications.SaveObjectAsync(uid, "UserInfo", converted);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
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

        private static object? ConvertJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var dict = new Dictionary<string, object?>();
                    foreach (var prop in element.EnumerateObject())
                        dict[prop.Name] = ConvertJsonElement(prop.Value);
                    return dict;

                case JsonValueKind.Array:
                    var list = new List<object?>();
                    foreach (var item in element.EnumerateArray())
                        list.Add(ConvertJsonElement(item));
                    return list;

                case JsonValueKind.String:
                    if (element.TryGetDateTime(out var dateTime))
                        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    return element.GetString();

                case JsonValueKind.Number:
                    if (element.TryGetInt64(out var l))
                        return l;
                    return element.GetDouble();

                case JsonValueKind.True:
                case JsonValueKind.False:
                    return element.GetBoolean();

                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                default:
                    return null;
            }
        }

        [HttpGet("check-username")]
        public async Task<IActionResult> CheckUsernameExists([FromQuery] string username)
        {

            if (string.IsNullOrEmpty(username))
                return BadRequest("Username is required.");

            try
            {

                var query = db.Collection("users").WhereEqualTo("displayName", username);
                var snapshot = await query.GetSnapshotAsync();

                if (snapshot.Count > 0)
                {
                    var doc = snapshot.Documents[0];

                    return Ok(new
                    {
                        success = true,
                        exists = true,
                        userId = doc.Id
                    });
                }


                return Ok(new { success = true, exists = false });
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("generate-event-time")]
        public async Task<ActionResult<(DateTime, TimeBlock)>> GenerateBestEventTime(
            string creatorUserId,
            DateTime dateOfEvent,
            int durationOfEvent,
            string eventName,
            string eventType,
            List<string> theListOfSelectedUserIDs)

        {
            try
            {
                // Step 1: Get UserInfo for each user
                var userInfoList = new List<UserInfo>();
                foreach (var uid in theListOfSelectedUserIDs)
                {
                    var userInfo = await UserInfoServices.GetUserInfo(uid);
                    if (userInfo != null)
                    {
                        userInfoList.Add(userInfo);
                    }
                }

                if (!userInfoList.Any())
                {
                    _logger.LogWarning("No valid user info found for event: {EventName}", eventName); // Added logging
                    return BadRequest("No valid user info found.");
                }

                // Step 2: Create an empty time block
                var emptyTimeBlock = new TimeBlock(TimeOnly.MinValue, TimeOnly.MinValue);

                // Step 3: Create the event
                string initiatingUserId = theListOfSelectedUserIDs[0]; // Use first user as creator (or pass this explicitly)
                var newEvent = await EventService.CreateEventAsync(
                    creatorUserId,
                    eventName,
                    dateOfEvent,
                    emptyTimeBlock,
                    eventType,
                    userInfoList
                );


                // Step 4: Generate the best time
                var (finalDateTime, finalTimeBlock) = EventService.GenerateEventTime(ref newEvent, durationOfEvent, dateOfEvent);

                _logger.LogInformation("Event time generated: {EventName}, Date: {FinalDateTime}, TimeBlock: {FinalTimeBlock}", eventName, finalDateTime, finalTimeBlock); // Added logging

                return Ok((finalDateTime, finalTimeBlock));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating event time for {EventName}", eventName); // Added logging
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
