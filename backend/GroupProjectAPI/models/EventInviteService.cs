namespace backend.models;

public class EventInviteService
{
    public void respondToInvite(ref Calendar userCal, bool RSVP, EventInvite curInvite)
    {
        if(RSVP)
        {
            //add event to userCal
        }
        //Remove the invite from the user's inbox
    }

    //To-Do: Sending invites to corresponding users in the DB     
    public void SendInvite(Event e, string userName){
        //Assumption: To get to the point of sending an invite, the 
        //program would have to get past the SearchForUser check that
        //Shows if the desired user is actually in the DB.

        //Outbound invites do not need to be stored locally, and can be discarded
        //from the program once they are succesfully stored in the DB.
    }

    
}