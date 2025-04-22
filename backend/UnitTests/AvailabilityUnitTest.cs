namespace UnitTests;
using Xunit;
using backend.models;

public class AvailabilityServiceTests
{
    [Fact]
    public void CreateAvailability_FBentryDoesNotExist_ReturnTrue()
    {
        //creating a new availability Service
        var availabilityService = new AvailabilityService();

        //Should return a new availability object (FBEntryExists being true returns null regardless, at time of writing)
        var result = availabilityService.CreateAvailability(FBEntryExists: false);

        //Assert that result exists and is of type Availability
        Assert.NotNull(result);
        Assert.IsType<Availability>(result);
    }

    [Fact]
    public void UpdateAvailability_()
    {
        var availabilityService = new AvailabilityService();
        var userAvailability = new Availability();
        var timeBlock1 = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("11:00"));
        var timeBlock2 = new TimeBlock(TimeOnly.Parse("10:00"), TimeOnly.Parse("10:30"));
        var timeBlock3 = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("10:00"));
        var timeBlock4 = new TimeBlock(TimeOnly.Parse("10:30"), TimeOnly.Parse("11:00"));
        int day = 1; // Example: Monday

        // Act
        availabilityService.UpdateAvailability(ref userAvailability, day, timeBlock1, isDelete: false);
        availabilityService.UpdateAvailability(ref userAvailability, day, timeBlock2, isDelete: true);

        // Assert
        Assert.NotNull(userAvailability);
        Assert.True(userAvailability.HasTimeBlock(day, timeBlock3));
        Assert.True(userAvailability.HasTimeBlock(day, timeBlock4));
    }

    [Fact]
    public void HasTimeBlock_ReturnsTrue_WhenMatchingBlockExists()
    {
        var schedule = new Availability();
        var block = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("11:00"));

        schedule.AddTimeBlock(1, block); // method to add a block
        bool result = schedule.HasTimeBlock(1, block);

        Assert.True(result);
    }

    [Fact]
    public void HasConflict_ReturnsTrue_WhenTimeBlocksOverlap()
    {
        var block1 = new TimeBlock(TimeOnly.Parse("10:30"), TimeOnly.Parse("11:00"));
        var block2 = new TimeBlock(TimeOnly.Parse("10:30"), TimeOnly.Parse("11:00"));

        var result = Availability.hasConflict(block1, block2);

        Assert.True(result);
    }
}
