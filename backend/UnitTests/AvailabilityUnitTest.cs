using Xunit;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class AvailabilityServiceTests
{
    [Fact]
    public void EditAvailability_AddTimeBlock_Success()
    {
        // Arrange
        var availability = new Availability();
        var timeBlock = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("10:00"));

        // Act
        availability.EditAvailability(0, timeBlock, false);

        // Assert
        Assert.True(availability.HasTimeBlock(0, timeBlock));
    }

    [Fact]
    public void EditAvailability_DeleteTimeBlock_Success()
    {
        // Arrange
        var availability = new Availability();
        var timeBlock = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("10:00"));
        availability.EditAvailability(0, timeBlock, false);

        // Act
        availability.EditAvailability(0, timeBlock, true);

        // Assert
        Assert.False(availability.HasTimeBlock(0, timeBlock));
    }

    [Fact]
    public void EditAvailability_MergeOverlappingBlocks_Success()
    {
        // Arrange
        var availability = new Availability();
        var block1 = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("11:00"));
        var block2 = new TimeBlock(TimeOnly.Parse("10:00"), TimeOnly.Parse("12:00"));
        var expectedBlock = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("12:00"));

        // Act
        availability.EditAvailability(0, block1, false);
        availability.EditAvailability(0, block2, false);

        // Assert
        Assert.True(availability.HasTimeBlock(0, expectedBlock));
    }

    [Fact]
    public void EditAvailability_DeletePartialBlock_CreatesGap()
    {
        // Arrange
        var availability = new Availability();
        var initialBlock = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("12:00"));
        var deleteBlock = new TimeBlock(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00"));
        var expectedBlock1 = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("10:00"));
        var expectedBlock2 = new TimeBlock(TimeOnly.Parse("11:00"), TimeOnly.Parse("12:00"));

        // Act
        availability.EditAvailability(0, initialBlock, false);
        availability.EditAvailability(0, deleteBlock, true);

        // Assert
        Assert.True(availability.HasTimeBlock(0, expectedBlock1));
        Assert.True(availability.HasTimeBlock(0, expectedBlock2));
    }

    [Fact]
    public void EditAvailability_InvalidDayIndex_NoEffect()
    {
        // Arrange
        var availability = new Availability();
        var timeBlock = new TimeBlock(TimeOnly.Parse("09:00"), TimeOnly.Parse("10:00"));

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() =>
            availability.EditAvailability(7, timeBlock, false));
    }
}
