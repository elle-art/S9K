namespace backend.models;

using Google.Cloud.Firestore;

[FirestoreData]
public struct TimeBlock
{
    [FirestoreProperty]
    public TimeOnly StartTime { get; set; }
    [FirestoreProperty]
    public TimeOnly EndTime { get; set; }

    public TimeBlock(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public int getLength()
    {
        return (int)(EndTime - StartTime).TotalHours;
    }
}