# Velentr.Logging
A simple and extensible logging library

# Installation
A nuget package is available: [Velentr.Logging](https://www.nuget.org/packages/Velentr.Logging/)

# Usage
Approach 1: Use a Logger directly
```
var logger = new ConsoleLogger("test_logger");
logger.Info("Hello World!");
logger.Update(); // optional if the logger is in LogMode.Instant mode
```

Approach 2: Use a LogManager
```
var logger = new ConsoleLogger("test_logger");
var manager = new LogManager();
manager.Info("test_logger", "Hello World!");
manager.Update(); // optional if all loggers are in LogMode.Instant mode
```

# Available Loggers
- AllLogger: Accepts a multitude of sub loggers as arguments and allows writing to all sub-loggers at the same time.
- ConsoleLogger: Logs to the Console. Accepts Color Markdown
- FileLogger: Logs to a file.

Additionally, adding a new logger is pretty easy! Just extend the base `Logger` class and implement the required methods. This can allow you to create loggers for use with databases, etc. in a straightforward means.

# ConsoleLogger Markdown
Adding the following commands to your message allows you to color specific parts of your message:
- `F`/`FORE`/`FOREGROUND`: Accepts the string name of any ConsoleColor color
- `B`/`BACK`/`BACKGROUND`: Accepts the string name of any ConsoleColor color

Notation Examples:
- `var msg = "Hello [f: BLUE][b: red]World!";`
- `var msg = "Hello [f: Blue][BACK: rEd]World![/] This text is back to normal!";`

Note: markdown is completely case-insensitive


# Future Plans
See list of issues under the Milestones: https://github.com/vonderborch/Velentr.Logging/milestones
