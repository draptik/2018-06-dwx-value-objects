using System;
using Examples.DateRangeDemo.V2;
using FluentAssertions;
using Xunit;

namespace Examples.Tests.DateRangeDemo.V2
{
    public class ZeitspanneTests
    {
        [Theory]
        [InlineData(2018, 1, 01, 2018, 04, 01, 2018, 1, 01, true)]
        [InlineData(2018, 1, 01, 2018, 04, 01, 2018, 5, 01, false)]
        [InlineData(2018, 1, 01, 2018, 04, 01, 2017, 9, 01, false)]
        public void Returns_correct_results(
            int fromYear,   int fromMonth,   int fromDay,
            int toYear,     int toMonth,     int toDay, 
            int actualYear, int actualMonth, int actualDay, 
            bool expected)
        {
            var from   = new DateTime(fromYear,   fromMonth,   fromDay);
            var to     = new DateTime(toYear,     toMonth,     toDay);
            var actual = new DateTime(actualYear, actualMonth, actualDay);
            var wasMitErstelltAm = new WasMitErstelltAm { ErstelltAm = actual };
            
            var sut = new Zeitspanne(from, to); // <--- testing Value Object

            sut.Umfasst(wasMitErstelltAm).Should().Be(expected);
        }

        private class WasMitErstelltAm : IHaveErstelltAm
        {
            public DateTime ErstelltAm { get; set; }
        }
    }
}