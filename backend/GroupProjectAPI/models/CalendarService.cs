using Backend.Services;

namespace backend.models;

public class CalendarService
{

    private static Calendar _calendar = new Calendar();

    //To-do: National Holiday retrieval logic
    public void retrieveNationalHolidays()
    {

    }

    //To-do: ICS Parsing
    //To-do: Event creation from parsed ICS files
    public void importCalendar(ref Calendar c)
    {

    }


    /// <summary>
    /// Saves the calendar to firebase
    /// </summary>
    /// <param name="uid">the user's id</param>
    /// <returns></returns>
    public static async Task SaveCalendarAsync(string uid)
    {
        await DBCommunications.SaveObjectAsync(uid, _calendar);
    }

    /// <summary>
    /// Adds an event to the calendar and sends it to firebase
    /// </summary>
    /// <param name="uid">user id</param>
    /// <param name="userEvent">event to be added</param>
    /// <returns></returns>
    public static async Task AddEventToCalendar(string uid, Event newEvent)
    {

        // TODO: handle event conflict logic
        _calendar.events.Add(newEvent);
        await SaveCalendarAsync(uid);
    }

    public static List<List<TimeBlock>> GenerateFreeTime(Calendar userCal, Availability userAvailability)
    {
        // Initialize freeTime with empty lists for all 7 days
        List<List<TimeBlock>> freeTime = new List<List<TimeBlock>>();
        for (int i = 0; i < 7; i++)
        {
            freeTime.Add(new List<TimeBlock>());
        }

        // Get today's day of week (6 for Saturday)
        int todayDayOfWeek = (int)DateTime.Today.DayOfWeek;

        // Copy availability for today and the next 6 days
        for (int i = 0; i < 7; i++)
        {
            // Calculate which day in weeklySchedule to use (wrapping around to beginning if needed)
            int sourceIndex = (todayDayOfWeek + i) % 7;

            if (userAvailability.weeklySchedule[sourceIndex] != null)
            {
                foreach (var timeBlock in userAvailability.weeklySchedule[sourceIndex])
                {
                    freeTime[i].Add(new TimeBlock(timeBlock.StartTime, timeBlock.EndTime));
                }
            }

            // Remove time blocks that overlap with events in the calendar for this day
            foreach (var userEvent in userCal.events)
            {
                if ((int)userEvent.EventDate.DayOfWeek == sourceIndex)
                {
                    freeTime[i] = TimeBlock.RemoveTimeBlock(freeTime[i], userEvent.EventTimeBlock);
                }
            }
        }

        return freeTime;
    }

    public static List<List<TimeBlock>> GenerateSchedule(Calendar userCal)
    {
        // Initialize busyTime with empty lists for all 7 days
        List<List<TimeBlock>> busyTime = new List<List<TimeBlock>>();
        for (int i = 0; i < 7; i++)
        {
            busyTime.Add(new List<TimeBlock>());
        }

        // Get today's day of week (6 for Saturday)
        int todayDayOfWeek = (int)DateTime.Today.DayOfWeek;

        // Iterate through the events in the calendar
        foreach (var userEvent in userCal.events)
        {
            int eventDayOfWeek = (int)userEvent.EventDate.DayOfWeek;
            int dayOffset = (eventDayOfWeek - todayDayOfWeek + 7) % 7;

            // Only consider events within the next 7 days
            if (dayOffset < 7)
            {
                busyTime[dayOffset].Add(userEvent.EventTimeBlock);
            }
        }

        return busyTime;
    }
    
}