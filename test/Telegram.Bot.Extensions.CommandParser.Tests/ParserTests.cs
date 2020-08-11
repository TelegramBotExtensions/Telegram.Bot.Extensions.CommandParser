using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Telegram.Bot.Extensions.CommandParser.Tests
{
    public class ParserTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ParserTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

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
            _testOutputHelper.WriteLine(command);
            _testOutputHelper.WriteLine(commandParser.Options.CommandFormat);

            var result = commandParser.ParseCommand(command);
            Assert.True(result.Successful);

            var arg1 = result.Variables["arg1"];

            var methodInfo = result.Variables.GetType()
                .GetMethod("Get", BindingFlags.Instance | BindingFlags.Public)?
                .MakeGenericMethod(runtimeType);

            var parsedValue = methodInfo?.Invoke(result.Variables, new [] {"arg1", null});

            Assert.NotNull(parsedValue);
            Assert.IsType(runtimeType, parsedValue);

            _testOutputHelper.WriteLine(arg1);
            Assert.Equal(parameterValue, arg1);
        }
    }
}
