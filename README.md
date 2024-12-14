# CLI.ProgressBar [![NuGet](https://img.shields.io/nuget/v/CLI.ProgressBar.svg)](https://www.nuget.org/packages/CLI.ProgressBar/) [![NuGet](https://img.shields.io/nuget/dt/CLI.ProgressBar.svg)](https://www.nuget.org/packages/CLI.ProgressBar/) ![Issues](https://img.shields.io/github/issues/guillaume-dore/ProgressBar) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
A simple progress bar for Console applications, cross platform ready, built on top of .NET 8.0 without any external dependency.

## Setup

Install [CLI.ProgressBar](https://www.nuget.org/packages/CLI.ProgressBar/) package running the following command line:

```bash
dotnet add package CLI.ProgressBar
```

## Usage

In order to use the progress bar include the following namespace: 
```csharp 
using CliProgressBar;
```

***Default Progress Bar:***

![Basic Usage](https://raw.githubusercontent.com/guillaume-dore/ProgressBar/master/img/progress_simple.gif)

```csharp
using var progressBar = new ProgressBar();
for (int i = 0; i < 100; i++)
{
	progressBar.AddSteps(1);
	Thread.Sleep(100);
}
```

***Variant Progress Bar:*** 

![Variant Usage](https://raw.githubusercontent.com/guillaume-dore/ProgressBar/master/img/progress_variant.gif)

```csharp
using var progressBar = new ProgressBar(Layout.Unix);
for (int i = 0; i < 100; i++)
{
	progressBar.AddSteps(1);
	Thread.Sleep(100);
}
```

***Progress Bar with console output:***

![Output Usage](https://raw.githubusercontent.com/guillaume-dore/ProgressBar/master/img/progress_text.gif)

```csharp
using var progressBar = new ProgressBar();
for (int i = 0; i < 100; i++)
{
	progressBar.WriteLine($"Line {i + 1}. Console left: {Console.GetCursorPosition().Left}, Console top: {Console.GetCursorPosition().Top}, Buffer Height: {Console.BufferHeight}, Window Height: {Console.WindowHeight}");
	progressBar.Report(i / 100f, $"step {i} of 100");
	Thread.Sleep(100);
}
```

The progress bar come with more possibilities of customization, check the [sample](https://github.com/guillaume-dore/ProgressBar/blob/master/sample/Program.cs) project or the [Customization](##customization) section to see what is possible.

### Update progress:

The ```ProgressBar``` class provide two way to update is progression:

- By using the `AddSteps` method, which will increment the progress bar by the given number of steps:
```csharp
	progressBar.AddSteps(1);
```
- Or by using the `Report` method, which will set the progress bar to the given percentage expressed with a ```float``` value:
```csharp
	progressBar.Report(0.5f);
```

Both methods accepts an optional string parameter used to set a message to display next to the progress bar:
```csharp
	progressBar.Report(0.5f, "step 50 of 100");
	progressBar.AddSteps(1, "incremented by 1");
```

### Update or define text:

The ```ProgressBar``` class provide two methods to update the underlying layout texts:

```csharp
// Used to configure the main text displayed on the progress bar.
public void SetText(string text)
// Used to configure the additional text displayed on the progress bar, if null is passed the additional text will be hidden.
public void SetAdditionalText(string? text)
```

## Customization

The ```ProgressBar``` is highly customizable, you can define the progress bar behaviors and appearance during it's initialization.

### ProgressBar:

Property | Type | Default Value | Description |
:--------:|:----:|:-------------:|:-----------:|
layout | ```Layout``` |```Layout.Default```|The layout object is provided by the package and is used to configure the aspect of the progress bar. 2 default layouts are provided ```Layout.Default``` and ```Layout.Unix```. |
maxStep|```int```|```100```|Maximum step number allowed.|
start|```bool```|```True```|```True``` to start showing immediatly the progress, otherwise ```False```.|
redirectConsoleOutput|```bool```|```False```|Define if  output should be redirected. Enable the progress bar rendering to manage redrawing on ```Console.WriteLine(string)``` instructions. It redirect the Console output implementing an internal intermediate ```TextWriter```. |

### Layout:

The ```Layout``` object is used to configure the aspect of the progress bar. It is composed of 3 elements: a bar, a text and an additional text. 
The bar is the progression bar, the text is the principal text displayed on the progress bar and the additional text is a secondary optional text displayed on the progress bar.

Property | Type | Default Value | Description |
:--------:|:----:|:-------------:|:-----------:|
Bar | ```BarLayout``` |N/A*|Object used to define the bar appearance. |
Text|```Element<string>```|N/A*|Principal text content of the layout.|
AdditionalText|```Element<string>?```|```null```|Secondary text content of the layout.|

\* *Values are required*

### BarLayout:

The ```BarLayout``` object define the appearance of the progression bar.

Property | Type | Default Value | Description |
:--------:|:----:|:-------------:|:-----------:|
ProgressIndicator | ```Element<char>``` |N/A*| Fullfilled indicator character (can be ASCII character). |
PendingIndicator|```Element<char>```|N/A*| Remaining indicator character (can be ASCII character).|
Direction |```BarDirection```|```BarDirection.Forward```| Determine the position of the percentage relative to the progression bar. ```Forward``` is after the bar and ```Reverse``` is before. |
Position |```LayoutPosition```|```LayoutPosition.Center```|Position of the bar in the parent layout.|
BracketOptions |```BracketLayout?```|```null```|Define the brackets to display around the bar and/or percentage. If value is ```null``` there is no brackets displayed.|	

\* *Values are required*

You can define the desired appearance of the progress bar defining your own ```Layout``` like the following:

```csharp
var bar = new BarLayout(
	new Element<char>('█') { ForegroundColor = ConsoleColor.Green }, 
	new Element<char>('░') { ForegroundColor = ConsoleColor.DarkGreen }
) {
	Direction = BarDirection.Reverse,
	Position = LayoutPosition.Center,
	BracketOptions = BracketLayout.Percentage | BracketLayout.Bar
}

var layout = new Layout(bar, new Element<string>("Principal text"), new Element<string>("Secondary text"));
using var progressBar = new ProgressBar(layout);
// your logic here
```

## Contributing

Any contributions are welcome. :raised_hands:

if you've found a bug, or a possible improvment you would wish to bring to this project, feel free to open an issue or to create a pull request.

