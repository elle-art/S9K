public class TaskService
{

    //To-do: Task Creation logic
    public Task addTask (string name, DateTime? date, string status)
    {
        return null;
    }

    //To-do: Task time Generation logic
    public TimeBlock GenerateTaskTime(Availability userAvailability, Calendar userCalendar)
    {
        return new TimeBlock(TimeOnly.MinValue, TimeOnly.MinValue);
    }

    //Task Editing logic
    public void EditTask(string name, DateTime date, string status)
    {

    }
}