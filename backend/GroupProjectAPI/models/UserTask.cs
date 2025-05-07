namespace Backend.Models;

using Google.Cloud.Firestore;

[FirestoreData]
public class UserTask
{
    [FirestoreProperty]
    public string TaskName { get; set; }
    [FirestoreProperty]
    public DateTime? TaskDate { get; set; }
    [FirestoreProperty]
    public bool TaskStatus { get; set; }

    public UserTask() {
        TaskName = "";
        TaskStatus = false;
    }

    public UserTask(string taskName, DateTime? taskDate, bool taskStatus)
    {
        this.TaskName = taskName;

        //Tasks can be created with or without a date
        if (taskDate != null)
        {
            this.TaskDate = taskDate;
        }

        this.TaskStatus = taskStatus;
    }

    //Looks at a combo of the availability and current calendar in order to suggest a task time.
}