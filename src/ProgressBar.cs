﻿namespace CliProgressBar;

/// <summary>
/// Provide an object to manage drawing and manipulating a console progress bar.
/// </summary>
public class ProgressBar : IDisposable, IProgress<float>
{
	private readonly Layout _layout = Layout.Default;

	private readonly TextWriter _out;

	private readonly int _maxSteps = 100;

	private readonly bool _isStandardOutputRedirected;

	private int _steps = 0;

	private bool _isStarted = false;

	/// <summary>
	/// Initializes a new instance of <see cref="ProgressBar"/> class.<br/>
	/// If <see cref="Console.IsOutputRedirected"/> is <c>True</c>, the progress bar will not be rendered.
	/// </summary>
	/// <param name="maxStep">Maximum step number allowed.</param>
	/// <param name="start"><c>True</c> to start showing immediatly the progress, otherwise <c>False</c></param>
	/// <param name="redirectConsoleOutput"><c>True</c> to redirect <see cref="Console"/> standard output to enable rendering progress on <see cref="Console.WriteLine(string)"/>.</param>
	public ProgressBar(int maxStep = 100, bool start = true, bool redirectConsoleOutput = false)
	{
		this._out = Console.Out;
		this._maxSteps = maxStep;
		this._isStandardOutputRedirected = redirectConsoleOutput;

		if (!Console.IsOutputRedirected && start)
			this.Start();
	}

	/// <inheritdoc cref="ProgressBar(int, bool, bool)"/>
	/// <param name="layout">Progress bar style.</param>
	/// <param name="maxStep"><inheritdoc cref="ProgressBar(int, bool, bool)" path="=/param[@name='maxStep']"/></param>
	/// <param name="start"><inheritdoc cref="ProgressBar(int, bool, bool)" path="=/param[@name='start']"/></param>
	/// <param name="redirectConsoleOutput"><c>True</c> to redirect <see cref="Console"/> standard output to enable rendering progress on <see cref="Console.WriteLine(string)"/>.</param>
	public ProgressBar(Layout layout, int maxStep = 100, bool start = true, bool redirectConsoleOutput = false) : this(maxStep, start, redirectConsoleOutput)
	{
		this._layout = layout;
	}

	/// <summary>
	/// Progress percentage to display.
	/// </summary>
	public double Percentage
	{
		get => (100.0 / this._maxSteps) * this._steps;
		private set
		{
			this._steps = (int)Math.Ceiling(value / 100.0 * this._maxSteps);
			this.Render();
		}
	}

	/// <summary>
	/// Set Layout primary text.
	/// </summary>
	/// <param name="text">Text to configure.</param>
	public void SetText(string text)
	{
		ArgumentNullException.ThrowIfNullOrEmpty(text, nameof(text));
		this._layout.Text.Value = text;
		this.Render();
	}

	/// <summary>
	/// Set Layout secondary optional text.
	/// </summary>
	/// <param name="text">Optional text to configure.</param>
	public void SetAdditionalText(string? text)
	{
		this._layout.AdditionalText.Value = text;
		this.Render();
	}

	/// <inheritdoc/>
	public void Report(float value)
	{
		if (value < 0 || value > 1)
			return;
		this.Percentage = value * 100;
	}

	/// <inheritdoc cref="Report(float)"/>
	/// <param name="value"><inheritdoc cref="Report(float)" path="=/param[@name='value']"/></param>
	/// <param name="updatedText">Dynamic information text to update.</param>
	public void Report(float value, string updatedText)
	{
		if (value < 0 || value > 1)
			return;
		this._layout.AdditionalText.Value = updatedText;
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
			this._layout.AdditionalText.Value = updatedText;

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

		if (this._isStandardOutputRedirected)
			Console.SetOut(new ProgressWriter(this));

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
		if (this._isStandardOutputRedirected)
			ResetConsoleOut();

	}

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Stop();
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
		List<string> valueLines = [];

