namespace Backend.Models;
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

    public static List<TimeBlock> RemoveTimeBlock(List<TimeBlock> blocks, TimeBlock blockToRemove)
    {
        var result = new List<TimeBlock>();

        foreach (var block in blocks)
        {
            if (!hasConflict(block, blockToRemove))
            {
                // If no overlap, keep the original block
                result.Add(block);
                continue;
            }

            // Case 1: blockToRemove fully overlaps the current block
            if (blockToRemove.StartTime <= block.StartTime && blockToRemove.EndTime >= block.EndTime)
            {
                continue; // Skip this block entirely
            }

            // Case 2: blockToRemove overlaps the start of the current block
            if (blockToRemove.StartTime <= block.StartTime && blockToRemove.EndTime < block.EndTime)
            {
                result.Add(new TimeBlock(blockToRemove.EndTime, block.EndTime));
                continue;
            }

            // Case 3: blockToRemove overlaps the end of the current block
            if (blockToRemove.StartTime > block.StartTime && blockToRemove.EndTime >= block.EndTime)
            {
                result.Add(new TimeBlock(block.StartTime, blockToRemove.StartTime));
                continue;
            }

            // Case 4: blockToRemove is in the middle of the current block (splitting it)
            if (blockToRemove.StartTime > block.StartTime && blockToRemove.EndTime < block.EndTime)
            {
                result.Add(new TimeBlock(block.StartTime, blockToRemove.StartTime));
                result.Add(new TimeBlock(blockToRemove.EndTime, block.EndTime));
            }
        }

        return result;
    }

    public override bool Equals(object? obj)
    {
        if (obj is TimeBlock other)
        {
            return this.StartTime == other.StartTime && this.EndTime == other.EndTime;
        }
        return false;
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
