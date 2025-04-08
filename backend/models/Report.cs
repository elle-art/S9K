public class Report {
    public DateTime reportDate {get; set;}
    public UserInfo userData {get; set;}
    public List<Task> completedTasks {get; set;}

    //Subject to change
    public List<Event> eventData {get; set;}

    public Report() {
        // constructor/createTask
    }
}