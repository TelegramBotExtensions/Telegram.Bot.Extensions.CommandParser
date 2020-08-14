using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Telegram.Bot.Extensions.CommandParser.Tests
{
    public class ParserTests
    {
        [Fact]
        public void Should_Parse_Command()
        {
            var commandParser = CommandParserBuilder.CreateDefaultParser(
                "!name {arg1:int} {arg2:bool}"
            );

            var result = commandParser.ParseCommand("!name 123 true");

            Assert.True(result.Successful);
            Assert.Throws<KeyNotFoundException>(() => result.Variables.Get<int>("arg3"));
            Assert.Throws<InvalidOperationException>(() => result.Variables.Get<bool>("arg1"));

            var arg1 = result.Variables.Get<int>("arg1");
            var arg2 = result.Variables.Get<bool>("arg2");
            Assert.Equal(123, arg1);
            Assert.True(arg2);
        }

        [Theory]
        [InlineData("0.1", "double", typeof(double))]
        [InlineData("-0.1234", "double", typeof(double))]
        [InlineData("-23.456", "double", typeof(double))]
        [InlineData("23.456", "double", typeof(double))]
        [InlineData("1", "int", typeof(int))]
        [InlineData("-1234567890", "long", typeof(long))]
        [InlineData("true", "bool", typeof(bool))]
        [InlineData("false", "bool", typeof(bool))]
        [InlineData("qwe123;*&^#@-_=+", "string", typeof(string))]
        public void Should_Parse_Arguments(string parameterValue, string parameterType, Type runtimeType)
        {
            var commandParser = CommandParserBuilder.CreateDefaultParser(
                $"/test {{arg1:{parameterType}}}"
            );

            var command = $"/test {parameterValue}";

            var result = commandParser.ParseCommand(command);
            Assert.True(result.Successful);

            var arg1 = result.Variables["arg1"];

            var methodInfo = result.Variables.GetType()
                .GetMethod("Get", BindingFlags.Instance | BindingFlags.Public)?
                .MakeGenericMethod(runtimeType);

            var parsedValue = methodInfo?.Invoke(result.Variables, new object[] {"arg1", null});

            Assert.NotNull(parsedValue);
            Assert.IsType(runtimeType, parsedValue);
            Assert.Equal(parameterValue, arg1);
        }

        [Fact]
        public void Should_Throw_On_When_Parsed_With_Invalid_Type()
        {
            var commandParser = CommandParserBuilder.CreateDefaultParser("/test {{arg1:int}}");
            var command = "/test 1123";
            var result = commandParser.ParseCommand(command);

            Assert.True(result.Successful);
            Assert.Throws<InvalidOperationException>(() => result.Variables.Get<bool>("arg1"));
        }

        [Fact]
        public void Should_Throw_On_When_Parsed_Missing_Variable()
        {
            var commandParser = CommandParserBuilder.CreateDefaultParser("/test {{arg1:int}}");
            var command = "/test 1123";
            var result = commandParser.ParseCommand(command);

            Assert.True(result.Successful);
            Assert.Throws<KeyNotFoundException>(() => result.Variables.Get<bool>("arg2"));
        }

        [Fact]
        public void Should_Throw_On_Invalid_Command_Pattern()
        {
            var exception = Assert.Throws<InvalidOperationException>(
                () => CommandParserBuilder.CreateDefaultParser("/test {{arg1:invalid}}")
            );

            Assert.Equal("Invalid variable type 'invalid'", exception.Message);
        }
    }
}
