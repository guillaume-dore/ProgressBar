namespace ProgressBar;

/// <summary>
/// Provide an object to manage drawing and manipulating a console progress bar.
/// </summary>
public class ProgressBar : IDisposable, IProgress<double>
{
	private readonly string _text = "Progress";

	private readonly char _loadingIndicator = '█';

	private readonly char _loadingIndicatorEmpty = '░';

	private string? _optionalText;

	private readonly int _maxSteps = 100;

	private int _steps = 0;

	private bool _isStarted = true;

	/// <summary>
	/// Initializes a new instance of <see cref="ProgressBar"/> class.<br/>
	/// If <see cref="Console.IsOutputRedirected"/> is <c>True</c>, the progress bar will not be rendered.
	/// </summary>
	/// <param name="maxStep">Maximum step number allowed.</param>
	/// <param name="message">Message to display to the left part of the console progress bar.</param>
	/// <param name="start"><c>True</c> to start showing immediatly the progress, otherwise <c>False</c></param>
	public ProgressBar(int maxStep = 100, string message = "Progress", bool start = true)
	{
		this._maxSteps = maxStep;
		this._text = message;
		if (Console.IsOutputRedirected)
			this._isStarted = false;
		else
		{
			this._isStarted = start;
			Console.CursorVisible = !_isStarted;
		}
	}

	/// <summary>
	/// Progress percentage to display.
	/// </summary>
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

	/// <inheritdoc/>
	public void Report(double value)
	{
		if (value < 0 || value > 100)
			return;
		this.Percentage = value;
	}

	/// <inheritdoc cref="Report(double)"/>
	/// <param name="updatedText">Dynamic information text to update.</param>
	public void Report(double value, string updatedText)
	{
		if (value < 0 || value > 100)
			return;
		this._optionalText = updatedText;
		this.Report(value);
	}

	/// <summary>
	/// Start rendering the progress bar to the console output.
	/// </summary>
	public void Start()
	{
		if (this._isStarted || Console.IsOutputRedirected)
			return;

		this._isStarted = true;
		Console.CursorVisible = false;
		this.Render();
	}

	/// <summary>
	/// Stop rendering the progress bar to the console output.
	/// </summary>
	public void Stop()
	{
		if (!this._isStarted)
			return;

		this._isStarted = false;
		this.Unrender();
		Console.CursorVisible = true;
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Unrender();
		Console.CursorVisible = true;
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Writes the specified string value, followed by the current line terminator,
	/// to the standard output stream.<br/> Then redraw the progress bar to the console output.
	/// </summary>
	/// <param name="value">The value to write.</param>
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

	/// <summary>
	/// Render the progress bar to the console output.
	/// </summary>
	private void Render()
	{
		if (!this._isStarted)
			return;

		(int left, int top) = Console.GetCursorPosition();
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		DrawProgressBar();
		Console.SetCursorPosition(left, top);
	}

	/// <summary>
	/// Clear the progress bar from the console output.
	/// </summary>
	private void Unrender()
	{
		(int left, int top) = Console.GetCursorPosition();
		Console.SetCursorPosition(0, Console.BufferHeight - 1);
		Console.Write(new string(' ', Console.BufferWidth));
		Console.SetCursorPosition(left, top);
	}

	/// <summary>
	/// Draw the progress bar updated to the console output.
	/// </summary>
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
