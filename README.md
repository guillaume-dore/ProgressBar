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

The progress bar come with many possibilities of personalization, check the [sample](https://github.com/guillaume-dore/ProgressBar/blob/master/sample/Program.cs) project to see what is possible.

### Examples:

#### Default Progress Bar

![Basic Usage](https://raw.githubusercontent.com/guillaume-dore/ProgressBar/master/img/progress_simple.gif)

```csharp
using var progressBar = new ProgressBar();
for (int i = 0; i < 100; i++)
{
	progressBar.AddSteps(1);
	Thread.Sleep(100);
}
```

#### Variant Progress Bar

![Variant Usage](https://raw.githubusercontent.com/guillaume-dore/ProgressBar/master/img/progress_variant.gif)

```csharp
using var progressBar = new ProgressBar(Layout.Unix);
for (int i = 0; i < 100; i++)
{
	progressBar.AddSteps(1);
	Thread.Sleep(100);
}
```

#### Progress Bar with console output

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

### Configuration:

#### Constructor

The ```ProgressBar``` is highly customizable, you can set the following properties during the object initialization to adapt it to your needs:

Parameter | Type | Default Value | Description |
:--------:|:----:|:-------------:|:-----------:|
layout | **Layout** |```Layout.Default```|The layout object is provided by the package and is used to configure the aspect of the progress bar. 2 default layouts are provided ```Layout.Default``` and ```Layout.Unix```. |
maxStep|**int**|```100```|Maximum step number allowed.|
start|**bool**|```True```|```True``` to start showing immediatly the progress, otherwise ```False```.|
redirectConsoleOutput|**bool**|```False```|Define if  output should be redirected. Enable the progress bar rendering to manage redrawing on ```Console.WriteLine(string)``` instructions. It redirect the Console output implementing an internal intermediate ```TextWriter```. |

#### Layout

TODO

## Contributing

TODO

