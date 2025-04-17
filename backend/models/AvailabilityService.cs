namespace backend.models;

public class AvailabilityService
{

    //To-Do: Firebase shenanigans to find if an existing availability is stored for this user (That search can be implemented in the controller)
    public Availability CreateAvailability(bool FBEntryExists = false)
    {
        if (FBEntryExists)
        {
            //aforementioned FireBase shenanigans go here

            //temp return to bypass error
            return null;
        }
        else
        {
            //To-do: Store this in the user info as userAvailability.
            return new Availability();
        }
    }

    public void UpdateAvailability(ref Availability userAvailability, int day, TimeBlock timeBlock, bool isDelete = false)
    {
        //Tentative implementation
        userAvailability.EditAvailability(day, timeBlock, isDelete);
    }

}