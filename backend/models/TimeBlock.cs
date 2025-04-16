public struct TimeBlock
{
    public TimeBlock(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public int getLength()
    {
        return (int)(EndTime - StartTime).TotalHours;
    }
}