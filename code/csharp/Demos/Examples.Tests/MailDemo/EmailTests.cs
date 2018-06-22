using System;
using Examples.MailDemo;
using FluentAssertions;
using Xunit;


namespace Examples.Tests.MailDemo
{
    public class EmailTests
    {
        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abc")]
        [InlineData("a@a@a")]
        [InlineData("@abc")]
        public void Creating_Email_should_throw_when_given_invalid_input(string invalidInput)
        {
            Action action = () => new Email(invalidInput);
            action.Should().Throw<InvalidEmailException>();
        }
    }
}