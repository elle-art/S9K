using Google.Cloud.Firestore;

[FirestoreData]
public class Availability
{
    //An array of integer tuple lists that represent the availability
    //of the given user.
    //[0-6] = [Monday-Sunday]
    [FirestoreProperty]

    private List<TimeBlock>[] weeklySchedule { get; set; }

    public Availability()
    {
        weeklySchedule = new List<TimeBlock>[7];
    }

    public void EditAvailability(int day, TimeBlock block, bool isDelete = false)
    {
        if (isDelete)
        {
            DeleteTimeBlock(day, block);
            return;
        }

        // Add the time block to the schedule
        AddTimeBlock(day, block);
    }

    private static TimeBlock mergeTimeBlock(TimeBlock initialTimeBlock, TimeBlock compTimeBlock)
    {
        TimeBlock mergedTimeBlock = new TimeBlock(initialTimeBlock.StartTime, initialTimeBlock.EndTime);

        if (initialTimeBlock.StartTime > compTimeBlock.StartTime)
            mergedTimeBlock.StartTime = compTimeBlock.StartTime;

        if (initialTimeBlock.EndTime < compTimeBlock.EndTime)
            mergedTimeBlock.EndTime = compTimeBlock.EndTime;

        return mergedTimeBlock;
    }

    private void AddTimeBlock(int day, TimeBlock blockToAdd)
    {
        if (weeklySchedule[day] == null)
            weeklySchedule[day] = new List<TimeBlock>();

        bool merged = false;

        for (int i = 0; i < weeklySchedule[day].Count; i++)
        {
            if (hasConflict(weeklySchedule[day][i], blockToAdd))
            {
                // Merge the conflicting time blocks
                weeklySchedule[day][i] = mergeTimeBlock(weeklySchedule[day][i], blockToAdd);
                merged = true;
                break;
            }
        }

        if (!merged)
        {
            // Add the new time block if no conflicts were found
            weeklySchedule[day].Add(blockToAdd);
        }
    }

    private void DeleteTimeBlock(int day, TimeBlock blockToDelete)
    {
        // Find and remove the matching time block
        for (int i = 0; i < weeklySchedule[day].Count; i++)
        {
            var currentBlock = weeklySchedule[day][i];

            // Check if the blockToDelete overlaps with the current block
            if (hasConflict(currentBlock, blockToDelete))
            {
                // Case 1: blockToDelete fully overlaps the current block
                if (blockToDelete.StartTime <= currentBlock.StartTime && blockToDelete.EndTime >= currentBlock.EndTime)
                {
                    weeklySchedule[day].RemoveAt(i);
                    i--; // Adjust index after removal
                    continue;
                }

                // Case 2: blockToDelete overlaps the start of the current block
                if (blockToDelete.StartTime <= currentBlock.StartTime && blockToDelete.EndTime < currentBlock.EndTime)
                {
                    currentBlock.StartTime = blockToDelete.EndTime;
                    weeklySchedule[day][i] = currentBlock;
                    return;
                }

                // Case 3: blockToDelete overlaps the end of the current block
                if (blockToDelete.StartTime > currentBlock.StartTime && blockToDelete.EndTime >= currentBlock.EndTime)
                {
                    currentBlock.EndTime = blockToDelete.StartTime;
                    weeklySchedule[day][i] = currentBlock;
                    return;
                }

                // Case 4: blockToDelete is in the middle of the current block (splitting it)
                if (blockToDelete.StartTime > currentBlock.StartTime && blockToDelete.EndTime < currentBlock.EndTime)
                {
                    // Split the current block into two
                    var newBlock = new TimeBlock(blockToDelete.EndTime, currentBlock.EndTime);
                    currentBlock.EndTime = blockToDelete.StartTime;

                    weeklySchedule[day][i] = currentBlock;
                    weeklySchedule[day].Insert(i + 1, newBlock);
                    return;
                }
            }
        }
    }

    private static bool hasConflict(TimeBlock timeBlock1, TimeBlock timeBlock2)
    {
        return timeBlock1.StartTime < timeBlock2.EndTime && timeBlock1.EndTime > timeBlock2.StartTime;
    }
}