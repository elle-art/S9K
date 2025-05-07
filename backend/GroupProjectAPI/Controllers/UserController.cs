using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using backend.models;
using System.Text.Json;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1; // if FireBaseCommunications or DBCommunications is here

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly FirestoreDb db = FirestoreDb.Create("the-scheduler-9000");

        [HttpPost("save")]
        public async Task<IActionResult> SaveUser([FromQuery] string uid, [FromBody] JsonElement rawUserData)
        {

            if (string.IsNullOrEmpty(uid))
                return BadRequest("UID is missing.");

            try
            {
                var converted = ConvertJsonElement(rawUserData);
                Console.Write(converted);
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
    }
}