using System;
using Examples.MoneyDemo.V1;
using FluentAssertions;
using Xunit;

namespace Examples.Tests.MoneyDemo.V1
{
    public class KontoTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        // [InlineData(0)] // <-- Problem?
        // [InlineData(-10)] // <-- Problem?
        public void Kontostand_ist_nach_Einzahlung_groesser_als_davor(int geld)
        {
            // Arrange
            var sut = new Konto(); // SUT: System Under Test

            // Act
            sut.Einzahlen(geld);
            
            // Assert
            sut.Kontostand.Should().BeGreaterThan(0);
        }
    }
}
