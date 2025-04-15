{
    public struct TimeBlock
    {
        public TimeBlock(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int getLength()
        {
            return (int)(EndTime - StartTime).TotalHours;
        }
    }
}