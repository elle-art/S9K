namespace backend.models;

using Google.Cloud.Firestore;

[FirestoreData]
public class UserTask
{
    [FirestoreProperty]
    public string taskName { get; set; }
    [FirestoreProperty]
    public DateTime? taskDate { get; set; }
    [FirestoreProperty]
    public bool taskStatus { get; set; }

    public UserTask(string taskName, DateTime? taskDate, bool taskStatus)
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