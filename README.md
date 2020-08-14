A simple regex-based parser for commands

Features:

- Parameter names can contain only alphanumeric characters, hyphens and underscores
- The type of a variable can be omitted, in that case the parser will fall back to the default
variable type
- Every parameter in a command is required, there's no notion of optionality
- Repeated parameter names are not allowed
- There's no real command format validation, be careful
- The parser is meant to be created once and be reused, so it's better to cache them
- Parameters are surrounded by `{{` and `}}` sequences

The parser has five built-in types: for int, long, double, bool and string types.

- int parser parses valid int values ranging from `int.MinValue` to `int.MaxValue`, zeroes in the 
prefix are not allowed (`01234` is not a valid int number)
- long parser parses valid int values ranging from `long.MinValue` to `long.MaxValue`, zeroes in
the prefix are not allowed (`01234` is not a valid long number)
- double parser parses double-looking strings but not all possible variations of double numbers. 
It parses a string with an optional sign (+/-) at the start and two numbers divided by a decimal
point. The left part does not allow prefix zeroes (`00.1234` is not a valid double number).
Double parser also doesn't validate the length of either integer or fractional part, so be careful.
- string parser parses a string that includes every non-whitespace character (i.e. linebreaks, 
whitespaces, tabs, etc. are not part of the parser string).

Usage example: 

```c#
var commandFormat = "/commandName {{argument1:bool}} {{argument2:long}} {{argument3}}";
var parser = CommandParserBuilder().CreateDefaultParser(commandFormat);

var result = commandParser.ParseCommand("/commandName false 123456789 qwerty&*(@#$_-123");

if (result.Successful)
{
    Console.WriteLine(result.Variables["argument1"]); // => "false"
    bool arg1 = result.Variables.Get<bool>("argument1"); // => false

    Console.WriteLine(result.Variables["argument2"]); // => "123456789"
    long arg2 = result.Variables.Get<long>("argument2"); // => 123456789

    // argument3 implicitly has "default" type since it was ommited in the command
    Console.WriteLine(result.Variables["argument3"]); // => "qwerty&*(@#$_-123"
    string arg3 = result.Variables.Get<string>("argument3"); // => qwerty&*(@#$_-123

    string arg3 = result.Variables.Get<string>("argument3"); // => throws InvalidOperationException
    string arg4 = result.Variables.Get<string>("argument4"); // => throws KeyNotFoundException
}
```

Manual parser creation:

```c#
var variableTypes = new Dictionary<string, IVariableType>
{
    { "string", new StringVariableType() },
    { "int", new IntVariableType() },
    { "long", new LongVariableType() },
    { "bool", new BoolVariableType() },
};
var parsers = new Dictionary<Type, IArgumentParser>
{
    { typeof(string), new StringParser() },
    { typeof(int), new IntParser() },
    { typeof(long), new LongParser() },
    { typeof(bool), new BoolParser() },
    { typeof(double), new DoubleParser() },
};
var defaultVariableType = new StringVariableType();
var commandFormat = "!commandName {{argument1:bool}} {{argument2:long}} {{argument3}}";

var defaultVariableType = new StringVariableType();

var commandParserOptions = new CommandParserOptions(
    commandFormat,
    variableTypes,
    parsers,
    defaultVariableType
)

var commandParser = new CommandParser(commandParserOptions);
var result = commandParser.ParseCommand("!commandName false 123456789 qwerty&*(@#$_-123");

if (result.Successful)
{
    Console.WriteLine(result.Variables["argument1"]); // => "false"
    bool arg1 = result.Variables.Get<bool>("argument1");

    Console.WriteLine(result.Variables["argument2"]); // => "123456789"
    long arg2 = result.Variables.Get<long>("argument2");
   
    // argument3 implicitly has "default" type since it was ommited in the command
    Console.WriteLine(result.Variables["argument3"]); // => "qwerty_-123"
    string arg2 = result.Variables.Get<string>("argument3");
}
```
