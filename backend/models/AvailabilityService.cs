using Google.Cloud.Firestore;

public class AvailabilityService
{

    //To-Do: Firebase shenanigans to find if an existing availability is stored for this user (That search can be implemented in the controller)
    public async Task<Availability> CreateAvailability(List<TimeBlock>[] schedule, bool FBEntryExists = false)
    {
        FirebaseCommunications db = new FirebaseCommunications();

        if (FBEntryExists)
        {
            //aforementioned FireBase shenanigans go here

            //temp return to bypass error
            return null;
        }
        else
        {
            //To-do: Store this in the user info as userAvailability.
            DocumentReference docRef = db.Collection("availabilities").Document('a'); // check collection name in FB + needs id/name
            Availability availability = new Availability
            {
                weeklySchedule = schedule
            };

            await docRef.SetAsync(availability);

            return availability;
        }
    }

    public void UpdateAvailability(ref Availability userAvailability, int day, TimeBlock timeBlock, bool isDelete = false)
    {
        //Tentative implementation
        userAvailability.EditAvailability(day, timeBlock, isDelete);
    }

}