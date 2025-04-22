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

    public static TimeBlock mergeTimeBlock(TimeBlock initialTimeBlock, TimeBlock compTimeBlock)
    {
        TimeBlock mergedTimeBlock = new TimeBlock(initialTimeBlock.StartTime, initialTimeBlock.EndTime);

        if (initialTimeBlock.StartTime > compTimeBlock.StartTime)
            mergedTimeBlock.StartTime = compTimeBlock.StartTime;

        if (initialTimeBlock.EndTime < compTimeBlock.EndTime)
            mergedTimeBlock.EndTime = compTimeBlock.EndTime;

        return mergedTimeBlock;
    }

    public static bool hasConflict(TimeBlock timeBlock1, TimeBlock timeBlock2)
    {
        return timeBlock1.StartTime < timeBlock2.EndTime && timeBlock1.EndTime > timeBlock2.StartTime;
    }
}