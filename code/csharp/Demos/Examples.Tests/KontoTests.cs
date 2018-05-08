using System;
using FluentAssertions;
using Xunit;

namespace Examples.Tests
{
    public class KontoTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(-10)]
        public void Kontostand_ist_nach_Einzahlung_groesser_als_davor(int betrag)
        {
            // Arrange
            var konto = new Konto();
            var before = konto.Kontostand;

            // Act
            konto.Einzahlen(betrag);
            
            // Assert
            konto.Kontostand.Should().BeGreaterThan(before);
        }
    }
}
