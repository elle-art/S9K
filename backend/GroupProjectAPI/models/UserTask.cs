using Google.Cloud.Firestore;
namespace Backend.Models
{
    [FirestoreData]
    public class UserTask
    {
        [FirestoreProperty]
        public required string taskName { get; set; }
        [FirestoreProperty]
        public DateTime? taskDate { get; set; }
        [FirestoreProperty]
        public required string taskStatus { get; set; }

        public UserTask(string taskName, DateTime? taskDate, string taskStatus)
        {
            this.taskName = taskName;

            //Tasks can be created with or without a date
            if (taskDate != null)
            {
                this.taskDate = taskDate;
            }

            this.taskStatus = taskStatus;
        }

        //Looks at a combo of the availability and current calendar in order to suggest a task time.
    }
}