namespace backend.models;
using Google.Cloud.Firestore;


[FirestoreData]
public class Availability
{
    //An array of integer tuple lists that represent the availability
    //of the given user.
    //[0-6] = [Sunday-Saturday]
    [FirestoreProperty]

    public List<TimeBlock>[] weeklySchedule { get; set; }

    public Availability()
    {
        weeklySchedule = new List<TimeBlock>[7];
    }


    public Availability(List<TimeBlock>[] list)
    {
        weeklySchedule = list;
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

    public bool HasTimeBlock(int day, TimeBlock block)
    {
        // Ensure the day is valid and the schedule for the day is not null
        if (day < 0 || day > 6 || weeklySchedule[day] == null)
            return false;

        // Iterate through the time blocks for the given day
        foreach (var tb in weeklySchedule[day])
        {
            if (tb.StartTime == block.StartTime && tb.EndTime == block.EndTime)
            {
                return true;
            }
        }

        // Return false if no matching time block is found
        return false;
    }

    public void AddTimeBlock(int day, TimeBlock blockToAdd)
    {
        if (weeklySchedule[day] == null)
            weeklySchedule[day] = new List<TimeBlock>();

        bool merged = false;

        for (int i = 0; i < weeklySchedule[day].Count; i++)
        {
            if (TimeBlock.hasConflict(weeklySchedule[day][i], blockToAdd))
            {
                // Merge the conflicting time blocks
                weeklySchedule[day][i] = TimeBlock.mergeTimeBlock(weeklySchedule[day][i], blockToAdd);
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
        if (weeklySchedule[day] == null) return;

        // Use TimeBlock's RemoveTimeFromBlocks method to handle the removal
        weeklySchedule[day] = TimeBlock.RemoveTimeBlock(weeklySchedule[day], blockToDelete);
    }
}