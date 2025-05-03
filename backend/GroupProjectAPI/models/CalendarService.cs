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
    public static async Task AddEventToCalendar(string uid, Event newEvent) {

        // TODO: handle event conflict logic
        _calendar.events.Add(newEvent);
        await SaveCalendarAsync(uid);
    }

    
}