		if (IsCursorExceedBufferHeightLimit(cursorTop, value) && this._isStarted)
		{
			valueLines = (value ?? " ").SplitByLength(Console.BufferWidth).ToList();
			if (cursorTop == Console.BufferHeight - 1)
			{
				Console.SetCursorPosition(0, Console.BufferHeight - 1);
				this._out.Write(Environment.NewLine);
				Console.SetCursorPosition(0, cursorTop - 1);
			}
			this._out.Write(valueLines[0].PadRight(Console.BufferWidth));
			Console.SetCursorPosition(0, Math.Min(cursorTop + 1, Console.BufferHeight - 1));
			valueLines.RemoveAt(0);
		}
		else
			this._out.WriteLine(value);

		Render();
		if (valueLines.Count > 0)
			this.WriteLine(string.Join(string.Empty, valueLines));
	}

	/// <summary>
	/// Determine if the output string exceed the buffer height limit.
	/// </summary>
	/// <param name="currentCursorTop">Get current console cursor top position.</param>
	/// <param name="value"><see cref="String"/> value to write to output stream.</param>
	/// <returns>Return <c>True</c> if the string value exceed the buffer limit, otherwise <c>False</c>.</returns>
	private bool IsCursorExceedBufferHeightLimit(int currentCursorTop, string? value)
	{
		int valueBufferHeighCount;
		if (value == null)
			valueBufferHeighCount = 1;
		else
			valueBufferHeighCount = (int)Math.Ceiling(value.Length / (double)Console.BufferWidth);

		return currentCursorTop + valueBufferHeighCount >= Console.BufferHeight - 1;
	}

	/// <summary>
	/// Reset standard console output stream.
	/// </summary>
	private void ResetConsoleOut()
		=> Console.SetOut(this._out);

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
		this._out.Write(new string(' ', Console.BufferWidth));
		Console.SetCursorPosition(left, top);
	}

	/// <summary>
	/// Draw the progress bar updated to the console output.
	/// </summary>
	private void DrawProgressBar()
	{
		string? text = this._layout.Text.Value;
		string? additionalText = this._layout.AdditionalText.Value;
		string percentage;
		if (this._layout.Bar.BracketOptions.HasValue && this._layout.Bar.BracketOptions.Value.HasFlag(BracketLayout.Percentage))
			percentage = $"[{this.Percentage:00}%]";
		else
			percentage = $"{this.Percentage:00}%";

		int totalTextualLenght = this._layout.GetTextualLengthWithSpacing() + percentage.Length;
		int availableWidth = Console.BufferWidth - totalTextualLenght;

		if (availableWidth < BarLayout.MinimumSize)
		{
			string ellipsis = "...";
			int charToRemove = BarLayout.MinimumSize - availableWidth + ellipsis.Length;
			if (additionalText != null)
			{
				if (additionalText.Length > charToRemove)
				{
					additionalText = additionalText.Remove(additionalText.Length - charToRemove) + ellipsis;
					charToRemove = 0;
				}
				else
				{
					charToRemove -= additionalText.Length;
					additionalText = null;
				}
			}

			if (text != null && charToRemove > 0)
			{
				if (text.Length > charToRemove)
				{
					text = text.Remove(text.Length - charToRemove) + ellipsis;
					charToRemove = 0;
				}
				else
				{
					charToRemove -= text.Length;
					text = null;
				}
			}
		}

		int availableBarWidth = Math.Max(availableWidth, BarLayout.MinimumSize);
		string progress = new string(this._layout.Bar.ProgressIndicator.Value, (int)Math.Floor(this.Percentage / 100.0 * availableBarWidth));
		string pending = new string(this._layout.Bar.PendingIndicator.Value, availableBarWidth - progress.Length);


		bool hasBarBrackets = this._layout.Bar.BracketOptions.HasValue && this._layout.Bar.BracketOptions.Value.HasFlag(BracketLayout.Bar);
		switch (this._layout.Bar.Position)
		{
			case LayoutPosition.Left:
				if (this._layout.Bar.Direction == BarDirection.Forward)
					this._out.Write(percentage.PadRight(percentage.Length + 1, ' '));
				if (hasBarBrackets)
					this._out.Write("[");
				DrawSection(progress, this._layout.Bar.ProgressIndicator.ForegroundColor, this._layout.Bar.ProgressIndicator.BackgroundColor);
				DrawSection(pending, this._layout.Bar.PendingIndicator.ForegroundColor, this._layout.Bar.PendingIndicator.BackgroundColor);
				if (hasBarBrackets)
					this._out.Write("]");
				if (this._layout.Bar.Direction == BarDirection.Reverse)
					this._out.Write(percentage.PadLeft(percentage.Length + 1, ' '));

				if (text != null)
					DrawSection(text.PadLeft(text.Length + 1, ' '), this._layout.Text.ForegroundColor, this._layout.Text.BackgroundColor);
				if (additionalText != null)
					DrawSection(additionalText.PadLeft(additionalText.Length + 1, ' '), this._layout.AdditionalText.ForegroundColor, this._layout.AdditionalText.BackgroundColor);
				break;
			case LayoutPosition.Right:
				if (text != null)
					DrawSection(text.PadRight(text.Length + 1, ' '), this._layout.Text.ForegroundColor, this._layout.Text.BackgroundColor);
				if (additionalText != null)
					DrawSection(additionalText.PadRight(additionalText.Length + 1, ' '), this._layout.AdditionalText.ForegroundColor, this._layout.AdditionalText.BackgroundColor);

				if (this._layout.Bar.Direction == BarDirection.Forward)
					this._out.Write(percentage.PadRight(percentage.Length + 1, ' '));
				if (hasBarBrackets)
					this._out.Write("[");
				DrawSection(progress, this._layout.Bar.ProgressIndicator.ForegroundColor, this._layout.Bar.ProgressIndicator.BackgroundColor);
				DrawSection(pending, this._layout.Bar.PendingIndicator.ForegroundColor, this._layout.Bar.PendingIndicator.BackgroundColor);
				if (hasBarBrackets)
					this._out.Write("]");
				if (this._layout.Bar.Direction == BarDirection.Reverse)
					this._out.Write(percentage.PadLeft(percentage.Length + 1, ' '));
				break;
			case LayoutPosition.Center:
				if (text != null)
					DrawSection(text.PadRight(text.Length + 1, ' '), this._layout.Text.ForegroundColor, this._layout.Text.BackgroundColor);

				if (this._layout.Bar.Direction == BarDirection.Forward)
					this._out.Write(percentage.PadRight(percentage.Length + 1, ' '));
				if (hasBarBrackets)
					this._out.Write("[");
				DrawSection(progress, this._layout.Bar.ProgressIndicator.ForegroundColor, this._layout.Bar.ProgressIndicator.BackgroundColor);
				DrawSection(pending, this._layout.Bar.PendingIndicator.ForegroundColor, this._layout.Bar.PendingIndicator.BackgroundColor);
				if (hasBarBrackets)
					this._out.Write("]");
				if (this._layout.Bar.Direction == BarDirection.Reverse)
					this._out.Write(percentage.PadLeft(percentage.Length + 1, ' '));

				if (additionalText != null)
					DrawSection(additionalText.PadLeft(additionalText.Length + 1, ' '), this._layout.AdditionalText.ForegroundColor, this._layout.AdditionalText.BackgroundColor);
				break;
		}
	}

	private void DrawSection(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
	{
		if (foregroundColor.HasValue)
			Console.ForegroundColor = foregroundColor.Value;
		if (backgroundColor.HasValue)
			Console.BackgroundColor = backgroundColor.Value;

		this._out.Write(text);
		Console.ResetColor();
	}
}
