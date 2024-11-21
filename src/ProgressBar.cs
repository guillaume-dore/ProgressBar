namespace ProgressBar;

public class ProgressBar : IDisposable, IProgress<float>
{
	private readonly string _text = "Progress";

	private readonly char _loadingIndicator = '█';

	private readonly char _loadingIndicatorEmpty = '░';

	private string? _optionalText;

	private bool _isStarted;

	public ProgressBar(bool start = true)
	{
		if (Console.IsOutputRedirected)
			this._isStarted = false;
		else
		{
			this._isStarted = start;
			Console.CursorVisible = !_isStarted;
		}
	}

	public ProgressBar(string text, bool start = true) : this(start)
	{
		_text = text;
	}

	public ProgressBar(string text, string optionalText, bool start = true) : this(text, start)
	{
		_optionalText = optionalText;
	}

	public void Dispose()
	{
		this.Unrender();
		Console.CursorVisible = true;
		GC.SuppressFinalize(this);
	}

	public void Start()
	{
		if (this._isStarted || Console.IsOutputRedirected)
			return;

		this._isStarted = true;
		Console.CursorVisible = false;
		this.Render(10, _optionalText);
	}

	public void Stop()
	{
		if (!this._isStarted)
			return;

		this._isStarted = false;
		this.Unrender();
		Console.CursorVisible = true;
	}

	public void WriteLine(string? value)
	{
		int cursorTop = Console.GetCursorPosition().Top;
		if (cursorTop == Console.BufferHeight - 1 && this._isStarted)
		{
			Console.SetCursorPosition(0, Console.BufferHeight - 1);
			Console.Write(Environment.NewLine);
			Console.SetCursorPosition(0, cursorTop - 1);
			Console.Write(value?.PadRight(Console.BufferWidth));
			Console.SetCursorPosition(0, Console.BufferHeight - 1);
		}
		else
			Console.WriteLine(value);
	}

	public void Report(float value)
	{

	}

	public void Report(float value, string updatedText)
	{

	}

	public void Render(int i, string? optionalText = null)
	{
		if (!this._isStarted)
			return;

		_optionalText = optionalText;
		int topPosition = Console.GetCursorPosition().Top;
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		DrawProgressBar(i);
		Console.SetCursorPosition(0, topPosition);
	}

	private void Unrender()
	{
		(int left, int top) = Console.GetCursorPosition();

		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.Write(new string(' ', Console.BufferWidth));
		Console.SetCursorPosition(left, top);
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
