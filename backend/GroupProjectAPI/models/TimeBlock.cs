namespace backend.models;
using Google.Cloud.Firestore;

[FirestoreData]
public struct TimeBlock
{
    [FirestoreProperty(ConverterType = typeof(TimeOnlyConverter))]
    public TimeOnly StartTime { get; set; }
    [FirestoreProperty(ConverterType = typeof(TimeOnlyConverter))]
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

    public static List<TimeBlock> MergeOverlappingBlocks(List<TimeBlock> timeBlocks)
    {
        if (!timeBlocks.Any()) return timeBlocks;

        var sortedBlocks = timeBlocks.OrderBy(b => b.StartTime).ToList();
        var mergedBlocks = new List<TimeBlock>();
        var currentBlock = sortedBlocks[0];

        for (int i = 1; i < sortedBlocks.Count; i++)
        {
            if (TimeBlock.hasConflict(currentBlock, sortedBlocks[i]))
            {
                currentBlock = TimeBlock.mergeTimeBlock(currentBlock, sortedBlocks[i]);
            }
            else
            {
                mergedBlocks.Add(currentBlock);
                currentBlock = sortedBlocks[i];
            }
        }

        mergedBlocks.Add(currentBlock);
        return mergedBlocks;
    }

    public override bool Equals(object? obj)
    {
        if (obj is TimeBlock other)
        {
            return this.StartTime == other.StartTime && this.EndTime == other.EndTime;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, EndTime);
    }
}

public class TimeOnlyConverter : IFirestoreConverter<TimeOnly>
{
    public TimeOnly FromFirestore(object value)
    {
        // Firestore will give us a string like "09:00:00"
        return TimeOnly.Parse((string)value);
    }

    public object ToFirestore(TimeOnly time)
    {
        // Store it as an ISO-style string
        Console.WriteLine("IN FIRESTORE CONVERTER");
        return time.ToString("HH:mm:ss");
    }
}
