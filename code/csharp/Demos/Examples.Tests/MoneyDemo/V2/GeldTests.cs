using System;
using Examples.MoneyDemo;
using Examples.MoneyDemo.V2;
using FluentAssertions;
using Xunit;

namespace Examples.Tests.MoneyDemo.V2
{
    public class GeldTests
    {
        [Fact]
        public void Geld_schmeisst_wenn_Betrag_zu_gross()
        {
            var max = Int32.MaxValue;

            Action action = () => new Geld(max + 1);
            
            action.Should().Throw<InvalidGeldValueException>();
        }
    }
}