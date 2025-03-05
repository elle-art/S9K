public class Task {
    public string taskName {get; set;}
    public DateTime? taskDate {get; set;}
    public string taskStatus {get; set;}

    public Task() {
        // constructor/createTask
    }

    public void EditTask(string name, DateTime date, string status) {
        
    }

    public Availability SuggestTaskTime(Availability userAvailability) {
        return taskAvailability;
    }
}