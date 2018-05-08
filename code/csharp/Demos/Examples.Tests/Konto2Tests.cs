using System;
using Examples.Exceptions;
using FluentAssertions;
using Xunit;

namespace Examples.Tests
{
    public class Konto2Tests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Einzahlung_mit_ungueltigem_Geldwert_schmeisst_Exception(int geld)
        {
            var sut = new Konto2();
            Action action = () => sut.Einzahlen(new Geld2(geld));
            action.Should().Throw<InvalidGeld2ValueException>();
        }
    }
}
