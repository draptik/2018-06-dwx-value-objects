using System;
using FluentAssertions;
using Xunit;

namespace Examples.Tests
{
    public class Konto1Tests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        // [InlineData(0)] // <-- Problem?
        // [InlineData(-10)] // <-- Problem?
        public void Kontostand_ist_nach_Einzahlung_groesser_als_davor(int geld)
        {
            // Arrange
            var sut = new Konto1(); // SUT: System Under Test

            // Act
            sut.Einzahlen(geld);
            
            // Assert
            sut.Kontostand.Should().BeGreaterThan(0);
        }
    }
}
