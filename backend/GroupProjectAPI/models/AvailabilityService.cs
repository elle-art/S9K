namespace backend.models;
using Backend.Services;
using System.Text.Json;
public class AvailabilityService
{

    /// <summary>
    /// Gets an existing availability from firebase
    /// </summary>
    /// <param name="uid">user's ID</param>
    /// <returns></returns>
    public static async Task<Availability> GetAvailabilityAsync(string uid)
    {
        return await DBCommunications.GetObjectAsync<Availability>(uid, "Availability");
    }

    /// <summary>
    /// Create a new availability object and save it to firebase
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="schedule"></param>
    /// <returns></returns>
    public static async Task<Availability> CreateAvailabilityAsync(string uid, List<TimeBlock>[] schedule)
    {
        Availability availability = new Availability(schedule);

        await DBCommunications.SaveObjectAsync(uid, availability);

        return availability;
    }


    /// <summary>
    /// Updates the user availability and saves it to firestore
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="userAvailability"></param>
    /// <param name="day"></param>
    /// <param name="timeBlock"></param>
    /// <param name="isDelete"></param>
    /// <returns></returns>
    public static async Task UpdateAvailabilityAsync(string uid, Availability userAvailability, int day, TimeBlock timeBlock, bool isDelete = false)
    {
        userAvailability.EditAvailability(day, timeBlock, isDelete);

        await DBCommunications.SaveObjectAsync(uid, userAvailability);
    }

    public static Availability CreateObjFromJson(JsonElement.ArrayEnumerator raw)
    {
        var weeklySchedule = new List<TimeBlock>[7];
        for (int i = 0; i < 7; i++)
            weeklySchedule[i] = new List<TimeBlock>();

        foreach (var block in raw)
        {
            int day = block.GetProperty("day").GetInt32(); // 0–6
            var start = block.GetProperty("startTime").GetString();
            var end = block.GetProperty("endTime").GetString();

            if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                continue;

            weeklySchedule[day].Add(new TimeBlock
            {
                StartTime = TimeOnly.Parse(start),
                EndTime = TimeOnly.Parse(end)
            });
        }

        return new Availability(weeklySchedule);
    }
}