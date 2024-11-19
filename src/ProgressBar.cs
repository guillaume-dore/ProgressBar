namespace ProgressBar;

public class ProgressBar : IDisposable
{
	private string _text = "Progress";

	private char _loadingIndicator = '█';

	private char _loadingIndicatorEmpty = '░';

	private string? _optionalText;

	public ProgressBar()
	{
		Console.CursorVisible = false;
	}

	public ProgressBar(string text, string? optionalText = null) : this()
	{
		_text = text;
		_optionalText = optionalText;
	}

	public void Dispose()
	{
		Console.CursorVisible = true;
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.Write(new string(' ', Console.BufferWidth));
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		GC.SuppressFinalize(this);
	}

	public void WriteLine(string? value)
	{
		if (value == null)
			return;

		int valueTop = Console.GetCursorPosition().Top;
		if (valueTop == Console.BufferHeight - 1)
		{
			Console.SetCursorPosition(0, Console.BufferHeight - 1);
			Console.Write(Environment.NewLine);
			var cursorPosition = Console.GetCursorPosition();
			Console.SetCursorPosition(0, valueTop - 1);
			Console.Write(value.PadRight(Console.BufferWidth));
		}
		else
			Console.WriteLine(value);
	}

	public void Render(int i, string? optionalText = null)
	{
		_optionalText = optionalText;
		int topPosition = Console.GetCursorPosition().Top;
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		DrawProgressBar(i);
		Console.SetCursorPosition(0, topPosition);
	}

	public void DrawProgressBar(int i)
	{
		var percentage = (int)Math.Ceiling(i / 50.0 * 50);
		string progressStart = _text + $": [{percentage:00}%] ";
		string progressEnd = " " + _optionalText;
		int progressTotalWidth = Console.BufferWidth - (progressStart.Length + progressEnd.Length);
		string progressCompleted = new string(_loadingIndicator, (int)Math.Ceiling(percentage / 100.0 * progressTotalWidth));
		string progressUndone = new string(_loadingIndicatorEmpty, progressTotalWidth - progressCompleted.Length);

		Console.Write(progressStart);
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write(progressCompleted);
		Console.ForegroundColor = ConsoleColor.DarkGreen;
		Console.Write(progressUndone);
		Console.ResetColor();
		Console.Write(progressEnd);
	}
}
