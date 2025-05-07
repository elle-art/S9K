using Xunit;
using Backend.Models;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class RetrieveNationalHolidaysTest
    {
        [Fact]
        public void TestRetrieveNationalHolidays()
        {
            // Arrange
            var calendarService = new CalendarService();

            // Act
            var holidays = calendarService.retrieveNationalHolidays();

            // Assert
            Assert.NotNull(holidays); // Holidays list should not be null
            Assert.NotEmpty(holidays); // Holidays list should contain at least one holiday
            Assert.All(holidays, h => Assert.True(h >= DateTime.Today, "All holidays should be in the future."));
        }
    }
}
