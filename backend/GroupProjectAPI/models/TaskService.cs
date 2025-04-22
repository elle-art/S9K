namespace backend.models;

using Google.Cloud.Firestore;
public class TaskService
{

    //To-do: Task Creation logic
    public async Task<UserTask> AddTask (string name, DateTime? date, string status)
    {
        FirebaseCommunications db = new FirebaseCommunications();
        DocumentReference docRef = db.Collection("tasks").Document(name); // check collection name in FB
        UserTask task = new UserTask(name, date, status);

        await docRef.SetAsync(task);

        return task;
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