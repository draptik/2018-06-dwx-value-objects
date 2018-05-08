using System;
using Examples.Exceptions;
using FluentAssertions;
using Xunit;

namespace Examples.Tests
{
    public class Geld2Tests
    {
        [Fact]
        public void Geld2_schmeisst_wenn_Betrag_zu_gross()
        {
            var max = Int32.MaxValue;

            Action action = () => new Geld2(max + 1);
            
            action.Should().Throw<InvalidGeld2ValueException>();
        }
    }
}