using Google.Cloud.Firestore;

namespace Backend.Models
{
    [FirestoreData]
    public class Calendar
    {
        [FirestoreProperty]

        public List<Event> events { get; set; }

        public Calendar()
        {
            // constructor
        }
    }
}