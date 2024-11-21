namespace ProgressBar;

public class ProgressBar : IDisposable, IProgress<float>
{
	private readonly string _text = "Progress";

	private readonly char _loadingIndicator = '█';

	private readonly char _loadingIndicatorEmpty = '░';

	private string? _optionalText;

	private bool _isStarted;

	private int _steps = 0;

	private int _maxSteps = 100;

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

	public double Percentage
	{
		get => (100.0 / this._maxSteps) * this._steps;
		set
		{
			if (value < 0 || value > 100)
				return;
			this._steps = (int)Math.Ceiling(value / 100.0 * this._maxSteps);
			this.Render();
		}
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
		this.Render();
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
		int cursorTop = Console.CursorTop;
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
		Render();
	}

	public void Report(float value)
	{

	}

	public void Report(float value, string updatedText)
	{

	}

	private void Render()
	{
		if (!this._isStarted)
			return;

		(int left, int top) = Console.GetCursorPosition();
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		DrawProgressBar();
		Console.SetCursorPosition(left, top);
	}

	private void Unrender()
	{
		(int left, int top) = Console.GetCursorPosition();
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.Write(new string(' ', Console.BufferWidth));
		Console.SetCursorPosition(left, top);
	}

	private void DrawProgressBar()
	{
		string progressStart = _text + $": [{this.Percentage:00}%] ";
		string progressEnd = " " + _optionalText;
		int progressTotalWidth = Console.BufferWidth - (progressStart.Length + progressEnd.Length);
		string progressCompleted = new string(_loadingIndicator, (int)Math.Ceiling(this.Percentage / 100.0 * progressTotalWidth));
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
