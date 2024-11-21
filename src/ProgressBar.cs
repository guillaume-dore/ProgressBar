namespace ProgressBar;

/// <summary>
/// Provide an object to manage drawing and manipulating a console progress bar.
/// </summary>
public class ProgressBar : IDisposable, IProgress<double>
{
	private readonly Layout _layout = Layout.Default;

	private readonly int _maxSteps = 100;

	private int _steps = 0;

	private bool _isStarted = true;

	/// <summary>
	/// Initializes a new instance of <see cref="ProgressBar"/> class.<br/>
	/// If <see cref="Console.IsOutputRedirected"/> is <c>True</c>, the progress bar will not be rendered.
	/// </summary>
	/// <param name="maxStep">Maximum step number allowed.</param>
	/// <param name="start"><c>True</c> to start showing immediatly the progress, otherwise <c>False</c></param>
	public ProgressBar(int maxStep = 100, bool start = true)
	{
		this._maxSteps = maxStep;
		if (Console.IsOutputRedirected)
			this._isStarted = false;
		else
		{
			this._isStarted = start;
			Console.CursorVisible = !_isStarted;
		}
	}

	/// <summary>
	/// Initializes a new instance of <see cref="ProgressBar"/> class.<br/>
	/// If <see cref="Console.IsOutputRedirected"/> is <c>True</c>, the progress bar will not be rendered.
	/// </summary>
	/// <param name="layout">Progress bar style.</param>
	/// <param name="maxStep">Maximum step number allowed.</param>
	/// <param name="start"><c>True</c> to start showing immediatly the progress, otherwise <c>False</c></param>
	public ProgressBar(Layout layout, int maxStep = 100, bool start = true) : this(maxStep, start)
	{
		this._layout = layout;
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
	/// <param name="value"><inheritdoc cref="Report(double)" path="=/param[@name='value']"/></param>
	/// <param name="updatedText">Dynamic information text to update.</param>
	public void Report(double value, string updatedText)
	{
		if (value < 0 || value > 100)
			return;
		this._layout.AdditionalText = updatedText;
		this.Report(value);
	}

	/// <summary>
	/// Add steps to the progress bar.
	/// </summary>
	/// <param name="steps">Number to steps to add.</param>
	/// <param name="updatedText">Optional text displayed to change.</param>
	public void AddSteps(int steps, string? updatedText = null)
	{
		int totalSteps = this._steps + steps;
		if (totalSteps < 0 || totalSteps > this._maxSteps)
			return;

		if (updatedText != null)
			this._layout.AdditionalText = updatedText;

		this._steps = totalSteps;
		this.Render();
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
			// If cursor near of the end of the buffer, keep an empty line to avoid flickering.
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
		string progressStart = this._layout.Text + $": [{this.Percentage:00}%] ";
		string progressEnd = " " + this._layout.AdditionalText;
		int progressTotalWidth = Console.BufferWidth - (progressStart.Length + progressEnd.Length);
		string progressCompleted = new string(this._layout.ProgressIndicator.Value, (int)Math.Ceiling(this.Percentage / 100.0 * progressTotalWidth));
		string progressUndone = new string(this._layout.PendingIndicator.Value, progressTotalWidth - progressCompleted.Length);

		Console.Write(progressStart);
		Console.ForegroundColor = this._layout.ProgressIndicator.ForegroundColor;
		Console.Write(progressCompleted);
		Console.ForegroundColor = this._layout.PendingIndicator.ForegroundColor;
		Console.Write(progressUndone);
		Console.ResetColor();
		Console.Write(progressEnd);
	}
}
