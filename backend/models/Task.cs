public class Task
{
    public string taskName { get; set; }
    public DateTime? taskDate { get; set; }
    public string taskStatus { get; set; }

    public Task(string taskName, DateTime? taskDate, string taskStatus)
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